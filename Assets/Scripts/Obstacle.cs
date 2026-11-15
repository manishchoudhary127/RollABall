using UnityEngine;

// Put this on every obstacle. Touching it ends the game.
// If moveDistance is more than 0 the obstacle slides left and right.
public class Obstacle : MonoBehaviour
{
    public float moveDistance = 0f; // 0 = does not move
    public float moveSpeed = 2f;

    private Vector3 startPosition;
    private Rigidbody rb;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (moveDistance <= 0f || rb == null)
        {
            return;
        }

        // PingPong goes 0 -> max -> 0 again and again.
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance * 2f) - moveDistance;
        rb.MovePosition(startPosition + new Vector3(offset, 0f, 0f));
    }
}
