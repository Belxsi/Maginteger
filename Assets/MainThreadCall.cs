using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MainThreadCall : MonoBehaviour
{
    public MainSaverForThreading msft;
    public Mattery magic;
    public void LocalAwake()
    {
        msft = new();
        msft.me = new(gameObject);
    }
    public void LocalUpdate()
    {
        if(magic.magic!=null)
        msft.magic = new(magic.magic);
        msft.Update();
    }
}
