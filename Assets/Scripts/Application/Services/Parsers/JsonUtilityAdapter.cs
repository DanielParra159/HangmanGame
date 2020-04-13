using Domain.Services.Parsers;
using UnityEngine;

namespace Application.Services.Parsers
{
    public class JsonUtilityAdapter : IJsonParser
    {
        public string ToJson<T>(T data)
        {
            return JsonUtility.ToJson(data);
        }

        public T FromJson<T>(string data)
        {
            return JsonUtility.FromJson<T>(data);
        }
    }
}