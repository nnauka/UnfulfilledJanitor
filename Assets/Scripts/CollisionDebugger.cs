using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDebugger : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Enter collision with: {collision.collider.name}");
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log($"Exit collision with: {collision.collider.name}");
    }
}
