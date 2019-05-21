using System;
using UnityEngine;
using UnityEngine.Events;

public enum ResourceType
{
    Iron = 0,
    Gold,
    Diamonds,
    Oil
}

public class PlayerResources : MonoBehaviour
{
    [Serializable]
    public class ResourceCountModifedEvent : UnityEvent<ResourceType, int, PlayerResources> { }

    private int[] resourceCounts = new int[Enum.GetNames(typeof(ResourceType)).Length];

    [SerializeField]
    private ResourceCountModifedEvent onResourceCountModified;
    public ResourceCountModifedEvent OnResourceCountModified { get => onResourceCountModified; }

    public void SetResource(ResourceType type, int count)
    {
        resourceCounts[(int)type] = count;
        onResourceCountModified.Invoke(type, count, this);
    }

    public int GetResource(ResourceType type)
    {
        return resourceCounts[(int)type];
    }
}
