using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerProfile SO", menuName = "Scriptable Object/Character/Player Profile")]
[System.Serializable]
public class PlayerProfileSO : CharacterProfileSO
{
    public float FireInterval;
    public MainWeaponType defaultWeaponType;

    public int OverdriveDodgeFactor;
    public float OverdriveSpeedFactor;
    public float OverdriveFireFactor;

    public bool Initialize(int maxHealth, float moveSpeed, float moveRotationAngle, float fireInterval, int overdriveDodgeFactor, float overdriveSpeedFactor, float overdriveFireFactor, int weaponType)
    {
        bool isSuccess = true;
        try
        {
            MaxHealth = maxHealth;

            MoveSpeed = moveSpeed;
            MoveRotationAngle = moveRotationAngle;

            FireInterval = fireInterval;
            defaultWeaponType = (MainWeaponType)weaponType;

            OverdriveDodgeFactor = overdriveDodgeFactor;
            OverdriveSpeedFactor = overdriveSpeedFactor;
            OverdriveFireFactor = overdriveFireFactor;
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
        try
        {
            return Initialize(
                int.Parse(datas[dataIndex++]),
                float.Parse(datas[dataIndex++]),
                float.Parse(datas[dataIndex++]),
                float.Parse(datas[dataIndex++]),
                int.Parse(datas[dataIndex++]),
                float.Parse(datas[dataIndex++]),
                float.Parse(datas[dataIndex++]),
                int.Parse(datas[dataIndex++]));
        }
        catch (System.Exception exception)
        {
            Debug.LogError(string.Format("Fail to Initialize profile: {0}", exception)); return false;
        }
    }
}
