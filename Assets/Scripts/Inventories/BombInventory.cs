using UnityEngine;

public class BombInventory : BonusInventory, IInteractable
{
    [Header("Bomb Inventory")]
    [SerializeField] private IntEventChannelSO updateBombEventSO;

    [SerializeField] private int bombAmount = 1;

    public override void Activate(Player player)
    {
        if (player.CanGainMissile())
        {
            updateBombEventSO.RaiseEvent(bombAmount);
            gameObject.SetActive(false);
        }
        else
            base.Activate(player);
    }
}