using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Object/Event/Float Event Channel")]
public class FloatEventChannelSO : EventChannelBaseSO
{
    public UnityAction<float> OnEventRaised;
    public void RaiseEvent(float index)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(index);
    }
}
