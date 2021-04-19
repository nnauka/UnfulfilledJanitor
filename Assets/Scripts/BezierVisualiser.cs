using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BezierVisualiser : MonoBehaviour
{
    [SerializeField]
    private float width;
    [SerializeField]
    private int tInterval = 10;

    public Mesh ConstructSegmentedMesh(List<Vector3> corners)
    {
        var mesh = new Mesh();
        if (corners.Count < 3)
        {
            return mesh;
        }
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var bezierCorners = new List<Vector3>();
        for (int i = 2; i < corners.Count; i += 2)
        {
            bezierCorners.AddRange(GetQuadraticBezierPoints(corners[i - 2], corners[i - 1], corners[i], tInterval));
        }
        var offsets = new List<Vector3>();
        for (int i = 1; i < bezierCorners.Count; i++)
        {
            var direction = (bezierCorners[i - 1] - bezierCorners[i]).normalized;
            var perp = Vector3.Cross(direction, Vector3.up) * width;
            offsets.Add(perp);
        }
        offsets.Add(Vector3.Cross((bezierCorners[bezierCorners.Count - 2] - bezierCorners[bezierCorners.Count - 1]).normalized, Vector3.up) * width);
        for (int i = 0; i < bezierCorners.Count; i++)
        {
            if(bezierCorners[i] == bezierCorners[i] - offsets[i])
            {
                continue;
            }
            vertices.Add(bezierCorners[i] - offsets[i]);
            vertices.Add(bezierCorners[i] + offsets[i]);
        }
        for (int i = 0; i < vertices.Count - 2; i += 2)
        {
            triangles.Add(i);
            triangles.Add(i + 2);
            triangles.Add(i + 1);
            triangles.Add(i + 2);
            triangles.Add(i + 3);
            triangles.Add(i + 1);
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }

    public static Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * p0 + 2f * oneMinusT * t * p1 + t * t * p2;
    }

    public static List<Vector3> GetQuadraticBezierPoints(Vector3 p0, Vector3 p1, Vector3 p2, int count)
    {
        var points = new List<Vector3>();
        float interval = 1f / (count);
        float t = 0;
        for (int i = 0; i < count + 1; i++)
        {
            points.Add(QuadraticBezier(p0, p1, p2, t));
            t += interval;
        }

        return points;
    }
}
