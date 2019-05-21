using System;
using UnityEngine;

[Serializable]
public class Cooldown
{
    [SerializeField]
    private float frequency;
    private float last = 0.0f;

    public Cooldown(float frequency)
    {
        this.frequency = frequency;
    }

    public bool Check(float time)
    {
        if (time >= last + frequency)
        {
            last = time;
            return true;
        }
        return false;
    }
}