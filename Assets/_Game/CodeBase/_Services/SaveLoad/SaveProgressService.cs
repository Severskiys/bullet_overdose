using System.Collections.Generic;
using System.Linq;
using CodeBase._Services.PersistentGameData;
using CodeBase.Infrastructure.Factory;
using CodeBase.SavedData;
using UnityEngine;

namespace CodeBase._Services.SaveLoad
{
    public class SaveProgressService : IService
    {
        private readonly PersistentProgress _progress;
        private List<ISaversHolder> _saversHolders;

        public SaveProgressService(PersistentProgress progress, IEnumerable<ISaversHolder> saversHolders)
        {
            _saversHolders = saversHolders.ToList();
            _progress = progress;
        }

        public void Save()
        {
            foreach (var saversHolder in _saversHolders)
                foreach (var saver in saversHolder.ProgressSavers)
                    saver.UpdateProgress(_progress.Progress);

            string progress = _progress.Progress.ToJson();
            PlayerPrefs.SetString(Constants.ProgressKey, progress);
        }
    }
}