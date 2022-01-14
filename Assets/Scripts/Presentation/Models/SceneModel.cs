public class SceneModel : ISceneModel
{
    public string NextScene { get; set; }
    public string PreviousScene { get; set; }
    public string LoadingScene { get; set; }

    public SceneModel()
    {
        LoadingScene = "Loading";
    }
}