using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] private int defaultAmount = 5;
    [SerializeField] private float cooldownTime = 1f;
    [SerializeField] private GameObject missilePrefab = null;
    [SerializeField] private AudioDataSO launchSFX = null;
    [SerializeField] private MissileDisplay missileDisplay;

    private bool isReady = true;
    private int amount;

    void Awake()
    {
        amount = defaultAmount;
    }

    void Start()
    {
        missileDisplay.UpdateAmountText(amount);
    }

    public void Launch(Transform muzzleTransform)
    {
        if (amount == 0 || !isReady) return;    // TODO: Add SFX && UI VFX here

        isReady = false;
        ObjectPoolManager.Release(missilePrefab, muzzleTransform.position);
        AudioManager.Instance.PlaySFX(launchSFX);
        amount--;
        missileDisplay.UpdateAmountText(amount);

        if (amount == 0)
        {
            missileDisplay.UpdateCooldownImage(1f);
        }
        else
        {
            StartCoroutine(CooldownCoroutine());
        }
    }

    IEnumerator CooldownCoroutine()
    {
        var cooldownValue = cooldownTime;

        while (cooldownValue > 0f)
        {
            missileDisplay.UpdateCooldownImage(cooldownValue / cooldownTime);
            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime, 0f);

            yield return null;
        }

        isReady = true;
    }
}