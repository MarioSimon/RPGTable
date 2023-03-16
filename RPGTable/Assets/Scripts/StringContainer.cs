using Unity.Netcode;

public class StringContainer : INetworkSerializable
{
    public string SomeText;

    public StringContainer() { }

    public StringContainer(string text)
    {
        SomeText = text;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref SomeText);              
    }
}