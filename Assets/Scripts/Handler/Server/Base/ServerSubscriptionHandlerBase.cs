using Mirror;

public abstract class ServerSubscriptionHandlerBase<T> : IServerSubscriptionHandler where T : struct, NetworkMessage
{
    public int MessageTypeHash { get; } = typeof(T).Name.GetHashCode();
    
    protected readonly INetworkServerService ServerService;

    protected ServerSubscriptionHandlerBase(INetworkServerService serverService)
    {
        ServerService = serverService;
    }

    public abstract void HandleSubscription(NetworkConnectionToClient connection);
}