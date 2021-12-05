using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubWeaponSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] subWeaponPrefab;
    [SerializeField] private AudioDataSO launchSFX = null;

    [Header("Sub Weapon Property")]
    [SerializeField] private SubWeaponType type;
    [SerializeField] private float cooldownTime;

    private WaitForSeconds waitForCooldownInterval;

    private bool isReady = true;

    void Awake()
    {
        waitForCooldownInterval = new WaitForSeconds(cooldownTime);
    }

    void Start()
    {

    }

    public void Launch(Muzzle[] muzzles, SubWeaponPower subWeaponPower)
    {
        if (!isReady) return;    // TODO: Add SFX && UI VFX here

        switch (subWeaponPower)
        {
            case SubWeaponPower.None:
                return;
            case SubWeaponPower.Level1:
                ObjectPoolManager.Release(subWeaponPrefab[(int)type], muzzles[0].GetMuzzle(MuzzlePower.Double));
                break;
            case SubWeaponPower.Level2:
                ObjectPoolManager.Release(subWeaponPrefab[(int)type], muzzles[5].GetMuzzle(MuzzlePower.Double));
                ObjectPoolManager.Release(subWeaponPrefab[(int)type], muzzles[6].GetMuzzle(MuzzlePower.Double));
                break;
            case SubWeaponPower.Level3:
                ObjectPoolManager.Release(subWeaponPrefab[(int)type], muzzles[0].GetMuzzle(MuzzlePower.Double));
                ObjectPoolManager.Release(subWeaponPrefab[(int)type], muzzles[5].GetMuzzle(MuzzlePower.Double));
                ObjectPoolManager.Release(subWeaponPrefab[(int)type], muzzles[6].GetMuzzle(MuzzlePower.Double));
                break;
            case SubWeaponPower.Level4:
            case SubWeaponPower.DEBUG:
                ObjectPoolManager.Release(subWeaponPrefab[(int)type], muzzles[1].GetMuzzle(MuzzlePower.Double));
                ObjectPoolManager.Release(subWeaponPrefab[(int)type], muzzles[2].GetMuzzle(MuzzlePower.Double));
                ObjectPoolManager.Release(subWeaponPrefab[(int)type], muzzles[5].GetMuzzle(MuzzlePower.Double));
                ObjectPoolManager.Release(subWeaponPrefab[(int)type], muzzles[6].GetMuzzle(MuzzlePower.Double));
                break;
            case SubWeaponPower.MAX:
                break;
            default:
                break;
        }
        isReady = false;
        AudioManager.Instance.PlaySFX(launchSFX);

        StartCoroutine(nameof(CooldownCoroutine));
    }

    IEnumerator CooldownCoroutine()
    {
        yield return waitForCooldownInterval;

        isReady = true;
    }
}
