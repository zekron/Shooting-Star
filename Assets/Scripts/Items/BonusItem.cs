using UnityEngine;

public class BonusItem : Item, IInteractable
{
    [SerializeField] private IntEventChannelSO updateTotalScoreEventSO;

    [SerializeField] private int bonusScore = 100;

    public void Activate(Player player)
    {
        updateTotalScoreEventSO.RaiseEvent(bonusScore);
        gameObject.SetActive(false);
    }
}