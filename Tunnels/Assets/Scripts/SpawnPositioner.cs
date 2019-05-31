using UnityEngine;

public class SpawnPositioner : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorial;

    [SerializeField]
    private GameObject bottomTutorial;

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

    private PlayerMovement playerMovement;
    private Rigidbody2D playerRigidBody;
    private SpriteRenderer playerSpriteRenderer;
    private PlayerDrill playerDrill;

    private Rigidbody2D aiRigidBody;
    private AiMovement aiMovement;
    private AiDrill aiDrill;

    private bool firstPlayerSpawn = true;
    private bool secondPlayerSpawn = false;
    private bool spawningPlayer = false;

    void Start()
    {
        playerRigidBody = player.GetComponent<Rigidbody2D>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerDrill = player.GetComponent<PlayerDrill>();
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();

        aiRigidBody = ai.GetComponent<Rigidbody2D>();
        aiMovement = ai.GetComponent<AiMovement>();
        aiDrill = ai.GetComponent<AiDrill>();
    }

    public void FreezePlayer()
    {
        playerRigidBody.simulated = false;
        playerMovement.enabled = false;
        playerDrill.enabled = false;
    }

    public void UnfreezePlayer()
    {
        playerRigidBody.simulated = true;
        playerMovement.enabled = true;
        playerDrill.enabled = true;
    }

    public void FreezeAI()
    {
        aiRigidBody.simulated = false;
        aiMovement.enabled = false;
        aiDrill.enabled = false;
    }

    public void UnfreezeAI()
    {
        aiRigidBody.simulated = true;
        aiMovement.enabled = true;
        aiDrill.enabled = true;
    }

    public void SpawnPlayer()
    {
        Bounds spawnArea = tileGenerator.GetSpawnArea();

        FreezePlayer();
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
            UnfreezePlayer();
            movement.TransformTarget = player.transform;
            movement.PositionTarget = null;

            spawningPlayer = false;
            if(firstPlayerSpawn)
            {
                firstPlayerSpawn = false;
                secondPlayerSpawn = true;
                tutorial.SetActive(false);
                SpawnAI();
                timer.Restart();
            }
            else if(secondPlayerSpawn)
            {
                bottomTutorial.SetActive(false);
                secondPlayerSpawn = false;
            }
        }
    }
}
