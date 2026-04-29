using System.Collections.Generic;
using UnityEngine;
using SlotGame.Data;

namespace SlotGame.Core
{
    /// <summary>
    /// Handles currency logic, bet tiers, and payout calculations.
    /// </summary>
    public class PayoutManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private SlotSettingsSO settings;

        public int CurrentBalance { get; private set; }
        public int CurrentBet { get; private set; }

        public System.Action<int> OnBalanceChanged;
        public System.Action<int> OnWinDetected;
        public System.Action<int> OnBetChanged;

        private readonly int[] betTiers = { 10, 50, 100, 200 };
        private int currentBetIndex = 0;

        private void Awake() 
        {
            if (settings != null) CurrentBalance = settings.initialBalance;
            CurrentBet = betTiers[currentBetIndex];
        }

        private void Start()
        {
            OnBalanceChanged?.Invoke(CurrentBalance);
            OnBetChanged?.Invoke(CurrentBet);
        }

        public void IncreaseBet()
        {
            if (currentBetIndex < betTiers.Length - 1)
            {
                currentBetIndex++;
                CurrentBet = betTiers[currentBetIndex];
                OnBetChanged?.Invoke(CurrentBet);
            }
        }

        public void DecreaseBet()
        {
            if (currentBetIndex > 0)
            {
                currentBetIndex--;
                CurrentBet = betTiers[currentBetIndex];
                OnBetChanged?.Invoke(CurrentBet);
            }
        }

        public bool CanAffordSpin() => CurrentBalance >= CurrentBet;

        public void DeductSpinCost()
        {
            CurrentBalance -= CurrentBet;
            OnBalanceChanged?.Invoke(CurrentBalance);
            Debug.Log($"[Economy] Spin cost deducted: {CurrentBet}. New Balance: {CurrentBalance}");
        }

        public int CalculatePotentialWin(List<SlotSymbolSO> results)
        {
            if (results == null || results.Count < 3) return 0;

            if (results[0].symbolID == results[1].symbolID && results[1].symbolID == results[2].symbolID)
            {
                int betMultiplier = CurrentBet / 10; 
                int multiplier = settings != null ? settings.jackpotMultiplier : 10;
                
                return results[0].basePayoutValue * multiplier * betMultiplier;
            }
            return 0;
        }

        public void AddWinToBalance(int amount)
        {
            CurrentBalance += amount;
            OnBalanceChanged?.Invoke(CurrentBalance);
            OnWinDetected?.Invoke(amount); 
            Debug.Log($"[Economy] Credits added: {amount}. New Balance: {CurrentBalance}");
        }
    }
}
