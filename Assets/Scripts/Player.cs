using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI healthText;
	[SerializeField] Slider healthBar;
	[SerializeField] float moveSpeed = 6;
	[SerializeField] TextMeshProUGUI coinText;

	private int coinCount = 0; // Variabel untuk menyimpan jumlah koin

	// Passive & Ability
	public bool hasVampireEffect = false; // hp regen

	// Poison effect
	public bool hasPoison = false;
	public float poisonChance = 0.3f; // 30% chance to poison
	public int poisonDamage = 5; // Damage per tick
	public float poisonDuration = 3f; // Duration in seconds
	public float poisonTickRate = 0.5f; // Damage every 0.5 seconds

	// Player.cs
	[Header("Shield")]
	[SerializeField] private int maxShield = 0;
	[SerializeField] private int currentShield = 0;
	[SerializeField] private Slider shieldBar;
	[SerializeField] private GameObject shieldUI; // UI panel untuk shield

	Animator anim;
	Rigidbody2D rb;

	public int maxHealth = 100;
	int currentHealth;

	bool dead = false;

	float moveHorizontal, moveVertical;
	Vector2 movement;

	int facingDirection = 1; // 1 = right, -1 = left

	private void Start()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();

		currentHealth = maxHealth;
		healthText.text = maxHealth.ToString();
		healthBar.maxValue = maxHealth;
		healthBar.value = currentHealth;

		if (shieldUI != null)
		{
			shieldUI.SetActive(false);
		}
	}

	public void SpendCoins(int amount) => coinCount -= amount;

	private void Update()
	{
		if (dead)
		{
			movement = Vector2.zero;
			anim.SetFloat("velocity", 0);
			return;
		}

		moveHorizontal = Input.GetAxisRaw("Horizontal");
		moveVertical = Input.GetAxisRaw("Vertical");

		movement = new Vector2(moveHorizontal, moveVertical).normalized;

		anim.SetFloat("velocity", movement.magnitude);

		if (movement.x != 0)
		{
			facingDirection = movement.x > 0 ? 1 : -1;
		}

		transform.localScale = new Vector2(facingDirection, 1);
	}

	private void FixedUpdate()
	{
		rb.velocity = movement * moveSpeed;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Enemy enemy = collision.gameObject.GetComponent<Enemy>();

		if (enemy != null)
			Hit(20);
	}

	void Hit(int damage)
	{
		anim.SetTrigger("hit");

		if (currentShield > 0)
		{
			int remainingDamage = Mathf.Max(0, damage - currentShield);
			currentShield = Mathf.Max(0, currentShield - damage);
			UpdateShieldUI();

			if (remainingDamage > 0)
			{
				currentHealth -= remainingDamage;
				UpdateHealthUI();
			}
		}
		else
		{
			currentHealth -= damage;
			UpdateHealthUI();
		}

		if (currentHealth <= 0)
			Die();
	}

	private void UpdateHealthUI()
	{
		healthText.text = Mathf.Clamp(currentHealth, 0, maxHealth).ToString();
		healthBar.value = (float)currentHealth;
	}

	public void AddCoins(int amount)
	{
		coinCount += amount; // Tambahkan jumlah koin
		coinText.text = Mathf.Clamp(coinCount, 0, coinCount).ToString();
	}

	public void UpdateCoinUI()
	{
		coinText.text = coinCount.ToString(); // Perbarui teks UI koin
	}

	public int GetCoinCount() // simpan coin saat ini
	{
		return coinCount;
	}

	public int GetMaxHealth() // maxhp saat ini
	{
		return maxHealth;
	}

	public int GetCurrentHealth()
	{
		return currentHealth;
	}

	// Shop Panel Handel Upgrad Player
	public void IncreaseCurrentHP(int amount)
	{
		currentHealth = Mathf.Min(currentHealth + amount, maxHealth); // Bisa juga tambahkan heal jika ingin
		UpdateHealthUI();
	}

	public void Vampire(int amount)
	{
		if (hasVampireEffect == true)
		{
			currentHealth += amount;
			UpdateHealthUI();
		}
	}

	// Method shild
	public void AddShield(int amount)
	{
		if (maxShield == 0) // Aktifkan UI jika pertama kali membeli shield
		{
			shieldUI.SetActive(true);
			maxShield = 100; // Atau nilai default yang Anda inginkan
		}

		currentShield = Mathf.Min(currentShield + amount, maxShield);
		UpdateShieldUI();
	}

	public int GetCurrentShield()
	{
		return currentShield;
	}

	private void UpdateShieldUI()
	{
		if (shieldBar != null)
		{
			shieldBar.maxValue = maxShield;
			shieldBar.value = currentShield;
		}
	}

	void Die()
	{
		dead = true;
		GameManager.instance.GameOver();
	}
}
