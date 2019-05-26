using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 1.0f;

    private new CircleCollider2D collider;
    private new Rigidbody2D rigidbody;
    private Vector2 movement = new Vector2();
    private int groundedMask;
    private SpriteRenderer spriteRenderer;
    private List<TileNode> path;
    public AiPathPlanner planner;
    public Vector2 tileSize;
    
    void Start()
    {
        path = new List<TileNode>();
        collider = GetComponent<CircleCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        groundedMask = ~LayerMask.GetMask("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        var tileGenerator = GameObject.Find("Tiles").GetComponent<TileGenerator>();
        tileSize = tileGenerator.GetTileSize();
        planner = new AiPathPlanner(tileGenerator);
    }

    public TileNode NextNode()
    {
        return path.FirstOrDefault();
    }

    void Update()
    {
        TileNode node = new TileNode();
        node.x = (int)Mathf.Round(transform.position.x / tileSize.x);
        node.y = (int)Mathf.Round(-transform.position.y / tileSize.y);
        path = planner.GetAiPath(node);

        TileNode target = path.FirstOrDefault();
        movement.x = target == null ? 0 : (float)target.x * tileSize.x - transform.position.x;

        //Set to 0.1 for the AI to be boring
        if(movement.x > 0.001)
        {
            movement.x = 1;
        }
        else if(movement.x < -0.001)
        {
            movement.x = -1;
        }
        else
        {
            movement.x = 0;
        }

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
