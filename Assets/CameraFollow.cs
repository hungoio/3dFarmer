using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 pos = Vector3.Lerp(
            transform.position,
            target.position,
            followSpeed * Time.deltaTime
        );

        transform.position = pos;
    }
}
