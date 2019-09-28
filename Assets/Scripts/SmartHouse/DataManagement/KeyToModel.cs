using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SmartHouse.DataManagement
{
    public class KeyToModel : MonoBehaviour
    {
        [Serializable]
        public class Couple
        {
            public GameObject model;
            public string key;
        }
        
        public List<Couple> modelList;

        public GameObject GetGameObjectByKey(string key)
        {
            var result = modelList.Where(x => x.key.Equals(key));
            var enumerable = result as List<Couple> ?? result.ToList();
            return !enumerable.Any() ? new GameObject("Cube") : enumerable.First().model;
        }
    }
}
