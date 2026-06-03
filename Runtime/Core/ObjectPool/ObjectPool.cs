using System.Collections.Generic;
using UnityEngine;

namespace RKode.Utils {
public class ObjectPool<T> where T : Object {
    private Queue<T> _itemPool = new();
    private List<T> _activeItems = new();
    private Transform _container;
    private T _prefab;

    public int ActiveItemCount => _activeItems.Count;
    public int PooledItemCount => _itemPool.Count;

    public int _itemCreationCount = 0;

    public ObjectPool(T prefab, Transform container, int preloadCount = 0) {
        _prefab = prefab;
        _container = container;

        Preload(preloadCount);
    }

    private void Preload(int count) {
        for(int i=0; i < count; i++) {
            ReturnToPool(CreateNew());
        }
    }

    public T CreateNew() {
        var newItem = Object.Instantiate(_prefab, _container);
        newItem.name = $"{_prefab.name}_{_itemCreationCount++}";
        return newItem;
    }

    public T Get() {
        var item = _itemPool.Count > 0 ? 
            _itemPool.Dequeue() : CreateNew();

        _activeItems.Add(item);

        var go = GetGameObject(item);
        go.SetActive(true);
        NotifyPoolable(item, true);

        return item;
    }

    public void ReturnToPool(T item) {
        if(_activeItems.Contains(item)) {
            _activeItems.Remove(item);
        }

        NotifyPoolable(item, false);

        var go = GetGameObject(item);
        go.SetActive(false);

        _itemPool.Enqueue(item);
    }

    public void ReturnAll() {
        if(_activeItems.Count == 0) {
            return;
        }

        while (_activeItems.Count > 0)
            ReturnToPool(_activeItems[0]);
    }
    
    private GameObject GetGameObject(T item) {
        if (item is GameObject go) return go;
        if (item is Component comp) return comp.gameObject;
        return null;
    }

    private void NotifyPoolable(T item, bool spawning) {
        // Check if the T itself is IPoolable (like a Component)
        // Or if the GameObject has an IPoolable component attached
        IPoolable poolable = item as IPoolable;
        
        if (poolable == null && item is GameObject go) {
            poolable = go.GetComponent<IPoolable>();
        }

        if (poolable != null) {
            if (spawning) poolable.OnSpawn();
            else poolable.OnDespawn();
        }
    }
}
}