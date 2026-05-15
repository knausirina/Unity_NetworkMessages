using System;
using Mirror;
using R3;

public class NetworkClientService : INetworkClientService, IDisposable
{
    private readonly Subject<UniversalMessageWrapper> _onMessageSubject = new();
    public Observable<UniversalMessageWrapper> OnRawMessageReceived => _onMessageSubject;

    private readonly Subject<Unit> _onConnectedSubject = new();
    public Observable<Unit> OnConnected => _onConnectedSubject;

    public void SubscribeToServer(int messageTypeHash)
    {
        if (NetworkClient.isConnected)
        {
            NetworkClient.Send(new SubscribeRequestMessage { MessageTypeHash = messageTypeHash });
        }
    }

    public void RegisterClientHandler()
    {
        NetworkClient.ReplaceHandler<UniversalMessageWrapper>(OnUniversalMessageReceived);
    }

    public void NotifyConnected() => _onConnectedSubject.OnNext(Unit.Default);

    private void OnUniversalMessageReceived(UniversalMessageWrapper wrapper) => _onMessageSubject.OnNext(wrapper);

    public void Dispose()
    {
        _onMessageSubject.Dispose();
        _onConnectedSubject.Dispose();
    }
}