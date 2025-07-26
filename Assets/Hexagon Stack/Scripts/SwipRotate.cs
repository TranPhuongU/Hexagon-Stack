using UnityEngine;

public class SwipeRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f; // Tốc độ xoay

    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool isSwiping = false;

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isSwiping = true;
            startTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isSwiping = false;
        }

        if (isSwiping)
        {
            currentTouchPosition = Input.mousePosition;
            float deltaX = currentTouchPosition.x - startTouchPosition.x;

            transform.Rotate(Vector3.up, deltaX * rotationSpeed * Time.deltaTime);

            startTouchPosition = currentTouchPosition;
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            isSwiping = true;
            startTouchPosition = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved && isSwiping)
        {
            currentTouchPosition = touch.position;
            float deltaX = currentTouchPosition.x - startTouchPosition.x;

            transform.Rotate(Vector3.up, deltaX * rotationSpeed * Time.deltaTime);

            startTouchPosition = currentTouchPosition;
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            isSwiping = false;
        }
    }
}
