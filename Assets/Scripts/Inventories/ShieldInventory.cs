using UnityEngine;

public class ShieldInventory : BonusInventory, IInteractable
{
    [Header("Shield Inventory")]
    [SerializeField] private FloatEventChannelSO restoreShieldEventSO;

    [SerializeField] private int restoreValue = 20;

    public override void Activate(Player player)
    {
        if (player.CanHeal())
        {
            restoreShieldEventSO.RaiseEvent(restoreValue);
            gameObject.SetActive(false);
        }
        else
            base.Activate(player);
    }
}