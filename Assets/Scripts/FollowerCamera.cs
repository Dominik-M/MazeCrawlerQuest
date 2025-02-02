using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerCamera : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    public float lerpSpeed = 10;
    public bool invertHorizontal = false, invertVertical = false;
    public float maxHorizontal = 2, maxVertical = 2;
    public Vector2 levelDimensions;

    private float horizontal, vertical;

    private void Update()
    {
        if (Input.GetButton("R3"))
        {
            horizontal = 0;
            vertical = 0;
            return;
        }

        // Horizontal Berechnung
        horizontal = Mathf.Clamp(
            Input.GetAxis("HorizontalAlt") * (invertHorizontal ? lerpSpeed : -lerpSpeed),
            -maxHorizontal,
            maxHorizontal
        );

        // Vertical Berechnung
        vertical = Mathf.Clamp(
            Input.GetAxis("VerticalAlt") * (invertVertical ? lerpSpeed : -lerpSpeed),
            -maxVertical,
            maxVertical
        );
    }


    void FixedUpdate()
    {
        Vector3 position = transform.position;
        Vector3 targetPosition = target.transform.position + offset;
        targetPosition.x += horizontal;
        targetPosition.z -= vertical;
        targetPosition.x = Mathf.Clamp(targetPosition.x, levelDimensions.x * -1, levelDimensions.x);
        targetPosition.z = Mathf.Clamp(targetPosition.z, levelDimensions.y * -1, levelDimensions.y);
        transform.position = Vector3.Lerp(position, targetPosition, Time.deltaTime * lerpSpeed);
    }
}
