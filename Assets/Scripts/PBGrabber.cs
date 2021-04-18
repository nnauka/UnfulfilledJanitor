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
    private Vector3 initialDistance;

    private void Awake()
    {
        grabAction.action.actionMap.Enable();
        grabAction.action.performed += ProcessGrabbing;
        enabled = false;
    }

    private void FixedUpdate()
    {
        var distanceFromObject = ((transform.position - grabbedRb.transform.position) - initialDistance) * strength;
        Vector3 force = (Vector3)(transform.forward * (-moveGrabbedAction.action.ReadValue<Vector2>() * strength)) + distanceFromObject;
        Debug.LogError($"Force: {force}");
        grabbedRb.AddForce(force);
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
                Debug.LogError($"Input Action {context.phase}");
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
            initialDistance = transform.position - rb.transform.position;
            grabbedRb = rb;
        }
    }
}
