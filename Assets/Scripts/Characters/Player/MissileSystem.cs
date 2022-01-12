using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] private int maxAmount = 5;
    [SerializeField] private float cooldownTime = 1f;
    [SerializeField] private GameObject missilePrefab = null;
    [SerializeField] private AudioDataSO launchSFX = null;

    [SerializeField] private IntEventChannelSO missileUpdateEventSO;
    [SerializeField] private FloatEventChannelSO missileCooldownTimeEventSO;

    private WaitForSeconds waitForCooldownInterval;

    private bool isReady = true;
    private int amount;

    void Awake()
    {
        amount = maxAmount;

        waitForCooldownInterval = new WaitForSeconds(cooldownTime);
    }

    void Start()
    {
        missileUpdateEventSO.RaiseEvent(amount);
        missileCooldownTimeEventSO.RaiseEvent(cooldownTime);
    }

    public bool CanGainMissile() => amount < maxAmount;

    public void UpdateMissile(int value)
    {
        missileUpdateEventSO.RaiseEvent(amount += value);

        StartCoroutine(nameof(CooldownCoroutine));
    }

    public void Launch(Transform muzzleTransform, bool isDebugMode = false)
    {
        if (!isDebugMode && (amount == 0 || !isReady)) return;    // TODO: Add SFX && UI VFX here

        isReady = false;
        ObjectPoolManager.Release(missilePrefab, muzzleTransform.position);
        AudioManager.Instance.PlaySFX(launchSFX);

        if (!isDebugMode) amount--;
        missileUpdateEventSO.RaiseEvent(amount);

        StartCoroutine(nameof(CooldownCoroutine));
    }

    IEnumerator CooldownCoroutine()
    {
        if (amount == 0) yield break;

        yield return waitForCooldownInterval;

        isReady = true;
    }
}