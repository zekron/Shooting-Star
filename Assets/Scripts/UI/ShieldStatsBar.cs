using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShieldStatsBar : StatsBar
{
    [SerializeField] protected Text percentText;

    void SetPercentText()
    {
        //percentText.text = Mathf.RoundToInt(targetFillAmount * 100f) + "%";
        percentText.text = targetFillAmount.ToString("p0");
    }

    public override void Initialize(float currentValue, float maxValue)
    {
        base.Initialize(currentValue, maxValue);
        Debug.Log(targetFillAmount);
        SetPercentText();
    }

    protected override IEnumerator BufferedFillingCoroutine(Image image)
    {
        SetPercentText();
        return base.BufferedFillingCoroutine(image);
    }
}