using System;

namespace CodeBase._Services.Ads
{
  public interface IAdsService : IService
  {
    event Action RewardedVideoReady;
    bool IsRewardedVideoReady { get; }
    int Reward { get; }
    void ShowRewardedVideo(Action onVideoFinished);
  }
}