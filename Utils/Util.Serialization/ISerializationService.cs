namespace Util.Serialization
{
    public interface ISerializationService
    {
        T Deserialize<T>(string data);

        string Serialize<T>(T data);
    }
}
