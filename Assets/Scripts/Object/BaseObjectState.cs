using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjectState : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private BaseObject _parentObject;
    public BaseObject parentObject
    {
        get
        {
            return this._parentObject;
        }
    }

    private ObjectState _state;
    public ObjectState state
    {
        get
        {
            return this._state;
        }
        set
        {
            this._state = value;
        }
    }

    // ========== Constructors ==========
    void Start()
    {
        _parentObject = transform.GetComponentInParent<BaseObject>();
    }
}
