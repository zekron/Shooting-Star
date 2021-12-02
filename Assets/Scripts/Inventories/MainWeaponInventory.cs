﻿using UnityEngine;

public class MainWeaponInventory : Inventory, IInteractable
{
    [SerializeField] private IntEventChannelSO upgradeWeaponPowerEventSO;
    [SerializeField] private IntEventChannelSO setWeaponTypeEventSO;
    [SerializeField] private WeaponType type;
    [SerializeField] private int levelToUpgrade;

    public void Activate(Player player)
    {
        setWeaponTypeEventSO.RaiseEvent((int)type);
        upgradeWeaponPowerEventSO.RaiseEvent(levelToUpgrade);
        gameObject.SetActive(false);
    }
}