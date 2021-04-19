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
        for (int i = 1; i < points.Count; i++)
        {
            Vector3 direction = (points[i].position - points[i - 1].position).normalized;
            var perp = Vector3.Cross(direction, Vector3.up);
            perp *= width;
            offsetsL.Add(points[i - 1].position + perp);
            offsetsR.Add(points[i - 1].position - perp);
        }
        for (int i = 1; i < BezierPoints.Count; i++)
        {
            Gizmos.DrawLine(BezierPoints[i - 1], BezierPoints[i]);
        }
        for (int i = 0; i < points.Count; i++)
        {
            if (i < offsetsL.Count)
            {
                Gizmos.DrawLine(offsetsL[i], points[i].position);
                Gizmos.DrawLine(points[i].position, offsetsR[i]);
            }
        }
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
        float interval = 1f / count;
        float t = 0;
        for (int i = 0; i < count; i++)
        {
            points.Add(QuadraticBezier(p0, p1, p2, t));
            t += interval;
        }
        points.Add(QuadraticBezier(p0, p1, p2, 1));
        return points;
    }
}
