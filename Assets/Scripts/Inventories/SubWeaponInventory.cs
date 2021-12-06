using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubWeaponInventory : WeaponPowerInventory
{
    private const int UPGRADE_LEVEL = 1;
    private SubWeaponType type;

    public override void Activate(Player player)
    {
        setWeaponTypeEventSO.RaiseEvent((int)type);

        if (player.CanUpgradeSubWeaponPower(UPGRADE_LEVEL))
        {
            upgradeWeaponPowerEventSO.RaiseEvent(UPGRADE_LEVEL);
            gameObject.SetActive(false);
        }
        else
            base.Activate(player);
    }
    protected override IEnumerator ChangeType()
    {
        while (gameObject.activeSelf)
        {
            type = StaticData.GetNextSubWeaponType(type);
            inventoryRenderer.material = materials[(int)type];
            yield return waitForChangeType;
        }
    }
}
