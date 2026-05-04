using UnityEngine;
using System.Collections.Generic;

public abstract class ObjectPool<T> : MonoBehaviour where T : Component
{
    private Dictionary<T, Queue<T>> _poolDictionary = new Dictionary<T, Queue<T>>();

    public T GetFromPool(T prefab, Vector3 position, Quaternion rotation)
    {
        if (!_poolDictionary.ContainsKey(prefab))
        {
            _poolDictionary[prefab] = new Queue<T>();
        }

        Queue<T> pool = _poolDictionary[prefab];
        T instance;

        if ( pool.Count > 0)
        {
            instance = pool.Dequeue();
        }
        else
        {
            instance = Instantiate(prefab, transform);
        }

        instance.transform.SetLocalPositionAndRotation(position, rotation);
        instance.gameObject.SetActive(true);
        return instance;
    }

    public void ReturnToPool(T prefab, T instance)
    {
        instance.gameObject.SetActive(false);
        _poolDictionary[prefab].Enqueue(instance);
    }
}
