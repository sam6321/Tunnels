using System.Collections;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [System.Serializable]
    public class WeightedRandomTileEntry: WeightedRandomEntry<GameObject> {}

    [SerializeField]
    private Transform tileContainer;

    [SerializeField]
    private WeightedRandomTileEntry[] tilePrefabs;

    [SerializeField]
    private GameObject tileWallPrefab;

    [SerializeField]
    private Vector2 tilingSize = new Vector2(0.5f, 0.5f);

    [SerializeField]
    private Vector2Int size = new Vector2Int(10, 10);

    private WeightedRandom<GameObject> tileWeightedRandom;
    private GameObject[,] tiles;
    private ParticleSystem sharedOnDrillParticleSystem;
    private Transform fallOffTrigger;

    // Start is called before the first frame update
    void Start()
    {
        sharedOnDrillParticleSystem = GetComponent<ParticleSystem>();
        tileWeightedRandom = new WeightedRandom<GameObject>(tilePrefabs);
        fallOffTrigger = transform.Find("FallOffTrigger");
    }

    public void Generate()
    {
        tiles = new GameObject[size.x, size.y];
        for(int x = 0; x < size.x; x++)
        {
            for(int y = 0; y < size.y; y++)
            {
                GameObject tile;
                if(x == 0 || ((x == 0 || x == size.x - 1) && y == 0) || x == size.x - 1 || ((x == 0 || x == size.x - 1) && y == size.y - 1))
                {
                    tile = Instantiate(tileWallPrefab, tileContainer);
                }
                else
                {
                    tile = Instantiate(tileWeightedRandom.GetItem(), tileContainer);
                    tile.GetComponent<Tile>().OnDrillParticleSystem = sharedOnDrillParticleSystem;
                }
                
                tile.transform.position = new Vector3(x, -y) * tilingSize;
                SetTile(x, y, tile);
            }
        }

        // Place the fall of trigger under the map, and make sure it's the right size
        Vector2 centre = GetCentre();
        Vector2 extent = GetExtent();
        centre.y -= extent.y;
        extent.x *= 2.0f;
        fallOffTrigger.position = centre;
        fallOffTrigger.localScale = extent;	
    }

    public GameObject GetTile(int x, int y)
    {
        if(x < 0 || x >= size.x || y < 0 || y >= size.y)
            return null;
        return tiles[x,y];
    }

    public Vector2 GetCentre()
    {
        Vector2 offset = GetExtent();
        offset.y = -offset.y;
        return (Vector2)transform.position + offset / 2.0f;
    }

    public Vector2 GetExtent()
    {
        return ((size - Vector2.one) * tilingSize);
    }

    public Vector2 GetTileSize()
    {
        return tilingSize;
    }

    private void SetTile(int x, int y, GameObject tile)
    {
        tiles[x,y] = tile;
    }

    public Bounds GetSpawnArea()
    {
        return new Bounds()
        {
            center = new Vector2
            (
                transform.position.x + ((size.x - 1) * tilingSize.x) / 2.0f,
                transform.position.y + 2
            ),
            extents = new Vector2
            (
                ((size.x - 1) * tilingSize.x) / 2.0f,
                2
            )
        };
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            StartCoroutine(PlayerFallOff(collider));
        }
        else if(collider.gameObject.CompareTag("AI"))
        {
            StartCoroutine(AiFallOff(collider));
        }
        else
        {
           Debug.Log(collider.gameObject.tag);
        }
    }

    IEnumerator PlayerFallOff(Collider2D collider)
    {
        // player has hit the bottom, so let them fall then move them back up to the top for respawn
        PlayerMovement playerMovement = collider.GetComponent<PlayerMovement>();
        playerMovement.enabled = false;
        Camera.main.GetComponent<CameraMovement>().TransformTarget = null;

        yield return new WaitForSeconds(2.0f);

        playerMovement.enabled = true;
        GameObject.Find("GameManager").GetComponent<SpawnPositioner>().SpawnPlayer();
    }

    IEnumerator AiFallOff(Collider2D collider)
    {
        yield return new WaitForSeconds(2.0f);
        GameObject.Find("GameManager").GetComponent<SpawnPositioner>().SpawnAI();
    }

    public Vector2Int GetSize()
    {
        return size;
    }
}
