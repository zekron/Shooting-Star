using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProfile SO", menuName = "Scriptable Object/Character/Enemy Profile")]
public class EnemyProfileSO : CharacterProfileSO
{
    public float MinFireInterval;
    public float MaxFireInterval;

    public int ScorePoint;
    public int DeathEnergyBonus;

    public bool Initialize(int maxHealth, float moveSpeed, float moveRotationAngle, float minFireInterval, float maxFireInterval, int scorePoint, int deathEnergyBonus)
    {
        bool isSuccess = true;
        try
        {
            MaxHealth = maxHealth;

            MoveSpeed = moveSpeed;
            MoveRotationAngle = moveRotationAngle;

            MinFireInterval = minFireInterval;
            MaxFireInterval = maxFireInterval;

            ScorePoint = scorePoint;
            DeathEnergyBonus = deathEnergyBonus;
        }
        catch (System.Exception exception)
        {
            isSuccess = false;
            Debug.LogError(exception.Message);
        }
        return isSuccess;
    }

    public override bool InitializeByString(string dataString)
    {
        string[] datas = dataString.Split(',');
        int dataIndex = 0;
        return Initialize(
            int.Parse(datas[dataIndex++]),
            float.Parse(datas[dataIndex++]),
            float.Parse(datas[dataIndex++]),
            float.Parse(datas[dataIndex++]),
            float.Parse(datas[dataIndex++]),
            int.Parse(datas[dataIndex++]),
            int.Parse(datas[dataIndex]));
    }
}
