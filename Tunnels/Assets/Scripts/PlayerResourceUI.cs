using UnityEngine;
using UnityEngine.UI;

public class PlayerResourceUI : MonoBehaviour
{
    [SerializeField]
    private Text scoreDisplay;

    [SerializeField]
    private PlayerResources resources;

    [SerializeField]
    private RectTransform baseDrillSpeed;

    [SerializeField]
    private RectTransform oilDrillSpeed;

    private float baseDrillSpeedStartSize;
    private float oilDrillSpeedStartSize;

    void Start()
    {
        baseDrillSpeedStartSize = baseDrillSpeed.sizeDelta.x;
        oilDrillSpeedStartSize = oilDrillSpeed.sizeDelta.x;
    }

    public void OnScoreModified(int score)
    {
        scoreDisplay.text = "x " + score;
    }

    private void Update()
    {
        Vector2 size = baseDrillSpeed.sizeDelta;
        size.x = resources.DrillBaseSpeed * 2.0f * baseDrillSpeedStartSize;
        baseDrillSpeed.sizeDelta = size;

        size = oilDrillSpeed.sizeDelta;
        size.x = resources.OilSpeed * 2.0f * oilDrillSpeedStartSize;
        oilDrillSpeed.sizeDelta = size;
    }
}
