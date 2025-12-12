using System;
using UnityEngine;

public class FactoryMonoObject<T> : IFactory<T>
{
    private readonly GameObject _prefab;
    private Transform _parent;

    public FactoryMonoObject(GameObject prefab, Transform parent)
    {
      
        _parent = parent;
        _prefab = prefab;
        var newParent = new GameObject();
        newParent.transform.parent = parent;
        _parent = newParent.transform;
        _parent.name = _prefab.name;
    }

    public T CreatePoolObject()
    {
        var newObject = GameObject.Instantiate(_prefab,_parent);

        var returnValue = newObject.GetComponent<T>();
        newObject.SetActive(false);
        if (returnValue != null)
        {
            return returnValue;
        }
        else
        {
            throw new InvalidOperationException(
                $"The requested object is missing from the prefab {typeof(T)} >> {_prefab.name}");
        }
    }
}