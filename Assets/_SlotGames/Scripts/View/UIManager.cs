using UnityEngine;
using TMPro;
using DG.Tweening;
using SlotGame.Core;

namespace SlotGame.View
{
    /// <summary>
    /// Manages all UI displays including balance, current bet, and win notifications.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("Engine References")]
        [SerializeField] private SlotMachineController controller;
        [SerializeField] private PayoutManager payoutManager;

        [Header("UI Text Elements")]
        [SerializeField] private TextMeshProUGUI balanceText;
        [SerializeField] private TextMeshProUGUI winAmountText;
        [SerializeField] private TextMeshProUGUI betText; 
        
        [Header("Win Display Settings")]
        [SerializeField] private GameObject winNotificationPanel;
        [SerializeField] private float winDisplayDuration = 3.0f;
        [SerializeField] private float winPanelDelay = 0.8f;

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

            if (controller != null)
            {
                controller.OnSpinStarted += ClearWinText;
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

            if (controller != null)
            {
                controller.OnSpinStarted -= ClearWinText;
            }
        }

        private void UpdateBalanceUI(int newBalance)
        {
            if (balanceText == null) return;
            balanceText.text = newBalance.ToString("N0"); 
            balanceText.rectTransform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
        }

        private void UpdateBetUI(int newBet)
        {
            if (betText == null) return;
            betText.text = newBet.ToString("N0"); 
            betText.rectTransform.DOPunchScale(Vector3.one * 0.15f, 0.15f);
        }

        public void OnIncreaseBetClicked() => payoutManager?.IncreaseBet();
        public void OnDecreaseBetClicked() => payoutManager?.DecreaseBet();

        private void ShowWinNotification(int winAmount)
        {
            if (winAmountText == null) return;

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

                DOVirtual.DelayedCall(winDisplayDuration, ClearWinText);
            });
        }

        public void OnWinPanelClicked() => ClearWinText();

        public void ClearWinText()
        {
            if (winAmountText != null) winAmountText.text = "";
            if (winNotificationPanel != null) winNotificationPanel.SetActive(false);
        }
    }
}
