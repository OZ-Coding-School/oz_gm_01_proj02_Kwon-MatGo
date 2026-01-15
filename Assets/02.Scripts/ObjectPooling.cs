using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling<T> where T : Component
{
    private readonly T prefab;
    private readonly Transform parent;

    private readonly Queue<T> pool = new Queue<T>();
    private readonly HashSet<T> activeSet = new HashSet<T>();

    public ObjectPooling(T prefab, Transform parent, int preload = 0)
    {
        this.prefab = prefab;
        this.parent = parent;

        for(int i = 0; i<preload; i++)
        {
            var obj = CreateNew();
            pool.Enqueue(obj); 
        }
    }

    private T CreateNew()
    {
        var obj = Object.Instantiate(prefab, parent);
        obj.gameObject.SetActive(false);
        return obj;
    }

    public T Get()
    {
        T obj = pool.Count > 0 ? pool.Dequeue() : CreateNew();
        obj.gameObject.SetActive(true);
        activeSet.Add(obj);
        return obj;
    }

    public void Release(T obj)
    {
        if (obj == null) return;
        if (!activeSet.Remove(obj)) return;

        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }

    public void ReleaseAll()
    {
        foreach (var obj in activeSet)
        {
            if (obj != null)
            {
                obj.gameObject.SetActive(false);
                pool.Enqueue(obj);
            }
        }
        activeSet.Clear();
    }
}