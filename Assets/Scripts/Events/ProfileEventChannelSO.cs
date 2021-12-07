using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Object/Event/Profile Event Channel")]
public class ProfileEventChannelSO : EventChannelBaseSO
{
    public UnityAction<PlayerProfileSO,GameObject> OnEventRaised;
    public void RaiseEvent(PlayerProfileSO index, GameObject gameObject)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(index, gameObject);
    }
}
