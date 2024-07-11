using CodeBase.SavedData;
using UnityEngine;

namespace CodeBase._Services.SaveLoad
{
    public class LoadProgressService : IService
    {
        public PlayerProgress Load()
        {
            return PlayerPrefs.GetString(Constants.ProgressKey)?
                .ToDeserialized<PlayerProgress>();
        }
    }
}
