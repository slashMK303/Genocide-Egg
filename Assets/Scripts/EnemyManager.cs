using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs; // Array untuk musuh biasa
    [SerializeField] GameObject[] chargerPrefabs; // Array untuk musuh dengan ability khusus
    [SerializeField] float specialEnemyChance = 10f; // Persentase kemunculan musuh khusus
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] int increasedHealth = 50;
    float currentTimeBetweenSpawns;

    // Save maxhealth enemy
    private int lastBonusWave = 0;
    private int currentBonusHealth = 0;

    Transform enemiesParent;

    public static EnemyManager Instance;

    private void Awake()
    {
        // To call it from other scripts
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        enemiesParent = GameObject.Find("Enemies").transform;
    }

    private void Update()
    {
        if (!WaveManager.Instance.WaveRunning()) return;

        currentTimeBetweenSpawns -= Time.deltaTime;

        if (currentTimeBetweenSpawns <= 0)
        {
            SpawnEnemy();
            currentTimeBetweenSpawns = timeBetweenSpawns;
        }
    }

    Vector2 RandomPosition()
    {
        return new Vector2(Random.Range(-16, 16), Random.Range(-8, 8));
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0) return;

        GameObject enemyToSpawn;
        float roll = Random.Range(0f, 100f);

        if (roll < specialEnemyChance && chargerPrefabs.Length > 0 && WaveManager.Instance.GetNextWave() > 3)
        {
            int randomIndex = Random.Range(0, chargerPrefabs.Length);
            enemyToSpawn = chargerPrefabs[randomIndex];
        }
        else
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            enemyToSpawn = enemyPrefabs[randomIndex];
        }

        var enemyGO = Instantiate(enemyToSpawn, RandomPosition(), Quaternion.identity);
        enemyGO.transform.SetParent(enemiesParent);

        // Perhitungan bonus health yang bertahan sampai kelipatan 5 berikutnya
        int currentWave = WaveManager.Instance.GetNextWave() - 1;

        // Jika ini wave kelipatan 5, update bonus health
        if (currentWave % 5 == 0)
        {
            lastBonusWave = currentWave;
            currentBonusHealth = lastBonusWave * increasedHealth;
        }

        Enemy enemyScript = enemyGO.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            int baseMaxHealth = 100; // Sesuaikan dengan default health enemy
            enemyScript.SetMaxHealth(baseMaxHealth + currentBonusHealth);
        }
    }


    public void DestroyAllEnemies()
    {
        foreach (Transform e in enemiesParent)
            Destroy(e.gameObject);
    }
}
