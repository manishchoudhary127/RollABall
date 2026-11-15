using UnityEngine;

// Put this on every coin. It just spins the coin.
public class Coin : MonoBehaviour
{
    public float spinSpeed = 120f; // degrees per second

    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
    }
}
