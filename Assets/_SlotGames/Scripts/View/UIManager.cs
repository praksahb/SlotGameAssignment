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
            
            balanceText.text = newBalance.ToString(); // Numbers only
            balanceText.rectTransform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
        }

        private void UpdateBetUI(int newBet)
        {
            if (betText == null) return;
            
            betText.text = newBet.ToString(); // Numbers only
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

            winAmountText.text = $"WIN: {winAmount}";
            
            winAmountText.rectTransform.localScale = Vector3.zero;
            winAmountText.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            
            if (winNotificationPanel != null)
            {
                winNotificationPanel.SetActive(true);
                winNotificationPanel.transform.localScale = Vector3.zero;
                winNotificationPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            }
        }

        public void ClearWinText()
        {
            if (winAmountText != null) winAmountText.text = "";
            if (winNotificationPanel != null) winNotificationPanel.SetActive(false);
        }
    }
}
