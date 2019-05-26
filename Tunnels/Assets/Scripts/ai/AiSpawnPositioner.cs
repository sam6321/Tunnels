using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSpawnPositioner : MonoBehaviour
{
    [SerializeField]
    private TileGenerator tileGenerator;

    [SerializeField]
    private GameObject player;

    private SpriteRenderer playerSpriteRenderer;

    private Bounds spawnArea;

    void OnEnable()
    {
        spawnArea = tileGenerator.GetSpawnArea();
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 size = playerSpriteRenderer.size / 2.0f;
        Vector2 newPos = new Vector2();
        newPos.x = Random.Range(spawnArea.min.x + size.x, spawnArea.max.x - size.x);
        newPos.y = Random.Range(spawnArea.min.y + size.y, spawnArea.max.y - size.y);
        player.transform.position = newPos;
        enabled = false;
    }
}
