using Unity.Netcode;

public class StringContainer : INetworkSerializable
{
    public string SomeText;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref SomeText);              
    }
}