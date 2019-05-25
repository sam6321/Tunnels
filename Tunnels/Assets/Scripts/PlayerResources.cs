using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum ResourceType
{
    Copper = 0,
    Silver,
    Gold,
    Platinum,
    Diamond,

    Iron,
    Oil
}

[Serializable]
public class ScoreModifiedEvent : UnityEvent<int> { }

public class Resource
{
    public delegate void OnModify();

    public readonly ResourceType type;
    private int amount;
    private int max;
    private OnModify onModify;

    public virtual int Amount
    {
        get => amount;
        set
        {
            amount = Mathf.Min(value, max);
            onModify();
        }
    }

    public Resource(ResourceType type, OnModify onModify, int amount=0, int max=int.MaxValue)
    {
        this.type = type;
        this.amount = amount;
        this.max = max;
        this.onModify = onModify;
    }
}

public class ScoreResource : Resource
{
    private int multiplier;

    public int Score => multiplier * Amount;

    public ScoreResource(ResourceType type, int multiplier, OnModify onModify) : base(type, onModify)
    {
        this.multiplier = multiplier;
    }
}

public class PlayerResources : MonoBehaviour
{
    static T[] JoinArrays<T>(params T[][] values)
    {
        T[] array = new T[values.Sum(v => v.Length)];

        int loadPosition = 0;
        foreach(T[] inArray in values)
        {
            Array.Copy(inArray, 0, array, loadPosition, inArray.Length);
            loadPosition += inArray.Length;
        }

        return array;
    }

    [SerializeField]
    private ScoreModifiedEvent onScoreModified;
    public ScoreModifiedEvent OnScoreModified { get => onScoreModified; }

    // Resources stored in order so they can be accessed by enum value directly
    private Resource[] resources;

    // Resources that combine to form the players score
    private ScoreResource[] scoreResources;

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
        scoreResources = new ScoreResource[]
        {
            new ScoreResource(ResourceType.Copper,   1, InternalOnScoreResourceModified),
            new ScoreResource(ResourceType.Silver,   2, InternalOnScoreResourceModified),
            new ScoreResource(ResourceType.Gold,     3, InternalOnScoreResourceModified),
            new ScoreResource(ResourceType.Platinum, 6, InternalOnScoreResourceModified),
            new ScoreResource(ResourceType.Diamond, 8, InternalOnScoreResourceModified),
        };

        Resource[] drillSpeedResources = new Resource[]
        {
            new Resource(ResourceType.Iron, InternalOnDrillResourceModified),
            new Resource(ResourceType.Oil, InternalOnDrillResourceModified, amount: 100, max: 100)
        };

        resources = JoinArrays(scoreResources, drillSpeedResources);
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
        return resources[(int)type] as T;
    }

    private void InternalOnScoreResourceModified()
    {
        score = scoreResources.Sum(r => r.Score);
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
