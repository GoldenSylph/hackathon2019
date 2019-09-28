using System;
using SmartHouse.VrInteractions;
using UnityEngine;

namespace SmartHouse.DataManagement
{
    [RequireComponent(typeof(SnapDropZone))]
    public class ProductWatcher : MonoBehaviour
    {
        private SnapDropZone mySnapDropZone;
        private ProductHolder fetchedProductHolder;
        
        private void Start()
        {
            mySnapDropZone = GetComponent<SnapDropZone>();
            mySnapDropZone.Snapped += Snapped;
            mySnapDropZone.Unsnapped += Unsnapped;
        }

        private void Snapped(Interactable interactable)
        {
            fetchedProductHolder = interactable.GetComponent<ProductHolder>();
        }

        private void Unsnapped(Interactable interactable)
        {
            fetchedProductHolder = null;
        }

        public Product getProduct()
        {
            return fetchedProductHolder.info;
        }

    }
}
