using Mirror;

public interface IClientMessageHandler
{
    int MessageTypeHash { get; }
    void Handle(NetworkReaderPooled reader);
}