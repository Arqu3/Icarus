using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealthProvider : IEntityHealthProvider
{
    public EntityHealthProvider(int startAmount)
    {
        Current = Max = startAmount;
    }

    public int Current { get; private set; }
    public float Percentage => (float)Current / Max;
    public int Max { get; private set; }

    public void Remove(int amount)
    {
        int absClamped = Mathf.Clamp(Mathf.Abs(amount), 0, Max);
        Current = Mathf.Clamp(Current - absClamped, 0, Max);
    }

    public void RemovePercentage(float percentage)
    {
        Remove((int)(Max * Mathf.Clamp(Mathf.Abs(percentage), 0f, 1f)));
    }

    public void Give(int amount)
    {
        Current = Mathf.Clamp(Current + Mathf.Abs(amount), 0, Max);
    }

    //public void Update()
    //{
    //    Current = Mathf.Clamp(Current + Time.deltaTime, 0f, Max);
    //}
}
