using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TileGenerator tileGenerator;

    [SerializeField]
    private Animator timerAnimator;

    [SerializeField]
    private FinalScorePanel scorePanel;

    private AudioSource alarmSound;
    private SpawnPositioner spawnPositioner;

    void Start()
    {
        alarmSound = GetComponent<AudioSource>();
        spawnPositioner = GetComponent<SpawnPositioner>();

        tileGenerator.Generate();
        spawnPositioner.SpawnPlayer(); // This also spawns the AI when the player clicks
    }

    public void OnTimerExpired(float startTime, float endTime)
    {
        StartCoroutine(EndGame());
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    IEnumerator EndGame()
    {
        // Beep and flash the timer a few times
        timerAnimator.SetTrigger("flash");
        alarmSound.Play();

        // disable player and AI movement
        spawnPositioner.FreezePlayer();
        spawnPositioner.FreezeAI();
        spawnPositioner.enabled = false;

        yield return new WaitForSeconds(4.0f);

        // show score panel
        scorePanel.ShowScores();
    }
}
