using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class SimpleFPPController : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;
    [SerializeField]
    private float mouseSens = 0.1f;
    [Header("Constraints")]
    [SerializeField]
    private float minXRotation = -30;
    [SerializeField]
    private float maxXRotation = 15;
    [Header("References")]
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private CharacterController charControl;
    [Header("Inputs")]
    [SerializeField]
    private InputActionReference lookAction;
    [SerializeField]
    private InputActionReference moveAction;

    private Vector2? moveDirection;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lookAction.action.actionMap.Enable();
        lookAction.action.performed += Look;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Cursor.visible)
        {
            return;
        }
#endif
        Move(moveAction.action.ReadValue<Vector2>());
    }

    private void Move(Vector2 input)
    {
        var movex = charControl.transform.forward * input.y;
        var movez = charControl.transform.right * input.x;
        charControl.SimpleMove((movex + movez) * speed);
    }

    public void RotateCamera(Vector2 input)
    {
        cam.transform.Rotate(-input.y, 0, 0, Space.Self);
        charControl.transform.Rotate(0, input.x, 0, Space.Self);
        var currentRot = cam.transform.rotation;
        var xRot = currentRot.eulerAngles.x;
        if (xRot > 180)
        {
            xRot -= 360;
        }
        currentRot.eulerAngles = new Vector3(Mathf.Clamp(xRot, minXRotation, maxXRotation), currentRot.eulerAngles.y, 0);
        cam.transform.rotation = currentRot;
    }

    private void Look(CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                var input = context.ReadValue<Vector2>() * mouseSens;
                RotateCamera(input);
                break;
            default:
                break;
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Debug.DrawLine(cam.transform.position, transform.forward * 5f + transform.position, Color.red);
    }
#endif
}
