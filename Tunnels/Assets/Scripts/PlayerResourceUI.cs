using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResourceUI : MonoBehaviour
{
    [SerializeField]
    private Text[] resourceTextValues;

    public void OnResourcesUpdated(ResourceType type, int count, PlayerResources resources)
    {
        resourceTextValues[(int)type].text = "x " + count;
    }
}
