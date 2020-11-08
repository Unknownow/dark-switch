using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateSortingLayerOrder
{
    HIDDEN_BACK_GROUND,
    VISIBLE_BACK_GROUND,
    HIDDEN_STATE,
    VISIBLE_STATE,
}

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

    private BaseObject _baseObject;

    // ========== MonoBehaviour Methods ==========
    private void Awake()
    {
        _baseObject = transform.parent.GetComponent<BaseObject>();
    }


    // ========== Public methods ==========
    public virtual void TransformState(bool isMainState)
    {
        int sortingOrder = (int)StateSortingLayerOrder.VISIBLE_STATE;
        SpriteMaskInteraction maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        if (isMainState)
        {
            sortingOrder = (int)StateSortingLayerOrder.VISIBLE_STATE;
            maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
        else
        {
            sortingOrder = (int)StateSortingLayerOrder.HIDDEN_STATE;
            maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }

        ChangeSortingOrder(sortingOrder);
        ChangeMaskInteraction(maskInteraction);
    }

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

    // ========== Protected methods ==========
    protected void ChangeSortingOrder(int sortingOrder)
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        if (renderer != null)
            renderer.sortingOrder = sortingOrder;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                spriteRenderer.sortingOrder = sortingOrder;
        }
    }

    protected void ChangeMaskInteraction(SpriteMaskInteraction maskInteraction)
    {

        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        if (renderer != null)
            renderer.maskInteraction = maskInteraction;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                spriteRenderer.maskInteraction = maskInteraction;
        }
    }
}
