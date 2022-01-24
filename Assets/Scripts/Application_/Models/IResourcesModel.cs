namespace Application_.Models
{
    public interface IResourcesModel
    {
        float Gold { get; set; }
        float Metal { get; set; }
        void AddResources(RetrievableResourceType type, float quantity);
    }
}