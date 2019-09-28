using System;
using UnityEditor.Android;
using UnityEngine;
using XR_Input;

namespace SmartHouse
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Interactable : MonoBehaviour
    {
        public delegate void GrabDropHandler(Interactable subject);
        public event GrabDropHandler Grabbed;
        public event GrabDropHandler Dropped;
        
        protected bool HandIsIn;
        protected virtual void Start()
        {
            XrPlayer.Player.LeftController.MyControllerEvents.OnGripPressed += OnGripPressed;
            XrPlayer.Player.LeftController.MyControllerEvents.OnGripReleased += OnGripReleased;
            XrPlayer.Player.RightController.MyControllerEvents.OnGripPressed += OnGripPressed;
            XrPlayer.Player.RightController.MyControllerEvents.OnGripReleased += OnGripReleased;
        }

        private void OnTriggerEnter(Collider other)
        {
            HandIsIn = true;
        }

        private void OnTriggerExit(Collider other)
        {
            HandIsIn = false;
        }

        public virtual void OnGripPressed(object sender, ControllerGripArgs e)
        {
            if (!HandIsIn) return;
            Connect(sender, true, gameObject);
            Grabbed?.Invoke(this);
        }
        
        public virtual void OnGripReleased(object sender, ControllerGripArgs e)
        {
            Connect(sender, false, gameObject);
            Dropped?.Invoke(this);
        }

        protected static void Connect(object sender, bool parent, GameObject toObject)
        {
            var hand = ((XrControllerEvents) sender).MyXrController;
            if (parent)
            {
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
                if (!fixedJoint) return;
                fixedJoint.connectedBody = null;
                Destroy(fixedJoint);
            }
        }
    }
}
