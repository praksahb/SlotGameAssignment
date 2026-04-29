using UnityEngine;
using UnityEngine.UI;
using SlotGame.Data;
using DG.Tweening;

namespace SlotGame.View
{
    public class SymbolView : MonoBehaviour
    {
        [SerializeField] private Image symbolImage;

        public int CurrentID { get; private set; }
        public SlotSymbolSO CurrentSymbol { get; private set; }

        public void UpdateSymbol(SlotSymbolSO data)
        {
            if (data == null) return;

            CurrentSymbol = data;
            CurrentID = data.symbolID;
            symbolImage.sprite = data.symbolSprite;
        }

        public void PlayWinAnimation()
        {
            // Pulse the symbol scale
            transform.DOScale(Vector3.one * 1.25f, 0.4f)
                .SetLoops(4, LoopType.Yoyo)
                .SetEase(Ease.OutQuad);

            // Optional: Flash the color
            if (symbolImage != null)
            {
                symbolImage.DOColor(Color.yellow, 0.4f)
                    .SetLoops(4, LoopType.Yoyo)
                    .OnComplete(() => symbolImage.color = Color.white);
            }
        }
    }
}
