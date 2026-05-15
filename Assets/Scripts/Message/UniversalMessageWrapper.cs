using Mirror;
using System;

public struct UniversalMessageWrapper : NetworkMessage
{
    public int TypeHash;
    public ArraySegment<byte> Payload; 
}