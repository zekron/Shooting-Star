using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [SerializeField] private Image fillImageBack;
    [SerializeField] protected Image fillImageFront;
    [SerializeField] private bool delayFill = true;
    [SerializeField] private float fillDelay = 0.5f;
    [SerializeField] private float fillSpeed = 0.1f;

    protected float targetFillAmount;
    private float currentFillAmount;
    private float previousFillAmount;
    private float coroutineTimer;

    WaitForSeconds waitForDelayFill;

    Coroutine bufferedFillingCoroutine;

    void Awake()
    {
        if (TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas.worldCamera = Camera.main;
        }

        waitForDelayFill = new WaitForSeconds(fillDelay);
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public virtual void Initialize(float currentValue, float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        targetFillAmount = currentFillAmount;
        fillImageBack.fillAmount = currentFillAmount;
        fillImageFront.fillAmount = currentFillAmount;
    }

    public void UpdateStats(float currentValue, float maxValue)
    {
        targetFillAmount = currentValue / maxValue;

        if (bufferedFillingCoroutine != null)
        {
            StopCoroutine(bufferedFillingCoroutine);
        }

        if (currentFillAmount > targetFillAmount)
        {
            fillImageFront.fillAmount = targetFillAmount;
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));

            return;
        }

        if (currentFillAmount < targetFillAmount)
        {
            fillImageBack.fillAmount = targetFillAmount;
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));
        }
    }

    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        if (delayFill)
        {
            yield return waitForDelayFill;
        }

        //previousFillAmount = currentFillAmount;
        coroutineTimer = 0f;

        while (coroutineTimer < fillSpeed)
        {
            coroutineTimer += Time.deltaTime;
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, coroutineTimer / fillSpeed);
            image.fillAmount = currentFillAmount;

            yield return null;
        }
    }
}