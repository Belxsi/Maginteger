using System.Collections;
using UnityEngine;

public class IsNearToEnergies : Condition
{
    public NodeParameter dirN, nearN, ScaleEnergy, factdir,type;
    public override void Init(params object[] vs)
    {
        AddParameter(0, "NPC",NPCTypePS, vs);
        AddParameter(1, "Else",NodeBehTypePS, vs);
        dirN = new(Vector2.zero,Vector2TypePS, this);
        nearN = new(Vector2.zero, Vector2TypePS, this);
        factdir = new(Vector2.zero, Vector2TypePS,this);
        ScaleEnergy = new(0f,FloatTypePS, this);
        type = new("", StringTypePS, this);
    }
    public override bool Check()
    {
        if (Player.TryGetPlayer())
            if (Player.me.NearEnergiesDirAndPos(transform.position, 14f, out Vector2 dir, out Vector2 near, out object energy, out bool player))
                if (!player)
                {
                    if (((Energy)energy).casting)
                    {
                        dirN.SetValue(-dir);
                        nearN.SetValue(near);
                        Debug.DrawRay(transform.position, -((Vector2)transform.position - near).normalized, Color.red, 0.01f);
                        factdir.SetValue(-((Vector2)transform.position - near).normalized);
                        type.SetValue("Energy");
                        ScaleEnergy.SetValue(((Energy)energy).transform.localScale.magnitude);
                        return true;
                    }
                }
                else
                {
                    dirN.SetValue(-dir);
                    nearN.SetValue(near);
                    Debug.DrawRay(transform.position, -((Vector2)transform.position - near).normalized, Color.red, 0.01f);
                    factdir.SetValue(-((Vector2)transform.position - near).normalized);
                    type.SetValue("Player");
                    ScaleEnergy.SetValue(((GameObject)energy).transform.localScale.magnitude);
                    return true;
                }
        return false;
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
}
