using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaintSource : MonoBehaviour
{
    [SerializeField, Range(0,1)]
    private float initialIntensity = 1f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.TryGetComponent(out ITaintable taintable))
        {
            taintable.Taint(initialIntensity);
        }
    }

    public void SetIntensity(float intensity)
    {
        initialIntensity = intensity;
    }
}
