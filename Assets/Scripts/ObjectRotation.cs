using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectRotation : MonoBehaviour
    {

        public GameObject item;

        public float turnSpeed;

        private float _currentAngle;
        // Start is called before the first frame   update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            _currentAngle += turnSpeed;
            item.transform.rotation = Quaternion.Euler(0f,_currentAngle,0f);
        }
    }
}
