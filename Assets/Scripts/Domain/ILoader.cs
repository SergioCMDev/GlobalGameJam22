using App;

namespace Domain
{
    public interface ILoader
    {
        Savegame LoadGame();
        bool HasSavedGame();
    }
}