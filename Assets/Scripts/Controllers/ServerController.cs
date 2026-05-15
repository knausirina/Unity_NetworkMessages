using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using R3;
using VContainer.Unity;

public class ServerController : IStartable, IDisposable
{
    private readonly INetworkServerService _serverService;
    private readonly Dictionary<int, IServerSubscriptionHandler> _handlersMap;
    private readonly CompositeDisposable _disposable = new();

    public ServerController(
        INetworkServerService serverService,
        IReadOnlyList<IServerSubscriptionHandler> handlers)
    {
        _serverService = serverService;
        _handlersMap = handlers.ToDictionary(h => h.MessageTypeHash);
    }

    public void Start()
    {
        _serverService.OnClientSubscribed
            .Subscribe(ProcessSubscription)
            .AddTo(_disposable);
    }

    private void ProcessSubscription((int MessageTypeHash, NetworkConnectionToClient Connection) data)
    {
        if (_handlersMap.TryGetValue(data.MessageTypeHash, out var handler))
        {
            handler.HandleSubscription(data.Connection);
        }
    }

    public void Dispose() => _disposable.Dispose();
}