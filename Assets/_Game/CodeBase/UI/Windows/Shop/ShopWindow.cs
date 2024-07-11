using CodeBase._Services.Ads;
using CodeBase._Services.PersistentGameData;
using TMPro;

namespace CodeBase.UI.Windows.Shop
{
    public class ShopWindow : WindowBase
    {
        public TextMeshProUGUI SkullText;
        public RewardedAdItem AdItem;

        public void Construct(IAdsService adsService, PersistentProgress progress)
        {
            base.Construct(progress);
            AdItem.Construct(adsService, progress);
        }

        protected override void Initialize()
        {
            AdItem.Initialize();
            RefreshSkullText(Progress.MoneyCount.Value);
        }

        protected override void SubscribeUpdates()
        {
            AdItem.Subscribe();
            Progress.MoneyCount.Subscribe(RefreshSkullText);
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            AdItem.Cleanup();
            Progress.MoneyCount.Unsubscribe(RefreshSkullText);
        }

        private void RefreshSkullText(int newValue)
        {
            SkullText.text = newValue.ToString();
        }
    }
}