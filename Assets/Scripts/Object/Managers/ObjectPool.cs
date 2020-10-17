using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private static ObjectPool _instance;
    public static ObjectPool instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField]
    private List<GameObject> _prefabsList = new List<GameObject>();
    private List<BaseObject> _pool;
    private ObjectPool()
    {
        if (_instance == null)
            _instance = this;
        _pool = new List<BaseObject>();
    }

    // ========== Public Methods ==========
    public BaseObject GetObject(ObjectType type)
    {
        BaseObject baseObject = null;
        foreach (BaseObject obj in _pool)
        {
            LogUtils.instance.Log(GetClassName(), "GET_OBJECT", obj.isObjectActive.ToString());
            if (obj.type == type && obj.isObjectActive == false)
            {
                LogUtils.instance.Log(GetClassName(), "GetObject", obj.isObjectActive.ToString());
                baseObject = obj;
                break;
            }
        }
        if (baseObject == null)
            baseObject = CreateObject(type);
        baseObject.transform.position = new Vector3(-100, -100, 0);
        baseObject.isObjectActive = true;
        return baseObject;
    }

    public void DestroyObject(BaseObject baseObject)
    {
        baseObject.isObjectActive = false;
    }

    // ========== Private Methods ==========
    private BaseObject CreateObject(ObjectType type, Transform parent = null)
    {
        LogUtils.instance.Log(GetClassName(), "CreateObject");
        GameObject objectPrefab = null;
        BaseObject createdObject = null;

        LogUtils.instance.Log(GetClassName(), "_prefabsList length", (_prefabsList.Count).ToString());
        foreach (GameObject prefab in _prefabsList)
        {
            BaseObject baseObjectComponent = prefab.GetComponent<BaseObject>();
            LogUtils.instance.Log(GetClassName(), "baseObjectComponent", (baseObjectComponent.type).ToString());
            if (baseObjectComponent != null && baseObjectComponent.type == type)
            {
                LogUtils.instance.Log(GetClassName(), "CreateObject", (baseObjectComponent.type == type).ToString());
                objectPrefab = prefab;
                break;
            }
        }
        LogUtils.instance.Log(GetClassName(), "CreateObject", "(objectPrefab != null)", (objectPrefab != null).ToString());
        if (objectPrefab != null)
        {
            if (parent == null)
                parent = _instance.transform;
            createdObject = GameObject.Instantiate(objectPrefab, Vector3.zero, Quaternion.identity, parent).GetComponent<BaseObject>();
            if (createdObject != null)
            {
                LogUtils.instance.Log(GetClassName(), "CreateObject", "(createdObject != null)", (createdObject != null).ToString());
                createdObject.isObjectActive = false;
                _pool.Add(createdObject);
            }
        }
        return createdObject;
    }
}
