using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaintSpreader : MonoBehaviour, ITaintable
{
    [SerializeField, Range(0,1f)]
    private float intensityReduction = 0.1f;
    [SerializeField]
    private float taintDistanceInterval = 100f;
    [SerializeField]
    private TaintSource taintObject;
    [SerializeField]
    private LayerMask taintRaycastMask;
    private float taintIntensity;
    private float distance;
    private Vector3 prevPosition;

    private void Start()
    {
        enabled = false;
    }

    public void Taint(float intensity)
    {
        if (intensity > taintIntensity)
        {
            taintIntensity = intensity;
            enabled = taintIntensity > float.Epsilon; 
        }
    }

    private void Update()
    {
        distance += Vector3.Magnitude(transform.position - prevPosition);
        prevPosition = transform.position;
        if(distance > taintDistanceInterval && Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 4, taintRaycastMask))
        {
            // Don't spread taint on taint...
            if (!hit.transform.TryGetComponent(out TaintSource source))
            {
                distance = 0f;
                LeaveTaint(hit.point, hit.transform);
                taintIntensity -= intensityReduction;
                enabled = taintIntensity > float.Epsilon; 
            }
        }
    }

    protected virtual void LeaveTaint(Vector3 position, Transform taintedObject)
    {
        var taint = Instantiate(taintObject, taintedObject, true);
        taint.SetIntensity(taintIntensity);
        taint.transform.position = position + Vector3.up * 0.01f;
    }
}
