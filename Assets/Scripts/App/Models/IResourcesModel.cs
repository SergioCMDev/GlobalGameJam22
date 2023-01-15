using App.Resources;

namespace App.Models
{
    public interface IResourcesModel
    {
        int Gold { get; set; }
        int Metal { get; set; }
        void AddResources(RetrievableResourceType type, int quantity);
        void OverrideResources(RetrievableResourceType type, int quantity);
    }
}