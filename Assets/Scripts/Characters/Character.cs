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

    private float paddingX;
    private float paddingY;

    protected float health;

    protected virtual void OnEnable()
    {
        health = maxHealth;

        //SetOnHeadHealthBar(showOnHeadHealthBar);
    }

    private void Awake()
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

    public void Fire()
    {
        throw new System.NotImplementedException();
    }

    public void Move(Vector3 deltaMovement)
    {
        deltaMovement.z = 0;
        transform.Translate(deltaMovement, Space.World);
        transform.position = Viewport.PlayerMoveablePosition(transform.position, paddingX, paddingY);
    }

    public void Rotate(Quaternion moveRotation)
    {
        transform.rotation = moveRotation;
    }
}