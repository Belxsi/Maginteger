using UnityEngine;

public class AnimatorManagerNode : NodeBeh
{
    public override void Init(params object[] vs)
    {

        AddParameter(0, "Animator", AnimatorTypePS, vs);
        AddParameter(1, "Name", StringTypePS, vs);
      
         AddParameter(2, "Value", BoolTypePS, vs);
    }

    public override void OnStart()
    {
        Animator anim = InterGetParameter<Animator>("Animator");
        string name = InterGetParameter<string>("Name");
         bool value = InterGetParameter<bool>("Value");
        anim.SetBool(name,value);

    }

    public override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override TaskResult TaskUpdate()
    {
        return TaskResult.COMPLETE;
    }
    public static NodeBeh AddNode<T>(T value, BehaviorExecutor be, Animator anim, string name,bool trfl) where T : NodeBeh
    {
        T node = be.gameObject.AddComponent<T>();
        node.InitBase(be.tree, be.nodeIstance, anim, name,trfl);

        be.nodeIstance.ReParent(node);
        be.nodes.Add(node);
        return node;

    }
    public AnimatorManagerNode (BehaviorExecutor be, Animator anim, string name,bool trfl)
    {
        AddNode(this, be, anim, name, trfl);

}
}
