using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IPoolItem
{
    bool IsActive { get; }
    void Deactivate();
}

public interface IObjectPool<T> where T : IPoolItem
{
    List<T> InitializePool(Transform parent, string prefabPath, int poolSize);
    T GetFromPool();
    void ReturnToPool(T item);
}

public class ObjectPool<T> : IObjectPool<T> where T : MonoBehaviour, IPoolItem
{
    private Queue<T> _pool = new Queue<T>();
    private Transform _parent;
    private GameObject _prefab;
    private DiContainer _diContainer;

    public ObjectPool(DiContainer diContainer)
    {
        _diContainer = diContainer;
    }

    public List<T> InitializePool(Transform parent, string prefabPath, int poolSize)
    {
        _parent = parent;
        _prefab = Resources.Load<GameObject>(prefabPath);
        if (_prefab == null)
        {
            Debug.LogError($"Prefab not found at path: {prefabPath}");
            return null;
        }

        var items = new List<T>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject instance = _diContainer.InstantiatePrefab(_prefab, parent);
            T item = instance.GetComponent<T>();
            if (item == null)
            {
                Debug.LogError($"Prefab does not have component of type {typeof(T)}");
                continue;
            }
            instance.SetActive(false);
            _pool.Enqueue(item);
            items.Add(item);
        }

        return items;
    }

    public T GetFromPool()
    {
        if (_pool.Count > 0)
        {
            var item = _pool.Dequeue();
            item.gameObject.SetActive(true);
            return item;
        }

        Debug.LogWarning("Pool is empty! Consider increasing the pool size.");
        return null;
    }

    public void ReturnToPool(T item)
    {
        item.Deactivate();
        item.gameObject.SetActive(false);
        _pool.Enqueue(item);
    }
}
