using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotGame.Data;
using SlotGame.View;
using DG.Tweening;

namespace SlotGame.Core
{
    /// <summary>
    /// Orchestrates the spin lifecycle, RNG generation, and reel coordination.
    /// </summary>
    public class SlotMachineController : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private SlotReel[] SlotReels;
        [SerializeField] private SlotDatabaseSO database;
        [SerializeField] private PayoutManager payoutManager;
        [SerializeField] private SlotSettingsSO settings;

        [Header("Debug Tools")]
        [SerializeField] private bool debugForceWin = false;

        public System.Action OnSpinStarted;
        public System.Action OnSpinStopping;

        private bool isSpinning = false;

        public void Spin()
        {
            if (isSpinning) return;

            if (payoutManager != null && !payoutManager.CanAffordSpin())
            {
                Debug.LogWarning("[SlotEngine] Insufficient balance for spin.");
                OnSpinStopping?.Invoke();
                return;
            }

            StartCoroutine(SpinRoutine());
        }

        private IEnumerator SpinRoutine()
        {
            isSpinning = true;
            OnSpinStarted?.Invoke();

            if (payoutManager != null) payoutManager.DeductSpinCost();

            // 1. Determine Results via RNG
            List<SlotSymbolSO> results = GenerateResults();

            // 2. Pre-calculate payout
            int potentialWin = payoutManager != null ? payoutManager.CalculatePotentialWin(results) : 0;

            // 3. Initiate Reel Motion
            yield return StartReelMotion(results);

            // 4. Finalize Outcomes
            if (potentialWin > 0)
            {
                payoutManager.AddWinToBalance(potentialWin);
                
                // Trigger win animations on the symbols
                foreach (var reel in SlotReels)
                {
                    SymbolView winSymbol = reel.GetCenterSymbolView();
                    if (winSymbol != null) winSymbol.PlayWinAnimation();
                }
            }

            isSpinning = false;
            OnSpinStopping?.Invoke();
        }

        private List<SlotSymbolSO> GenerateResults()
        {
            List<SlotSymbolSO> results = new List<SlotSymbolSO>();
            
            if (debugForceWin)
            {
                SlotSymbolSO forcedSymbol = database.GetRandomSymbol();
                for (int i = 0; i < SlotReels.Length; i++) results.Add(forcedSymbol);
            }
            else
            {
                for (int i = 0; i < SlotReels.Length; i++) results.Add(database.GetRandomSymbol());
            }
            
            return results;
        }

        private IEnumerator StartReelMotion(List<SlotSymbolSO> results)
        {
            float speed = settings != null ? settings.spinSpeed : 1500f;

            // Spin-up phase
            foreach (var reel in SlotReels)
            {
                reel.StartSpin(speed);
                if (settings != null) yield return new WaitForSeconds(settings.delayBetweenReels);
            }

            if (settings != null) yield return new WaitForSeconds(settings.spinDuration);

            // Sequential stop phase
            for (int i = 0; i < SlotReels.Length; i++)
            {
                SlotReels[i].StopSpin(results[i]);
                if (settings != null) yield return new WaitForSeconds(settings.delayBetweenReels);
            }

            // Await physical stabilization
            while (IsAnyReelSpinning()) yield return null;
        }

        private bool IsAnyReelSpinning()
        {
            foreach (var reel in SlotReels)
                if (reel.IsSpinning) return true;
            return false;
        }
    }
}
