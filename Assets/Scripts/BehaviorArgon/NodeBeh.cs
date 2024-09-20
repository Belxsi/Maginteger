using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
[Serializable]
public abstract class NodeBeh :MonoBehaviour
{
    public List<NodeBeh> nodes = new();
    public TreeBehaviour myTree;
    public static List<Type> FloatTypePS = new() { typeof(Single) },
        StringTypePS = new() { typeof(string) },
        IntTypePS = new() { typeof(int) },
        BoolTypePS = new() { typeof(bool) },
        GameObjectTypePS= new() { typeof(GameObject) },
        NodeBehTypePS = new() { typeof(NodeBeh) },
        NPCTypePS = new() { typeof(NPCBehaviour),typeof(MasterBehaviourNPC) },
        Vector2TypePS = new() { typeof(Vector2),typeof(Vector3) };
    public NodeBeh parent;
    public Vector2Int IndexPos;
    public Dictionary<string,INodeParameter> In = new();
    public Dictionary<string, INodeParameter> Out = new();
    public int unicalkey;
    public void InitBase(TreeBehaviour myTree,NodeIstance ni,params object[] vs)
    {
        this.myTree = myTree;
        this.parent = ni.parent;
        
        if (parent != null)
        {
           unicalkey= parent.GetHashCode() + GetHashCode() + UnityEngine.Random.Range(int.MinValue, int.MaxValue);

        }
        else
        {
            unicalkey = GetHashCode()+UnityEngine.Random.Range(int.MinValue,int.MaxValue);
        }
        myTree.AddBranch(this);
       
        Init(vs);
      
    }
    public int UnicalKey()
    {
        return unicalkey;
    }
    public void SetParameter(string key, object value,List<Type> ps)
    {
        GetParameter(key,ps).SetValue(value,ps);
    }
    public T GetParameter<T>(string named)
    {
        if (In.TryGetValue(named, out INodeParameter np))
        {
        }
        else
        {
            np = new NodeParameter(new() {typeof(T) },this);
            In.Add(named, np);

        }
        if (np.GetValue() == null)
        {
            return (T)default;
        }
        else
        {
          
          
            return (T)np.GetValue();
        }
    }
   

    public static bool Implements<T>(object Object)
    {
        return typeof(T).IsInstanceOfType(Object);
    }
    public T InterGetParameter<T>(string named) 
    {

        if (In.TryGetValue(named, out INodeParameter np))
        {
            if (Implements<INodeParameter>(np.GetValue()))
            {
                INodeParameter n = (INodeParameter)np.GetValue();

                return (T)n.GetValue();
            }
            else
            {
                try
                {
                    return (T)GetParameter(named, null).GetValue();
                }
                catch (InvalidCastException ice)
                {
                    Debug.Break();
                }
            }
        }
        else
        {
            np = new NodeParameter(new() { typeof(T) }, this);
            In.Add(named, np);

        }
        if (np.GetValue() == null)
        {
            return (T)default;
        }
        else
        {
            return (T)np.GetValue();
        }
    }
    public INodeParameter GetParameter(string named,List<Type> ps)
    {
        if (In.TryGetValue(named, out INodeParameter np))
        {
        }
        else
        {
            np = new NodeParameter(ps, this);
            In.Add(named, np);

        }
        return np;
    }
    
    public bool TryGetVS(int i, out object result, params object[] vs)
    {
        if (i < vs.Length)
        {
            result = vs[i];
            if (result == null) return false;
            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }
    public abstract void Init(params object[] vs);
    public abstract void OnStart();
    public abstract void OnUpdate();
    public abstract TaskResult TaskUpdate();
    public virtual IEnumerator WaitForEndTask(Action action)
    {
        while (TaskUpdate()==TaskResult.PROCESS)
        {
            action.Invoke();
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    public void AddParameter(int i,string nam,List<Type> ps, params object[] vs)
    {
        if (TryGetVS(i, out object value, vs))
            SetParameter(nam, value,ps);
    }
    
    public void GetEnumeratorActivatorStart(NodeParameter nodeParameter)
    {
        nodeParameter.SetValue(ActivatorStart());
    }
    public virtual IEnumerator ActivatorStart()
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
                
                break;
        }


    }
    

}

public class NodeIstance{
   public NodeBeh parent;
    public IndexTree pos;
    public Vector2Int step;
    public NodeIstance()
    {

    }
    public void ReParent(NodeBeh parent)
    {
        this.parent = parent;
    }
    public NodeIstance NewOfBranch(Vector2Int ste)
    {
        return new(null, IndexTree.AddedXY(pos.Pos, ste), step);
    }
    public NodeIstance(NodeBeh parent, IndexTree pos, Vector2Int step)
    {
        this.parent = parent;
        this.pos = pos;
        this.step = step;
    }
}

