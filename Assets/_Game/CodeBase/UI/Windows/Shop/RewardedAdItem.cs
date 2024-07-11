using CodeBase._Services.Ads;
using CodeBase._Services.PersistentGameData;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Shop
{
    public class RewardedAdItem : MonoBehaviour
    {
        public Button ShowAdButton;
        public GameObject[] AdActiveObjects;
        public GameObject[] AdInactiveObjects;

        private IAdsService _adsService;
        private PersistentProgress _progress;

        public void Construct(IAdsService adsService, PersistentProgress progres)
        {
            _adsService = adsService;
            _progress = progres;
        }

        public void Initialize()
        {
            ShowAdButton.onClick.AddListener(OnShowAdClicked);
            RefreshAvailableAd();
        }

        public void Subscribe() =>
            _adsService.RewardedVideoReady += RefreshAvailableAd;

        public void Cleanup() =>
            _adsService.RewardedVideoReady -= RefreshAvailableAd;

        private void OnShowAdClicked() =>
            _adsService.ShowRewardedVideo(OnVideoFinished);

        private void OnVideoFinished()
        {
           // _progress.Progress.WorldData.LootData.Add(_adsService.Reward);
        }

        private void RefreshAvailableAd()
        {
            bool isVideoReady = _adsService.IsRewardedVideoReady;

            foreach (GameObject adActiveObject in AdActiveObjects)
                adActiveObject.SetActive(isVideoReady);

            foreach (GameObject adInactiveObject in AdInactiveObjects)
                adInactiveObject.SetActive(!isVideoReady);
        }
    }
}