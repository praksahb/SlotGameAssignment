using UnityEngine;

namespace SlotGame.Data
{
    [CreateAssetMenu(fileName = "SlotSettings", menuName = "SlotGame/Settings")]
    public class SlotSettingsSO : ScriptableObject
    {
        [Header("Spin Settings")]
        public float spinDuration = 2.0f;
        public float delayBetweenReels = 0.25f;
        public float spinSpeed = 1500f;

        [Header("Economy Settings")]
        public int initialBalance = 1000;
        public int costPerSpin = 10;
        public int jackpotMultiplier = 10;
    }
}
