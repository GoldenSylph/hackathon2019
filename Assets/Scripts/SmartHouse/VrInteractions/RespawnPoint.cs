using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace SmartHouse.VrInteractions
{
    public class RespawnPoint : MonoBehaviour
    {
        public GameObject toRespawn;
        public float distanceToRespawn;
        public float respawnTime;
        private void Start()
        {
            StartCoroutine(RespawnCoroutine());
        }

        private IEnumerator RespawnCoroutine()
        {
            while (true)
            {
                if (Vector3.Distance(toRespawn.transform.position, transform.position) >= distanceToRespawn)
                {
                    Debug.Log($"Respawning {toRespawn.name}");
                    toRespawn = Instantiate(toRespawn);
                    toRespawn.transform.position = transform.position;
                }
                yield return new WaitForSeconds(respawnTime);
            }
        }
    }
}
