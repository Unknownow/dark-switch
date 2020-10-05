using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private EventListener[] _eventListener = null;
    private Vector3 _lastTouchPosition = Vector3.zero;

    // ========== Constructors ==========
    void Start()
    {
        _lastTouchPosition = Vector3.zero;
        AddListeners();
    }

    private void AddListeners()
    {
        _eventListener = new EventListener[1];
        _eventListener[0] = EventSystem.instance.AddListener(EventCode.ON_MOVING_TOUCH, this, OnMovingTouch);
    }

    private void RemoveListeners()
    {
        foreach (EventListener listener in _eventListener)
        {
            EventSystem.instance.RemoveListener(listener.eventCode, listener);
        }
    }

    // ========== Methods ==========

    private void OnMovingTouch(object[] eventParam)
    {
        Touch touch = (Touch)eventParam[0];
        switch (touch.phase)
        {
            case TouchPhase.Began:
                OnTouchBegan(touch);
                break;
            case TouchPhase.Moved:
                OnTouchMoved(touch);
                break;
            case TouchPhase.Ended:
                OnTouchEnded(touch);
                break;
        }
    }

    private void OnTouchBegan(Touch touch)
    {
        Vector3 currentTouchPosition = touch.position;
        currentTouchPosition = Camera.main.ScreenToWorldPoint(currentTouchPosition);
        currentTouchPosition.z = 0;
        _lastTouchPosition = currentTouchPosition;
    }

    private void OnTouchMoved(Touch touch)
    {
        Vector3 delta = touch.deltaPosition;
        if (delta.sqrMagnitude > 20)
        {
            Vector3 currentTouchPosition = touch.position;
            currentTouchPosition = Camera.main.ScreenToWorldPoint(currentTouchPosition);
            currentTouchPosition.z = 0;
            transform.position += currentTouchPosition - _lastTouchPosition;
            _lastTouchPosition = currentTouchPosition;
        }
    }

    private void OnTouchEnded(Touch touch)
    {
        _lastTouchPosition = Vector3.zero;
        return;
    }
}
