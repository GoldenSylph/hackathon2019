using UnityEngine;

namespace SmartHouse.VrInteractions
{
    [RequireComponent(typeof(Collider))]
    public class SnapDropZone : MonoBehaviour
    {
        public bool appendScale;
        
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
            var subjectTransform = subject.transform;
            var myTransform = transform;
            subjectTransform.position = myTransform.position;
            subjectTransform.rotation = myTransform.rotation;
            if (appendScale)
            {
                subjectTransform.localScale = myTransform.localScale;
            }
            subject.Dropped -= OnDrop;
            occupied = true;
            Snapped?.Invoke(subject);
        }
    }
}
