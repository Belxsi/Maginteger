using System.Collections;
using UnityEngine;

public class IfNode : Condition
{
    public NodeParameter result;
    public override void Init(params object[] vs)
    {
        AddParameter(0, "Value",BoolTypePS, vs);
        AddParameter(1, "Else", NodeBehTypePS, vs);
        result = new(false,BoolTypePS, this);

    }
    public override bool Check()
    {
        bool r = InterGetParameter<bool>("Value");
        result.SetValue(r);
        return r;
    }
    public override IEnumerator ActivatorStart()
    {
        OnStart();
        switch (TaskUpdate())
        {

            case TaskResult.COMPLETE:
                foreach (var node in nodes)
                {
                    yield return StartCoroutine(node.ActivatorStart());
                }
                break;
            case TaskResult.PROCESS:
                yield return WaitForEndTask(OnUpdate);
                myTree.be.StartCoroutine(ActivatorStart());
                break;
            case TaskResult.ERROR:
                NodeBeh els = GetParameter<NodeBeh>("Else");

                if (els != null)
                {
                    yield return els.ActivatorStart();

                }
                break;
        }
    }
    public static NodeBeh AddNode<T>(T value, BehaviorExecutor be, NodeParameter val,NodeBeh els=null) where T : NodeBeh
    {
        T node = be.gameObject.AddComponent<T>();
        node.InitBase(be.tree, be.nodeIstance, val,els);

        be.nodeIstance.ReParent(node);
        be.nodes.Add(node);
        return node;

    }
    public IfNode(BehaviorExecutor be, NodeParameter val,out IfNode me, NodeBeh els=null)
    {
       me= (IfNode)AddNode(this, be, val,els);
    }
}
