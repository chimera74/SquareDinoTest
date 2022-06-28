using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int maxSize = 10;
    public int startingSize = 10;
    
    private Dictionary<GameObject, bool> pool;

    public void Awake()
    {
        pool = new Dictionary<GameObject, bool>();
        for (int i = 0; i < startingSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj, false);
        }
    }

    public GameObject Get()
    {
        foreach (KeyValuePair<GameObject, bool> entry in pool)
        {
            if (!entry.Value)
            {
                pool[entry.Key] = true;
                entry.Key.SetActive(true);
                return entry.Key;
            }
        }
        if (pool.Count < maxSize)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj, false);
            return obj;
        } else {
            Debug.Log("Pool is full");
            return null;
        }
    }
    
    public void Release(GameObject obj)
    {
        if (pool.ContainsKey(obj))
        {
            pool[obj] = false;
            obj.SetActive(false);
        }
    }

    public void OnDestroy()
    {
        foreach (KeyValuePair<GameObject, bool> entry in pool)
        {
            Destroy(entry.Key);
        }
        pool.Clear();
    }
}
