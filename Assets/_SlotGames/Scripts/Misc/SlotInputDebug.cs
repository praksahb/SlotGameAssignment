using UnityEngine;
using SlotGame.Core;

namespace SlotGame.Misc
{
    public class SlotInputDebug : MonoBehaviour
    {
        [SerializeField] private SlotMachineController controller;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (controller != null)
                {
                    controller.Spin();
                }
            }
        }
    }
}
