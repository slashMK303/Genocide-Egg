using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 18f;

    private Gun shooter;

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    public void SetShooter(Gun gun)
    {
        shooter = gun;
    }

    // Projectile.cs
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            Destroy(gameObject);
            CameraShake.instance.Shake(0.01f, 0.01f);
            enemy.Hit(25);

            // Tambahkan efek racun jika pemain memiliki kemampuan ini
            Player player = FindObjectOfType<Player>();
            if (player != null && player.hasPoison)
            {
                // Cek chance untuk menerapkan efek racun
                if (Random.value <= player.poisonChance)
                {
                    // Tambahkan komponen PoisonEffect jika belum ada
                    PoisonEffect existingPoison = enemy.GetComponent<PoisonEffect>();
                    if (existingPoison == null)
                    {
                        PoisonEffect poison = enemy.gameObject.AddComponent<PoisonEffect>();
                        poison.Initialize(enemy, player.poisonDamage, player.poisonDuration, player.poisonTickRate);
                    }
                }
            }
        }
    }
}