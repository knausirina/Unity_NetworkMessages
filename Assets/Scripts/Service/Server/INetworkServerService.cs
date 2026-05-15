using R3;
using Mirror;

public interface INetworkServerService
{
    void SendToSubscribed<T>(T message) where T : struct, NetworkMessage;
    Observable<(int MessageTypeHash, Mirror.NetworkConnectionToClient Connection)> OnClientSubscribed { get; }
}