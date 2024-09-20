using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBodyExtract : MonoBehaviour
{
    public BodyObject bo;
    void Start()
    {
        bo.InitThisParameter(new(1, 1000, 0.01f, 0.99f, BodyObject.TypeExtract.random));
        bo.AwakeExtract();
    }

    // Update is called once per frame
    void Update()
    {
        if (bo.currentState != BodyObject.ExtractState.Good)
        {
            bo.UpdateExtract();
        }
    }
}
