using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Element
{
 
  
    public CreaterMagic cm;
    public ArhitectGameObject ag;
    public NpcVisual playerVisual;
    public AutoWalk autoWalk;
    public NPCBehaviour behaviour;
    public GameObject bars;
    public Coroutine coroutine;
    public NPCAudioEffect npcAE;
    public RoomFight fight;
    // Start is called before the first frame update
    public void Awake()
    {
       
        Life life = new(parameters);
        PhysicMove physic = gameObject.AddComponent<PhysicMove>();
       
        playerVisual = ag.VisualObj.AddComponent<NpcVisual>();
        Init(life, physic, playerVisual);

        physic.Init(this);
        tc = new(TypeTargetCast.Player, transform);
        playerVisual.Init();
        playerVisual.LocalInit();
        
        bars =Instantiate(BaseFunc.GetPrefab("NPCBars"), GameplayPublicField.BigFather());
        bars.GetComponent<NPCBars>().Init(this);
        playerVisual.in_color = playerVisual.spriteRenderer.color;
      
    }
    public float GetDolyLife()
    {
        return life.valueLife / parameters.life;
    }
    public float GetDolyEnergy()
    {
        return life.valueEnergy / parameters.energy;
    }
    public void DamageEffect()
    {
        
        if (coroutine == null)
        {
            coroutine = StartCoroutine(playerVisual.DamageColor(0.5f));
            playerVisual.colortine = coroutine;
        }
        else
        {
            StopCoroutine(coroutine);
            coroutine = StartCoroutine(playerVisual.DamageColor(0.5f));
            playerVisual.colortine = coroutine;

        }

    }
    
    public void Action()
    {
        
        physic.Move(behaviour.vectorMovement * parameters.speed, ForceMode2D.Impulse);
        life.Action();
        playerVisual.Action();
        
        if(behaviour.enabled)
        GetMoveCharacter(BaseFunc.MaxedVector(behaviour.vectorMovement));
        currentVelocity = behaviour.vectorMovement;
        if (cm.mattery != null)
            cm.mattery.magic.dir = tc.GetTarget();
        if (meBody.matteryEnergy.LiqEnergy != 0)
        {
            life.Damage(meBody.matteryEnergy.LiqEnergy);
            BaseFunc.VisualDamage(meBody.matteryEnergy.LiqEnergy, transform.position);
            DamageEffect();
            meBody.matteryEnergy.LiqEnergy = 0;
        }
       
          
        
        if (life.dead)
        {
          
            Instantiate(BaseFunc.GetPrefab("Grave"), transform.position, Quaternion.identity, GameplayPublicField.BigFather());
            if (cm.MC != null)
                cm.MC.WakeDown();
            if (fight != null)
            {
                fight.npcs.Remove(this);
            }
            cm.mp.Flush();
            Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        Action();
    }
    [Serializable]
    public class ArhitectGameObject
    {
        public GameObject VisualObj;
    }
}
