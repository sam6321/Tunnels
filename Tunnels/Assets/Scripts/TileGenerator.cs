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
    private GameObject[] tiles;
    private ParticleSystem sharedOnDrillParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        sharedOnDrillParticleSystem = GetComponent<ParticleSystem>();
        tileWeightedRandom = new WeightedRandom<GameObject>(tilePrefabs);
        Generate();
    }

    void Generate()
    {
        tiles = new GameObject[size.x * size.y];
        for(int x = 0; x < size.x; x++)
        {
            for(int y = 0; y < size.y; y++)
            {
                GameObject tile;
                if(x == 0 || ((x == 0 || x == size.x - 1) && y == 0) || x == size.x - 1 || y == size.y - 1)
                {
                    tile = Instantiate(tileWallPrefab, tileContainer);
                }
                else
                {
                    tile = Instantiate(tileWeightedRandom.GetItem(), tileContainer);
                    tile.GetComponent<Tile>().OnDrillParticleSystem = sharedOnDrillParticleSystem;
                }
                
                tile.transform.position = new Vector3(x, -y) * tilingSize;
            }
        }
    }

    GameObject GetTile(int x, int y)
    {
        return tiles[x + y * size.y];
    }

    void SetTile(int x, int y, GameObject tile)
    {
        tiles[x + y * size.y] = tile;
    }
}
