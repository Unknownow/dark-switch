using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private int _currentFingerId = -1;
    private Vector3 _lastTouchPosition = Vector3.zero;

    // ========== Constructors ==========
    void Start()
    {
        _lastTouchPosition = Vector3.zero;
        _currentFingerId = -1;
    }

    void Update()
    {
        movementControl();
    }

    // ========== Methods ==========
    private void movementControl()
    {
        if (Input.touchCount > 0)
        {
            if (_currentFingerId == -1)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (touch.position.y < Screen.height / 2)
                    {
                        _currentFingerId = touch.fingerId;
                        break;
                    }
                }
            }
            if (_currentFingerId >= 0)
            {
                try
                {
                    Touch currentTouch = TouchUtils.instance.getTouchByFingerID(_currentFingerId);
                    onTouch(currentTouch);
                }
                catch (UnityException e)
                {
                    LogUtils.instance.Log(e.Message);
                }
            }
        }
    }

    private void onTouch(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                onTouchBegan(touch);
                return;
            case TouchPhase.Ended:
                onTouchEnded(touch);
                return;
            case TouchPhase.Moved:
                onTouchMoved(touch);
                return;
        }
    }

    private void onTouchBegan(Touch touch)
    {
        Vector3 currentTouchPosition = touch.position;
        currentTouchPosition = Camera.main.ScreenToWorldPoint(currentTouchPosition);
        currentTouchPosition.z = 0;
        _lastTouchPosition = currentTouchPosition;
    }

    private void onTouchMoved(Touch touch)
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

    private void onTouchEnded(Touch touch)
    {
        _lastTouchPosition = Vector3.zero;
        _currentFingerId = -1;
        return;
    }
}
