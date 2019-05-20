using System.Linq;
using UnityEngine;

[System.Serializable]
public class WeightedRandomEntry<T>
{
    public T item;
    public int weight;
}

class WeightedRandom<T>
{
    private int totalWeight = 0;
    private WeightedRandomEntry<T>[] items;

    public WeightedRandom(params WeightedRandomEntry<T>[] items)
    {
        totalWeight = items.Sum(item => item.weight);
        this.items = items;
    }

    public T GetItem()
    {
        int weight = Random.Range(0, totalWeight);
        foreach(WeightedRandomEntry<T> entry in items)
        {
            if(weight < entry.weight)
            {
                return entry.item;
            }
            weight -= entry.weight;
        }
        Debug.Log("Should not be here");
        return items[0].item;
    }
}
