using UnityEngine;
using DG.Tweening;
using SlotGame.Core;

namespace SlotGame.View
{
    /// <summary>
    /// Handles all visual feedback and "Juice" for the slot machine, 
    /// such as screen shakes and particle triggers.
    /// </summary>
    public class SlotEffectManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SlotMachineController controller;
        [SerializeField] private PayoutManager payoutManager;
        [SerializeField] private RectTransform mainMachineObject;

        [Header("Shake Settings")]
        [SerializeField] private float startShakeDuration = 0.2f;
        [SerializeField] private float startShakeStrength = 8f;
        [SerializeField] private float winShakeStrength = 20f;
        [SerializeField] private float winShakeDuration = 1.5f;

        private void OnEnable()
        {
            if (controller != null)
                controller.OnSpinStarted += PlayStartShake;

            if (payoutManager != null)
                payoutManager.OnWinDetected += PlayWinShake;
        }

        private void OnDisable()
        {
            if (controller != null)
                controller.OnSpinStarted -= PlayStartShake;

            if (payoutManager != null)
                payoutManager.OnWinDetected -= PlayWinShake;
        }

        private void PlayStartShake()
        {
            if (mainMachineObject == null) return;
            mainMachineObject.DOShakeAnchorPos(startShakeDuration, startShakeStrength, 10);
        }

        private void PlayWinShake(int winAmount)
        {
            if (mainMachineObject == null) return;
            mainMachineObject.DOShakeAnchorPos(winShakeDuration, winShakeStrength, 15);
        }
    }
}
