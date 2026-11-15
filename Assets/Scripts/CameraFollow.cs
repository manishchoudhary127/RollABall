using UnityEngine;

// Put this on the camera. It keeps the same distance from the ball.
public class CameraFollow : MonoBehaviour
{
    public Transform target;                         // the ball
    public Vector3 offset = new Vector3(0f, 9f, -8f);

    // LateUpdate runs after the ball has moved.
    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        transform.position = target.position + offset;
        transform.LookAt(target);
    }
}
