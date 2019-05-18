using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void OnStartGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
