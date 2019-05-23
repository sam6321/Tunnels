using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileResourceOnBreak : MonoBehaviour
{
    [SerializeField]
    private int amount;

    [SerializeField]
    private ResourceType type;

    void OnTileBreak(DrillAttackInfo info)
    {
        PlayerResources resources = info.attacker.GetComponent<PlayerResources>();
        if (resources)
        {
            int current = resources.GetResourceCount(type);
            resources.SetResourceCount(type, current + amount);
        }
    }
}
