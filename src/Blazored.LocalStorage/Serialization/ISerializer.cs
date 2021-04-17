namespace Blazored.LocalStorage.Serialization
{
    public interface ISerializer
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string text);
    }
}
