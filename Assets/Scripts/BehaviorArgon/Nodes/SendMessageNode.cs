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
    public static NodeBeh AddNode<T>(T value, BehaviorExecutor be, string text) where T : NodeBeh
    {
        T node = be.gameObject.AddComponent<T>();
        node.InitBase(be.tree, be.nodeIstance, text);

        be.nodeIstance.ReParent(node);
        be.nodes.Add(node);
        return node;

    }
    public SendMessageNode(BehaviorExecutor be, string text)
    {
        AddNode(this, be, text);
    }
}
