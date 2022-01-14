using UnityEngine;

namespace Domain
{
    public class JsonUtililyTransformer : IJsonator
    {
        public string ToJson(object objectToJson)
        {
            return JsonUtility.ToJson(objectToJson);
        }

        public T FromJson<T>(string objectToJson)
        {
            return JsonUtility.FromJson<T>(objectToJson);
        }
    }
}