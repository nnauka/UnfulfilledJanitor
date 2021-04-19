using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierVisualiser : MonoBehaviour
{
    [SerializeField]
    private List<Transform> points;
    [SerializeField]
    private float width;
    [SerializeField]
    private float tInterval = 0.1f;

    private void OnDrawGizmos()
    {
        var BezierPoints = new List<Vector3>();
        var offsetsL = new List<Vector3>();
        var offsetsR = new List<Vector3>();
        for (int i = 2; i < points.Count; i++)
        {
            BezierPoints = GetQuadraticBezierPoints(points[0].position, points[1].position, points[2].position, 10);
        }
        for (int i = 1; i < BezierPoints.Count; i++)
        {
            Vector3 direction = (BezierPoints[i] - BezierPoints[i - 1]).normalized;
            var perp = Vector3.Cross(direction, Vector3.up);
            perp *= width;
            offsetsL.Add(BezierPoints[i - 1] + perp);
            offsetsR.Add(BezierPoints[i - 1] - perp);
        }
        for (int i = 1; i < BezierPoints.Count; i++)
        {
            //Gizmos.DrawLine(BezierPoints[i - 1], BezierPoints[i]);
        }
        for (int i = 0; i < BezierPoints.Count; i++)
        {
            if (i < offsetsL.Count)
            {
                Gizmos.DrawLine(offsetsL[i], BezierPoints[i]);
                Gizmos.DrawLine(BezierPoints[i], offsetsR[i]);
            }
        }
        for (int i = 1; i < offsetsL.Count; i++)
        {
            Gizmos.DrawLine(offsetsL[i - 1], offsetsL[i]);
            Gizmos.DrawLine(offsetsR[i - 1], offsetsR[i]);
        }
    }

    private Mesh ConstructSegmentedMesh(List<Vector3> corners)
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
            bezierCorners.AddRange(GetQuadraticBezierPoints(corners[0], corners[1], corners[2], 10));
        }
        var offsets = new List<Vector3>();
        for (int i = 1; i < bezierCorners.Count; i++)
        {
            var direction = (bezierCorners[i-1] - bezierCorners[i]).normalized;
            var perp = Vector3.Cross(direction, Vector3.up) * width;
            offsets.Add(perp);
        }
        offsets.Add(Vector3.Cross((bezierCorners[bezierCorners.Count - 1] - bezierCorners[bezierCorners.Count - 2]).normalized, Vector3.up) * width);
        for (int i = 0; i < bezierCorners.Count; i++)
        {
            vertices.Add(bezierCorners[i] - offsets[i]);
            vertices.Add(bezierCorners[i] + offsets[i]);
        }
        for (int i = 0; i < vertices.Count-2; i++)
        {
            triangles.Add(i);
            triangles.Add(i+2);
            triangles.Add(i+1);
            triangles.Add(i+2);
            triangles.Add(i+3);
            triangles.Add(i+1);
        }
        mesh.RecalculateNormals();
        return mesh;
    }

    public static Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return //Quadratic formula =>  B(t) = (1 - t)^2 P0 + 2 (1 - t) t P1 + t^2 P2
            oneMinusT * oneMinusT * p0 + 2f * oneMinusT * t * p1 + t * t * p2;
    }

    public static List<Vector3> GetQuadraticBezierPoints(Vector3 p0, Vector3 p1, Vector3 p2, int count)
    {
        var points = new List<Vector3>();
        float interval = 1f / (count - 1);
        float t = 0;
        for (int i = 0; i < count + 1; i++)
        {
            points.Add(QuadraticBezier(p0, p1, p2, t));
            t += interval;
        }

        return points;
    }
}
