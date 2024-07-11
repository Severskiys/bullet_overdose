using System.Collections.Generic;
using CodeBase._Services;
using CodeBase._Services.PersistentGameData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface ISaversHolder : IService
    {
        public List<IProgressSaver> ProgressSavers { get; }
        public List<IProgressLoader> ProgressLoaders { get; }
        public void RegisterProgressWatchers(GameObject gameObject);
        public void UnRegisterProgressWatchers(GameObject gameObject);
    }
    
    public class SaversHolder : ISaversHolder
    {
        public List<IProgressSaver> ProgressSavers { get; } = new List<IProgressSaver>();
        public List<IProgressLoader> ProgressLoaders { get; } = new List<IProgressLoader>();
        
        public void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (IProgressLoader progressLoader in gameObject.GetComponentsInChildren<IProgressLoader>())
            {
                if (progressLoader is IProgressSaver progressWriter)
                    ProgressSavers.Add(progressWriter);
                ProgressLoaders.Add(progressLoader);
            }
        }
        
        public void UnRegisterProgressWatchers(GameObject gameObject)
        {
            foreach (IProgressLoader progressLoader in gameObject.GetComponentsInChildren<IProgressLoader>())
            {
                if (progressLoader is IProgressSaver progressWriter)
                    ProgressSavers.Remove(progressWriter);
                ProgressLoaders.Remove(progressLoader);
            }
        }
    }
}