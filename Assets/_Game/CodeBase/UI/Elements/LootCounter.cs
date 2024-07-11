using System;
using CodeBase.SavedData;
using CodeBase.StaticData;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class LootCounter : MonoBehaviour
    {
        public TextMeshProUGUI Counter;
        private PlayerProgress _playerProgress;

        public void Construct(PlayerProgress playerProgress)
        {
            _playerProgress = playerProgress;
            _playerProgress.MoneyCount.Subscribe(UpdateCounter);
            UpdateCounter(_playerProgress.MoneyCount.Value);
        }

        public void CleanUp() => _playerProgress.MoneyCount.Unsubscribe(UpdateCounter);

        private void UpdateCounter(int moneyCount) => Counter.text = $"{moneyCount}";
    }
}