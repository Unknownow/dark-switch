using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    EventListener test;
    // Start is called before the first frame update
    void Start()
    {
        test = EventManager.instance.AddListener(EventCode.TESTING_EVENT_1, this, testAction);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventManager.instance.DispatchEvent(EventCode.TESTING_EVENT_1, new object[] { Vector3.zero });
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            EventManager.instance.RemoveListener(EventCode.TESTING_EVENT_1, test);
        }
    }

    void testAction(object[] eventParam)
    {
        Debug.Log("TESTING TESTING!");
        Debug.Log(eventParam[0]);
    }
}
