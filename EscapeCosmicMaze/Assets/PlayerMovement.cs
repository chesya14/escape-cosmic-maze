using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;

    void Update()
    {
        // Input dari keyboard (WASD / Panah)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // Gerakin fisik karakter
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}