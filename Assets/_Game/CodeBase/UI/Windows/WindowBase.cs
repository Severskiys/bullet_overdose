using CodeBase._Services.PersistentGameData;
using CodeBase.SavedData;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        [SerializeField] private Button CloseButton;

        private PersistentProgress _persistentProgress;
        protected PlayerProgress Progress => _persistentProgress.Progress;

        public void Construct(PersistentProgress progress) => _persistentProgress = progress;

        private void Awake() => OnAwake();

        private void Start()
        {
            Initialize();
            SubscribeUpdates();
        }

        private void OnDestroy() => Cleanup();

        protected void OnAwake() => CloseButton.onClick.AddListener(() => Destroy(gameObject));

        protected virtual void Initialize()
        {
        }

        protected virtual void SubscribeUpdates()
        {
        }

        protected virtual void Cleanup()
        {
        }
    }
}