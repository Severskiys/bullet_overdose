using CodeBase.SavedData;

namespace CodeBase._Services.PersistentGameData
{
  public interface IProgressLoader
  {
    void LoadProgress(PlayerProgress progress);
  }
}