using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
    [SerializeField] private ShieldStatsBar shieldBar;
    [SerializeField] private EnergyStatsBar energyBar;
    [SerializeField] private MissileDisplay missileDisplay;

    [SerializeField] private FloatEventChannelSO shieldInitEventSO;
    [SerializeField] private FloatEventChannelSO shieldUpdateEventSO;
    [SerializeField] private IntEventChannelSO energyInitEventSO;
    [SerializeField] private IntEventChannelSO energyUpdateEventSO;
    [SerializeField] private IntEventChannelSO missileUpdateEventSO;
    [SerializeField] private FloatEventChannelSO missileCooldownTimeEventSO;

    private int maxEnergy;
    private float maxHealth;
    private float cooldownTime;

    private void OnEnable()
    {
        shieldUpdateEventSO.OnEventRaised += UpdateShield;
        shieldInitEventSO.OnEventRaised += InitShield;
        energyUpdateEventSO.OnEventRaised += UpdateEnergy;
        energyInitEventSO.OnEventRaised += InitEnergy;
        missileUpdateEventSO.OnEventRaised += UpdateMissile;
        missileCooldownTimeEventSO.OnEventRaised += UpdateMissileCooldownTime;
    }

    private void OnDisable()
    {
        shieldUpdateEventSO.OnEventRaised -= UpdateShield;
        shieldInitEventSO.OnEventRaised -= InitShield;
        energyUpdateEventSO.OnEventRaised -= UpdateEnergy;
        energyInitEventSO.OnEventRaised -= InitEnergy;
        missileUpdateEventSO.OnEventRaised -= UpdateMissile;
        missileCooldownTimeEventSO.OnEventRaised -= UpdateMissileCooldownTime;
    }

    private void InitShield(float value)
    {
        maxHealth = value;
        shieldBar.Initialize(value, maxHealth);
    }

    private void UpdateShield(float value)
    {
        shieldBar.UpdateStats(value, maxHealth);
    }

    private void InitEnergy(int value)
    {
        maxEnergy = value;
        energyBar.Initialize(0, maxEnergy);
    }

    private void UpdateEnergy(int value)
    {
        energyBar.UpdateStats(value, maxEnergy);
    }

    #region Missile
    private void UpdateMissile(int value)
    {
        missileDisplay.UpdateAmountText(value);

        if (value == 0)
        {
            missileDisplay.UpdateCooldownImage(1f);
        }
        else
        {
            StartCoroutine(CooldownCoroutine());
        }
    }

    private void UpdateMissileCooldownTime(float value)
    {
        cooldownTime = value;
    }

    private IEnumerator CooldownCoroutine()
    {
        float cooldownValue = cooldownTime;

        while (cooldownValue > 0f)
        {
            missileDisplay.UpdateCooldownImage(cooldownValue / cooldownTime);
            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime, 0f);

            yield return null;
        }
    }
    #endregion
}
