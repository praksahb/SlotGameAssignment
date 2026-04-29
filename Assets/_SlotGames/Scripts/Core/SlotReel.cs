using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotGame.Data;
using SlotGame.View;
using DG.Tweening; // DOTween Power!

namespace SlotGame.Core
{
    public class SlotReel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform reelContainer;
        [SerializeField] private RectTransform windowViewReference;
        [SerializeField] private GameObject symbolPrefab;
        [SerializeField] private SlotDatabaseSO database;

        [Header("Settings")]
        [SerializeField] private float verticalSpacing = 20f;
        [SerializeField] private float centeringOffset = 0f;

        private List<RectTransform> activeSymbols = new List<RectTransform>();
        private bool isSpinning = false;
        public bool IsSpinning => isSpinning;
        private bool stopRequested = false;
        private SlotSymbolSO targetResult;
        private float symbolHeight;
        private float slotStep;

        private void Start()
        {
            RectTransform refRect = windowViewReference != null ? windowViewReference : reelContainer;
            slotStep = refRect.rect.height / 3f;
            symbolHeight = slotStep - verticalSpacing;

            float visualCenter = windowViewReference != null ? windowViewReference.anchoredPosition.y : 0;
            float targetY = visualCenter + centeringOffset;

            if (database != null && database.allSymbols != null)
            {
                List<SlotSymbolSO> strip = new List<SlotSymbolSO>(database.allSymbols);
                for (int i = 0; i < strip.Count; i++)
                {
                    SlotSymbolSO temp = strip[i];
                    int randomIndex = Random.Range(i, strip.Count);
                    strip[i] = strip[randomIndex];
                    strip[randomIndex] = temp;
                }

                float startY = targetY - slotStep;
                for (int i = 0; i < strip.Count; i++)
                {
                    SpawnSymbol(startY + (i * slotStep), strip[i]);
                }
            }
        }

        private void SpawnSymbol(float yPos, SlotSymbolSO initialData)
        {
            GameObject go = Instantiate(symbolPrefab, reelContainer);
            go.name = $"Symbol_{initialData.symbolName}";
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, yPos);
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, symbolHeight);
            
            SymbolView view = go.GetComponent<SymbolView>();
            if (view != null) view.UpdateSymbol(initialData);
            
            activeSymbols.Add(rect);
        }

        public void StartSpin(float speed)
        {
            if (isSpinning) return;
            stopRequested = false;
            StartCoroutine(SpinRoutine(speed));
        }

        public void StopSpin(SlotSymbolSO result)
        {
            targetResult = result;
            stopRequested = true;
        }

        private IEnumerator SpinRoutine(float maxSpeed)
        {
            isSpinning = true;
            float currentSpeed = 0;

            // 1. Smooth Acceleration
            Tween accelTween = DOTween.To(() => currentSpeed, x => currentSpeed = x, maxSpeed, 0.5f).SetEase(Ease.InSine);

            while (!stopRequested || accelTween.IsActive())
            {
                MoveSymbols(currentSpeed * Time.deltaTime);
                yield return null;
            }

            // 2. Stop Requested: Calculate target and setup DOTween Glide
            RectTransform winnerRect = null;
            foreach (var rect in activeSymbols)
            {
                if (rect.GetComponent<SymbolView>().CurrentID == targetResult.symbolID)
                {
                    winnerRect = rect;
                    break;
                }
            }

            float visualCenter = windowViewReference != null ? windowViewReference.anchoredPosition.y : 0;
            float targetY = visualCenter + centeringOffset;

            float distToTarget = winnerRect.anchoredPosition.y - targetY;
            float fullReelLoop = slotStep * database.allSymbols.Count;
            if (distToTarget < 0) distToTarget += fullReelLoop;
            
            float totalStopDistance = distToTarget + (fullReelLoop * 2);
            
            // 3. Dynamic Glide (Pure Deceleration)
            float stopDuration = (totalStopDistance / maxSpeed) * 2.2f; // Smooth 2.2s average
            float distanceMoved = 0;
            bool glideFinished = false;

            DOTween.To(() => distanceMoved, x => {
                float delta = x - distanceMoved;
                MoveSymbols(delta);
                distanceMoved = x;
            }, totalStopDistance, stopDuration)
            .SetEase(Ease.OutCubic) // Always slows down, never speeds up
            .OnComplete(() => {
                SnapToPositions(winnerRect, targetY);                
                glideFinished = true;
            });

            while (!glideFinished) yield return null;
            
            isSpinning = false;
        }

        private void MoveSymbols(float distance)
        {
            float totalReelHeight = slotStep * activeSymbols.Count;
            float visualCenter = windowViewReference != null ? windowViewReference.anchoredPosition.y : 0;
            float bottomThreshold = visualCenter - (slotStep * 2f);

            foreach (var symbol in activeSymbols)
            {
                symbol.anchoredPosition -= new Vector2(0, distance);

                // Stable wrapping: if it goes below the window, move it to the top of the stack
                if (symbol.anchoredPosition.y < bottomThreshold)
                {
                    symbol.anchoredPosition += new Vector2(0, totalReelHeight);
                }
            }
        }

        private void SnapToPositions(RectTransform referenceSymbol, float targetY)
        {
            float diff = targetY - referenceSymbol.anchoredPosition.y;
            foreach (var symbol in activeSymbols)
            {
                symbol.anchoredPosition += new Vector2(0, diff);
                float yFromTarget = symbol.anchoredPosition.y - targetY;
                float snappedYFromTarget = Mathf.Round(yFromTarget / slotStep) * slotStep;
                symbol.anchoredPosition = new Vector2(0, targetY + snappedYFromTarget);
            }
        }

        private float GetHighestSymbolY()
        {
            float max = -float.MaxValue;
            foreach (var symbol in activeSymbols)
            {
                if (symbol.anchoredPosition.y > max) max = symbol.anchoredPosition.y;
            }
            return max;
        }
    }
}
