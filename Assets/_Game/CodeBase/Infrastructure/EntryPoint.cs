using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        private static bool _isStarted;
        public GameBootstrapper BootstrapperPrefab;
        private const string BootstrapName = "BootstrapScene";
        
        private async void Awake()
        {
            if (_isStarted)
                return;
            
            _isStarted = true;
            
            if (SceneManager.GetActiveScene().name != BootstrapName)
            {
                await SceneManager.LoadSceneAsync(BootstrapName, LoadSceneMode.Single);
                return;
            }
            
            GameBootstrapper bootstrapper = FindObjectOfType<GameBootstrapper>();
            
            if (bootstrapper != null)
                return;

            Instantiate(BootstrapperPrefab); 
        }
    }
}