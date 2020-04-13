namespace Domain.Services.Parsers
{
    public interface IJsonParser
    {
        string ToJson<T>(T data);
        T FromJson<T>(string data);
    }
}