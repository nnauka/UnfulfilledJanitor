using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaintSource : MonoBehaviour
{
    [SerializeField, Range(0,1)]
    private float initialIntensity = 1f;
    [SerializeField]
    private Renderer renderer;

    public Renderer Renderer { get => renderer; }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.TryGetComponent(out ITaintable taintable))
        {
            taintable.Taint(initialIntensity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ITaintable taintable))
        {
            taintable.Taint(initialIntensity);
        }
    }

    public void SetIntensity(float intensity)
    {
        initialIntensity = intensity;
        var colour = Renderer.material.color;
        colour.a = Mathf.Clamp01(intensity);
        Renderer.material.color = colour;
    }
}
