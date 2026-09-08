using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento (top-down, 4 ejes)")]
    public float moveSpeed = 6f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        if (moveInput.x > 0.01f && spriteRenderer != null)
            spriteRenderer.flipX = false;
        else if (moveInput.x < -0.01f && spriteRenderer != null)
            spriteRenderer.flipX = true;
    }

    void FixedUpdate()
    {
        Vector2 direccion = moveInput.normalized;
        rb.linearVelocity = direccion * moveSpeed;
    }
}