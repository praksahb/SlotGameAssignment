using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotGame.Data;
using SlotGame.View;

namespace SlotGame.Core
{
    public class SlotMachineController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SlotReel[] SlotReels;
        [SerializeField] private SlotDatabaseSO database;
        [SerializeField] private PayoutManager payoutManager;
        [SerializeField] private SlotSettingsSO settings;
        [SerializeField] private UIManager uiManager;

        public System.Action OnSpinStarted;
        public System.Action OnSpinStopping;

        private bool isSpinning = false;

        public void Spin()
        {
            if (isSpinning) return;

            if (uiManager != null) uiManager.ClearWinText();

            if (payoutManager != null && !payoutManager.CanAffordSpin())
            {
                Debug.LogWarning("Insufficient balance!");
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

            // 1. Generate RNG Results
            List<SlotSymbolSO> results = new List<SlotSymbolSO>();
            for (int i = 0; i < SlotReels.Length; i++)
            {
                results.Add(database.GetRandomSymbol());
            }

            // 2. Pre-calculate win
            int potentialWin = 0;
            if (payoutManager != null)
            {
                potentialWin = payoutManager.CalculatePotentialWin(results);
            }

            // 3. Start Slot reels & Wait for duration
            foreach (var reel in SlotReels)
            {
                float speed = settings != null ? settings.spinSpeed : 1500f;
                reel.StartSpin(speed);
                if (settings != null) yield return new WaitForSeconds(settings.delayBetweenReels);
            }

            if (settings != null) yield return new WaitForSeconds(settings.spinDuration);

            for (int i = 0; i < SlotReels.Length; i++)
            {
                SlotReels[i].StopSpin(results[i]);
                if (settings != null) yield return new WaitForSeconds(settings.delayBetweenReels);
            }

            // 4. Wait for all Slot reels to physically finish snapping
            bool anyReelSpinning = true;
            while (anyReelSpinning)
            {
                anyReelSpinning = false;
                foreach (var reel in SlotReels)
                {
                    if (reel.IsSpinning)
                    {
                        anyReelSpinning = true;
                        break;
                    }
                }
                yield return null;
            }

            // 5. Finalize payout
            if (potentialWin > 0)
            {
                payoutManager.AddWinToBalance(potentialWin);
                Debug.Log($"<color=green>WINNER! You won {potentialWin} credits!</color>");
            }

            isSpinning = false;

            // 6. All Slots stopped
            OnSpinStopping?.Invoke();
        }
    }
}
