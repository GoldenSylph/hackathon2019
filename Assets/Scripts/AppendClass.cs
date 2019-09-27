using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Assets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AppendClass : MonoBehaviour
{
    private List<Product> listik;
    public GameObject grid;
    public GameObject itemExample;
    // Start is called before the first frame update
    public void Append(List<Product> list)
    {
        listik = list;
    }

    void Start()
    {

        List<Product> b= new List<Product>();
        ProductList.FillList(ref b);
        Append(b);
        listik.ForEach(x =>
        {
            var temp = Instantiate(itemExample);
            temp.transform.Find("Price").GetComponent<Text>().text = x.Price.ToString(CultureInfo.InvariantCulture);
            temp.transform.Find("Name").GetComponent<Text>().text = x.Name;
            temp.transform.Find("Discription").GetComponent<Text>().text = x.Description;
            temp.transform.SetParent(grid.transform);

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
