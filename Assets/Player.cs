using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Element
{
    public static Player me;
    public PlayerController pc;
    public CreaterMagic cm;
    public ArhitectGameObject ag;
    public NpcVisual playerVisual;
    public AutoWalk autoWalk;
    public Coroutine coroutine;
    public NPCAudioEffect npcAE;
    public List<Energy> energies;
    public Transform Hand,HandPoint;
    public ItemObject Hand_IO;
    public Animator HandAnimator;

    public void TakeItem(ItemObject itemObject)
    {
        if (Hand_IO == null)
        {
            Hand_IO = itemObject;

            itemObject.transform.SetParent(Hand.transform);
            itemObject.isHanded = true;
            itemObject.transform.localPosition = Vector3.zero;
            HandAnimator = HandPoint.GetComponent<Animator>();
        }
        else
        {
            DropItem();
            TakeItem(itemObject);
        }

    }
    public void DropItem()
    {
        if (Hand_IO != null)
        {
            Hand_IO.transform.SetParent(GameplayPublicField.BigFather());
            Hand_IO.isHanded = false;
            Hand_IO = null;
        }
        
      

    }
    public void AtackItem(bool state)
    {
        if (Hand_IO != null)
        {
            HandAnimator.SetBool("atack", state);
        }



    }
    public void PutInBug()
    {
        if (Hand_IO != null)
        {
            GameObject obj = Hand_IO.gameObject;
            ItemObject io = Hand_IO;
           
            DropItem();
            Inventory.AddItem(io.item);




            SafeGameObjects.Save(obj, io.item.name,IItemIteraction.Features(io.item.iii));
        }
       
    }
    public void PutInBug(ItemObject io)
    {
        if (Hand_IO == null)
        {
            GameObject obj = io.gameObject;
            Inventory.AddItem(io.item);
            SafeGameObjects.Save(obj, io.item.name, IItemIteraction.Features(io.item.iii));
        }
    }
    // Start is called before the first frame update
    public static bool TryGetPlayer()
    {
        
        if (me != null)
        {
           
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool NearEnergiesPos(Vector2 me,float min,out Vector2 near)
    {
        
        float d=100000;
        Vector2 dv=Vector2.zero;
        Energy energy=null;
        bool result = false ;
        if (energies.Count > 0)
        {
            for (int i = 0; i < energies.Count; i++)
            {
                if (energies[i] != null)
                {
                    if (energies[i].pullet.used)
                    {
                        float bufd = Vector2.Distance(me, energies[i].transform.position);
                        if (d > bufd)
                        {
                            d = bufd;
                            dv = energies[i].transform.position;
                            result = true;
                            energy = energies[i];
                        }
                    }
                    else
                    {
                        if(energies[i].casting)
                        energies.Remove(energies[i]);
                    }
                }else energies.Remove(energies[i]);
            }
            if (d > min)
            {
                near = Vector2.zero;
               
                return false;
            }
        }
        near = dv;
        return result;
    }
    public bool NearEnergiesDirAndPos(Vector2 me, float min, out Vector2 dir,out Vector2 near,out object outenergy,out bool player)
    {

        float d = Vector2.Distance(transform.position,me);
        Vector2 dv = transform.position, dr=physic.rg.velocity;
        object energy = gameObject;
        bool result = true;
        outenergy =energy;
        player = true;
        if (energies.Count > 0)
        {
            for (int i = 0; i < energies.Count; i++)
            {
                if (energies[i] != null)
                {
                    if (energies[i].pullet.used)
                    {
                        float bufd = Vector2.Distance(me, energies[i].transform.position);
                        if (d > bufd)
                        {
                            d = bufd;
                            dv = energies[i].gameObject.transform.position;
                            dr = energies[i].physic.rg.velocity.normalized;
                            result = true;
                            energy = energies[i];
                            player = false;
                        }
                    }
                    else
                    {
                        if (energies[i].casting)
                            energies.Remove(energies[i]);
                    }
                }
                else energies.Remove(energies[i]);
            }
            if (d > min)
            {
                near = Vector2.zero;
                dir = Vector2.zero;
                return false;
            }
        }
        near = dv;
        dir = dr;
        outenergy = energy;
        return result;
    }
    Vector2 Cen(params Vector2[] list) 
    {
        Vector2 result=Vector2.zero,sum=Vector2.zero;
        foreach(var value in list)
        {
            sum += value;
        }
       result= sum / (float)list.Length;
        return result;
    
    }
    public bool SumEnergiesDirAndPos(Vector2 me, float min, out Vector2 dir, out Vector2 near, out Energy energys)
    {
        

        float d = Vector2.Distance(transform.position,me);
        Vector2 dv = transform.position, dr = Vector2.zero;
        Energy energy = null;
        energys = energy;
        bool result = false;
        List<Vector2> dirs = new();
        if (energies.Count > 0)
        {
            for (int i = 0; i < energies.Count; i++)
            {
                if (energies[i] != null)
                {
                    if (energies[i].pullet.used)
                    {
                        float bufd = Vector2.Distance(me, energies[i].transform.position);
                        if (d > bufd)
                        {
                            d = bufd;
                            dv = energies[i].gameObject.transform.position;
                            dr = energies[i].physic.rg.velocity.normalized*(1/bufd);
                            energy = energies[i];
                            result = true;
                            dirs.Add(dr);
                        }
                        else
                        {
                            dr = energies[i].physic.rg.velocity.normalized * (1 / bufd);
                            dirs.Add(dr);
                        }
                    }
                    else
                    {
                        //if (energies[i].casting)
                         //   energies.Remove(energies[i]);
                    }
                }
                else energies.Remove(energies[i]);
            }
            if (d > min)
            {
                near = Vector2.zero;
                dir = Vector2.zero;
                return false;
            }
        }
        near = dv;
        dir = Cen(dirs.ToArray()).normalized;
        energys = energy;
        return result;
    }
    public void Awake()
    {
        
        Life life = new(parameters);
        PhysicMove physic = gameObject.AddComponent<PhysicMove>();
        playerVisual =ag.VisualObj.AddComponent<NpcVisual>();
        
        Init(life,physic,playerVisual);
        tc = new(TypeTargetCast.Mouse, transform);
        physic.Init(this);
        pc = gameObject.AddComponent<PlayerController>();
        meBody = GetComponent<BodyObject>();
        playerVisual.Init();
        playerVisual.LocalInit();
        CameraFollow.Follower = gameObject;
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
    public bool Managment()
    {
        if (!CustomInputField.me.select)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                DropItem();
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                PutInBug();
            }
            
            return true;
        }
        return false;
    }
    public void Action()
    {
        physic.Move(pc.vectorMovement*parameters.speed, ForceMode2D.Impulse);
        life.Action();
        pc.enabled = Managment();
        playerVisual.Action();
        //if(pc.enabled)
        GetMoveCharacter(BaseFunc.MaxedVector(pc.vectorMovement));
        currentVelocity = pc.vectorMovement;
        if(cm.mattery!=null)
        cm.mattery.magic.dir = tc.GetTarget();
        if (meBody.matteryEnergy.LiqEnergy != 0)
        {
            life.Damage(meBody.matteryEnergy.LiqEnergy);
            DamageEffect();
            meBody.matteryEnergy.LiqEnergy = 0;
        }
        if (life.dead)
        {
            
            CameraFollow.Follower= Instantiate(BaseFunc.GetPrefab("Grave"),transform.position,Quaternion.identity, GameplayPublicField.BigFather());
            if (cm.MC != null)
            cm.MC.WakeDown();
            Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
    public void DamageEffect()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(playerVisual.DamageColor(0.5f));
            playerVisual.colortine = coroutine;
        }else
        {
            StopCoroutine(coroutine);
            coroutine = StartCoroutine(playerVisual.DamageColor(0.5f));
            playerVisual.colortine = coroutine;

        }
        
    }
    public void Update()
    {
        me = this;
        Action();
    }
    [Serializable]
    public class ArhitectGameObject
    {
        public GameObject VisualObj;
    }
}

