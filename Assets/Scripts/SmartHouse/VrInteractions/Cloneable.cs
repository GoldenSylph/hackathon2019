using UnityEngine;
using XR_Input;

namespace SmartHouse.VrInteractions
{
    public class Cloneable : Interactable
    {
        public delegate void CloneHandler(Interactable subject);
        public event CloneHandler Cloned;
        
        protected override void Start()
        {
            base.Start();
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().isTrigger = true;
        }

        public override void OnGripPressed(object sender, ControllerGripArgs e)
        {
            if (!HandIsIn) return;
            Debug.Log("Clone");
            var cloned = Instantiate(gameObject);
            var cloneableScript = cloned.GetComponent<Cloneable>();
            Destroy(cloneableScript);
            var interactableCloned = cloned.AddComponent<Interactable>();
            cloned.GetComponent<Collider>().isTrigger = false;
            cloned.GetComponent<Rigidbody>().isKinematic = false;
            Cloned?.Invoke(interactableCloned);
            Connect(sender, true, cloned);
        }
    }
}
