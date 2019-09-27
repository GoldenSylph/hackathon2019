using UnityEngine;

namespace SmartHouse.DataManagement
{
    public class ObjectRotation : MonoBehaviour
    {
        public GameObject item;
        public float turnSpeed;
    
        private void Update()
        {
            item.transform.rotation = Quaternion.Euler(0f,Time.deltaTime * turnSpeed,0f);
        }
    }
}