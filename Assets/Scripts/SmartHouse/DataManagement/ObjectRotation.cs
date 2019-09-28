using UnityEngine;

namespace SmartHouse.DataManagement
{
    public class ObjectRotation : MonoBehaviour
    {
        public float turnSpeed;

        private float currentRotation;
        
        private void Update()
        {
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0f,currentRotation += Time.deltaTime * turnSpeed,0f);
        }
    }
}