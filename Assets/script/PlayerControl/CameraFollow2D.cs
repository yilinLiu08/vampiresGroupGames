using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 5f;

    public Vector3 offset = new Vector3(0, 0, -10);



    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 targetPos = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            smoothSpeed * Time.deltaTime
        );
    }
}