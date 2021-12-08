using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Object/Event/Boolean Event Channel")]
public class BooleanEventChannelSO : EventChannelBaseSO
{
    public UnityAction<bool> OnEventRaised;
    public void RaiseEvent(bool index)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(index);
    }
}
