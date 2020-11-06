using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColliderType
{
    COLLIDER_0,
    COLLIDER_1,
    INSTANT_DIE,
}

public class BaseObjectCollider : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    private ColliderType _type;
    public ColliderType type
    {
        get
        {
            return this._type;
        }
        set
        {
            this._type = value;
        }
    }
}
