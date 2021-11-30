using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProfile SO", menuName = "Scriptable Object/Character/Enemy Profile")]
public class BossProfileSO : EnemyProfileSO
{
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
            int.Parse(datas[dataIndex++]),
            float.Parse(datas[dataIndex]));
    }
}
