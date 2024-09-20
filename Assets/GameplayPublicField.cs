using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayPublicField : MonoBehaviour
{
    public static GameObject me;
    void Awake()
    {
        me = gameObject;
    }
    public static Transform BigFather()
    {
        return me.transform;
    }
    
   
}
