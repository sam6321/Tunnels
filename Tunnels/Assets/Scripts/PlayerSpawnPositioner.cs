using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPositioner : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private CameraMovement movement;

    [SerializeField]
    private TileGenerator tileGenerator;

    private Bounds spawnArea;
    private Rigidbody2D playerRigidBody;
    private SpriteRenderer playerSpriteRenderer;
    private PlayerDrill playerDrill;

    void OnEnable()
    {
        spawnArea = tileGenerator.GetSpawnArea();

        playerRigidBody = player.GetComponent<Rigidbody2D>();
        playerRigidBody.simulated = false;

        playerDrill = player.GetComponent<PlayerDrill>();
        playerDrill.enabled = false;

        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();

        movement.TransformTarget = null;
        movement.PositionTarget = spawnArea.center;
    }

    void Update()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 size = playerSpriteRenderer.size / 2.0f;
        worldPosition.x = Mathf.Clamp(worldPosition.x, spawnArea.min.x + size.x, spawnArea.max.x - size.x);
        worldPosition.y = Mathf.Clamp(worldPosition.y, spawnArea.min.y + size.y, spawnArea.max.y - size.y);

        player.transform.position = worldPosition;

        if(Input.GetButtonDown("Fire1"))
        {
            // Spawn the player here
            playerRigidBody.simulated = true;
            playerDrill.enabled = true;
            movement.TransformTarget = player.transform;
            movement.PositionTarget = null;
            enabled = false;
        }
    }
}
