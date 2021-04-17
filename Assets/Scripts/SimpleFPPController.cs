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
    [Header("Constraints")]
    [SerializeField]
    private float minXRotation = -20;
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
        lookAction.action.actionMap.Enable();
        lookAction.action.performed += Look;
    }

    private void FixedUpdate()
    {
        Move(moveAction.action.ReadValue<Vector2>());
    }

    private void Move(Vector2 input)
    {
        charControl.SimpleMove(new Vector3(input.x, 0, input.y) * speed);
    }

    public void RotateCamera(Vector2 input)
    {
        input *= 0.1f;
        cam.transform.Rotate(-input.y, input.x, 0, Space.Self);
        var currentRot = cam.transform.localRotation;
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
                var input = context.ReadValue<Vector2>();
                RotateCamera(input);
                break;
            default:
                break;
        }
    }
}
