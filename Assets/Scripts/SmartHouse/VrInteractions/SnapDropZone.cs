using System;
using UnityEngine;

namespace SmartHouse
{
    [RequireComponent(typeof(Collider))]
    public class SnapDropZone : MonoBehaviour
    {
        public delegate void SnapDropZoneHandler(Interactable subject);
        public event SnapDropZoneHandler Snapped;
        public event SnapDropZoneHandler Unsnapped;
        
        private bool occupied;
        private void OnTriggerEnter(Collider other)
        {
            if (occupied) return;
            var otherInteractable = other.gameObject.GetComponent<Interactable>();
            otherInteractable.Dropped += OnDrop;
            otherInteractable.Grabbed += OnGrab;
        }

        private void OnGrab(Interactable subject)
        {
            subject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            occupied = false;
            Unsnapped?.Invoke(subject);
        }
        
        private void OnDrop(Interactable subject)
        {
            subject.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            subject.transform.position = transform.position;
            subject.Dropped -= OnDrop;
            occupied = true;
            Snapped?.Invoke(subject);
        }
    }
}
