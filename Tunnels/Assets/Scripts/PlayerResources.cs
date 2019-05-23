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
    Diamonds,

    Iron,
    Oil
}

public class PlayerResources : MonoBehaviour
{
    private static ResourceType[] scoringResource =
    {
        ResourceType.Copper,
        ResourceType.Silver,
        ResourceType.Gold,
        ResourceType.Platinum,
        ResourceType.Diamonds
    };

    [Serializable]
    public class ResourceCountModifedEvent : UnityEvent<ResourceType, int, PlayerResources> { }

    [Serializable]
    public class ResourceEntry
    {
        public ResourceType type;
        public int multiplier;
        public int count;

        public int Score { get => multiplier * count; }
    }

    [SerializeField]
    private ResourceEntry[] resources = new ResourceEntry[Enum.GetNames(typeof(ResourceType)).Length];

    [SerializeField]
    private ResourceCountModifedEvent onResourceCountModified;
    public ResourceCountModifedEvent OnResourceCountModified { get => onResourceCountModified; }

    public int Score { get => scoringResource.Sum(GetResourceScore); }

    void Start()
    {
        Array.Sort(resources, (a, b) => a.type - b.type);
    }

    public void SetResourceCount(ResourceType type, int count)
    {
        GetResource(type).count = count;
        onResourceCountModified.Invoke(type, count, this);
    }

    public int GetResourceCount(ResourceType type)
    {
        return GetResource(type).count;
    }

    public int GetResourceMultiplier(ResourceType type)
    {
        return GetResource(type).multiplier;
    }

    public int GetResourceScore(ResourceType type)
    {
        return GetResource(type).Score;
    }

    private ResourceEntry GetResource(ResourceType type)
    {
        return resources[(int)type];
    }
}
