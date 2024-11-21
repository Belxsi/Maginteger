using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakerRepeatNode : NodeBeh
{
    public override void Init(params object[] vs)
    {
        AddParameter(0, "Repeater", NodeBehTypePS, vs);
    }

    public override void OnStart()
    {
        RepeatSequance repeatSequance = InterGetParameter<RepeatSequance>("Repeater");
        repeatSequance.SetParameter("Infinity", false, BoolTypePS);

    }

    public override void OnUpdate()
    {
        
    }

    public override TaskResult TaskUpdate()
    {
        return TaskResult.COMPLETE;
    }
    public static NodeBeh AddNode<T>(T value, BehaviorExecutor be, RepeatSequance rs) where T : NodeBeh
    {
        T node = be.gameObject.AddComponent<T>();
        node.InitBase(be.tree, be.nodeIstance, rs);

        be.nodeIstance.ReParent(node);
        be.nodes.Add(node);
        return node;

    }
    public BreakerRepeatNode(BehaviorExecutor be, RepeatSequance rs,out BreakerRepeatNode me)
    {
       me= (BreakerRepeatNode)AddNode(this, be, rs);
    }
}
