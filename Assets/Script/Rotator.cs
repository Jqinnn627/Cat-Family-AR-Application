using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 20f;

    private Vector2 lastTouchPosition;
    private bool isDragging = false;

    private void Update() {
#if UNITY_EDITOR //for testing in editor
        if (Input.GetMouseButtonDown(0))
        {
            lastTouchPosition = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
        else if (isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastTouchPosition;
            transform.Rotate(Vector3.up, -delta.x * rotationSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.right, delta.y * rotationSpeed * Time.deltaTime, Space.World);
            lastTouchPosition = Input.mousePosition;
        }
#else //for mobile
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 delta = touch.position - lastTouchPosition;
                transform.Rotate(Vector3.up, -delta.x * rotationSpeed * Time.deltaTime, Space.World);
                transform.Rotate(Vector3.right, delta.y * rotationSpeed * Time.deltaTime, Space.World);
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
            }
        }
#endif
    }
}
