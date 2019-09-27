using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class Product
    {
        public bool Available;
        public string Description;
        public string Name;
        public Vector3 Position;
        public float Price;

        public static Product Create()
        {
            
            return new Product();
        }

        public Product SetDescription(string description)
        {
            Description = description;
            return this;
        }

        public Product SetPrice(int price)
        {
            Price = price;
            return this;
        }

        public Product SetName(string name)
        {
            Name = name;
            return this;
        }

        public Product SetPosition(Vector3 position)
        {
            Position = position;
            return this;
        }
    }

    public class ProductList
    {
        List<Product>_produstList=new List<Product>();

        public static void FillList(ref List<Product>List)
        {
            //List.Add(new Product().Create().SetName("lol").SetPrice(1));
            for (int i = 0; i < 10; i++)
            {
                List.Add( Product.Create().SetName("lol"+i).SetPrice(1+i).SetDescription("Item #"+i));
            }
        }
    }

}