using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundObject : BaseObject
{
    // ========== Fields and properties ==========

    // ========== MonoBehaviour Methods ==========
    protected override void Awake()
    {
        InitObject();
        this._type = ObjectType.BACK_GROUND;
    }

    // ========== Public Methods ==========
    public void TransformRandomlyToStateWithAllowedCollider(List<ColliderType> allowedColliderList)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            BaseObjectState objectState = transform.GetChild(i).GetComponent<BaseObjectState>();
            if (objectState == null)
                continue;
            if (objectState.IsHavingAllowedCollider(allowedColliderList))
            {
                TransformState((ObjectState)i);
                if (Random.Range(0, 1) > 0.5)
                    return;
            }
        }
    }

    public void TransformRandomlyToStateWithUnallowedCollider(List<ColliderType> allowedColliderList)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            BaseObjectState objectState = transform.GetChild(i).GetComponent<BaseObjectState>();
            if (objectState == null)
                continue;
            if (!objectState.IsHavingAllowedCollider(allowedColliderList))
            {
                TransformState((ObjectState)i);
                if (Random.Range(0, 1) > 0.5)
                    return;
            }
        }
    }
}
