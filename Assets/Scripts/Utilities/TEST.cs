using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CocosAction;

public class TEST : MonoBehaviour
{
    EventListener test;
    Actor actor;
    // Start is called before the first frame update
    void Start()
    {
        // test = EventSystem.instance.AddListener(EventCode.TESTING_EVENT_1, this, testAction);
        actor = transform.GetComponent<Actor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // EventSystem.instance.DispatchEvent(EventCode.TESTING_EVENT_1, new object[] { Vector3.zero });
            StartAction();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            // EventSystem.instance.RemoveListener(EventCode.TESTING_EVENT_1, test);
        }
    }

    void testAction(object[] eventParam)
    {
        Debug.Log("TESTING TESTING!");
        Debug.Log(eventParam[0]);
    }

    public void StartAction()
    {
        ActionInstant seq = new ActionRepeat(new ActionSequence(new ActionInstant[]
        {
            new ActionFadeIn(2),
            new ActionParallel(new ActionInstant[] {
                new ActionMoveBy(new Vector3(1, 1, 0), 1),
                // new ActionRotateBy(new Vector3(90, 90, 0), 1),
                new ActionTintBy(new Vector4(-50, 50, -150), 1)
            }),
            new ActionScaleBy(new Vector3(2, 2, 1), 1),
            new ActionScaleBy(new Vector3(0.5F, 0.5F, 2), 1),
            new ActionDelay(1),
            new ActionBlink(5, 0.1F, 0.4F),
            new ActionDelay(1),
            new ActionDelay(1),
            new ActionJumpBy(new Vector3(-2, 0, 0), 1, 4, 1),
            new ActionJumpTo(new Vector3(2, 2, 2), 1, 3, 1),
            new ActionRotateBy(new Vector3(90, 0, 0), 1),
            new ActionJumpBy(new Vector3(-2, 0, 0), 1, 2, 1),
            new ActionDelay(0.5F),
            new ActionBezierRel(new Vector2 (5, 0), new Vector2(5, -10), new Vector2 (0, -10), 2),
            new ActionScaleTo(new Vector3(2, 2, 2), 1),
            new ActionRotateTo(new Vector3(0, 0, 0), 1),
            new ActionFadeOut(2),
            new ActionSetTint(new Vector4(67, 105, 181)),
            new ActionSendMessage("MessageHello"),
            new ActionSendMessage("MessageHelloTo", "world"),
        }), 5);
        actor.AttachAction(seq);

        actor.AddMethodToCache(MessageHello);
        actor.AddMethodToCache<string>(MessageHelloTo);
    }

    void MessageHello()
    {
        Debug.Log("Hello!");
    }

    void MessageHelloTo(string who)
    {
        Debug.Log("Hello " + who.ToString() + "!");
    }
}
