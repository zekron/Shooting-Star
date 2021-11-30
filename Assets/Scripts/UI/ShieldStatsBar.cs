using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShieldStatsBar : StatsBar
{
    [SerializeField] protected Text percentText;

    protected virtual void SetPercentText()
    {
        percentText.text = targetFillAmount.ToString("p0");
    }

    public override void Initialize(float currentValue, float maxValue)
    {
        base.Initialize(currentValue, maxValue);
        SetPercentText();
    }

    protected override IEnumerator BufferedFillingCoroutine(Image image)
    {
        SetPercentText();
        return base.BufferedFillingCoroutine(image);
    }
}