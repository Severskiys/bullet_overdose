using System;
using UnityEngine;

namespace CodeBase._Services.Ads
{
    public class AdsService : IAdsService //, IUnityAdsListener
    {
        private const string AndroidGameId = "4106937";
        private const string IOSGameId = "4106936";

        private const string UnityRewardedVideoIdAndroid = "Rewarded_Android";
        private const string UnityRewardedVideoIdIOS = "Rewarded_iOS";

        private string _gameId;
        private string _placementId;

        private Action _onVideoFinished;

        public event Action RewardedVideoReady;

        public int Reward => 15;

        public AdsService()
        {
            SetIdsForCurrentPlatform();
            //Advertisement.AddListener(this);
        }


        public void ShowRewardedVideo(Action onVideoFinished)
        {
            _onVideoFinished = onVideoFinished;
        }

        public bool IsRewardedVideoReady => false; //Advertisement.IsReady(_placementId);

        public void OnUnityAdsReady(string placementId)
        {
            Debug.Log($"OnUnityAdsReady {placementId}");

            if (placementId == _placementId)
                RewardedVideoReady?.Invoke();
        }

        public void OnUnityAdsDidError(string message) =>
            Debug.Log($"OnUnityAdsDidError {message}");

        public void OnUnityAdsDidStart(string placementId) =>
            Debug.Log($"OnUnityAdsDidStart {placementId}");

        private void SetIdsForCurrentPlatform()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _gameId = AndroidGameId;
                    _placementId = UnityRewardedVideoIdAndroid;
                    break;

                case RuntimePlatform.IPhonePlayer:
                    _gameId = IOSGameId;
                    _placementId = UnityRewardedVideoIdIOS;
                    break;

                case RuntimePlatform.WindowsEditor:
                    _gameId = IOSGameId;
                    _placementId = UnityRewardedVideoIdIOS;
                    break;

                default:
                    Debug.Log("Unsupported platform for ads.");
                    break;
            }
        }
    }
}
