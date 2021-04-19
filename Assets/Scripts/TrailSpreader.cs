using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailSpreader : MonoBehaviour, ITaintable
{
    private const float ZFightPreventionOffset = 0.01f;

    [SerializeField]
    private BezierVisualiser bezier;
    [SerializeField]
    private float maxTrailLength;
    [SerializeField]
    private MeshFilter basePrefab;

    private Collider currentCollider;
    private float distance;
    private float intenisty;
    private Vector3 prevPosition;
    private List<Vector3> currentTrailCorners = new List<Vector3>();

    private void Start()
    {
        enabled = false;
    }

    private void FixedUpdate()
    {
        var currentPosition = transform.position;
        distance += Vector3.SqrMagnitude(currentPosition - prevPosition);
        if (distance > maxTrailLength)
        {
            currentTrailCorners.Add(currentCollider.ClosestPointOnBounds(currentPosition));
        }
        prevPosition = currentPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out ITaintable taintable) && collision.transform.CompareTag("Ground") && intenisty > float.Epsilon)
        {
            currentCollider = collision.collider;
            enabled = true;
            currentTrailCorners.Add(collision.GetContact(0).point); 
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.TryGetComponent(out ITaintable taintable) && collision.transform.CompareTag("Ground") && enabled)
        {
            enabled = false;
            LeaveTaint(transform.position, collision.transform);
            currentTrailCorners.Clear();
        }
    }

    public void LeaveTaint(Vector3 position, Transform taintedObject)
    {
        var filter = Instantiate(basePrefab, taintedObject, true);
        filter.mesh = bezier.ConstructSegmentedMesh(currentTrailCorners);
        var renderer = filter.GetComponent<Renderer>();
        var colour = renderer.sharedMaterial.color;
        colour.a = intenisty;
        renderer.material.color = colour;
        intenisty -= 0.1f;
        distance = 0;
        AlignWithSurfaceBelow(filter.transform);
    }

    public void AlignWithSurfaceBelow(Transform item)
    {
        if (Physics.Raycast(item.position + Vector3.up, Vector3.down, out RaycastHit hit, 4))
        {
            item.rotation = Quaternion.FromToRotation(item.up, hit.normal) * item.rotation;
            item.position = hit.point;
            item.position += new Vector3(0, ZFightPreventionOffset, 0);
        }
    }

    public void Taint(float intensity)
    {
        this.intenisty = intensity;
    }
}
