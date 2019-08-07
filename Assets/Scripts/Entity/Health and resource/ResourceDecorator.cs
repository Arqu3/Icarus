using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDecorator : BaseEntityResourceProvider, IResourceDecorator
{
    public BaseEntityResourceProvider provider { get; set; }

    public ResourceDecorator(BaseEntityResourceProvider provider)
    {
        this.provider = provider;
    }

    public override float GetCurrent()
    {
        return provider.GetCurrent();
    }

    public override float GetMax()
    {
        return provider.GetMax();
    }

    public override float GetPercentage()
    {
        return GetCurrent() / GetMax();
    }

    public override void Give(float amount)
    {
        provider.Give(amount);
    }

    public override void GivePercentage(float percentage)
    {
        provider.GivePercentage(percentage);
    }

    public override bool Spend(float amount)
    {
        return provider.Spend(amount);
    }

    public override bool SpendPercentage(float percentage)
    {
        return provider.SpendPercentage(percentage);
    }

    public override void Update()
    {
        provider.Update();
    }
}
