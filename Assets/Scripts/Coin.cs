using UnityEngine;

public class Coin : MonoBehaviour
{
    private int value; // Nilai koin

    [Header("Sound")]
    [SerializeField] private AudioClip coinSound; // Suara koin
    private AudioSource audioSource; // AudioSource dari prefab Coin itu sendiri


    public void SetValue(int amount)
    {
        value = amount; // Set nilai dari enemy yang mati
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(coinSound, transform.position);

            collision.GetComponent<Player>().AddCoins(value); // Tambahkan koin ke player
            Destroy(gameObject); // Hancurkan koin setelah suara selesai
        }
    }
}

