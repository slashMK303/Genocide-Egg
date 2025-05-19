using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GunManager gunManager;
    [SerializeField] GameManager gameManager;
    private int maxWeapons = 6;


    // One Buy
    [Header("One Buy")]
    [SerializeField] private GameObject buyGunPanel;
    [SerializeField] private GameObject buyApplePanel;
    [SerializeField] private GameObject buyVampire;
    [SerializeField] private GameObject buyPoison;
    [SerializeField] private GameObject buyShieldPanel;

    [Header("Sound")]
    [SerializeField] private AudioClip buttonSound; // Tambahkan AudioClip
    private AudioSource playerAudioSource; // Ambil AudioSource dari Player
    private Transform target;

    private void Start()
    {
        target = GameObject.Find("Player").transform;

        if (target != null)
        {
            playerAudioSource = target.GetComponent<AudioSource>(); // Ambil AudioSource dari Player
        }
    }

    public void BuyGun()
    {
        playerAudioSource.PlayOneShot(buttonSound); // Mainkan suara menggunakan AudioSource dari Player

        if (player.GetCoinCount() >= 50 && gunManager.GetWeaponCount() < maxWeapons)
        {

            gunManager.BuyGun();
            player.SpendCoins(50); // Kurangi koin
            gameManager.UpdateShopUI(); // Update UI
            buyGunPanel.SetActive(false);
        }
    }

    public void BuyApple()
    {
        playerAudioSource.PlayOneShot(buttonSound); // Mainkan suara menggunakan AudioSource dari Player

        if (player.GetCoinCount() >= 35 && player.GetCurrentHealth() < player.GetMaxHealth())
        {
            player.IncreaseCurrentHP(30);
            player.SpendCoins(35); // Kurangi koin
            gameManager.UpdateShopUI(); // Update UI
            buyApplePanel.SetActive(false);
        }
    }

    public void BuyVampire()
    {
        playerAudioSource.PlayOneShot(buttonSound); // Mainkan suara menggunakan AudioSource dari Player

        if (player.GetCoinCount() >= 105 && player.hasVampireEffect == false)
        {
            player.SpendCoins(105); // Kurangi koin
            player.hasVampireEffect = true;
            gameManager.UpdateShopUI(); // Update UI
            buyVampire.SetActive(false);
        }
    }

    // ShopManager.cs
    public void BuyPoison()
    {
        playerAudioSource.PlayOneShot(buttonSound);

        if (player.GetCoinCount() >= 110 && player.hasPoison == false)
        {
            player.SpendCoins(110);
            player.hasPoison = true;
            gameManager.UpdateShopUI();
            buyPoison.SetActive(false);
        }
    }

    public void BuyShield()
    {
        playerAudioSource.PlayOneShot(buttonSound);

        if (player.GetCoinCount() >= 50)
        {
            player.AddShield(100); // Tambah 50 shield
            player.SpendCoins(50);
            gameManager.UpdateShopUI();
            buyShieldPanel.SetActive(false);
        }
    }
}
