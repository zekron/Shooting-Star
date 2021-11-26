using System.Collections;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] int scorePoint = 100;
    [SerializeField] int deathEnergyBonus = 3;

    [SerializeField] private float minFireInterval;
    [SerializeField] private float maxFireInterval;


    protected override void OnEnable()
    {
        base.OnEnable();

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
        ScoreManager.Instance.AddScore(scorePoint);
        PlayerEnergy.Instance.GainEnergy(deathEnergyBonus);
        EnemyManager.Instance.RemoveFromList(gameObject);
        base.GetDie();
    }

    public override void Move(Vector3 movement)
    {
        transform.position = movement;
    }
    public override void Rotate(Quaternion moveRotation)
    {
        transform.rotation = moveRotation;
    }
}