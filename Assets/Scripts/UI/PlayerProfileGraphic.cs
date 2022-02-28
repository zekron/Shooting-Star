using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileGraphic : Graphic
{
    private const int MAX_CIRCLE_VERTICES_COUNT = 36;
    [SerializeField] private float animationDuration = 0.5f;


    [Header("Vertex")]
    [SerializeField, Range(0, 1f)] private float[] vertexValues;
    [SerializeField] private Color vertexColor;
    [SerializeField, Tooltip("Use for normalization of graphics")] private float maxValue;
    [SerializeField] private float vertexWidth = 5;

    [Header("Bound")]
    [SerializeField] private Color boundColor;
    [SerializeField] private float boundWidth = 5;

    private List<Vector3> verticesPoints = new List<Vector3>();
    private List<LineMesh> boundPoints = new List<LineMesh>();
    private Coroutine valueCoroutine;

    private float[] currentValues;
    private float[] previousValues;
    private int vertexAmount;
    private float delta;

    protected override void Awake()
    {
        base.Awake();

        vertexAmount = vertexValues.Length;
        delta = Mathf.PI * 2 / vertexAmount;
    }

    public void SetValues(params float[] values)
    {
        currentValues = values;
        previousValues = vertexValues;

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
            for (int i = 0; i < vertexValues.Length; i++)
            {
                vertexValues[i] = Mathf.Lerp(previousValues[i], currentValues[i], timer / animationDuration);
            }
            SetVerticesDirty();
            yield return null;
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        boundPoints.Clear();
        verticesPoints.Clear();

        vertexAmount = vertexValues.Length;
        delta = Mathf.PI * 2 / vertexAmount;
        float x = 0, y = 0;
        vh.AddVert(Vector3.zero, Color.clear, new Vector2(0, 0));
        for (int i = 0; i < vertexAmount; i++)
        {
            //第一个点从Y轴开始 Start from axisY
            x = Mathf.Cos(i * delta);
            y = Mathf.Sin(i * delta);
            verticesPoints.Add(new Vector3(x * vertexValues[i] * maxValue, y * vertexValues[i] * maxValue));
            vh.AddVert(verticesPoints[i], color, new Vector2(x, y));

            if (i > 0)
            {
                vh.AddTriangle(0, i, i + 1);
            }
        }
        vh.AddTriangle(0, vertexAmount, 1);

        DrawBounds(vh);
        DrawVertexCircle(vh);
    }

    private void DrawBounds(VertexHelper vh)
    {
        float x = 0, y = 0, a = 0, b = 0;
        Vector3 center = new Vector3();
        Vector3 vertices = new Vector3();

        for (int i = 0; i < vertexAmount; i++)
        {
            //第一个点从Y轴开始 Start from axisY
            x = Mathf.Cos(i * delta) * maxValue;
            y = Mathf.Sin(i * delta) * maxValue;
            center.x = x;
            center.y = y;
            vertices.x = x + a;
            vertices.y = y + b;

            if (i > 0)  //线框第二个顶点处理
            {
                //加一份前个顶点的旋转量
                boundPoints.Add(new LineMesh(center,
                vertices,
                MyMath.LinePoint.RotateAroundPoint(vertices, center, Mathf.PI)));

                //两个顶点连线
                LineMesh.DrawLine(vh, boundPoints[i - 1], boundPoints[i], boundColor);
            }

            //新的旋转量
            a = Mathf.Cos(delta / 2 + delta * i) * boundWidth;
            b = Mathf.Sin(delta / 2 + delta * i) * boundWidth;
            vertices.x = x + a;
            vertices.y = y + b;

            if (i == 0)
            {
                boundPoints.Add(new LineMesh(center,
                    vertices,
                    MyMath.LinePoint.RotateAroundPoint(vertices, center, Mathf.PI)));
            }
            else
            {
                boundPoints[i].AddVertics(vertices,
                    MyMath.LinePoint.RotateAroundPoint(vertices, center, Mathf.PI));
            }

            if (i == vertexAmount - 1)
            {
                vertices.x = boundPoints[0].Center.x + a;
                vertices.y = boundPoints[0].Center.y + b;

                boundPoints[0].AddVertics(vertices,
                    MyMath.LinePoint.RotateAroundPoint(vertices, boundPoints[0].Center, Mathf.PI));
                LineMesh.DrawLine(vh, boundPoints[i], boundPoints[0], boundColor);
            }
        }
    }

    private void DrawVertexCircle(VertexHelper vh)
    {
        for (int i = 0; i < verticesPoints.Count; i++)
        {
            DrawCircle(vh, verticesPoints[i], i + 1);
        }
    }

    private void DrawCircle(VertexHelper vh, Vector3 vector3, int circleIndex = 0)
    {
        float x = 0, y = 0;
        Vector3 temp = new Vector3();
        for (int i = 0; i < MAX_CIRCLE_VERTICES_COUNT; i++)
        {
            x = Mathf.Cos(i * Mathf.PI * 2 / MAX_CIRCLE_VERTICES_COUNT) * vertexWidth;
            y = Mathf.Sin(i * Mathf.PI * 2 / MAX_CIRCLE_VERTICES_COUNT) * vertexWidth;

            temp.x = vector3.x + x;
            temp.y = vector3.y + y;

            vh.AddVert(temp, vertexColor, Vector2.zero);

            if (i > 0)
            {
                vh.AddTriangle(circleIndex, vh.currentVertCount - 1, vh.currentVertCount - 2);
            }
        }
        vh.AddTriangle(circleIndex, vh.currentVertCount - 1, vh.currentVertCount - MAX_CIRCLE_VERTICES_COUNT);
    }
}
