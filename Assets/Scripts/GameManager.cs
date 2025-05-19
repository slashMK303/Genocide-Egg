using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField] GameObject gameOverPanel;
	[SerializeField] Button restartButton;
	[SerializeField] GameObject shopPanel;
	[SerializeField] Button goButton;
	[SerializeField] Button mainmenuButton;
	[SerializeField] Button mainmenuButtonPause;

	[Header("Shop Panel")]
	[SerializeField] private TextMeshProUGUI shopWaveText;
	[SerializeField] private TextMeshProUGUI shopCoinText;
	[SerializeField] private TextMeshProUGUI shopMaxHpText;
	[SerializeField] private TextMeshProUGUI shopCurrentHpText;
	[SerializeField] private TextMeshProUGUI shopWeaponsText;
	[SerializeField] private TextMeshProUGUI shopHpRegenText;
	[SerializeField] private TextMeshProUGUI shopPoisonText;
	[SerializeField] private TextMeshProUGUI shopShieldText;

	[Header("Random Shop Settings")]
	[SerializeField] private int minItemsToShow = 2;
	[SerializeField] private int maxItemsToShow = 4;

	// One Buy
	[Header("One Buy")]
	[SerializeField] private GameObject buyGunPanel;
	[SerializeField] private GameObject buyApplePanel;
	[SerializeField] private GameObject buyVampire;
	[SerializeField] private GameObject buyPoison;
	[SerializeField] private GameObject buyShield;

	[Header("Pause Panel")]
	[SerializeField] private GameObject pausePanel;
	[SerializeField] private Button resumeButton;

	[Header("GO Panel")]
	[SerializeField] private TextMeshProUGUI goCoinText;
	[SerializeField] private TextMeshProUGUI goWaveText;

	[Header("Wave Manager")]
	public float delayWave = 3f;

	[Header("Sound")]
	[SerializeField] private AudioClip buttonSound; // Tambahkan AudioClip
	private AudioSource playerAudioSource; // Ambil AudioSource dari Player
	private Transform target;

	// Shop random item
	private List<GameObject> allShopItems = new List<GameObject>();


	private bool gameRunning;
	public static GameManager instance;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	void Start()
	{
		Time.timeScale = 1f;
		// Cursor Hide
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		restartButton.onClick.AddListener(RestartGame);
		goButton.onClick.AddListener(OnGoButtonPressed);
		mainmenuButton.onClick.AddListener(MainMenu);
		mainmenuButtonPause.onClick.AddListener(MainMenu);

		InitializeShopItems();

		target = GameObject.Find("Player").transform;

		if (target != null)
		{
			playerAudioSource = target.GetComponent<AudioSource>(); // Ambil AudioSource dari Player
		}
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			pausePanel.SetActive(true);
			Time.timeScale = 0f;
		}
	}

	public void RestartGame()
	{
		playerAudioSource.PlayOneShot(buttonSound); // Mainkan suara menggunakan AudioSource dari Player
		StartCoroutine(DelayLoadScene());
		Time.timeScale = 1f;
	}

	IEnumerator DelayLoadScene()
	{
		yield return new WaitForSeconds(buttonSound.length);
		SceneManager.LoadScene(1);
	}

	public void MainMenu()
	{
		playerAudioSource.PlayOneShot(buttonSound); // Mainkan suara menggunakan AudioSource dari Player
		StartCoroutine(DelayLoadSceneMM());
		Time.timeScale = 1f;
	}

	IEnumerator DelayLoadSceneMM()
	{
		yield return new WaitForSeconds(buttonSound.length);
		SceneManager.LoadScene(0);
	}

	public bool IsGameRunning()
	{
		return gameRunning;
	}

	// Pause handel
	public void ResumeGame()
	{
		playerAudioSource.PlayOneShot(buttonSound); // Mainkan suara menggunakan AudioSource dari Player
		pausePanel.SetActive(false);
		Time.timeScale = 1f;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public void GameOver()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Time.timeScale = 0f;
		gameRunning = false;
		gameOverPanel.SetActive(true);

		// Ambil data
		int currentCoins = FindObjectOfType<Player>().GetCoinCount();
		int currentWave = WaveManager.Instance.GetWave();

		goCoinText.text = currentCoins.ToString();
		goWaveText.text = "Wave: " + currentWave.ToString();
	}

	public void ShowShopPanel()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		gameRunning = false;
		shopPanel.SetActive(true);

		// Initialize dan randomize shop items
		InitializeShopItems();
		RandomizeShopItems();

		// Ambil data dari WaveManager
		int nextWave = WaveManager.Instance.GetNextWave(); // Gunakan wave berikutnya
		int currentCoins = FindObjectOfType<Player>().GetCoinCount();
		int maxHp = FindObjectOfType<Player>().GetMaxHealth();
		int currentHP = FindObjectOfType<Player>().GetCurrentHealth();
		int countWP = FindObjectOfType<GunManager>().GetWeaponCount();
		Player player = FindObjectOfType<Player>(); // Cari player

		// Update teks pada Shop Panel
		shopWaveText.text = "Shop (Wave " + nextWave + ")";
		shopCoinText.text = currentCoins.ToString();
		shopMaxHpText.text = "Max Hp: " + maxHp;
		shopCurrentHpText.text = "Hp: " + currentHP;
		shopWeaponsText.text = "Weapons(" + countWP + "/6)";
		player.UpdateCoinUI();
	}

	void OnGoButtonPressed()
	{
		playerAudioSource.PlayOneShot(buttonSound); // Mainkan suara menggunakan AudioSource dari Player
		shopPanel.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		StartCoroutine(ResumeGameAfterDelay());
	}

	IEnumerator ResumeGameAfterDelay()
	{
		playerAudioSource.PlayOneShot(buttonSound); // Mainkan suara menggunakan AudioSource dari Player
		yield return new WaitForSeconds(delayWave); // Delay sebelum wave berjalan lagi
		gameRunning = true;
		WaveManager.Instance.StartNewWave();
	}

	public void UpdateShopUI()
	{
		int currentCoins = FindObjectOfType<Player>().GetCoinCount();
		int maxHp = FindObjectOfType<Player>().GetMaxHealth();
		int currentHP = FindObjectOfType<Player>().GetCurrentHealth();
		int countWP = FindObjectOfType<GunManager>().GetWeaponCount();
		string hasVampire = FindObjectOfType<Player>().hasVampireEffect ? "+2" : "0";
		string hasPoison = FindObjectOfType<Player>().hasPoison ? "Poison" : "0";
		int currentShield = FindObjectOfType<Player>().GetCurrentShield();
		Player player = FindObjectOfType<Player>(); // Cari player

		shopCoinText.text = currentCoins.ToString();
		shopMaxHpText.text = "Max Hp: " + maxHp;
		shopCurrentHpText.text = "Hp: " + currentHP;
		shopWeaponsText.text = "Weapons(" + countWP + "/6)";
		shopHpRegenText.text = "Hp Regeneration: " + hasVampire;
		shopPoisonText.text = "Effect: " + hasPoison;
		shopShieldText.text = "Shield: " + currentShield;
		player.UpdateCoinUI();
	}

	// Handle random shop item
	private void InitializeShopItems()
	{
		allShopItems.Clear();
		allShopItems.Add(buyGunPanel);
		allShopItems.Add(buyApplePanel);
		allShopItems.Add(buyVampire);
		allShopItems.Add(buyPoison);
		allShopItems.Add(buyShield);
	}

	private void RandomizeShopItems()
	{
		// Reset semua item ke inactive
		foreach (var item in allShopItems)
		{
			item.SetActive(false);
		}

		// Tentukan berapa banyak item yang akan ditampilkan
		int itemsToShow = Random.Range(minItemsToShow, maxItemsToShow + 1);

		// Acak item yang akan ditampilkan
		List<GameObject> shuffledItems = new List<GameObject>(allShopItems);
		ShuffleList(shuffledItems);

		// Aktifkan item yang terpilih
		for (int i = 0; i < Mathf.Min(itemsToShow, shuffledItems.Count); i++)
		{
			shuffledItems[i].SetActive(true);
		}
	}

	private void ShuffleList<T>(List<T> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			T temp = list[i];
			int randomIndex = Random.Range(i, list.Count);
			list[i] = list[randomIndex];
			list[randomIndex] = temp;
		}
	}
}
