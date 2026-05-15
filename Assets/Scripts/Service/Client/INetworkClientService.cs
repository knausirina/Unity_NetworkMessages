using R3;

public interface INetworkClientService
{
    Observable<UniversalMessageWrapper> OnRawMessageReceived { get; }
    void SubscribeToServer(int messageTypeHash);
    Observable<Unit> OnConnected { get; }
}