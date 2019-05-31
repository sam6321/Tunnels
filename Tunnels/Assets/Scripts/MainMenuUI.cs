using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private GameObject attributions;

    public void OnStartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnAttributions()
    {
        attributions.SetActive(!attributions.activeSelf);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
