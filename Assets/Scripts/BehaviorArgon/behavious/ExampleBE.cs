using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class ExampleBE : BehaviorExecutor
{
    public GlobalPar par;
  
    public float inv;
    
    public static float AOrB(float A,float B)
    {
        if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
        {
            return A;
        }
        else return B;
    }



   
    public override void InitConstruct()
    {
        AddNode<RepeatSequance>(nodeIstance, true, true, 99);
        /* на верху стартовая нода
         * пишутся ноды
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * */






    }
    /*
    AddNode<TimeOutNode>(nodeIstance, true, 1f);
        AddNode<RepeatSequance>(nodeIstance, true, true, 99);
        giop = AddNode<GetInfoOfPlayer>(nodeIstance, true);
        inte = AddNode<IsNearToEnergies>(nodeIstance, true, behaviour, AvoidWallGroup());
        //iaam = AddNode<IsAimedAtMe>(nodeIstance, true, inte.nearN, inte.dirN, inte.ScaleEnergy, 10f, AvoidWallGroup());
       
        RayPathNode hit = AddNode<RayPathNode>(nodeIstance, true, inte.factdir, inte.factdir, 2, new List<string>() { "Energy", "Player" }, behaviour);
    //AddNode<FloatComparisonCond>(nodeIstance, true, hit.dist, 4f, TypeComparison.Greater, UnRun(hit.dir));
    //AddNode<TimeOutNode>(nodeIstance, true, 0.5f);

    AddNode<WalkForNPC>(nodeIstance, true,hit.dir, behaviour);
    */
   
    
    /*
    public NodeIstance CreateParrallel()
    {
        NodeBeh ps = nodeIstance.parent;
        NodeIstance parallel = new(ps, nodeIstance.pos, new(1, 0));

        //return nodeIstance;
            parallel.ReParent(ps);
        
    }
    */
    [Serializable]
    public struct GlobalPar
    {
        public FloatNP A, B;
    }
    
    
    public override void Awake()
    {
        inv = AOrB(-1, 1);
        base.Awake();
    }
    public override void Update()
    {
        if (active)
        {
            active = false;
            tree.OnInterpreter();

        }
    }
    public override NodeBeh R()
    {
        NodeBeh node= gameObject.AddComponent<EmptyNode>();
        
        return node;

    }


}

