using CodeBase.SavedData;

namespace CodeBase._Services.PersistentGameData
{
    public interface IProgressSaver : IProgressLoader
    {
        void UpdateProgress(PlayerProgress progress);
    }
}