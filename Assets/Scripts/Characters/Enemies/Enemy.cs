using System.Collections;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private VoidEventChannelSO enemyDestroyEventSO;
    [SerializeField] private IntEventChannelSO updateTotalScoreEventSO;

    [SerializeField] protected EnemyProfileSO enemyProfile;

    private int deathEnergyBonus;
    private int scorePoint;
    private float minFireInterval;
    private float maxFireInterval;

    protected override void OnEnable()
    {
        base.OnEnable();

        StartCoroutine(nameof(FireCoroutine));
    }

    protected override void SetProfile()
    {
        Health = enemyProfile.MaxHealth;
        MoveSpeed = enemyProfile.MoveSpeed;
        MoveRotationAngle = enemyProfile.MoveRotationAngle;
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

    void OnCollisionEnter2D(Collision2D other)
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