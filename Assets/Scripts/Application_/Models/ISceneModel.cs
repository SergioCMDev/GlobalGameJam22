namespace Application_.Models
{
    public interface ISceneModel
    {
        public string NextScene { get; set; }
        public string PreviousScene{ get; set; }
        public string LoadingScene{ get; set; }
    
    }
}