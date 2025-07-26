using UnityEngine;
using VContainer;
using VContainer.Unity;

[RequireComponent(typeof(GameManager))]
public class Di : LifetimeScope
{
    public static IObjectResolver instance;
    
    [SerializeField]
    private GameManager gameManager;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(gameManager);
        instance = Container;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        instance = null;
    }
}