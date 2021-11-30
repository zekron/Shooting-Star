using System.Collections;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private VoidEventChannelSO enemyDestroyEventSO;
    [SerializeField] private IntEventChannelSO updateTotalScoreEventSO;

    [SerializeField] protected EnemyProfileSO enemyProfile;

    private EnemyController enemyController;

    private int deathEnergyBonus;
    private int scorePoint;
    protected float minFireInterval;
    protected float maxFireInterval;

    protected override void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

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

    void OnDisable()
    {
        StopAllCoroutines();
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
        PlayerEnergy.Instance.GainEnergy(deathEnergyBonus);
        base.GetDie();
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
}