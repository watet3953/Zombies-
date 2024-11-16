using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary> Prefab to make new copies off of. </summary>
    public T prefab;

    /// <summary> The initial size of the object pool. </summary>
    public int poolSize = 16;
    
    /// <summary> The data structure to track what pool objects are in use. </summary>
    public HashSet<T> pool = new();

    /// <summary> Holds all available items in pool. </summary>
    public Queue<T> available = new();

    private void Awake()
    {
        // initial pool
        for (int i = 0; i < poolSize; i++)
        {
            CreatePooledObject();
        }
    }

    private void CreatePooledObject()
    {
        T obj = Instantiate(prefab, transform);
        obj.enabled = false;

        pool.Add(obj);

        available.Enqueue(obj);
    }

    protected abstract void OnGet(T obj);

    public T Get()
    {
        return Get(null);
    }

    public T Get(Vector3? pos)
    {

        if (available.Count <= 0)
        {
            CreatePooledObject(); // FIXME: this could probably be replaced with spaced expansions (i.e double the size of the pool each time or something)
        }
        T obj = available.Dequeue();
        if (pos != null) obj.transform.position = (Vector3)pos;
        OnGet(obj);
        return obj;
    }

    protected abstract void OnReturn(T obj);

    public void ReturnToPool(T obj)
    {
        if (!pool.Contains(obj))
        {
            Debug.LogError("Object returned to pool does not belong to it.");
            return;
        }
        OnReturn(obj);
        available.Enqueue(obj);
    }
}
