using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LinePoint
{
    public Vector3 Center { get; private set; }
    public List<Vector3> Vertices { get; private set; }

    public LinePoint() { }

    public LinePoint(Vector3 center)
    {
        Center = center;
    }

    public LinePoint(Vector3 center, params Vector3[] vertices)
    {
        Center = center;
        Vertices = new List<Vector3>();

        foreach (var vertic in vertices)
        {
            Vertices.Add(vertic);
        }
    }

    public void AddVertics(params Vector3[] vertices)
    {
        foreach (var vertic in vertices)
        {
            Vertices.Add(vertic);
        }
    }
    public static void CalculateLinePoint(Vector3 center, float width)
    {

    }

    public static Vector3 RotateAroundOrigin(Vector3 point, float angle)
    {
        return RotateAroundPoint(point, Vector3.zero, angle);
    }
    public static Vector3 RotateAroundPoint(Vector3 point, Vector3 aroundPoint, float angle)
    {
        return new Vector3(
            (point.x - aroundPoint.x) * Mathf.Cos(angle) - (point.y - aroundPoint.y) * Mathf.Sin(angle) + aroundPoint.x,
            (point.y - aroundPoint.y) * Mathf.Cos(angle) + (point.x - aroundPoint.x) * Mathf.Sin(angle) + aroundPoint.y);
    }

    public static void DrawLine(UnityEngine.UI.VertexHelper vh, LinePoint start, LinePoint end,Color color)
    {
        int startCount = start.Vertices.Count;
        int endCount = end.Vertices.Count;

        vh.AddVert(start.Vertices[startCount - 2], color, new Vector2(0, 0));   //vh.currentVertCount - 4
        vh.AddVert(start.Vertices[startCount - 1], color, new Vector2(0, 0));   //vh.currentVertCount - 3
        vh.AddVert(end.Vertices[endCount - 2], color, new Vector2(0, 0));       //vh.currentVertCount - 2
        vh.AddVert(end.Vertices[endCount - 1], color, new Vector2(0, 0));       //vh.currentVertCount - 1

        vh.AddTriangle(vh.currentVertCount - 1, vh.currentVertCount - 2, vh.currentVertCount - 3);
        vh.AddTriangle(vh.currentVertCount - 3, vh.currentVertCount - 2, vh.currentVertCount - 4);
    }
}

