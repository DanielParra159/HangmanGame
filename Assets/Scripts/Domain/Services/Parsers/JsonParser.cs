namespace Domain.Services.Parsers
{
    public interface JsonParser
    {
        string ToJson<T>(T data);
        T FromJson<T>(string data);
    }
}