using Mirror;

public abstract class ClientMessageHandlerBase<T> : IClientMessageHandler where T : struct, NetworkMessage
{
    public int MessageTypeHash { get; } = typeof(T).Name.GetHashCode();

    public void Handle(NetworkReaderPooled reader)
    {
        T message = reader.Read<T>();
        OnReceive(message);
    }

    protected abstract void OnReceive(T message);
}