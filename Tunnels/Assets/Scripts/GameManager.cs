using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TileGenerator tileGenerator;

    private SpawnPositioner spawnPositioner;

    void Start()
    {
        tileGenerator.Generate();
        spawnPositioner = GetComponent<SpawnPositioner>();
        spawnPositioner.SpawnPlayer(); // This also spawns the AI when the player clicks
    }

    public void OnTimerExpired(float startTime, float endTime)
    {
        // End game here

    }
}
