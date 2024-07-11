using System;
using CodeBase._Tools;

namespace CodeBase.SavedData
{
    [Serializable]
    public class PlayerProgress
    {
        public ReactiveVariable<int> MoneyCount;
        public ReactiveVariable<int> HardCount;
        public ReactiveVariable<int> KillCount;

        public PlayerProgress(int startMoney)
        {
            MoneyCount = new ReactiveVariable<int>(startMoney);
            KillCount = new ReactiveVariable<int>(0);
            HardCount = new ReactiveVariable<int>(0);
        }
    }
}