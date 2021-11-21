using UnityEngine;
using UnityEngine.Events;

public class PlayerOverdrive : MonoBehaviour
{
    public static UnityAction On = delegate { };
    public static UnityAction Off = delegate { };

    [SerializeField] private GameObject triggerVFX;
    [SerializeField] private GameObject engineVFXNormal;
    [SerializeField] private GameObject engineVFXOverdrive;

    [SerializeField] private AudioDataSO openSFX;
    [SerializeField] private AudioDataSO stopSFX;

    private void Awake()
    {
        On += Open;
        Off += Stop;
    }

    private void OnDestroy()
    {
        On -= Open;
        Off -= Stop;
    }

    private void Open()
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