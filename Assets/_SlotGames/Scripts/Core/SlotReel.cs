using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotGame.Data;
using SlotGame.View;

namespace SlotGame.Core
{
    public class SlotReel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform reelContainer;
        [SerializeField] private GameObject symbolPrefab;
        [SerializeField] private SlotDatabaseSO database;

        [Header("Settings")]
        [SerializeField] private int symbolsToSpawn = 5;
        [SerializeField] private float spinSpeed = 1500f;

        private List<RectTransform> activeSymbols = new List<RectTransform>();
        private bool isSpinning = false;
        private float symbolHeight;

        private void Start()
        {
            // Assuming 3 symbols visible, calculate height from container
            symbolHeight = reelContainer.rect.height / 3f;

            for (int i = 0; i < symbolsToSpawn; i++)
            {
                SpawnSymbol(i * symbolHeight);
            }
        }

        private void SpawnSymbol(float yPos)
        {
            GameObject go = Instantiate(symbolPrefab, reelContainer);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, yPos);

            UpdateSymbolVisual(go, database.GetRandomSymbol());
            activeSymbols.Add(rect);
        }

        private void UpdateSymbolVisual(GameObject symbolGo, SlotSymbolSO symbolData)
        {
            SymbolView view = symbolGo.GetComponent<SymbolView>();
            if (view != null && symbolData != null)
            {
                view.UpdateSymbol(symbolData);
            }
        }

        public void StartSpin()
        {
            if (isSpinning) return;
            StartCoroutine(SpinRoutine());
        }

        private IEnumerator SpinRoutine()
        {
            isSpinning = true;
            while (isSpinning)
            {
                foreach (var symbol in activeSymbols)
                {
                    symbol.anchoredPosition -= new Vector2(0, spinSpeed * Time.deltaTime);

                    if (symbol.anchoredPosition.y <= -symbolHeight)
                    {
                        float newY = GetHighestSymbolY() + symbolHeight;
                        symbol.anchoredPosition = new Vector2(0, newY);
                        UpdateSymbolVisual(symbol.gameObject, database.GetRandomSymbol());
                    }
                }
                yield return null;
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

        public void StopSpin()
        {
            isSpinning = false;
        }
    }
}
