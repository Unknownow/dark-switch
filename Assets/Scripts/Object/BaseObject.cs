using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectState
{
    BLACK = 0,
    WHITE = 1
}

public enum ObjectType
{
    BASE,
    BACK_GROUND,
}

public class BaseObject : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private EventListener[] _eventListener = null;

    [SerializeField]
    private GameObject[] _stateObjectList;

    [SerializeField]
    private ObjectState _currentState = ObjectState.WHITE;
    public ObjectState currentState
    {
        get
        {
            return this._currentState;
        }
        set
        {
            this._currentState = value;
        }
    }

    [SerializeField]
    private int _sortingOrder = 0;
    public int sortingOrder
    {
        get
        {
            return this._sortingOrder;
        }
        set
        {
            this._sortingOrder = value;
            foreach (GameObject child in this._stateObjectList)
            {
                child.GetComponent<SpriteRenderer>().sortingOrder = value;
            }
        }
    }

    [SerializeField]
    private ObjectType _type = ObjectType.BASE;
    public ObjectType type
    {
        get
        {
            return this._type;
        }
    }

    [SerializeField]
    private bool _isObjectActive = false;
    public bool isObjectActive
    {
        get
        {
            return this._isObjectActive;
        }
        set
        {
            this._isObjectActive = value;
            gameObject.SetActive(value);
        }
    }

    // ========== Constructors ==========
    void Start()
    {
        InitObject();
        sortingOrder = _sortingOrder;
        TransformState(_currentState);
    }

    private void InitObject()
    {
        int stateCount = transform.childCount;
        _stateObjectList = new GameObject[stateCount];
        for (int i = 0; i < stateCount; i++)
        {
            _stateObjectList[i] = transform.GetChild(i).gameObject;
        }
        AddListeners();
    }

    private void ClearObject()
    {
        RemoveListeners();
    }

    private void AddListeners()
    {
        _eventListener = new EventListener[1];
        _eventListener[0] = EventSystem.instance.AddListener(EventCode.ON_TRANSFORM_TOUCH, this, OnTransformTouch);
    }

    private void RemoveListeners()
    {
        foreach (EventListener listener in _eventListener)
        {
            EventSystem.instance.RemoveListener(listener.eventCode, listener);
        }
    }

    // ========== Public Methods ==========
    public virtual void TransformState(ObjectState state = ObjectState.WHITE)
    {
        LogUtils.instance.Log(GetClassName(), gameObject.name, "TransformState", state.ToString());
        _currentState = state;
        for (int i = 0; i < _stateObjectList.Length; i++)
            _stateObjectList[i].SetActive(i == (int)_currentState);
    }

    // ========== Private Methods ==========
    private void OnTransformTouch(object[] eventParam)
    {
        Touch touch = (Touch)eventParam[0];
        if (touch.phase == TouchPhase.Began)
        {
            TransformState((ObjectState)(((int)_currentState + 1) % 2));
        }
    }
}
