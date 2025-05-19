using System;
using TMPro;
using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] float nextWaveDelay = 0.1f;

    public static WaveManager Instance;

    bool waveRunning = true;
    int currentWave = 0;
    int currentWaveTime;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        StartNewWave();
        timeText.text = "30";
        waveText.text = "Wave: 1";
    }

    public bool WaveRunning() => waveRunning;

    public void StartNewWave()
    {
        StopAllCoroutines();
        timeText.color = Color.white;
        currentWave++;
        waveRunning = true;
        timeText.text = "30";
        currentWaveTime = 30;
        waveText.text = "Wave: " + currentWave;
        StartCoroutine(WaveTimer());
    }

    IEnumerator WaveTimer()
    {
        while (waveRunning)
        {
            yield return new WaitForSeconds(1f);
            currentWaveTime--;
            timeText.text = currentWaveTime.ToString();

            if (currentWaveTime <= 0)
            {
                waveRunning = false;
                StartCoroutine(CheckEnemiesAndShowShopPanel());
                yield break;
            }
        }
    }

    private void WaveComplete()
    {
        StopAllCoroutines();
        EnemyManager.Instance.DestroyAllEnemies();
        waveRunning = false;
        currentWaveTime = 30;
        timeText.text = currentWaveTime.ToString();
        timeText.color = Color.red;
    }

    IEnumerator CheckEnemiesAndShowShopPanel()
    {
        timeText.color = Color.red;
        EnemyManager.Instance.DestroyAllEnemies();

        CollectAllCoins(); // Tambahkan ini

        yield return new WaitForSeconds(nextWaveDelay);
        GameManager.instance.ShowShopPanel();
    }

    public int GetNextWave() // menyimpan wave saat ini
    {
        return currentWave + 1;
    }

    public int GetWave() // menyimpan wave saat ini
    {
        return currentWave;
    }

    // COllect all coin
    private void CollectAllCoins()
    {
        GameObject[] allCoins = GameObject.FindGameObjectsWithTag("Coin");
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            Player player = playerObj.GetComponent<Player>();

            foreach (GameObject coin in allCoins)
            {
                Coin coinScript = coin.GetComponent<Coin>();

                if (coinScript != null)
                {
                    player.AddCoins(GetPrivateCoinValue(coinScript));
                    Destroy(coin);
                }
            }

            player.UpdateCoinUI();
        }
    }

    // Karena value di Coin bersifat private, kita butuh refleksi untuk mengambil nilainya
    private int GetPrivateCoinValue(Coin coin)
    {
        var field = typeof(Coin).GetField("value", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (int)field.GetValue(coin);
    }

}
