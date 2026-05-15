using System.Linq;
using System.Reflection;
using VContainer;
using VContainer.Unity;

public class NetworkLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        var networkManager = FindFirstObjectByType<CustomNetworkManager>();
        builder.RegisterComponent(networkManager);
        
        RegisterServices(builder);
        RegisterHandlers(builder);
        RegisterEntryPoints(builder);
    }

    private void RegisterEntryPoints(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<ClientController>().AsSelf();
        builder.RegisterEntryPoint<ServerController>();
    }

    private void RegisterServices(IContainerBuilder builder)
    {
        builder.Register<NetworkClientService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<NetworkServerService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
    }

    private void RegisterHandlers(IContainerBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var clientHandlers = assembly.GetTypes()
            .Where(t => typeof(IClientMessageHandler).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var handlerType in clientHandlers)
        {
            builder.Register(handlerType, Lifetime.Singleton).As<IClientMessageHandler>();
        }

        var serverHandlers = assembly.GetTypes()
            .Where(t => typeof(IServerSubscriptionHandler).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var handlerType in serverHandlers)
        {
            builder.Register(handlerType, Lifetime.Singleton).As<IServerSubscriptionHandler>();
        }
    }
}