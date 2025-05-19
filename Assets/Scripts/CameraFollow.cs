using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float smoothSpeed = 0.125f;

    private Vector3 shakeOffset = Vector3.zero; // Offset untuk efek shake

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector2 desiredPosition = (Vector2)target.position;
        Vector2 smoothedPosition = Vector3.Lerp((Vector2)transform.position, desiredPosition, smoothSpeed);

        transform.position = new Vector3(smoothedPosition.x + shakeOffset.x, 
                                         smoothedPosition.y + shakeOffset.y, 
                                         -10);
    }

    public void ApplyShake(Vector3 offset)
    {
        shakeOffset = offset;
    }
}
