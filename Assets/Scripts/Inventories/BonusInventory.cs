using UnityEngine;

public class BonusInventory : Inventory, IInteractable
{
    [Header("Bonus Inventory")]
    [SerializeField] private IntEventChannelSO updateTotalScoreEventSO;

    [SerializeField] private int bonusScore = 100;

    public virtual void Activate(Player player)
    {
        updateTotalScoreEventSO.RaiseEvent(bonusScore);
        gameObject.SetActive(false);
    }
}