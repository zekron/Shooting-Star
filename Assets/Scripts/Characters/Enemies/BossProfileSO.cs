using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProfile SO", menuName = "Scriptable Object/Character/Enemy Profile")]
public class BossProfileSO : EnemyProfileSO
{
    //public float MinFireInterval;
    //public float MaxFireInterval;

    //public int ScorePoint;
    //public int DeathEnergyBonus;
    public float ContinuousFireDuration;

    public bool Initialize(int maxHealth, float moveSpeed, float moveRotationAngle, float minFireInterval, float maxFireInterval, int scorePoint, int deathEnergyBonus, float continuousFireDuration)
    {
        bool isSuccess = true;

        try
        {
            Initialize(maxHealth, moveSpeed, moveRotationAngle, minFireInterval, maxFireInterval, scorePoint, deathEnergyBonus);
            ContinuousFireDuration = continuousFireDuration;
        }
        catch (System.Exception exception)
        {
            isSuccess = false;
            Debug.LogError(exception.Message);
        }
        return isSuccess;
    }
    //public bool Initialize(int maxHealth, float moveSpeed, float moveRotationAngle, float minFireInterval, float maxFireInterval, int scorePoint, int deathEnergyBonus)
    //{
    //    bool isSuccess = true;
    //    try
    //    {
    //        MaxHealth = maxHealth;

    //        MoveSpeed = moveSpeed;
    //        MoveRotationAngle = moveRotationAngle;

    //        MinFireInterval = minFireInterval;
    //        MaxFireInterval = maxFireInterval;

    //        ScorePoint = scorePoint;
    //        DeathEnergyBonus = deathEnergyBonus;
    //    }
    //    catch (System.Exception exception)
    //    {
    //        isSuccess = false;
    //        Debug.LogError(exception.Message);
    //    }
    //    return isSuccess;
    //}
}
