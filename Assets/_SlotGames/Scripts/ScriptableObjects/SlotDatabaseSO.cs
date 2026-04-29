using System.Collections.Generic;
using UnityEngine;

namespace SlotGame.Data
{
    [CreateAssetMenu(fileName = "SlotDatabase", menuName = "SlotGame/Database")]
    public class SlotDatabaseSO : ScriptableObject
    {
        public List<SlotSymbolSO> allSymbols;

        public SlotSymbolSO GetRandomSymbol()
        {
            if (allSymbols == null || allSymbols.Count == 0) return null;
            int randomIndex = Random.Range(0, allSymbols.Count);
            return allSymbols[randomIndex];
        }


    }
}
