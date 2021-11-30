using System.Collections;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private VoidEventChannelSO enemyDestroyEventSO;
    [SerializeField] private IntEventChannelSO updateTotalScoreEventSO;

    [SerializeField] protected EnemyProfileSO enemyProfile;
    protected override void OnEnable()
    {
        base.OnEnable();

        health = enemyProfile.MaxHealth;
        MoveSpeed = enemyProfile.MoveSpeed;
        MoveRotationAngle = enemyProfile.MoveRotationAngle;

        StartCoroutine(nameof(FireCoroutine));
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    protected override IEnumerator FireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(enemyProfile.MinFireInterval, enemyProfile.MaxFireInterval));

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
        PlayerEnergy.Instance.GainEnergy(enemyProfile.DeathEnergyBonus);
        base.GetDie();
        updateTotalScoreEventSO.RaiseEvent(enemyProfile.ScorePoint);
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