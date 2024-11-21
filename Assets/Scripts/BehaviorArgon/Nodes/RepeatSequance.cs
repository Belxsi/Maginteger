using System.Collections;
using UnityEngine;

public class RepeatSequance : NodeBeh
{

    public NodeParameter current_count;
    public override void Init(params object[] vs)
    {
        AddParameter(0, "Infinity",BoolTypePS, vs);
        AddParameter(1, "Count",IntTypePS, vs);
        current_count = new(0, NodeParameterTypePS, this);
    }
    public bool infinity;
    public int count;

    

    public override void OnStart()
    {
        infinity = GetParameter<bool>("Infinity");
        count = GetParameter<int>("Count");
        current_count.SetValue(count,IntTypePS);
    }

    public override void OnUpdate()
    {
        return;
    }
    public override IEnumerator ActivatorStart()
    {
        OnStart();
        switch (TaskUpdate())
        {

            case TaskResult.COMPLETE:
                if (infinity)
                {
                    while (infinity)
                     {
                        OnStart();
                        foreach (var node in nodes)
                        {
                            yield return StartCoroutine(node.ActivatorStart());
                        }
                    }
                }
                else
                {
                    for(int i = 0; i < count; i++)
                    {
                        foreach (var node in nodes)
                        {
                            yield return StartCoroutine(node.ActivatorStart());
                        }
                        count--;
                        i = -1;
                        current_count.SetValue(count);
                    }
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
    
    public static NodeBeh AddNode<T>(T value, BehaviorExecutor be, bool infinity, int Count = 0) where T : NodeBeh 
    {
        T node = be.gameObject.AddComponent<T>();
        node.InitBase(be.tree, be.nodeIstance, infinity, Count);

        be.nodeIstance.ReParent(node);
        be.nodes.Add(node);
        return node;

    }
    public RepeatSequance(BehaviorExecutor be, bool infinity, out RepeatSequance me, int Count = 0)
    {
        me= (RepeatSequance)AddNode(this,be, infinity, Count);
    }

}
