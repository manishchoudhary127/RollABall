using UnityEngine;

// Put this on the ball. It rolls the ball with WASD / arrow keys,
// collects coins and ends the game when the ball hits an obstacle.
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 12f;   // how hard we push the ball

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // FixedUpdate is used for physics.
    void FixedUpdate()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            return; // no moving after the game ends
        }

        Vector2 input = GameInput.Move();
        Vector3 push = new Vector3(input.x, 0f, input.y);
        rb.AddForce(push * speed);
    }

    // Called when the ball enters a trigger (the coins are triggers).
    void OnTriggerEnter(Collider other)
    {
        Coin coin = other.GetComponent<Coin>();
        if (coin != null)
        {
            Destroy(other.gameObject);
            GameManager.Instance.CoinCollected();
        }
    }

    // Called when the ball bumps into something solid.
    void OnCollisionEnter(Collision collision)
    {
        Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
        if (obstacle != null)
        {
            rb.isKinematic = true; // stop the ball
            GameManager.Instance.GameOver();
        }
    }
}
