using Mirror;

public interface IServerSubscriptionHandler
{
    int MessageTypeHash { get; }
    void HandleSubscription(NetworkConnectionToClient connection);
}