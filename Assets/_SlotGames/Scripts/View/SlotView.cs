using UnityEngine;
using UnityEngine.UI;
using SlotGame.Data;

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
    }
}
