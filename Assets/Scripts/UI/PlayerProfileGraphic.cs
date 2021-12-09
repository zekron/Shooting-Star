using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileGraphic : Graphic
{
    [SerializeField] private float[] verticValues;
    [SerializeField] private Color verticColor;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField, Tooltip("Use for normalization of graphics")] private float maxValue;

    Coroutine valueCoroutine;

    private float[] currentValues;
    private float[] previousValues;
    public void SetValues(params float[] values)
    {
        currentValues = values;
        previousValues = verticValues;

        if (valueCoroutine != null)
        {
            StopCoroutine(valueCoroutine);
        }
        valueCoroutine = StartCoroutine(nameof(ValueCoroutine));
    }

    private IEnumerator ValueCoroutine()
    {
        float timer = 0;
        while (timer < animationDuration)
        {
            timer += Time.deltaTime;
            for (int i = 0; i < verticValues.Length; i++)
            {
                verticValues[i] = Mathf.Lerp(previousValues[i], currentValues[i], timer / animationDuration);
            }
            SetVerticesDirty();
            yield return null;
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        int length = verticValues.Length;
        float delta = Mathf.PI * 2 / length;
        float x = 0, y = 0;
        vh.AddVert(new Vector3(0, 0, 0), Color.white, new Vector2(0, 0));
        for (int i = 0; i < length; i++)
        {
            //第一个点从Y轴开始 Start from axisY
            x = Mathf.Sin(i * delta);
            y = Mathf.Cos(i * delta);
            vh.AddVert(new Vector3(x * verticValues[i] * maxValue, y * verticValues[i] * maxValue, 0), verticColor, new Vector2(x, y));

            if (i > 0)
            {
                vh.AddTriangle(0, i, i + 1);
            }
        }
        vh.AddTriangle(0, length, 1);
    }
}
