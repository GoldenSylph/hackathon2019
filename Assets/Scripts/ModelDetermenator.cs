using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.WSA;

public class ModelDetermenator : MonoBehaviour
{
    [Serializable]
    public class Couple
    {
        public GameObject model;
        public string key;
    }
    // Start is called before the first frame update\
    public List<Couple> modelList;

    public GameObject GetGameObjectByKey(string key)
    {
        var result = modelList.Where(x => x.key.Equals(key));
        var enumerable = result as List<Couple> ?? result.ToList();
        return !enumerable.Any() ? new GameObject("Cube") : enumerable.First().model;
    }
}
