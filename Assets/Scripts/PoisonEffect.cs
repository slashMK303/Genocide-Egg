// PoisonEffect.cs
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    private Enemy enemy;
    private int damagePerTick;
    private float duration;
    private float tickRate;
    private float nextTickTime;
    private float endTime;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Color poisonColor = new Color(0.5f, 1f, 0.5f, 1f); // Warna hijau terang untuk efek racun

    public void Initialize(Enemy target, int damage, float dur, float tick)
    {
        enemy = target;
        damagePerTick = damage;
        duration = dur;
        tickRate = tick;

        nextTickTime = Time.time + tickRate;
        endTime = Time.time + duration;

        spriteRenderer = enemy.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            spriteRenderer.color = poisonColor;
        }

        enabled = true;
    }

    private void Update()
    {
        if (Time.time >= endTime)
        {
            RemoveEffect();
            return;
        }

        if (Time.time >= nextTickTime)
        {
            enemy.Hit(damagePerTick);
            nextTickTime = Time.time + tickRate;
        }
    }

    private void RemoveEffect()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        Destroy(this);
    }
}