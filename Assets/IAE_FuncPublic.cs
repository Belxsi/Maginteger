using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAE_FuncPublic : MonoBehaviour
{

    public InterfaceAudioEffect iae;
    public AudioClip current;
    public bool loop, replace;
    public bool active;
   
    void Update()
    {
        if (active)
        {
            iae.AudioAwake(current, loop, replace);
            active = false;
        }
    }
}
