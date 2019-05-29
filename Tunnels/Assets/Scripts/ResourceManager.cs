using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum ResourceType
{
    Copper = 0,
    Iron,
    Silver,
    Gold,
    Platinum,
    Diamond,

    Oil
}

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

public class ResourceManager
{
    // Resources stored in order so they can be accessed by enum value directly
    public Resource[] resources;

    // Resources that combine to form the players score
    public ScoreResource[] scoreResources;

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

    public ResourceManager(Resource.OnModify scoreModify, Resource.OnModify drillModify)
    {
        scoreResources = new ScoreResource[]
        {
            new ScoreResource(ResourceType.Copper,   1, scoreModify),
            new ScoreResource(ResourceType.Iron,     2, scoreModify),
            new ScoreResource(ResourceType.Silver,   3, scoreModify),
            new ScoreResource(ResourceType.Gold,     4, scoreModify),
            new ScoreResource(ResourceType.Platinum, 6, scoreModify),
            new ScoreResource(ResourceType.Diamond,  8, scoreModify),
        };

        Resource[] drillSpeedResources = new Resource[]
        {
            new Resource(ResourceType.Oil, drillModify, amount: 100, max: 100)
        };

        resources = JoinArrays(scoreResources, drillSpeedResources);
    }
}
