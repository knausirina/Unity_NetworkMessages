using Mirror;

public class HelloServerHandler : ServerSubscriptionHandlerBase<HelloMessage>
{
    private const string Hello = "Hello";
    
    public HelloServerHandler(INetworkServerService serverService) : base(serverService) { }

    public override void HandleSubscription(NetworkConnectionToClient connection)
    {
        ServerService.SendToSubscribed(new HelloMessage { Text = Hello });
    }
}