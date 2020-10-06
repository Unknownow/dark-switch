using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: WRITE CODE
public class ObjectSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ObjectPool.instance.GetObject(ObjectType.BACK_GROUND).isObjectActive = true;
        }
    }
}
