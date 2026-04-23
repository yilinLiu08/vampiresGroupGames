using UnityEngine;
using UnityEngine.InputSystem;

public class TitleBackgroundParallax : MonoBehaviour
{
    public float parallaxStrength = 20f;
    public float smoothSpeed = 5f;

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
            originalPosition = rectTransform.anchoredPosition;
        else
            originalPosition = transform.position;
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        float moveX = (mousePos.x / Screen.width - 0.5f) * 2f;
        float moveY = (mousePos.y / Screen.height - 0.5f) * 2f;

        Vector3 offset = new Vector3(moveX, moveY, 0) * parallaxStrength;
        targetPosition = originalPosition + offset;

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(
                rectTransform.anchoredPosition,
                targetPosition,
                Time.deltaTime * smoothSpeed
            );
        }
        else
        {
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                Time.deltaTime * smoothSpeed
            );
        }
    }
}