using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] private int defaultAmount = 5;
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
        amount = defaultAmount;

        waitForCooldownInterval = new WaitForSeconds(cooldownTime);
    }

    void Start()
    {
        missileUpdateEventSO.RaiseEvent(amount);
        missileCooldownTimeEventSO.RaiseEvent(cooldownTime);
    }

    public void Launch(Transform muzzleTransform)
    {
        if (amount == 0 || !isReady) return;    // TODO: Add SFX && UI VFX here

        isReady = false;
        ObjectPoolManager.Release(missilePrefab, muzzleTransform.position);
        AudioManager.Instance.PlaySFX(launchSFX);
        amount--;
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