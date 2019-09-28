using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace SmartHouse.DataManagement
{
    public class AppendClass : MonoBehaviour
    {
        public GameObject grid;
        public GameObject itemExample;

        private void Start()
        {
            Append(new Dto
            {
                products = new List<Product>
                {
                    new Product
                    {
                        available = 0, 
                        description = "desc", 
                        name = "name",
                        position = Vector3.zero, 
                        price = 40f
                    }
                }
            });
        }

        public void Append(Dto dto)
        {
            dto.products.ForEach(x =>
            {
                var temp = Instantiate(itemExample, grid.transform, false);
                temp.transform.Find("Price").GetComponent<Text>().text = x.price.ToString(CultureInfo.InvariantCulture);
                temp.transform.Find("Name").GetComponent<Text>().text = x.name;
                temp.transform.Find("Description").GetComponent<Text>().text = x.description;
            });
        }
    }
}
