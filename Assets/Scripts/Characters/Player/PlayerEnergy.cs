using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour, IEnergy
{
    [SerializeField] private float overdriveInterval = 0.1f;
    [SerializeField] private IntEventChannelSO energyInitEventSO;
    [SerializeField] private IntEventChannelSO energyUpdateEventSO;

    bool available = true;

    public const int MAX = 100;
    public const int PERCENT = 1;
    int energy;

    private WaitForSeconds waitForOverdriveInterval;

    protected void Awake()
    {
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
        energyInitEventSO.RaiseEvent(MAX);
        //GainEnergy(MAX);
    }

    public void GainEnergy(int value)
    {
        if (energy == MAX || !available || !gameObject.activeSelf) return;

        energy = Mathf.Clamp(energy + value, 0, MAX);
        energyUpdateEventSO.RaiseEvent(energy);
    }

    public void DrainEnergy(int value)
    {
        energy -= value;
        energyUpdateEventSO.RaiseEvent(energy);

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