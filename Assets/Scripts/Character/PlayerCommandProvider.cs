using UnityEngine;
using Foundation;

namespace Character
{
    public class PlayerCommandProvider : MonoBehaviour, ICommandProvider
    {
        public bool InputFromHuman { get { return true; } }
        public bool Attacking { get; private set; }
        public bool Moving { get { return ((XMovement != 0.0f) || (ZMovement != 0.0f)); } }
        public float XMovement { get; private set; }
        public float ZMovement { get; private set; }

        private void Update()
        {
            XMovement = Input.GetAxis("Horizontal");
            ZMovement = Input.GetAxis("Vertical");
            Attacking = Input.GetButtonDown("Jump");
        }
    }
}