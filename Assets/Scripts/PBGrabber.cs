using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PBGrabber : MonoBehaviour
{
    [SerializeField]
    private float strength;
    [SerializeField]
    private InputActionReference grabAction;
    [SerializeField]
    private InputActionReference moveGrabbedAction;
    [SerializeField]
    private ObjectHighlighter highlighter;

    private Rigidbody grabbedRb;
    private float initialDistance;

    private Vector3 force;

    private void Awake()
    {
        grabAction.action.actionMap.Enable();
        grabAction.action.performed += ProcessGrabbing;
        enabled = false;
    }

    private void FixedUpdate()
    {
        Vector3 force = transform.forward * initialDistance + transform.position - grabbedRb.position;
        grabbedRb.AddForce(force * strength);
    }

    private void ProcessGrabbing(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                if (context.ReadValueAsButton())
                {
                    TryGrabbing(); 
                }
                else
                {
                    ReleaseGrab();
                }
                break;
            default:
                break;
        }
    }

    private void ReleaseGrab()
    {
        enabled = false;
        grabbedRb = null;
    }

    private void TryGrabbing()
    {
        if (highlighter.HighlightedObject != null && highlighter.HighlightedObject.TryGetComponent(out Rigidbody rb))
        {
            enabled = true;
            initialDistance = Vector3.Distance(transform.position, rb.position);
            grabbedRb = rb;
        }
    }
}
