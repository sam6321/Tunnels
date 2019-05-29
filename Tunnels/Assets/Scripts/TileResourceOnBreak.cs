using UnityEngine;

public class TileResourceOnBreak : MonoBehaviour
{
    [SerializeField]
    private int amount;

    [SerializeField]
    private ResourceType type;

    [SerializeField]
    private GameObject onBreakPopup;

    void OnTileBreak(DrillAttackInfo info)
    {
        PlayerResources resources = info.attacker.GetComponent<PlayerResources>();
        if (resources)
        {
            int current = resources.GetResourceCount(type);
            int score = resources.Score;
            resources.SetResourceCount(type, current + amount);
            int newCount = resources.GetResourceCount(type);

            if (current != newCount)
            {
                // Changed, so need popup text
                Vector3 position = Camera.main.WorldToScreenPoint(transform.position);
                Transform parent = GameObject.Find("Canvas").transform;
                GameObject popup = Instantiate(onBreakPopup, position, Quaternion.identity, parent);
                ScoreText text = popup.GetComponent<ScoreText>();
                switch (type)
                {
                    case ResourceType.Oil:
                        text.Text = "Oil!";
                        break;
                    default:
                        text.Text = "+" + (resources.Score - score);
                        break;
                }
            }
        }
    }

    public ResourceType GetResourceType()
    {
        return type;
    }
}
