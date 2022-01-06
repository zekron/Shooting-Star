using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO animationClipFinishedEventSO;

    private Text waveText;
    private Animator animator;

    private void Awake()
    {
        //GetComponent<Canvas>().worldCamera = Camera.main;
        waveText = GetComponentInChildren<Text>();
        animator = GetComponent<Animator>();
    }

    public void UpdateWave(int value)
    {
        if (value == 0)
        {
            animator.SetTrigger("Ready");
        }
        else
        {
            animator.SetTrigger("Warning");
        }
        //waveText.text = $"- WAVE {value} -";
    }

    #region Animation Event
    private void SetText(string text)
    {
        waveText.text = text;
    }
    private void OnAnimationFinished()
    {
        animationClipFinishedEventSO.RaiseEvent();
        //gameObject.SetActive(false);
    }
    #endregion
}