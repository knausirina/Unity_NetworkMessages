using UnityEngine;

public class HelloClientHandler : ClientMessageHandlerBase<HelloMessage>
{
    protected override void OnReceive(HelloMessage message)
    {
        Debug.Log($"<color=green>[Client]</color> Received text: \"{message.Text}\"");
    }
}