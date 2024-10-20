using System;
using System.Collections;
using UnityEngine;

public class SpawningComponent : MonoBehaviour
{
    [SerializeField] private GameObject spawnObjectPrefab;
    
    [SerializeField] private float destroyAfterSeconds = 0f;

    public GameObject Spawn(Vector3 spawnPosition, GameObject parent = null)
    {
        GameObject newObject = Instantiate(spawnObjectPrefab, spawnPosition, Quaternion.identity);
        if (parent != null)
        {
            newObject.transform.SetParent(parent.transform);
        }

        if (destroyAfterSeconds > 0f)
        {
            StartCoroutine(Despawn(newObject));
        }

        return newObject;
    }

    IEnumerator Despawn(GameObject despawnee)
    {
        yield return new WaitForSeconds(destroyAfterSeconds);
        Destroy(despawnee);
    }
}