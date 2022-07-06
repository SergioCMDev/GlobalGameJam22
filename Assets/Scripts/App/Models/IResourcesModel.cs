namespace App.Models
{
    public interface IResourcesModel
    {
        int Gold { get; set; }
        int Metal { get; set; }
        void AddResources(RetrievableResourceType type, int quantity);
    }
}