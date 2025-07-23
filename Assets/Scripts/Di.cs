using UnityEngine;
using VContainer;
using VContainer.Unity;

public class Di : LifetimeScope
{
    public static IObjectResolver instance;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<ActorsManager>();
        instance = Container;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        instance = null;
    }
}