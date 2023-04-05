using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling
{
    public List<GameObject> pooledList = new List<GameObject>();

    public void AddObj(GameObject obj)
    {
        pooledList.Add(obj);
    }

    public void RemoveObj(GameObject obj)
    {
        pooledList.Remove(obj);
    }

    public int ListCount()
    {
        return pooledList.Count;
    }

    public GameObject FirstObj()
    {
        if (ListCount() > 0)
            return pooledList[0];
        else
            return null;
    }
}
