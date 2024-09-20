using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]

public abstract class BehaviorExecutor:MonoBehaviour
{
    public TreeBehaviour tree;
    public NodeIstance nodeIstance;
    public bool active;
    
    
    
    public List<NodeBeh> nodes = new();
    
    public abstract void InitConstruct();
    public abstract NodeBeh R();
    public virtual void Awake()
    {
        tree = new(R(), this);
        InitConstruct();
        

    }
    public T AddNode<T>(NodeIstance ni,bool reparent, params object[] vs) where T : NodeBeh
    {
        
        
        T node = gameObject.AddComponent<T>();
        node.InitBase(tree, ni, vs);
        if(reparent)
        ni.ReParent(node);
        nodes.Add(node);
        return node;
    }
    public void AddNode(NodeBeh node,NodeIstance ni, params object[] vs)
    {


      
        node.InitBase(tree, ni, vs);
        ni.ReParent(node);
        nodes.Add(node);
    }
    public virtual void Update()
    {
       

        if (active)
            tree.OnInterpreter();
    }

}
[Serializable]

public enum TaskResult
{
    COMPLETE,
    PROCESS,
    ERROR
}
