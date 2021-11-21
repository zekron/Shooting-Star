using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("---- DEATH ----")]
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private AudioDataSO deathSFX;

    [Header("---- HEALTH ----")]
    [SerializeField] protected float maxHealth;
    [SerializeField] private bool showOnHeadHealthBar = true;
    [SerializeField] private StatsBar onHeadHealthBar;

    protected float health;

    protected virtual void OnEnable()
    {
        health = maxHealth;

        SetOnHeadHealthBar(showOnHeadHealthBar);
    }

    public void SetOnHeadHealthBar(bool flag)
    {
        onHeadHealthBar.gameObject.SetActive(flag);

        if (flag)
        {
            onHeadHealthBar.Initialize(health, maxHealth);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (showOnHeadHealthBar && gameObject.activeSelf)
        {
            onHeadHealthBar.UpdateStats(health, maxHealth);
        }

        if (health <= 0f)
        {
            Die();
        }
    }

    public virtual void RestoreHealth(float value)
    {
        if (health == maxHealth) return;

        // health += value;
        // health = Mathf.Clamp(health, 0f, maxHealth);
        health = Mathf.Clamp(health + value, 0f, maxHealth);

        if (showOnHeadHealthBar)
        {
            onHeadHealthBar.UpdateStats(health, maxHealth);
        }
    }

    public virtual void Die()
    {
        health = 0f;
        AudioManager.Instance.PlaySFX(deathSFX);
        ObjectPoolManager.Release(deathVFX, transform.position);
        gameObject.SetActive(false);
    }

    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health < maxHealth)
        {
            yield return waitTime;

            RestoreHealth(maxHealth * percent);
        }
    }

    protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health > 0f)
        {
            yield return waitTime;

            TakeDamage(maxHealth * percent);
        }
    }
}