using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class ScoreModifiedEvent : UnityEvent<int> { }

public class PlayerResources : MonoBehaviour
{
    [SerializeField]
    private ScoreModifiedEvent onScoreModified;
    public ScoreModifiedEvent OnScoreModified { get => onScoreModified; }

    private ResourceManager resourceManager;

    private int score = 0;
    public int Score => score;

    private float drillSpeed = 1.0f;
    public float DrillSpeed => drillSpeed;

    private float drillBaseSpeed = 0.5f;
    public float DrillBaseSpeed => drillBaseSpeed;

    private float oilSpeed = 0.5f;
    public float OilSpeed => oilSpeed;

    void Start()
    {
        resourceManager = new ResourceManager(InternalOnScoreResourceModified, InternalOnDrillResourceModified);
    }

    public void SetResourceCount(ResourceType type, int count)
    {
        GetResource<Resource>(type).Amount = count;
    }

    public int GetResourceCount(ResourceType type)
    {
        return GetResource<Resource>(type).Amount;
    }

    private T GetResource<T>(ResourceType type) where T : Resource
    {
        return resourceManager.resources[(int)type] as T;
    }

    private void InternalOnScoreResourceModified()
    {
        score = resourceManager.scoreResources.Sum(r => r.Score);
        onScoreModified.Invoke(score);
    }

    private void InternalOnDrillResourceModified()
    {
        // Drill base speed is 50% + 5% bonus for each iron the player has, up to 100% at 20 iron
        drillBaseSpeed = Mathf.Min(0.5f + GetResourceCount(ResourceType.Iron) * 0.05f, 1.0f);
        // Oil bonus speed is 0.5% per oil, increasing drill speed up to 100%
        oilSpeed = Mathf.Min(GetResourceCount(ResourceType.Oil) / 200.0f, 1.0f - drillBaseSpeed);
        drillSpeed = drillBaseSpeed + oilSpeed;
    }
}
