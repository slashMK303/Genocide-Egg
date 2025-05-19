using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Stats")]
	[SerializeField] int maxHealth = 100;
	[SerializeField] float speed = 2f;

	[Header("Sound")]
	[SerializeField] private AudioClip deathSound;
	private AudioSource audioSource; // AudioSource dari prefab Coin itu sendiri

	[Header("Charger")]
	[SerializeField] bool isCharger;
	[SerializeField] float distanceToCharge = 5f;
	[SerializeField] float chargeSpeed = 12f;
	[SerializeField] float prepareTime = 2f;
	bool isCharging = false;
	bool isPreparingCharge = false;

	[Header("Item Drop")]
	[SerializeField] GameObject coinPrefab; // Tambahkan ini di bagian variabel
	[SerializeField] float coinDropChance = 0.5f; // 50% drop chance

	[Header("Particle Effect")]
	[SerializeField] GameObject deathEffectPrefab; // Prefab efek partikel saat mati

	private int currentHealth;

	Animator anim;
	Transform target; // Follow target

	private void Start()
	{
		currentHealth = maxHealth;
		target = GameObject.Find("Player").transform;
		anim = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>(); // Ambil AudioSource dari objek ini

	}

	private void Update()
	{
		if (!WaveManager.Instance.WaveRunning()) return;
		if (isPreparingCharge) return;

		if (target != null)
		{
			Vector3 direction = target.position - transform.position;
			direction.Normalize();

			transform.position += direction * speed * Time.deltaTime;

			var playerToTheRight = target.position.x > transform.position.x;
			transform.localScale = new Vector2(playerToTheRight ? -1 : 1, 1);

			if (isCharger && !isCharging && Vector2.Distance(transform.position, target.position) < distanceToCharge)
			{
				isPreparingCharge = true;
				Invoke("StarCharging", prepareTime);
			}
		}
	}

	void StarCharging()
	{
		isPreparingCharge = false;
		isCharging = true;
		speed = chargeSpeed;
	}

	public void Hit(int damage)
	{
		currentHealth -= damage;
		anim.SetTrigger("hit");

		if (currentHealth <= 0)
		{
			PlayDeathSound();
			SpawnDeathEffect(); // Memunculkan efek partikel
			TryDropCoin(); // Coba menjatuhkan koin (tidak selalu)
			Destroy(gameObject);

			Player playerScript = GameObject.Find("Player").GetComponent<Player>();
			if (playerScript.hasVampireEffect == true && playerScript.GetCurrentHealth() < playerScript.GetMaxHealth()) // Jika pemain memiliki efek vampir
			{
				playerScript.Vampire(2);
			}
		}
	}

	// handel maxhp enemy perkelipatan 5
	public void SetMaxHealth(int value)
	{
		maxHealth = value;
		currentHealth = value;
	}


	void PlayDeathSound()
	{
		if (deathSound != null && audioSource != null)
		{
			AudioSource.PlayClipAtPoint(deathSound, transform.position);
		}
	}

	void SpawnDeathEffect()
	{
		if (deathEffectPrefab != null)
		{
			GameObject effect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
			Destroy(effect, 2f); // Hapus efek setelah 2 detik agar tidak menumpuk di scene
		}
	}

	void TryDropCoin()
	{
		float dropRoll = Random.value; // Menghasilkan angka antara 0.0 - 1.0
		if (dropRoll <= coinDropChance) // Jika angka lebih kecil dari peluang drop, drop koin
		{
			DropCoin();
		}
	}

	void DropCoin()
	{
		Vector2 dropPosition = (Vector2)transform.position + Random.insideUnitCircle * 0.5f; // Posisi drop sedikit acak
		GameObject coin = Instantiate(coinPrefab, dropPosition, Quaternion.identity);

		int coinValue = Random.Range(1, 4); // Nilai koin antara 1 - 3
		coin.GetComponent<Coin>().SetValue(coinValue); // Set nilai koin
	}

}
