namespace Application_.Models
{
    public class ResourcesModel : IResourcesModel
    {
        public float Gold { get; set; }
        public float Metal { get; set; }

        public ResourcesModel()
        {
            Gold = 200;
            Metal = 200;
        }

        public void AddResources(RetrievableResourceType type, float quantity)
        {
            //TODO USING SOLID
        }
    }
}