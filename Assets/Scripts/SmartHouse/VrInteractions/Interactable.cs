using UnityEngine;
using UnityEngine.Serialization;
using XR_Input;

namespace SmartHouse.VrInteractions
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Interactable : MonoBehaviour
    {
        public delegate void GrabDropHandler(Interactable subject);
        public virtual event GrabDropHandler Grabbed;
        public virtual event GrabDropHandler Dropped;

        public bool handIsIn;
        
        protected virtual void Start()
        {
            XrPlayer.Player.LeftController.MyControllerEvents.OnGripPressed += OnGripPressed;
            XrPlayer.Player.LeftController.MyControllerEvents.OnGripReleased += OnGripReleased;
            XrPlayer.Player.RightController.MyControllerEvents.OnGripPressed += OnGripPressed;
            XrPlayer.Player.RightController.MyControllerEvents.OnGripReleased += OnGripReleased;
        }

        private void OnTriggerEnter(Collider other)
        {
            handIsIn = true;
        }

        private void OnTriggerExit(Collider other)
        {
            handIsIn = false;
        }

        public virtual void OnGripPressed(object sender, ControllerGripArgs e)
        {
            if (!handIsIn) return;
            Connect(sender, true, gameObject);
            Grabbed?.Invoke(this);
        }
        
        public virtual void OnGripReleased(object sender, ControllerGripArgs e)
        {
            if (!handIsIn) return;
            Connect(sender, false, gameObject);
            Dropped?.Invoke(this);
        }

        protected void Connect(object sender, bool parent, GameObject toObject)
        {
            var hand = ((XrControllerEvents) sender).MyXrController;
            if (parent)
            {
                transform.position = hand.transform.position;
                var fixedJoint = toObject.GetComponent<FixedJoint>();
                if (!fixedJoint)
                {
                    fixedJoint = toObject.gameObject.AddComponent<FixedJoint>();
                }
                fixedJoint.connectedBody = hand.GetComponent<Rigidbody>();
            }
            else
            {
                var fixedJoint = toObject.GetComponent<FixedJoint>();
                var subjectRigidBody = toObject.GetComponent<Rigidbody>();
                if (subjectRigidBody)
                {
                    subjectRigidBody.isKinematic = false;
                    subjectRigidBody.useGravity = true;
                }
                if (!fixedJoint) return;
                fixedJoint.connectedBody = null;
                Destroy(fixedJoint);
            }
        }
    }
}
