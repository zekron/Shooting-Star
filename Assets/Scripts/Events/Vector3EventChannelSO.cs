using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Object/Event/Vector3 Event Channel")]
public class Vector3EventChannelSO : EventChannelBaseSO
{
    public UnityAction<Vector3> OnEventRaised;
    public void RaiseEvent(Vector3 index)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(index);
    }
}
