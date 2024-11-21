using System;
using System.Collections;
using UnityEngine;

public class FloatComparisonCond : Condition
{
    public NodeParameter result;
    public override void Init(params object[] vs)
    {
        AddParameter(0, "A",FloatTypePS, vs);
        AddParameter(1, "B", FloatTypePS, vs);
        AddParameter(2, "Type",new() { typeof(TypeComparison)}, vs);
        AddParameter(3, "Else",NodeBehTypePS ,vs);
        result = new(false, BoolTypePS, this);

    }
    public override TaskResult TaskUpdate()
    {
        bool b = Check();
        result.SetValue(b);
        if (b)
        {
            return TaskResult.COMPLETE;
        }
        else
        {
            return TaskResult.ERROR;
        }
    }
    public override bool Check()
    {
        return GetParameter<TypeComparison>("Type") switch
        {
            TypeComparison.Greater => InterGetParameter<float>("A") > InterGetParameter<float>("B"),
            TypeComparison.Less => InterGetParameter<float>("A") < InterGetParameter<float>("B"),
            TypeComparison.Equal => InterGetParameter<float>("A") == InterGetParameter<float>("B"),
            TypeComparison.NotEqual => InterGetParameter<float>("A") != InterGetParameter<float>("B"),
            TypeComparison.GreaterOrEqual => InterGetParameter<float>("A") >= InterGetParameter<float>("B"),
            TypeComparison.LessOrEqual => InterGetParameter<float>("A") <= InterGetParameter<float>("B"),
            _ => false,
        };
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
                
                if (els!= null)
                {
                    yield return els.ActivatorStart();

                }
                else
                {
                    break;
                    foreach (var node in nodes)
                    {
                        yield return StartCoroutine(node.ActivatorStart());
                    }
                }
                break;
        }
    }
    public static NodeBeh AddNode<T>(T value, BehaviorExecutor be,float A,float B,TypeComparison type,NodeBeh Els=null) where T : NodeBeh
    {
        T node = be.gameObject.AddComponent<T>();
        node.InitBase(be.tree, be.nodeIstance,A,B,type,Els);

        be.nodeIstance.ReParent(node);
        be.nodes.Add(node);
        return node;

    }
    public FloatComparisonCond(BehaviorExecutor be, float A, float B, TypeComparison type, NodeBeh Els = null)
    {
        AddNode(this, be, A, B, type, Els);
    }
    public FloatComparisonCond (BehaviorExecutor be, NodeParameter A, NodeParameter B, TypeComparison type, NodeBeh Els = null)
    {
        AddNode(this, be,A,B,type,Els);
}
    public static NodeBeh AddNode<T>(T value, BehaviorExecutor be, NodeParameter A, NodeParameter B, TypeComparison type, NodeBeh Els = null) where T : NodeBeh
    {
        T node = be.gameObject.AddComponent<T>();
        node.InitBase(be.tree, be.nodeIstance,A,B,type,Els);

        be.nodeIstance.ReParent(node);
        be.nodes.Add(node);
        return node;

    }
   
}

public enum TypeComparison
{
    Greater,
    Less,
    Equal,
    NotEqual,
    GreaterOrEqual,
    LessOrEqual
}

