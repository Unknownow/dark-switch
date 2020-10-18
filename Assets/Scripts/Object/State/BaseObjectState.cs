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
    [SerializeField]
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

    // ========== Fields and properties ==========
    public bool IsHavingAllowedCollider(List<ColliderType> allowedCollidersList)
    {
        bool isHavingAllowedCollider = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            Collider2D collider = transform.GetChild(i).GetComponent<Collider2D>();
            if (collider == null)
                continue;

            BaseObjectCollider baseObjectCollider = transform.GetChild(i).GetComponent<BaseObjectCollider>();
            if (baseObjectCollider == null)
                continue;

            foreach (ColliderType type in allowedCollidersList)
            {
                if (type == baseObjectCollider.type)
                {
                    isHavingAllowedCollider = true;
                    break;
                }
            }
        }
        return isHavingAllowedCollider;
    }
}
