using System.Collections.Generic;
using UnityEngine;
using SlotGame.Data;

namespace SlotGame.Core
{
    public class PayoutManager : MonoBehaviour
    {
        [SerializeField] private SlotSettingsSO settings;

        public int CurrentBalance { get; private set; }

        private void Awake() 
        {
            if (settings != null) CurrentBalance = settings.initialBalance;
        }

        public bool CanAffordSpin() => settings != null ? CurrentBalance >= settings.costPerSpin : true;

        public void DeductSpinCost()
        {
            if (settings != null)
            {
                CurrentBalance -= settings.costPerSpin;
                Debug.Log($"Spin Cost Deducted. Balance: {CurrentBalance}");
            }
        }

        public int CalculatePotentialWin(List<SlotSymbolSO> results)
        {
            if (results == null || results.Count < 3) return 0;

            // DEBUG LOGS to see what is happening
            Debug.Log($"Checking Payout: R1:{results[0].symbolName}(ID:{results[0].symbolID}) | " +
                      $"R2:{results[1].symbolName}(ID:{results[1].symbolID}) | " +
                      $"R3:{results[2].symbolName}(ID:{results[2].symbolID})");

            // Assignment Rule: All 3 reels must match
            if (results[0].symbolID == results[1].symbolID && results[1].symbolID == results[2].symbolID)
            {
                int multiplier = settings != null ? settings.jackpotMultiplier : 10;
                return results[0].basePayoutValue * multiplier;
            }
            return 0;
        }

        public void AddWinToBalance(int amount)
        {
            CurrentBalance += amount;
            Debug.Log($"<color=green>Credits Added: {amount}. New Balance: {CurrentBalance}</color>");
        }
    }
}
