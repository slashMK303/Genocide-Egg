using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MMBtnManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button backButton;
    [SerializeField] Button creditsButton;
    [SerializeField] GameObject creditsPanel;

    [Header("Sound")]
    [SerializeField] private AudioClip buttonSound; // Tambahkan AudioClip
    private AudioSource mainAudioSource; // Ambil AudioSource dari Player

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(BtnStart);
        creditsButton.onClick.AddListener(BtnCredits);
        backButton.onClick.AddListener(BtnBack);
        exitButton.onClick.AddListener(ExitGame);
        mainAudioSource = GetComponent<AudioSource>();
    }

    public void BtnStart()
    {
        mainAudioSource.PlayOneShot(buttonSound); // Mainkan suara menggunakan AudioSource dari Player
        StartCoroutine(DelayLoadScene());
    }

    IEnumerator DelayLoadScene()
    {
        yield return new WaitForSeconds(buttonSound.length);
        SceneManager.LoadScene(1);
    }

    public void BtnCredits()
    {
        mainAudioSource.PlayOneShot(buttonSound); // Mainkan suara menggunakan AudioSource dari Player
        creditsPanel.SetActive(true);
    }

    public void BtnBack()
    {
        mainAudioSource.PlayOneShot(buttonSound); // Mainkan suara menggunakan AudioSource dari Player
        creditsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        mainAudioSource.PlayOneShot(buttonSound); // Mainkan suara menggunakan AudioSource dari Player
        StartCoroutine(DelayExit());
    }

    IEnumerator DelayExit()
    {
        yield return new WaitForSeconds(buttonSound.length);

        // Jika sedang di editor Unity, berhenti play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Jika di build game, keluar dari aplikasi
        Application.Quit();
#endif
    }

}
