using Mirror;
using VContainer;

public class CustomNetworkManager : NetworkManager
{
    private NetworkClientService _clientService;
    private NetworkServerService _serverService;

    [Inject]
    public void Construct(
        NetworkClientService clientService, 
        NetworkServerService serverService)
    {
        _clientService = clientService;
        _serverService = serverService;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        _serverService.RegisterServerHandler();
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        _clientService.RegisterClientHandler();
        _clientService.NotifyConnected();
    }
}