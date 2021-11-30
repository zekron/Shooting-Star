using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour, IHealth, IShooting, IMoveable
{
    [Header("---- DEATH ----")]
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private AudioDataSO deathSFX;

    [Header("---- HEALTH ----")]
    [SerializeField] private bool showOnHeadHealthBar = true;
    [SerializeField] private StatsBar onHeadHealthBar;
    protected float maxHealth;
    private float health;

    [Header("---- FIRE ----")]
    [SerializeField] protected GameObject[] projectiles;
    [SerializeField] protected Transform[] muzzles;
    [SerializeField] protected AudioDataSO projectileLaunchSFX;

    [SerializeField] public float MoveSpeed { get; protected set; }
    [SerializeField] public float MoveRotationAngle { get; protected set; }

    protected WaitForSeconds waitForFireInterval = new WaitForSeconds(0.5f);

    private UnityEvent<bool> onHealthChanged = new UnityEvent<bool>();

    protected float paddingX;
    protected float paddingY;

    protected bool isAlive;

    public float Health
    {
        get => health;
        set
        {
            health = value;
            onHealthChanged.Invoke(showOnHeadHealthBar);
        }
    }

    protected virtual void Awake()
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;

        SetProfile();
    }

    protected virtual void OnEnable()
    {
        onHealthChanged.AddListener(SetOnHeadHealthBar);
        Health = maxHealth;
        isAlive = true;
    }

    protected virtual void SetProfile() { }

    public void SetOnHeadHealthBar(bool flag)
    {
        onHeadHealthBar.gameObject.SetActive(flag);

        if (flag && gameObject.activeSelf)
        {
            onHeadHealthBar.Initialize(Health, maxHealth);
        }
    }

    #region Health
    public virtual void GetDamage(float damage)
    {
        if (health <= 0) return;

        Health -= damage;

        if (Health <= 0f && isAlive)
        {
            isAlive = false;
            GetDie();
        }
    }

    public virtual void GetHealing(float healing)
    {
        if (Health == maxHealth) return;

        // health += value;
        // health = Mathf.Clamp(health, 0f, maxHealth);
        Health = Mathf.Clamp(Health + healing, 0f, maxHealth);
    }

    public virtual void GetDie()
    {
        Health = 0f;
        AudioManager.Instance.PlaySFX(deathSFX);
        ObjectPoolManager.Release(deathVFX, transform.position);
        gameObject.SetActive(false);
    }

    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (Health < maxHealth)
        {
            yield return waitTime;

            GetHealing(maxHealth * percent);
        }
    }

    protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (Health > 0f)
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
    public virtual void Move(Vector2 deltaMovement)
    {
        transform.Translate(deltaMovement, Space.World);
    }

    public virtual void Rotate(Quaternion moveRotation)
    {
        transform.rotation = moveRotation;
    }
    #endregion
}