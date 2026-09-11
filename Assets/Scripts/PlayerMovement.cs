using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento (top-down, 4 ejes)")]
    public float moveSpeed = 6f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Vector2 moveInput;

    public Vector2 UltimoInput => moveInput;

    [Header("Animación")]
    public float animDampTime = 0.1f; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {

         if (anim != null)                            
        {
        anim.SetFloat("Horizontal", moveInput.x, animDampTime, Time.deltaTime);
        anim.SetFloat("Vertical", moveInput.y, animDampTime, Time.deltaTime);
        }    
    }

    void FixedUpdate()
    {
        Vector2 direccion = moveInput.normalized;
        rb.linearVelocity = direccion * moveSpeed;
    }
}