using System.Collections;
using UnityEngine;

public class Enemy : Character
{
    [Header("---- Enemy ----")]
    [SerializeField] private VoidEventChannelSO enemyDestroyEventSO;
    [SerializeField] private IntEventChannelSO updateTotalScoreEventSO;
    [SerializeField] private IntEventChannelSO updatePlayerEnergyEventSO;
    [SerializeField] private IntEventChannelSO enemyLevelUpEventSO;

    [SerializeField] protected EnemyProfileSO enemyProfile;
    [SerializeField] protected InventoryPackage inventoryPackage;

    [SerializeField] protected int levelUpFactor = 2;

    protected EnemyController enemyController;
    private Inventory currentDropInventory;

    private int deathEnergyBonus;
    private int scorePoint;
    protected float minFireInterval;
    protected float maxFireInterval;

    protected override void Awake()
    {
        enemyController = GetComponent<EnemyController>();

        enemyLevelUpEventSO.OnEventRaised += EnemyLevelUp;
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        SetDropInventory();
        StartCoroutine(nameof(FireCoroutine));
    }

    protected override void SetProfile()
    {
        maxHealth = enemyProfile.MaxHealth;
        MoveSpeed = enemyProfile.MoveSpeed;
        MoveRotationAngle = enemyProfile.MoveRotationAngle;
        enemyController.SetMoveProfile(MoveSpeed, MoveRotationAngle);
        minFireInterval = enemyProfile.MinFireInterval;
        maxFireInterval = enemyProfile.MaxFireInterval;
        deathEnergyBonus = enemyProfile.DeathEnergyBonus;
        scorePoint = enemyProfile.ScorePoint;
    }

    protected override void OnDisable()
    {
        StopAllCoroutines();
        base.OnDisable();
    }

    protected override IEnumerator FireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

            if (GameManager.CurrentGameState == GameState.GameOver) yield break;

            foreach (var projectile in projectiles)
            {
                ObjectPoolManager.Release(projectile, muzzles[0].position);
            }

            AudioManager.Instance.PlaySFX(projectileLaunchSFX);
            muzzleVFX.Play();
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.GetDie();
            GetDie();
        }
    }

    public override void GetDie()
    {
        updatePlayerEnergyEventSO.RaiseEvent(deathEnergyBonus);
        base.GetDie();
        DropInventory();
        updateTotalScoreEventSO.RaiseEvent(scorePoint);
        enemyDestroyEventSO.RaiseEvent();
    }

    public override void Move(Vector2 movement)
    {
        transform.position = movement;
    }

    //public override void Rotate(Quaternion moveRotation)
    //{
    //    transform.rotation = moveRotation;
    //}

    protected virtual void EnemyLevelUp(int value)
    {
        maxHealth += value / levelUpFactor;
    }

    private void SetDropInventory()
    {
        currentDropInventory = inventoryPackage.CanDrop();
    }

    private void DropInventory()
    {
        if (!currentDropInventory) return;

        ObjectPoolManager.Release(currentDropInventory.gameObject, transform.position);
    }
}