using UnityEngine;

public class Enemy : Character
{
    [SerializeField] int scorePoint = 100;
    [SerializeField] int deathEnergyBonus = 3;

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
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);
        EnemyManager.Instance.RemoveFromList(gameObject);
        base.GetDie();
    }
}