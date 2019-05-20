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
            int current = resources.GetResource(type);
            resources.SetResource(type, current + amount);
        }
    }
}
