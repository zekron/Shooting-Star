using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IPointerTest : MonoBehaviour
{
    [SerializeField] private float boundWidth = 0.5f;
    [SerializeField] private float maxValue = 5;
    [SerializeField, Range(3, 36)] private int verticAmount = 4;
    [SerializeField] private MeshFilter filter;
    [SerializeField] private List<LineMesh> linePoints = new List<LineMesh>();

    private float delta;
    private void Awake()
    {
    }
    private void OnValidate()
    {
        linePoints.Clear();
        Draw();
    }
    [ContextMenu("TestDraw")]
    private void Draw()
    {
        Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        material.SetColor("_BaseColor", Color.red);

        GetComponent<Renderer>().material = material;
        delta = Mathf.PI * 2 / verticAmount;
        using (VertexHelper vh = new VertexHelper())
        {
            vh.Clear();
            float x = 0, y = 0, a = 0, b = 0;
            for (int i = 0; i < verticAmount; i++)
            {
                //第一个点从Y轴开始 Start from axisY
                x = Mathf.Cos(i * delta) * maxValue;
                y = Mathf.Sin(i * delta) * maxValue;

                if (i > 0)  //线框第二个顶点处理
                {
                    //加一份前个顶点的旋转量
                    linePoints.Add(new LineMesh(new Vector3(x, y),
                    new Vector3(x + a, y + b),
                    MyMath.LinePoint.RotateAroundPoint(new Vector3(x + a, y + b), new Vector3(x, y), Mathf.PI)));

                    //两个顶点连线
                    LineMesh.DrawLine(vh, linePoints[i - 1], linePoints[i], Color.green);
                }

                //新的旋转量
                a = Mathf.Cos(delta / 2 + delta * i) * boundWidth;
                b = Mathf.Sin(delta / 2 + delta * i) * boundWidth;

                if (i == 0)
                {
                    linePoints.Add(new LineMesh(new Vector3(x, y),
                        new Vector3(x + a, y + b),
                        MyMath.LinePoint.RotateAroundPoint(new Vector3(x + a, y + b), new Vector3(x, y), Mathf.PI)));
                }
                else
                {
                    linePoints[i].AddVertics(new Vector3(x + a, y + b),
                        MyMath.LinePoint.RotateAroundPoint(new Vector3(x + a, y + b), new Vector3(x, y), Mathf.PI));
                }

                if (i == verticAmount - 1)
                {
                    linePoints[0].AddVertics(new Vector3(linePoints[0].Center.x + a, linePoints[0].Center.y + b),
                        MyMath.LinePoint.RotateAroundPoint(new Vector3(linePoints[0].Center.x + a, linePoints[0].Center.y + b), linePoints[0].Center, Mathf.PI));
                    LineMesh.DrawLine(vh, linePoints[i], linePoints[0], Color.green);
                }
            }

            Mesh mesh = new Mesh();
            mesh.name = "Quad";
            vh.FillMesh(mesh);
            filter.mesh = mesh;
        }
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < linePoints.Count; i++)
        {
            Debug.DrawRay(linePoints[i].Center, Vector3.right, Color.red);
            Debug.DrawRay(linePoints[i].Center, Vector3.up, Color.green);
        }
    }
}

