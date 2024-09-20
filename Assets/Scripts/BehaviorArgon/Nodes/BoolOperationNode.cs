using UnityEngine;

public class BoolOperationNode : NodeBeh
{
    public NodeParameter result;
    public override void Init(params object[] vs)
    {
        AddParameter(0, "A", BoolTypePS, vs);
        AddParameter(1, "B", BoolTypePS, vs);
        AddParameter(2, "Type", new() { typeof(TypeBoolOperate) }, vs);
        result = new(false, BoolTypePS, this);
    }

    public override void OnStart()
    {
        bool a = InterGetParameter<bool>("A");
        switch (InterGetParameter<TypeBoolOperate>("Type"))
        {
            case TypeBoolOperate.And:
                bool b = InterGetParameter<bool>("B");
                result.SetValue(a & b);
                break;
            case TypeBoolOperate.Or:
                b = InterGetParameter<bool>("B");
                result.SetValue(a || b);
                break;
            case TypeBoolOperate.Not:
                result.SetValue(!a);
                break;
        }
    }

    public override void OnUpdate()
    {
        
    }

    public override TaskResult TaskUpdate()
    {
        return TaskResult.COMPLETE;
    }
}
public enum TypeBoolOperate
{
    And,
    Or,
    Not
}
