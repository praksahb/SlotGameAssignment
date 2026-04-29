using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using SlotGame.Core;

namespace SlotGame.View
{
    public class SlotLever : MonoBehaviour, IPointerClickHandler
    {
        [Header("References")]
        [SerializeField] private SlotMachineController slotController;
        [SerializeField] private Transform leverUpTransform;
        [SerializeField] private GameObject leverDownGameObject;

        [Header("Settings")]
        [SerializeField] private float pullAngle = 111f;
        [SerializeField] private float animationDuration = 0.4f;

        private bool isBusy = false;

        private void OnEnable()
        {
            if (slotController != null)
            {
                slotController.OnSpinStopping += ReturnLever;
            }
            
            // Initial State
            if (leverDownGameObject != null) leverDownGameObject.SetActive(false);
            if (leverUpTransform != null) leverUpTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        private void OnDisable()
        {
            if (slotController != null)
            {
                slotController.OnSpinStopping -= ReturnLever;
            }
        }

        // 1. Interaction: Click to Pull
        public void OnPointerClick(PointerEventData eventData)
        {
            if (isBusy) return;
            
            PullLever();
        }

        private void PullLever()
        {
            if (leverUpTransform == null) return;
            
            isBusy = true;

            // Rotate Up Transform from 0 to 111 on X
            leverUpTransform.DOLocalRotate(new Vector3(pullAngle, 0, 0), animationDuration)
                .SetEase(Ease.InCubic)
                .OnComplete(() => {
                    // Enable the Down state visual
                    if (leverDownGameObject != null) leverDownGameObject.SetActive(true);
                    
                    // Trigger the machine
                    if (slotController != null) slotController.Spin();
                });
        }

        private void ReturnLever()
        {
            if (leverUpTransform == null) return;

            // Start moving back to 0
            // "This time as it starts to go towards 0, we will disable the LeverDown game object"
            if (leverDownGameObject != null) leverDownGameObject.SetActive(false);

            leverUpTransform.DOLocalRotate(Vector3.zero, animationDuration)
                .SetEase(Ease.OutBack)
                .OnComplete(() => {
                    isBusy = false;
                });
        }
    }
}
