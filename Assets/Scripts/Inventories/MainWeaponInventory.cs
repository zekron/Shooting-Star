using System.Collections;
using UnityEngine;

public class MainWeaponInventory : WeaponPowerInventory
{

    private const int UPGRADE_LEVEL = 1;
    private MainWeaponType type;

    public override void Activate(Player player)
    {
        setWeaponTypeEventSO.RaiseEvent((int)type);

        if (player.CanUpgradeMainWeaponPower(UPGRADE_LEVEL))
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
            type = StaticData.GetNextMainWeaponType(type);
            inventoryRenderer.material = materials[(int)type];
            yield return waitForChangeType;
        }
    }
}