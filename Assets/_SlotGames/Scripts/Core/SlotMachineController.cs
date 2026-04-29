using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotGame.Data;

namespace SlotGame.Core
{
    public class SlotMachineController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SlotReel[] reels;
        [SerializeField] private SlotDatabaseSO database;
        [SerializeField] private PayoutManager payoutManager;
        [SerializeField] private SlotSettingsSO settings;

        private bool isSpinning = false;

        public void Spin()
        {
            if (isSpinning) return;
            if (payoutManager != null && !payoutManager.CanAffordSpin())
            {
                Debug.LogWarning("Insufficient balance!");
                return;
            }

            StartCoroutine(SpinRoutine());
        }

        private IEnumerator SpinRoutine()
        {
            isSpinning = true;
            if (payoutManager != null) payoutManager.DeductSpinCost();

            // 1. Generate RNG Results
            List<SlotSymbolSO> results = new List<SlotSymbolSO>();
            for (int i = 0; i < reels.Length; i++)
            {
                results.Add(database.GetRandomSymbol());
            }

            // 2. Pre-calculate win
            int potentialWin = 0;
            if (payoutManager != null)
            {
                potentialWin = payoutManager.CalculatePotentialWin(results);
            }

            // 3. Start reels & Wait for duration
            foreach (var reel in reels)
            {
                float speed = settings != null ? settings.spinSpeed : 1500f;
                reel.StartSpin(speed);
                if (settings != null) yield return new WaitForSeconds(settings.delayBetweenReels);
            }

            if (settings != null) yield return new WaitForSeconds(settings.spinDuration);

            // 4. Stop reels
            for (int i = 0; i < reels.Length; i++)
            {
                reels[i].StopSpin(results[i]);
                if (settings != null) yield return new WaitForSeconds(settings.delayBetweenReels);
            }

            // 5. Wait for all reels to physically finish snapping
            bool anyReelSpinning = true;
            while (anyReelSpinning)
            {
                anyReelSpinning = false;
                foreach (var reel in reels)
                {
                    if (reel.IsSpinning)
                    {
                        anyReelSpinning = true;
                        break;
                    }
                }
                yield return null;
            }

            // 6. Finalize payout
            if (potentialWin > 0)
            {
                payoutManager.AddWinToBalance(potentialWin);
                Debug.Log($"<color=green>WINNER! You won {potentialWin} credits!</color>");
            }

            isSpinning = false;
        }
    }
}
