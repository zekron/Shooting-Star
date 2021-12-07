using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Object/Event/Profile Event Channel")]
public class ProfileEventChannelSO : EventChannelBaseSO
{
    public UnityAction<PlayerProfileSO> OnEventRaised;
    public void RaiseEvent(PlayerProfileSO index)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(index);
    }
}
