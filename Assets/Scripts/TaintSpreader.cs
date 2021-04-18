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
    private float taintIntensity;
    private float distance;
    private Vector3 prevPosition;

    public void Taint(float intensity)
    {
        if (intensity > taintIntensity)
        {
            taintIntensity = intensity;
            enabled = taintIntensity > float.Epsilon; 
        }
    }

    private void FixedUpdate()
    {
        distance += Vector3.SqrMagnitude(transform.position - prevPosition);
        prevPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.TryGetComponent<ITaintable>(out ITaintable taintable) && distance > taintDistanceInterval)
        {
            distance = 0f;
            LeaveTaint(collision.GetContact(0).point, collision.transform);
            taintable.Taint(taintIntensity);
            taintIntensity -= intensityReduction;
            enabled = taintIntensity > float.Epsilon;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }

    protected virtual void LeaveTaint(Vector3 position, Transform taintetObject)
    {
        var taint = Instantiate(taintObject, position, transform.rotation, taintetObject);
        taint.SetIntensity(taintIntensity);
    }
}
