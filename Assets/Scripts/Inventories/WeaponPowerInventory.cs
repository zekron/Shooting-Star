using System.Collections;
using UnityEngine;

public class WeaponPowerInventory : BonusInventory
{
    [Header("Weapon Power Inventory")]
    [SerializeField] protected IntEventChannelSO upgradeWeaponPowerEventSO;
    [SerializeField] protected IntEventChannelSO setWeaponTypeEventSO;
    [SerializeField] protected Material[] materials;
    [SerializeField] protected Renderer inventoryRenderer;
    [SerializeField] protected float changeTypeInterval = 3f;
    protected WaitForSeconds waitForChangeType;

    protected override void Awake()
    {
        base.Awake();

        waitForChangeType = new WaitForSeconds(changeTypeInterval);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(nameof(ChangeType));
    }
    public override void Activate(Player player)
    {
        base.Activate(player);
    }

    protected virtual IEnumerator ChangeType() { yield return null; }
}