using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private int _currentMovingFingerId = -1;

    // ========== MonoBehaviour Methods ==========
    void Start()
    {
        _currentMovingFingerId = -1;
    }

    void Update()
    {
        GetPlayerInput();
        GetMouseInput();
    }

    // ========== Private Methods ==========
    private void GetMouseInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mousePosition = Input.mousePosition;
            if (mousePosition.y >= Screen.height / 2)
            {
                EventSystem.instance.DispatchEvent(EventCode.ON_TRANSFORM_CLICK, new object[] { });
            }
        }
    }

    /// <summary>
    /// Get player input and dispatch it.
    /// </summary>
    private void GetPlayerInput()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.position.y >= Screen.height / 2)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        EventSystem.instance.DispatchEvent(EventCode.ON_TRANSFORM_TOUCH, new object[] { touch });
                        break;
                    }
                }
            }

            if (_currentMovingFingerId == -1)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (touch.position.y < Screen.height / 2)
                    {
                        _currentMovingFingerId = touch.fingerId;
                        break;
                    }
                }
            }

            if (_currentMovingFingerId >= 0)
            {
                try
                {
                    Touch currentTouch = TouchUtils.instance.GetTouchByFingerID(_currentMovingFingerId);
                    OnTouch(currentTouch);
                }
                catch (UnityException e)
                {
                    LogUtils.instance.Log(e.Message);
                }
            }
        }
    }

    private void OnTouch(Touch touch)
    {
        EventSystem.instance.DispatchEvent(EventCode.ON_MOVING_TOUCH, new object[] { touch });
        if (touch.phase == TouchPhase.Ended)
            _currentMovingFingerId = -1;
    }
}
