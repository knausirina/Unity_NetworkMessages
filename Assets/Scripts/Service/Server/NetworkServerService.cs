using System;
using System.Collections.Generic;
using Mirror;
using R3;
using UnityEngine;

public class NetworkServerService : INetworkServerService, IDisposable
{
    private readonly Dictionary<int, HashSet<NetworkConnectionToClient>> _subscriptions = new();
    private readonly Subject<(int, NetworkConnectionToClient)> _onClientSubscribedSubject = new();
    
    public Observable<(int MessageTypeHash, NetworkConnectionToClient Connection)> OnClientSubscribed => _onClientSubscribedSubject;
    
    public void RegisterServerHandler()
    {
        NetworkServer.RegisterHandler<SubscribeRequestMessage>(OnSubscribeRequestFromClient);
    }

    private void OnSubscribeRequestFromClient(NetworkConnectionToClient conn, SubscribeRequestMessage msg)
    {
        int hash = msg.MessageTypeHash;

        if (!_subscriptions.ContainsKey(hash))
            _subscriptions[hash] = new HashSet<NetworkConnectionToClient>();

        _subscriptions[hash].Add(conn);
        
        Debug.Log($"[Server] Client {conn.connectionId} subscribed to hash: {hash}");
        
        _onClientSubscribedSubject.OnNext((hash, conn));
    }

    public void SendToSubscribed<T>(T message) where T : struct, NetworkMessage
    {
        int hash = typeof(T).Name.GetHashCode();
        
        if (!_subscriptions.TryGetValue(hash, out var connections)) return;

        using (NetworkWriterPooled writer = NetworkWriterPool.Get())
        {
            writer.Write(message);
            var payload = writer.ToArraySegment();
            
            var wrapper = new UniversalMessageWrapper { TypeHash = hash, Payload = payload };

            foreach (var connection in connections)
            {
                if (connection != null && connection.isReady) 
                    connection.Send(wrapper);
            }
        }
    }

    public void Dispose()
    {
        _onClientSubscribedSubject.Dispose();
        _subscriptions.Clear();
    }
}