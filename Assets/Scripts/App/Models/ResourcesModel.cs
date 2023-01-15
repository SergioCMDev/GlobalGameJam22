using App.Resources;

namespace App.Models
{
    public class ResourcesModel : IResourcesModel
    {
        public int Gold { get; set; }
        public int Metal { get; set; }

        public ResourcesModel()
        {
            Gold = 200;
            Metal = 200;
        }

        public void AddResources(RetrievableResourceType type, int quantity)
        {
            Gold += quantity;
            //TODO USING SOLID
        }

        public void OverrideResources(RetrievableResourceType type, int quantity)
        {
            Gold = quantity;
        }
    }
}