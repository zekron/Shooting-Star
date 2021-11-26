using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>, IEnergy
{
    [SerializeField] private EnergyStatsBar energyBar;
    [SerializeField] private float overdriveInterval = 0.1f;

    bool available = true;

    public const int MAX = 100;
    public const int PERCENT = 1;
    int energy;

    private WaitForSeconds waitForOverdriveInterval;

    protected override void Awake()
    {
        base.Awake();
        waitForOverdriveInterval = new WaitForSeconds(overdriveInterval);
    }

    void OnEnable()
    {
        PlayerOverdrive.On += OpenPlayerOverdrive;
        PlayerOverdrive.Off += StopPlayerOverdrive;
    }

    void OnDisable()
    {
        PlayerOverdrive.On -= OpenPlayerOverdrive;
        PlayerOverdrive.Off -= StopPlayerOverdrive;
    }

    void Start()
    {
        energyBar.Initialize(energy, MAX);
        GainEnergy(MAX);
    }

    public void GainEnergy(int value)
    {
        if (energy == MAX || !available || !gameObject.activeSelf) return;

        energy = Mathf.Clamp(energy + value, 0, MAX);
        energyBar.UpdateStats(energy, MAX);
    }

    public void DrainEnergy(int value)
    {
        energy -= value;
        energyBar.UpdateStats(energy, MAX);

        // if player is overdriving and energy = 0
        if (energy == 0 && !available)
        {
            // player stop overdriving
            PlayerOverdrive.Off.Invoke();
        }
    }

    public bool IsEnough(int value) => energy >= value;

    void OpenPlayerOverdrive()
    {
        available = false;
        StartCoroutine(nameof(KeepUsingCoroutine));
    }

    void StopPlayerOverdrive()
    {
        available = true;
        StopCoroutine(nameof(KeepUsingCoroutine));
    }

    IEnumerator KeepUsingCoroutine()
    {
        while (gameObject.activeSelf && energy > 0)
        {
            // every 0.1 seconds 
            yield return waitForOverdriveInterval;

            // use 1% of max energy, every 1 second use 10% of max energy 
            // means that overdrive last for 10 seconds
            DrainEnergy(PERCENT);
        }
    }
}