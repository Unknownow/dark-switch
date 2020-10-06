using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    [SerializeField]
    private LayerMask _objectMask;

    // ========== Constructors ==========
    private void Update()
    {
        CheckPlayerCollision();
    }

    // ========== Private Methods ==========
    private void CheckPlayerCollision()
    {
        bool hasWhite = IsWhiteColliderAvailable();
        if (!hasWhite)
            LogUtils.instance.Log(GetClassName(), "NO WHITE AVAILABLE");
        else
            LogUtils.instance.Log(GetClassName(), "HAS WHITE");

    }

    private bool IsWhiteColliderAvailable()
    {
        bool hasWhiteFlag = false;
        int maxSortingOrder = -1;
        Collider2D[] collidersList = Physics2D.OverlapBoxAll(transform.position, new Vector2(1, 1), 0, _objectMask);

        if (collidersList.Length > 0)
        {
            foreach (Collider2D collider in collidersList)
            {
                BaseObject baseObject = collider.transform.GetComponent<BaseObject>();
                if (baseObject)
                {
                    if (maxSortingOrder < baseObject.sortingOrder)
                    {
                        hasWhiteFlag = false;
                        if (baseObject.currentState == ObjectState.WHITE)
                            hasWhiteFlag = true;
                    }
                }

            }
        }
        return hasWhiteFlag;
    }
}
