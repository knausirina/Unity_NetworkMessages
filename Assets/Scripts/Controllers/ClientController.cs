using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using R3;
using VContainer.Unity;

public class ClientController : IStartable, IDisposable
{
    private readonly INetworkClientService _clientService;
    private readonly Dictionary<int, IClientMessageHandler> _handlersMap;
    private readonly CompositeDisposable _disposable = new();

    public ClientController(
        INetworkClientService clientService,
        IReadOnlyList<IClientMessageHandler> handlers)
    {
        _clientService = clientService;
        _handlersMap = handlers.ToDictionary(h => h.MessageTypeHash);
    }

    public void Start()
    {
        _clientService.OnRawMessageReceived
            .Subscribe(DispatchMessage)
            .AddTo(_disposable);

        _clientService.OnConnected
            .Subscribe(_ => SendSubscriptionRequests())
            .AddTo(_disposable);
    }
    
    private void DispatchMessage(UniversalMessageWrapper wrapper)
    {
        if (_handlersMap.TryGetValue(wrapper.TypeHash, out var handler))
        {
            using (NetworkReaderPooled reader = NetworkReaderPool.Get(wrapper.Payload))
            {
                handler.Handle(reader);
            }
        }
    }

    private void SendSubscriptionRequests()
    {
        foreach (var handlerHash in _handlersMap.Keys)
        {
            _clientService.SubscribeToServer(handlerHash);
        }
    }

    public void Dispose() => _disposable.Dispose();
}