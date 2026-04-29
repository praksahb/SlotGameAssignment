using UnityEngine;
using TMPro;
using DG.Tweening;
using SlotGame.Core;

namespace SlotGame.View
{
    public class UIManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PayoutManager payoutManager;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI balanceText;
        [SerializeField] private TextMeshProUGUI winAmountText;
        [SerializeField] private TextMeshProUGUI betText; // New text for current bet
        [SerializeField] private GameObject winNotificationPanel;
        [SerializeField] private float winDisplayDuration = 3.0f; // Seconds to show win panel
        [SerializeField] private float winPanelDelay = 0.8f; // Delay before panel pops up

        private void Start()
        {
            if (winNotificationPanel != null) winNotificationPanel.SetActive(false);
        }

        private void OnEnable()
        {
            if (payoutManager != null)
            {
                payoutManager.OnBalanceChanged += UpdateBalanceUI;
                payoutManager.OnWinDetected += ShowWinNotification;
                payoutManager.OnBetChanged += UpdateBetUI;
            }
        }

        private void OnDisable()
        {
            if (payoutManager != null)
            {
                payoutManager.OnBalanceChanged -= UpdateBalanceUI;
                payoutManager.OnWinDetected -= ShowWinNotification;
                payoutManager.OnBetChanged -= UpdateBetUI;
            }
        }

        private void UpdateBalanceUI(int newBalance)
        {
            if (balanceText == null) return;
            
            // Format: 40,000 (just the number with commas)
            balanceText.text = newBalance.ToString("N0"); 
            balanceText.rectTransform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
        }

        private void UpdateBetUI(int newBet)
        {
            if (betText == null) return;
            
            // Format: 500 or 1,000
            betText.text = newBet.ToString("N0"); 
            betText.rectTransform.DOPunchScale(Vector3.one * 0.15f, 0.15f);
        }

        public void OnIncreaseBetClicked()
        {
            if (payoutManager != null) payoutManager.IncreaseBet();
        }

        public void OnDecreaseBetClicked()
        {
            if (payoutManager != null) payoutManager.DecreaseBet();
        }

        private void ShowWinNotification(int winAmount)
        {
            if (winAmountText == null) return;

            // Wait for the delay (anticipation) before showing the panel
            DOVirtual.DelayedCall(winPanelDelay, () => {
                
                winAmountText.text = winAmount.ToString("N0");
                winAmountText.rectTransform.localScale = Vector3.zero;
                winAmountText.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
                
                if (winNotificationPanel != null)
                {
                    winNotificationPanel.SetActive(true);
                    winNotificationPanel.transform.localScale = Vector3.zero;
                    winNotificationPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
                }

                // Auto-hide after it has been displayed for its full duration
                DOVirtual.DelayedCall(winDisplayDuration, ClearWinText);
            });
        }

        public void OnWinPanelClicked()
        {
            ClearWinText();
        }

        public void ClearWinText()
        {
            if (winAmountText != null) winAmountText.text = "";
            if (winNotificationPanel != null) winNotificationPanel.SetActive(false);
        }
    }
}
