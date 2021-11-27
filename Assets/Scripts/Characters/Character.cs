using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour, IHealth, IShooting, IMoveable
{
    [Header("---- DEATH ----")]
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private AudioDataSO deathSFX;

    [Header("---- HEALTH ----")]
    [SerializeField] protected float maxHealth;
    [SerializeField] private bool showOnHeadHealthBar = true;
    [SerializeField] private StatsBar onHeadHealthBar;
    protected float health;

    [Header("---- FIRE ----")]
    [SerializeField] protected GameObject[] projectiles;
    [SerializeField] protected Transform[] muzzles;
    [SerializeField] protected AudioDataSO projectileLaunchSFX;

    protected WaitForSeconds waitForFireInterval = new WaitForSeconds(0.5f);

    private float paddingX;
    private float paddingY;

    protected virtual void OnEnable()
    {
        health = maxHealth;

        SetOnHeadHealthBar(showOnHeadHealthBar);
    }

    protected virtual void Awake()
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
    }

    public void SetOnHeadHealthBar(bool flag)
    {
        onHeadHealthBar.gameObject.SetActive(flag);

        if (flag)
        {
            onHeadHealthBar.Initialize(health, maxHealth);
        }
    }

    #region Health
    public virtual void GetDamage(float damage)
    {
        health -= damage;

        if (showOnHeadHealthBar && gameObject.activeSelf)
        {
            onHeadHealthBar.UpdateStats(health, maxHealth);
        }

        if (health <= 0f)
        {
            GetDie();
        }
    }

    public virtual void GetHealing(float healing)
    {
        if (health == maxHealth) return;

        // health += value;
        // health = Mathf.Clamp(health, 0f, maxHealth);
        health = Mathf.Clamp(health + healing, 0f, maxHealth);

        if (showOnHeadHealthBar)
        {
            onHeadHealthBar.UpdateStats(health, maxHealth);
        }
    }

    public virtual void GetDie()
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

            GetHealing(maxHealth * percent);
        }
    }

    protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health > 0f)
        {
            yield return waitTime;

            GetDamage(maxHealth * percent);
        }
    }
    #endregion

    #region Fire
    public void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
    }

    public void StopFire()
    {
        StopCoroutine(nameof(FireCoroutine));
    }

    protected virtual IEnumerator FireCoroutine()
    {
        while (true)
        {
            ObjectPoolManager.Release(projectiles[0], muzzles[0].position);

            AudioManager.Instance.PlaySFX(projectileLaunchSFX);

            yield return waitForFireInterval;
        }
    }
    #endregion

    #region Move
    public virtual void Move(Vector3 deltaMovement)
    {
        deltaMovement.z = 0;
        transform.Translate(deltaMovement, Space.World);
        transform.position = Viewport.PlayerMoveablePosition(transform.position, paddingX, paddingY);
    }

    public virtual void Rotate(Quaternion moveRotation)
    {
        transform.rotation = moveRotation;
    }
    #endregion
}