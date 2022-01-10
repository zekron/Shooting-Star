using UnityEngine;
using UnityEngine.Events;

public class PlayerOverdrive : MonoBehaviour
{
    [SerializeField] private FloatEventChannelSO overdriveOnEvent;
    [SerializeField] private VoidEventChannelSO overdriveOffEvent;

    [SerializeField] private GameObject triggerVFX;
    [SerializeField] private GameObject engineVFXNormal;
    [SerializeField] private GameObject engineVFXOverdrive;

    [SerializeField] private AudioDataSO openSFX;
    [SerializeField] private AudioDataSO stopSFX;

    private void Awake()
    {
        overdriveOnEvent.OnEventRaised += Open;
        overdriveOffEvent.OnEventRaised += Stop;
    }

    private void OnDestroy()
    {
        overdriveOnEvent.OnEventRaised -= Open;
        overdriveOffEvent.OnEventRaised -= Stop;
    }

    private void Open(float unused)
    {
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(true);
        AudioManager.Instance.PlaySFX(openSFX);
    }

    private void Stop()
    {
        engineVFXOverdrive.SetActive(false);
        engineVFXNormal.SetActive(true);
        AudioManager.Instance.PlaySFX(stopSFX);
    }
}