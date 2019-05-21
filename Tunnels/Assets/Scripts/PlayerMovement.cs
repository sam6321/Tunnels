using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 1.0f;

    private new CircleCollider2D collider;
    private new Rigidbody2D rigidbody;
    private Vector2 movement = new Vector2();
    private int groundedMask;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        collider = GetComponent<CircleCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        groundedMask = ~LayerMask.GetMask("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        if (movement.x != 0)
        {
            spriteRenderer.flipX = movement.x < 0;
        }
    }

    void FixedUpdate()
    {
        if(movement.sqrMagnitude > 0 && IsGrounded())
        {
            rigidbody.AddForce(movement * movementSpeed * Time.fixedDeltaTime);
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, collider.radius, Vector2.down, 0.05f, groundedMask);
        return hit.collider != null;
    }
}
