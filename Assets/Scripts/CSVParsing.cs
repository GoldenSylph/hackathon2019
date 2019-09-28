using System;
using UnityEngine;
using System.IO;
using System.Threading;
using Boo.Lang;
using SmartHouse.DataManagement;


public class CSVParsing : MonoBehaviour
{
    public static List<Product> list;

    public static string fileName;
    private readonly string _path = Directory.GetParent(Application.dataPath) + "/" + fileName;

    void Start()
    {
        ReadData();
    }

    // Прочитать данные с файла
    public void ReadData()
    {
        var values = File.ReadAllText(_path).Split(',');

        foreach (var value in values)
        {
            var info = value.Split(',');
            var tempProduct = new Product
            {
                available = int.Parse(info[0]),
                description = info[1],
                name = info[2],
                position = Vector3FromString(info[3]),
                price = float.Parse(info[4])
            };
            list.Add(tempProduct);
        }
    }

    public void AddData(Product item)
    {
        string data = $"{item.available},{item.description},{item.name},{Vector3ToString(item.position)},{item.price}";
        File.AppendAllText(_path,data);
    }
    public void AddData(Product[] items)
    {

        foreach (var item in items)
        {
            string data = $"{item.available},{item.description},{item.name},{Vector3ToString(item.position)},{item.price}";
            File.AppendAllText(_path, data);
        }
    }

    // Получить путь к CSV файлу
    private static string GetPath()
    {
        return Application.dataPath;
    }
    //Конвертер 
    private static string Vector3ToString(Vector3 v)
    { 
        return $"{v.x:0.00};{v.y:0.00};{v.z:0.00}";
    }
    //Конвертер
    private static Vector3 Vector3FromString(String s)
    {
        var parts = s.Split(new string[] { ";" }, StringSplitOptions.None);
        return new Vector3(
            float.Parse(parts[0]),
            float.Parse(parts[1]),
            float.Parse(parts[2]));
    }

}