using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalScorePanel : MonoBehaviour
{
    [System.Serializable]
    public class ScoreTextHolder
    {
        [SerializeField]
        public Text copper;

        [SerializeField]
        public Text iron;

        [SerializeField]
        public Text gold;

        [SerializeField]
        public Text platinum;

        [SerializeField]
        public Text diamond;

        [SerializeField]
        public Text finalScore;

        public void SetFromResources(PlayerResources resources)
        {
            copper.text = "X " + resources.GetResourceCount(ResourceType.Copper);
            iron.text = "X " + resources.GetResourceCount(ResourceType.Iron);
            gold.text = "X " + resources.GetResourceCount(ResourceType.Gold);
            platinum.text = "X " + resources.GetResourceCount(ResourceType.Platinum);
            diamond.text = "X " + resources.GetResourceCount(ResourceType.Diamond);
            finalScore.text = resources.Score.ToString();
        }
    }

    [SerializeField]
    private ScoreTextHolder playerScores;

    [SerializeField]
    private PlayerResources playerResources;

    [SerializeField]
    private ScoreTextHolder aiScores;

    [SerializeField]
    private PlayerResources aiResources;

    public void ShowScores()
    {
        playerScores.SetFromResources(playerResources);
        aiScores.SetFromResources(aiResources);

        gameObject.SetActive(true);
        gameObject.transform.localScale = new Vector3(0, 1, 1);
        GetComponent<Animator>().SetTrigger("appear");
    }

    public void HideScores()
    {
        GetComponent<Animator>().SetTrigger("disappear");
    }

    public void OnPlayAgainClicked()
    {
        HideScores();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnQuitClicked()
    {
        HideScores();
    }

    public void OnHideComplete()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}
