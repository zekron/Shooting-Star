using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer), typeof(RectTransform))]
[AddComponentMenu("UI/Model Renderer", 1000)]
public class UIModelRenderer : UIBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private Material currentMaterial;
    [SerializeField] private Texture2D currentTexture;

    private Mesh mesh;
    private CanvasRenderer canvasRenderer;

    protected override void Awake()
    {
        //CreateRenderer();
        base.Awake();
    }

    protected override void OnValidate()
    {
        CreateRenderer();
        base.OnValidate();
    }

    private void SetCenter(Vector3[] vector3Array, Vector3 center)
    {
        Vector3 pos;
        for (int i = 0; i < vector3Array.Length; i++)
        {
            pos = vector3Array[i] - center;
            vector3Array[i] = pos;
        }
    }

    private Mesh CreateMesh()
    {
        Mesh result = new Mesh();

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0,0),
            new Vector3(0,100),
            new Vector3(100,100),
            new Vector3(100,0),
        };
        SetCenter(vertices, new Vector3(50, 50, 50));

        List<UIVertex> uIVertices = new List<UIVertex>
        {
            new UIVertex{position = vertices[0],color = Color.white,uv0 = new Vector2(0,0)},
            new UIVertex{position = vertices[1],color = Color.white,uv0 = new Vector2(0,1)},
            new UIVertex{position = vertices[2],color = Color.white,uv0 = new Vector2(1,1)},
            new UIVertex{position = vertices[3],color = Color.white,uv0 = new Vector2(1,0)},
        };

        List<int> indexs = new List<int>
        {
            0,1,2,
            0,2,3
        };

        using (VertexHelper vh = new VertexHelper())
        {
            vh.AddUIVertexStream(uIVertices, indexs);
            vh.FillMesh(result);
        }

        result.RecalculateBounds();
        return result;
    }

    private void SetRenderer(Mesh mesh)
    {
        if (!mesh)
        {
            return;
        }
        canvasRenderer = GetComponent<CanvasRenderer>();
        if (!canvasRenderer)
        {
            return;
        }
        canvasRenderer.Clear();
        canvasRenderer.cullTransparentMesh = false;
        canvasRenderer.cull = false;
        canvasRenderer.SetMesh(mesh);
        canvasRenderer.SetMaterial(currentMaterial, currentTexture);
        canvasRenderer.SetColor(Color.white);
    }

    private Mesh GetModelMesh()
    {
        if (!model) return null;
        MeshFilter filter = model.GetComponent<MeshFilter>();

        if (!filter) return null;

        Mesh modelMesh = filter.sharedMesh;

        Vector3[] vers = modelMesh.vertices;
        for (int i = 0; i < vers.Length; i++)
        {
            vers[i] = vers[i] * 100;
        }

        Mesh result = new Mesh();

        result.SetVertices(vers);
        result.SetTriangles(modelMesh.triangles, 0);
        result.SetUVs(0, modelMesh.uv);
        result.SetNormals(modelMesh.normals);
        result.SetTangents(modelMesh.tangents);
        result.SetColors(result.colors);

        return result;
    }

    private void CreateRenderer()
    {
        mesh = GetModelMesh();

        SetRenderer(mesh);
    }
}
