using CodeBase._Services.SaveLoad;
using CodeBase._Services.StaticData;
using CodeBase.SavedData;

namespace CodeBase._Services.PersistentGameData
{
    public class PersistentProgress : IService
    {
        private readonly StaticDataService _staticData;
        private LoadProgressService _loadProgress;
        public PlayerProgress Progress { get; set; }

        public PersistentProgress(LoadProgressService loadProgress, StaticDataService staticData)
        {
            _loadProgress = loadProgress;
            _staticData = staticData;
            Progress = _loadProgress.Load() ?? NewProgress();
        }

        private PlayerProgress NewProgress()
        {
            PlayerProgress progress = new PlayerProgress(_staticData.GetPlayerData().StartMoney);
            return progress;
        }
    }
}
