using System.Collections;
using UnityEngine;

public class IntroPuttingTextNode : NodeBeh
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public IntroPuttingText ipt;
    public override void Init(params object[] vs)
    {
        AddParameter(0, "Step", NodeParameterTypePS, vs);
        
    }

    public override void OnStart()
    {
        ipt = GetComponent<IntroPuttingText>();
        
    }

    public override void OnUpdate()
    {
       
    }
    public override IEnumerator ActivatorStart()
    {
        OnStart();
        switch (TaskUpdate())
        {

            case TaskResult.COMPLETE:
                yield return ipt.SetSplit(InterGetParameter<int>("Step"));
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

                break;
        }


    }
    public override TaskResult TaskUpdate()
    {
        return TaskResult.COMPLETE;
    }
    public static NodeBeh AddNode<T>(T value, BehaviorExecutor be, int step) where T : NodeBeh
    {
        T node = be.gameObject.AddComponent<T>();
        node.InitBase(be.tree, be.nodeIstance, step);

        be.nodeIstance.ReParent(node);
        be.nodes.Add(node);
        return node;

    }
    public static NodeBeh AddNode<T>(T value, BehaviorExecutor be, NodeParameter step) where T : NodeBeh
    {
        T node = be.gameObject.AddComponent<T>();
        node.InitBase(be.tree, be.nodeIstance, step);

        be.nodeIstance.ReParent(node);
        be.nodes.Add(node);
        return node;

    }
    public IntroPuttingTextNode(BehaviorExecutor be,int step)
    {
        AddNode(this, be, step);
    }
    public IntroPuttingTextNode(BehaviorExecutor be, NodeParameter step)
    {
        AddNode(this, be, step);
    }
}
