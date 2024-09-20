using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class TreeBehaviour
{
    public List<INodeParameter> StaticsParameters = new();
    public NodeBeh R;
    public BehaviorExecutor be;
    public Dictionary<int, NodeBeh> TreeConst = new();
    public TreeBehaviour(NodeBeh r, BehaviorExecutor be)
    {
        R = r;

        this.be = be;
        this.be.tree = this;
        r.myTree = this;
        be.nodeIstance = new(null, new(Vector2Int.zero), Vector2Int.up);
        be.AddNode(r, be.nodeIstance, "Hello world!");

        // tree.AddBranch(r, index);
    }


    public void OnInterpreter()
    {
        be.StartCoroutine(R.ActivatorStart());
    }

    public void AddBranch(NodeBeh node)
    {
        //node.parent = GetBranch(index);
        TreeConst.Add(node.UnicalKey(), node);
        if (node.parent != null)
            node.parent.nodes.Add(node);
    }





}
[Serializable]
public class FloatNP : INodeParameter
{
    public float value;
    public List<Type> ps;
    public object GetValue()
    {
        return value;
    }

    public void SetValue(object v)
    {
        value = (float)v;
        if (!IsValueTargetType(value))
            throw new UnEqualTypesException("Тип переменной " + value + " не сходится с ожидаемым " + ps[0], null, value, ps[0]);
    }
    public bool IsValueTargetType(object val)
    {
        bool result = false;
        foreach (var type in ps)
        {
            if (val.GetType() == type)
            {
                result = true;
                break;

            }
        }
        if (val.GetType() == typeof(NodeParameter))
            if (IsValueTargetType(((NodeParameter)val).value))
            {

                result = true;


            }
        return result;
    }
    public void SetValue(object v, List<Type> ps)
    {
        value = (float)v;
        this.ps = ps;
        if (!IsValueTargetType(value))
            throw new UnEqualTypesException("Тип переменной " + value + " не сходится с ожидаемым " + ps[0], null, value, ps[0]);

    }

    public T GetValue<T>()
    {
        return default;
    }
}
[Serializable]
public class NodeParameter : INodeParameter
{
    public object value;
    public List<Type> ps;
    public NodeBeh node;
    public NodeParameter(object value, List<Type> ps,NodeBeh node)
    {
        this.value = value;
        this.ps = ps;
        this.node = node;
        if (!IsValueTargetType(value))
            throw new UnEqualTypesException("Тип переменной " + value + " не сходится с ожидаемым " + ps[0], null, value, ps[0]);
    }
    public void SetValue(object v, List<Type> ps)
    {
        value =v;

        this.ps = ps;
        if (!IsValueTargetType(value))

            throw new UnEqualTypesException("Тип переменной " + value + " не сходится с ожидаемым " + ps[0],null, value, ps[0]);


    }
   
    public bool IsValueTargetType(object val)
    {
        bool result = false;
        
        foreach (var type in ps)
        {
            if (val.GetType() == type || (NodeBeh.Implements<NodeBeh>(val))) ;
            {
                result = true;
                break;

            }
        }
        if (val.GetType() == typeof(NodeParameter))
            if (IsValueTargetType(((NodeParameter)val).value))
            {

                result = true;


            }
        return result;
    }
    public NodeParameter(List<Type> ps,NodeBeh node)
    {
        this.ps = ps;
        this.node = node;
    }
    public object GetValue()
    {
        return value;
    }
    public T GetValue<T>()
    {
        return (T) value;
    }
    public void SetValue(object v)
    {
        value = v;
        if (!IsValueTargetType(value))
            throw new UnEqualTypesException("Тип переменной " + value + " не сходится с ожидаемым " + ps[0], null, value, ps[0]);

    }
}

public interface INodeParameter
{


    public object GetValue();
    public T GetValue<T>();
    public void SetValue(object v);
    public void SetValue(object v, List<Type> ps);
}
public class IndexTree
{

    public Vector2Int Pos;


    public IndexTree(Vector2Int pos)
    {
        Pos = pos;
    }
    public void AddY(int y)
    {
        Pos = new(Pos.x, Pos.y + y);
    }
    public void AddX(int x)
    {
        Pos = new(Pos.x + x, Pos.y);
    }
    public void AddXY(Vector2Int xy)
    {
        Pos = new(Pos.x + xy.x, Pos.y + xy.y);
    }
    public static Vector2Int AddedX(Vector2Int Pos, int x)
    {
        return new(Pos.x + x, Pos.y);
    }
    public static Vector2Int AddedY(Vector2Int Pos, int y)
    {
        return new(Pos.x, Pos.y + y);
    }
    public static IndexTree AddedXY(Vector2Int Pos, Vector2Int added)
    {
        return new(new(Pos.x + added.x, Pos.y + added.y));
    }
    // public List<List<int>> indexes = new();

    //  public IndexTree(List<List<int>> indexes)
    //  {
    //      this.indexes = indexes;
    // }
    //  public void Add(List<int> x)
    //   {
    //     indexes.Add(x);
    //  }


}

