using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawnOnBreak : MonoBehaviour
{
    [SerializeField]
    private GameObject[] onBreakObjects;

    private void OnTileBreak(DrillAttackInfo info)
    {
        foreach(GameObject onBreakObject in onBreakObjects)
        {
            Instantiate(onBreakObject, transform.position, Quaternion.identity);
        }
    }
}
