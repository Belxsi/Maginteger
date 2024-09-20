using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class NpcBE : BehaviorExecutor
{
    public GlobalPar par;
    public NPCBehaviour behaviour;
    public float inv;
    
    public static float AOrB(float A,float B)
    {
        if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
        {
            return A;
        }
        else return B;
    }



    public NodeIstance BranchRun,NoEnergyBehaviour,magnite;
    public IsNearToEnergies inte;
    public IsAimedAtMe iaam;
    public GetInfoOfPlayer giop;
    public NodeBeh els,els2;
    public SpellCasterNode scn;
   public EqualStringNode esn;
    public RayPathNode rpn;
    public NodeBeh MagicCast(NodeIstance magic)
    {
       
        NodeBeh vetka= AddNode<EmptyNode>(magic, true);
        NodeParameter min = FloatOper(1f, TypeOperate.Sub, FloatOper(giop.distance, TypeOperate.Div, 28f,magic),magic) ;
        
        scn=AddNode<SpellCasterNode>(magic, true, behaviour,GeneratorScroll.me.GetRandomSpell(),min,giop.dir);
        return vetka;
    }
    public NodeParameter SuperDir(NodeIstance ist)
    {
        NodeParameter pos = AddNode<SumVector>(ist, true, giop.pos, giop.velocity).result;
        NodeParameter dir = AddNode<VectorNormalizeNode>(ist,true, AddNode<VectorOperationNode>(ist, true, pos, giop.mypos, TypeOperate.Sub).result).result;
        return dir;
    }
    public NodeParameter FloatOper(object a,TypeOperate type, object b,NodeIstance ist)
    {
       return AddNode<FloatOperationsNode>(ist, true, a, b, type).result;

    }
    public NodeParameter VectorOper(object a, TypeOperate type, object b)
    {
        return AddNode<FloatOperationsNode>(nodeIstance, true, a, b, type).result;

    }
    public NodeParameter BoolOper(object a, TypeBoolOperate type, object b,NodeIstance ist)
    {
        return AddNode<BoolOperationNode>(ist, true, a, b, type).result;

    }
    public NodeBeh Atack()
    {
        NodeIstance atack= new(null, new(Vector2Int.zero), Vector2Int.zero);
        
        NodeBeh beh= AddNode<WalkForNPC>(atack, true, giop.pos, giop.pos, behaviour,true);
        return beh;
    }
    public NodeBeh Atack(NodeIstance atack)
    {
        

        NodeBeh beh = AddNode<WalkForNPC>(atack, true, giop.pos, giop.pos, behaviour, true);
        return beh;
    }
    public NodeBeh RunAtack(NodeIstance run)
    {
        
        NodeBeh vetka = AddNode<EmptyNode>(run, true);
        esn = AddNode<EqualStringNode>(run, true, inte.type, "Energy");
        NodeParameter ifed = BoolOper(scn.nocasted, TypeBoolOperate.Or, esn.result, run);
        FloatComparisonCond fcc = AddNode<FloatComparisonCond>(run, true, giop.distance, 10f, TypeComparison.Less);
        AddNode<IfNode>(run, true, BoolOper(ifed, TypeBoolOperate.Or, fcc.result, run), Atack());
        AddNode<WalkForNPC>(run, true, rpn.pos, inte.nearN, behaviour, false);
        return vetka;
    }
    public NodeBeh BaseWalk(NodeIstance walk)
    {
       return AddNode<WalkForNPC>(walk, true, rpn.pos, inte.nearN, behaviour, false);
    }
    public override void InitConstruct()
    {
        AddNode<RepeatSequance>(nodeIstance, true, true, 99);
        giop = AddNode<GetInfoOfPlayer>(nodeIstance, true);
        inte = AddNode<IsNearToEnergies>(nodeIstance, true, behaviour, null);
       
        rpn = AddNode<RayPathNode>(nodeIstance, true, inte.factdir, inte.nearN, 2, new List<string>() { "Energy", "Player" }, behaviour);
        
        AddNode<FloatComparisonCond>(nodeIstance, true, giop.energyCast, 0.3f, TypeComparison.Greater, BaseWalk(new()));
        
        AddNode<FloatComparisonCond>(nodeIstance, true, giop.distance, 10f, TypeComparison.Less, Atack());
        MagicCast(nodeIstance);
        RunAtack(nodeIstance);
      
     
        
      




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
    public void OlderScript()
    {
        AddNode<TimeOutNode>(nodeIstance, true, 1f);
        BranchRun = new(null, IndexTree.AddedXY(nodeIstance.pos.Pos, new(2, 0)), new(0, 1));

        //AddNode<FloatComparisonCond>(nodeIstance, true,par.A,par.B,TypeComparison.Greater,els.ActivatorStart());
        AddNode<RepeatSequance>(nodeIstance, true, true, 99);
        giop = AddNode<GetInfoOfPlayer>(nodeIstance, true);
        NoEnergyBehaviour = new(null, IndexTree.AddedXY(BranchRun.pos.Pos, new(4, 0)), new(0, 1));
        magnite = new(null, IndexTree.AddedXY(NoEnergyBehaviour.pos.Pos, new(4, 0)), new(0, 1));
        NodeIstance rand = magnite.NewOfBranch(new(1, 0));
        VectorPerlinRandom radper = AddNode<VectorPerlinRandom>(rand, true);
        AddNode<WalkForNPC>(rand, true, radper.result, behaviour);

        NodeIstance els3NI = NoEnergyBehaviour.NewOfBranch(new(4, 0));
        NodeBeh els3 = AddNode<FloatComparisonCond>(els3NI, true, giop.distance, 8f, TypeComparison.Greater, radper); ;
        AddNode<WalkForNPC>(els3NI, true, giop.dir, behaviour);
        NodeBeh b = AddNode<FloatComparisonCond>(NoEnergyBehaviour, true, giop.distance, 3f, TypeComparison.Less, els3);
        InverseVector invec = AddNode<InverseVector>(NoEnergyBehaviour, true, giop.dir);
        AddNode<WalkForNPC>(NoEnergyBehaviour, true, invec.result, behaviour);



        //AddNode<WalkForNPC>(nodeIstance, true, GetInfoOfPlayer.me.dir, behaviour);
        inte = AddNode<IsNearToEnergies>(nodeIstance, true, behaviour, b);
        iaam = AddNode<IsAimedAtMe>(nodeIstance, true, inte.nearN, inte.dirN, inte.ScaleEnergy, 60f,AvoidWallGroup());

        els2 = RunGroup();
        els = AddNode<IsWallDir>(BranchRun, true, inte.factdir, 3f, els2);
        AvoidGroup();




        AddNode<FloatComparisonCond>(nodeIstance, true, iaam.distance, 5f, TypeComparison.Greater, els);
        AvoidGroup();
    }
    
    public NodeBeh RunGroup()
    {
        NodeIstance ist = new(null, new(Vector2Int.zero), Vector2Int.zero);
        NodeBeh r= AddNode<SendMessageNode>(ist, true, "Run");
        AddNode<WalkForNPC>(ist, true, inte.factdir, behaviour);
        return r;

    }
    public NodeBeh UnRun(NodeParameter dir)
    {
        NodeIstance ist = new(null, new(Vector2Int.zero), Vector2Int.zero);
        RotationVector2D rd = AddNode<RotationVector2D>(ist, true, dir, 90f*behaviour.fis);
        InverseVector inv =AddNode<InverseVector>(ist, true, dir);
      
        NodeBeh r = AddNode<WalkForNPC>(ist, true, AddNode<SumVector>(ist, true, rd.RotatedVector, inv.result).result, behaviour);
      
        return r;

    }
    public NodeBeh RunPlayerGroup()
    {
        NodeIstance ist = new(null, new(Vector2Int.zero), Vector2Int.zero);

        NodeBeh r = AddNode<WalkForNPC>(ist, true, giop.dir, behaviour);

        return r;

    }
    public NodeBeh UnClosingPlayerGroup()
    {
        NodeIstance ist = new(null, new(Vector2Int.zero), Vector2Int.zero);

        NodeBeh r = AddNode<WalkForNPC>(ist, true, AddNode<InverseVector>(ist, true, giop.dir).result, behaviour);

        return r;

    }
   
    public NodeBeh BaseBehavior()
    {
        NodeIstance ist = new(null, new(Vector2Int.zero), Vector2Int.zero);
        NodeBeh r = AddNode<FloatComparisonCond>(ist, true, giop.distance, 4f, TypeComparison.Greater, UnClosingPlayerGroup());
        AddNode<FloatComparisonCond>(ist, true, giop.distance, 8f, TypeComparison.Less, RunPlayerGroup());
       
        AddNode<WalkForNPC>(ist, true, AddNode<VectorPerlinRandom>(ist, true).result, behaviour);
        return r;
    }
    public NodeBeh AvoidWallGroup()
    {
     
        NodeIstance ist = new(null, new(Vector2Int.zero), Vector2Int.zero);
        
        AvoidingWall r = AddNode<AvoidingWall>(ist, true, behaviour);
        NodeBeh els3 = AddNode<FloatComparisonCond>(ist, true, r.dist, 2f, TypeComparison.Less,BaseBehavior()); ;
        AddNode<WalkForNPC>(ist, true, r.neared, behaviour);
        return r;

    }
    
    public NodeBeh AvoidGroup()
    {
        NodeIstance ist = new(null, new(Vector2Int.zero), Vector2Int.zero);
        RotationVector2D rv2d = AddNode<RotationVector2D>(ist, true, inte.dirN, 90f);
        RotationVector2D rv2d2 = AddNode<RotationVector2D>(ist, true, inte.dirN, -90f);
        NodeBeh rv2d_side= AddNode<WalkForNPC>(ist.NewOfBranch(new(1, 0)), true, rv2d.RotatedVector, behaviour);
        NodeBeh rv2d_side2 = AddNode<WalkForNPC>(ist.NewOfBranch(new(1, 0)), true, rv2d2.RotatedVector, behaviour);
        AddNode<IsWallDir>(ist, true, rv2d.RotatedVector, 3f, rv2d_side);
        AddNode<IsWallDir>(ist, true, rv2d2.RotatedVector, 3f, rv2d_side2);
        AddNode<WalkForNPC>(ist, true, inte.factdir, behaviour);

        return rv2d;
    }
       

    
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

