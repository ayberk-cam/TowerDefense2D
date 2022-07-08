using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject[] objectPrefabs;

    public GameObject GetObject(string type)
    {
        foreach (var t in objectPrefabs)
        {
            if (t.name != type) continue;
            var newObject = Instantiate(t);
            newObject.name = type;
            return newObject;
        }
        return null;
    }

    public void ReleaseObject(GameObject tmpGameObject)
    {
        tmpGameObject.SetActive(false);
        Destroy(tmpGameObject);
    }
}
