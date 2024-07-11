using CodeBase._Tools.Helpers;
using UnityEngine;

namespace CodeBase._Tools.CameraTools
{
    public class CameraInstaller : MonoBehaviour
    {
        [SerializeField] private Canvas _worldCanvas;

        private void Awake()
        {
            _worldCanvas.worldCamera = Helper.Camera;
        }
    }
}