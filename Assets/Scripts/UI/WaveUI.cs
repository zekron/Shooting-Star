using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private IntEventChannelSO updateWaveEventSO;
    [SerializeField] private VoidEventChannelSO animationClipFinishedEventSO;

    private Text waveText;

    private void Awake()
    {
        //GetComponent<Canvas>().worldCamera = Camera.main;
        waveText = GetComponentInChildren<Text>();

        updateWaveEventSO.OnEventRaised += UpdateWave;

    }

    private void UpdateWave(int value)
    {
        waveText.text = $"- WAVE {value} -";
    }

    private void OnAnimationFinished()
    {
        animationClipFinishedEventSO.RaiseEvent();
        gameObject.SetActive(false);
    }
}