using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SendMessageNode : NodeBeh
{
   

    public override void OnStart()
    {
        object text = GetParameter("Text",null).GetValue();
        string result = "";
        if (text.GetType().FullName=="NodeParameter")
        {
            result = ((INodeParameter)text).GetValue() + "";
        }
        else
        {
            result = text.ToString();
        }
        Debug.Log(result);
    }

    public override void OnUpdate()
    {
    }

    public override TaskResult TaskUpdate()
    {
        return TaskResult.COMPLETE;
    }
    
    public override void Init(params object[] vs)
    {
        AddParameter(0, "Text",StringTypePS, vs);
    }
}
