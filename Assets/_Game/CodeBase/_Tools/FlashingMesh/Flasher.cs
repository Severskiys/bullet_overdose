using DG.Tweening;
using UnityEngine;

namespace CodeBase._Tools.FlashingMesh
{
    public class Flasher : MonoBehaviour
    {
        [SerializeField] private MeshRenderer        _meshRenderer;
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;

        [SerializeField] private string _emissionProperty = "_EmissionColor";
        [SerializeField] private Color _flashColor;
        [SerializeField] private float _flashDuration;

        public void DoFlash()
        {
            if (_meshRenderer != default) Flash(_meshRenderer.material);
            if (_skinnedMeshRenderer != default) Flash(_skinnedMeshRenderer.material);
        }

        private void Flash(Material material)
        {
            material.DOKill();
            material.DOColor(_flashColor, _emissionProperty, _flashDuration)
                .OnComplete(() => material.DOColor(Color.clear, _emissionProperty, _flashDuration));
        }
    }
}