using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace SmartHouse.DataManagement
{
    public class DataBuilder : MonoBehaviour
    {
        [Serializable]
        public class Estimation
        {
            public List<Product> products;
            public float totalPrice;
        }
        
        public ProductWatcher[] gathered;
        public Estimation estimation;
        public string path;
        
        private void Start()
        {
            gathered = FindObjectsOfType<ProductWatcher>();
            path = Path.Combine(Application.persistentDataPath, "Estimation.csv");
        }

        public void BuildDto()
        {
            foreach (var productWatcher in gathered)
            {
                estimation = new Estimation();
                if (estimation.products == null)
                {
                    estimation.products = new List<Product>();
                }
                estimation.products.Add(productWatcher.getProduct());
                estimation.totalPrice = estimation.products.Select(e => e.price).Sum();
                SaveEstimation(estimation, path);
            }
        }
        
        private static void SaveEstimation(Estimation data, string path)
        {
            var csvString = ToCsv(data);
            using (var streamWriter = File.CreateText(path))
            {
                streamWriter.Write(csvString);
            }
        }

        private static string ToCsv(Estimation estimation)
        {
            var result = new StringBuilder();
            const string SEPARATOR = ",";
            foreach (var product in estimation.products)
            {
                result.Append(product.name)
                    .Append(SEPARATOR)
                    .Append(product.description)
                    .Append(SEPARATOR)
                    .Append(product.available)
                    .Append(SEPARATOR)
                    .Append(product.position.x)
                    .Append(SEPARATOR)
                    .Append(product.position.y)
                    .Append(SEPARATOR)
                    .Append(product.position.z)
                    .Append(SEPARATOR)
                    .Append(product.price)
                    .Append("\n");
            }
            result.Append("")
                .Append(SEPARATOR)
                .Append("")
                .Append(SEPARATOR)
                .Append("")
                .Append(SEPARATOR)
                .Append("")
                .Append(SEPARATOR)
                .Append("")
                .Append(SEPARATOR)
                .Append("")
                .Append(SEPARATOR)
                .Append(estimation.totalPrice)
                .Append("\n");
            return result.ToString();
        }
    }
}
