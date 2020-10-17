using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }
}
