public interface IJsonator
{
    string ToJson(object objectToJson);
    T FromJson<T>(string objectToJson);
}