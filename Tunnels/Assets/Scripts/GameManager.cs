using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TileGenerator tileGenerator;

    [SerializeField]
    private Animator timerAnimator;

    [SerializeField]
    private FinalScorePanel scorePanel;

    private SpawnPositioner spawnPositioner;

    void Start()
    {
        tileGenerator.Generate();
        spawnPositioner = GetComponent<SpawnPositioner>();
        spawnPositioner.SpawnPlayer(); // This also spawns the AI when the player clicks
    }

    public void OnTimerExpired(float startTime, float endTime)
    {
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        // Beep and flash the timer a few times
        timerAnimator.SetTrigger("flash");
        // disable player and AI movement
        spawnPositioner.FreezePlayer();
        spawnPositioner.FreezeAI();
        // start beep sound
        yield return new WaitForSeconds(2.0f);

        // show score panel
        scorePanel.ShowScores();
    }
}
