using UnityEngine;

public class SpawnPositioner : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject ai;

    [SerializeField]
    private CameraMovement movement;

    [SerializeField]
    private TileGenerator tileGenerator;

    [SerializeField]
    private CountdownTimer timer;

    private Rigidbody2D playerRigidBody;
    private SpriteRenderer playerSpriteRenderer;
    private PlayerDrill playerDrill;

    private bool firstPlayerSpawn = true;
    private bool spawningPlayer = false;

    public void SpawnPlayer()
    {
        Bounds spawnArea = tileGenerator.GetSpawnArea();

        playerRigidBody = player.GetComponent<Rigidbody2D>();
        playerRigidBody.simulated = false;

        playerDrill = player.GetComponent<PlayerDrill>();
        playerDrill.enabled = false;

        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();

        movement.TransformTarget = null;
        movement.PositionTarget = spawnArea.center;

        spawningPlayer = true;
    }

    public void SpawnAI()
    {
        Bounds spawnArea = tileGenerator.GetSpawnArea();
        Vector2 size = ai.GetComponent<SpriteRenderer>().size / 2.0f;
        ai.transform.position = new Vector2(
            Random.Range(spawnArea.min.x + size.x, spawnArea.max.x - size.x),
            Random.Range(spawnArea.min.y + size.y, spawnArea.max.y - size.y)
        );
        ai.SetActive(true);
    }

    void Update()
    {
        if(!spawningPlayer)
        {
            return;
        }

        Bounds spawnArea = tileGenerator.GetSpawnArea();
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
            spawningPlayer = false;
            if(firstPlayerSpawn)
            {
                firstPlayerSpawn = false;
                SpawnAI();
                timer.Restart();
            }
        }
    }
}
