using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SmartHouse.DataManagement
{
    [Serializable]
    public class Product
    {
        public int available;
        public string description;
        public string name;
        public Vector3 position;
        public float price;

        public static Product Create()
        {
            return new Product();
        }

        public Product SetDescription(string newDescription)
        {
            this.description = newDescription;
            return this;
        }

        public Product SetPrice(int newPrice)
        {
            this.price = newPrice;
            return this;
        }

        public Product SetName(string newName)
        {
            name = newName;
            return this;
        }

        public Product SetPosition(Vector3 newPosition)
        {
            position = newPosition;
            return this;
        }

        public Product SetAvailable(int count)
        {
            available = count;
            return this;
        }
        
    }

    [Serializable]
    public class Dto
    {
        public List<Product> products;
    }
}