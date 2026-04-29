using UnityEngine;

namespace SlotGame
{
    public class ReelTester : MonoBehaviour
    {
        public Core.SlotReel reel;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S)) // S for Spin
            {
                reel.StartSpin();
            }
            if (Input.GetKeyDown(KeyCode.Space)) // Space for Stop
            {
                reel.StopSpin();
            }
        }
    }
}
