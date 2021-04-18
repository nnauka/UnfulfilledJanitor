using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHighlighter : MonoBehaviour
{
    public event Action<Transform> OnObjectHighlight;

    [SerializeField]
    private float range;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private Camera cam;

    private Transform highlightedObject;
    private RaycastHit highlightedObjectHitInfo;

    public RaycastHit HighlightedObjectHitInfo { get => highlightedObjectHitInfo; }
    public Transform HighlightedObject { get => highlightedObject; }

    private void Awake()
    {
        OnObjectHighlight += LogObjectName;
    }

    private void LogObjectName(Transform obj)
    {
        if(obj != null)
        {
            Debug.Log($"Highlight {obj.name}");
        }
    }

    private void LateUpdate()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, range, mask))
        {
            if(hit.transform != highlightedObject)
            {
                highlightedObject = hit.transform;
                highlightedObjectHitInfo = hit;
                OnObjectHighlight?.Invoke(highlightedObject);
            }
        }
        else
        {
            highlightedObject = null;
            OnObjectHighlight?.Invoke(highlightedObject);
        }
    }

    private Transform LookForObject()
    {
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, range, mask))
        {
            highlightedObjectHitInfo = hit;
            return hit.transform;
        }
        return null;
    }
}
