using UnityEngine;
using UnityEngine.UI;

public class PlayerResourceUI : MonoBehaviour
{
    [SerializeField]
    private Text scoreDisplay;

    // private something fuelDisplay

    public void OnResourcesUpdated(ResourceType type, int count, PlayerResources resources)
    {
        scoreDisplay.text = "x " + resources.Score;
    }
}
