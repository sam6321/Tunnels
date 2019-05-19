﻿using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform tileContainer;

    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private Vector2 tilingSize = new Vector2(0.5f, 0.5f);

    [SerializeField]
    private Vector2Int size = new Vector2Int(10, 10);

    private GameObject[] tiles;
    private ParticleSystem sharedOnDrillParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        sharedOnDrillParticleSystem = GetComponent<ParticleSystem>();
        Generate();
    }

    void Generate()
    {
        tiles = new GameObject[size.x * size.y];
        for(int x = 0; x < size.x; x++)
        {
            for(int y = 0; y < size.y; y++)
            {
                GameObject tile = Instantiate(tilePrefab, tileContainer);
                tile.transform.position = new Vector3(x, -y) * tilingSize;
                tile.GetComponent<Tile>().OnDrillParticleSystem = sharedOnDrillParticleSystem;
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
