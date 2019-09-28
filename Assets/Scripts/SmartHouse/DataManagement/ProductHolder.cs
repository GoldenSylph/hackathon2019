using System;
using UnityEngine;

namespace SmartHouse.DataManagement
{
    public class ProductHolder : MonoBehaviour
    {
        public Product info;

        private void Start()
        {
            info = Product.Create().SetPosition(transform.position);
        }
    }
}
