using UnityEngine;

namespace SlotGame.Data
{
    [CreateAssetMenu(fileName = "NewSlotSymbol", menuName = "SlotGame/Symbol")]
    public class SlotSymbolSO : ScriptableObject
    {
        [Header("Symbol Info")]
        public string symbolName;
        public Sprite symbolSprite;
        public int symbolID;
        
        [Header("Payout Info")]
        public int basePayoutValue;


    }
        
}
