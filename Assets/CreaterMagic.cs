using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CreaterMagic : MonoBehaviour
{
    public Mattery mattery;
    public bool drag, realize;
    public float minDolyCast;
    public VisualCircleMagic MC;
    public float GetDolyMany()
    {

        if (mattery != null)
            if (mattery.magic.spellActivator.TryGetParameter("MI", out Param param))
            {
                return (1 - Magic.GetCircleStrong((TypeMagicCircle)param.value));
            }

        return float.NaN;
    }
    public MagicPullet mp;
    public MagicPullet.Pullet pullet;
    public Element creater;
    public void Awake()
    {

        mp = new(10, BaseFunc.GetPrefab("Energy"));
        creater = GetComponent<NPC>();
    }
    public bool CreateMagic(string spell, Element element, Vector2 pos, Quaternion rot)
    {

        mp.Init(pos, rot);

        pullet = mp.Export(pos, rot);
        Energy energy = pullet.obj.GetComponent<Energy>();
        energy.LocalAwake(this, mp, pullet);
        creater = element;
        energy.Speel = spell;
        energy.gameObject.SetActive(false);
        mattery = energy;
        energy.cm = this;
        drag = true;
        realize = mattery.energy.UseMagic(creater);

        MC = Instantiate(BaseFunc.GetPrefab("CircleMagic"), pos, rot, GameplayPublicField.BigFather()).GetComponent<VisualCircleMagic>();
        MC.mattery = mattery;
        MC.WakeUp();
        return true;



    }
    public bool CreateMagic(string spell, Element element, Vector2 pos, Quaternion rot, out Energy energy)
    {

        mp.Init(pos, rot);

        pullet = mp.Export(pos, rot);
        energy = pullet.obj.GetComponent<Energy>();
        energy.LocalAwake(this, mp, pullet);
        creater = element;
        energy.Speel = spell;
        energy.gameObject.SetActive(false);
        mattery = energy;
        energy.cm = this;
        drag = true;
        realize = mattery.energy.UseMagic(creater);

        MC = Instantiate(BaseFunc.GetPrefab("CircleMagic"), pos, rot, GameplayPublicField.BigFather()).GetComponent<VisualCircleMagic>();
        MC.mattery = mattery;
        MC.WakeUp();
        return true;



    }
    public void Fire(CreaterMagic cm)
    {
        cm.drag = false;
        switch (cm.mattery.typeMattery)
        {
            case TypeMattery.Energy:
                cm.mattery.starttime = Time.time;
                cm.mattery.energy.casting = true;
                if (MC != null)
                {
                    MC.WakeDown();
                    MC = null;
                }
                break;
        }
    }
    public bool FomritMagic(string spell, Element e, CreaterMagic cm)
    {
        return cm.CreateMagic(spell, e, e.transform.position, Quaternion.identity);
    }
    public bool FomritMagic(string spell, Element e, CreaterMagic cm, out Energy energy)
    {
        return cm.CreateMagic(spell, e, e.transform.position, Quaternion.identity, out energy);
    }
    public float GetDeltaMany()
    {

        float gcs = 0, gcp = 0;
        if (mattery.magic.spellActivator.TryGetParameter("MI", out Param param))
        {
            gcs = Magic.GetCircleStrong((TypeMagicCircle)param.value);

            gcp = Magic.GetCircleSpeed((TypeMagicCircle)param.value);
        }
        return (1 * gcs) * gcp * Time.deltaTime;

        return float.NaN;
    }
    public IEnumerator CoolDown(int hash)
    {
        yield return new WaitForSeconds(0.05f);
        if (mattery != null)
            if (hash == mattery.GetHashCode())
            {
                mattery.gameObject.SetActive(true);
                mattery.energy.casting = true;
                mattery = null;
                MC.WakeDown();
                MC = null;
            }
    }
    public float CloseEnergyToLimit()
    {
        if (creater == null) return 0;
        if (mattery == null) return 0;
        float valE = 0, maxE = 0, gcs = 0, gcp = 0;
        valE = creater.life.valueEnergy;
        maxE = creater.life.parameters.energy;
        if (mattery.magic.spellActivator.TryGetParameter("MI", out Param mi))
        {
            gcs = Magic.GetCircleStrong((TypeMagicCircle)mi.value);
           
        }
        float d1 = (1 - gcs), d2 = (valE / maxE);
        return (d2-d1)/(1-d1);
        
    }
    public float CloseEnergyToLimit(float max)
    {
       
        float valE = 0, maxE = 0, gcs = max;
        NPC npc = GetComponent<NPC>();
        valE = npc.life.valueEnergy;
        maxE =npc.life.parameters.energy;
        valE = Mathf.Clamp(valE, float.NegativeInfinity, maxE);
        float d1 = (1 - gcs), d2 = (valE / maxE);
        return (d2 - d1) / (1 - d1);

    }
    public void Update()
    {
        if (!BaseFunc.Pause)
        {
            if (mattery != null)
                if (drag)
                {

                    switch (mattery.typeMattery)
                    {
                        case TypeMattery.Energy:

                            if (realize)
                            {

                                mattery.energy.UseMagic(creater);


                                if (mattery.magic.spellActivator.TryGetParameter("MOD", out Param param))
                                    switch ((string)param.value)
                                    {
                                        case "O":
                                            mattery.gameObject.SetActive(true);
                                            float valE = 0, maxE = 0, gcs = 0, gcp = 0;
                                            valE = creater.life.valueEnergy;
                                            maxE = creater.life.parameters.energy;
                                            if (mattery.magic.spellActivator.TryGetParameter("MI", out Param mi))
                                            {
                                                gcs = Magic.GetCircleStrong((TypeMagicCircle)mi.value);
                                                gcp = Magic.GetCircleSpeed((TypeMagicCircle)mi.value);
                                            }
                                            minDolyCast = gcs;
                                            mattery.transform.position = transform.position;
                                            if(MC!=null)
                                            MC.transform.position = transform.position;

                                            if (valE / maxE > 1 - gcs)
                                            {
                                                if (mattery.magic.spellActivator.TryGetParameter("E", out Param E))
                                                {
                                                    creater.life.valueEnergy -= GetDeltaMany() * Convert.ToSingle(E.value);
                                                    mattery.matteryEnergy.LiqEnergy += GetDeltaMany();
                                                }
                                                else
                                                {
                                                    mattery = null;
                                                    drag = false;
                                                    mp.Import(pullet.pos);
                                                    MC.WakeDown();
                                                    MC = null;

                                                }
                                            }
                                            else
                                            {
                                                mattery = null;
                                                drag = false;
                                                mp.Import(pullet.pos);
                                                MC.WakeDown();
                                                MC = null;

                                            }
                                            break;
                                        case "I":
                                            mattery.gameObject.SetActive(true);
                                            valE = creater.life.valueEnergy;
                                            maxE = creater.life.parameters.energy;
                                            gcs = 0;
                                            if (mattery.magic.spellActivator.TryGetParameter("MOD", out Param mi1))
                                            {
                                                gcs = Magic.GetCircleStrong((TypeMagicCircle)mi1.value);
                                                gcp = Magic.GetCircleSpeed((TypeMagicCircle)mi1.value);
                                            }
                                            minDolyCast = gcs;
                                            mattery.transform.position = transform.position;



                                            mattery.energy.UseMagic(creater);

                                            mattery.energy.casting = true;
                                            if (valE / maxE > 1 - gcs)
                                            {
                                                creater.life.valueEnergy -= GetDeltaMany();
                                                mattery.matteryEnergy.LiqEnergy += GetDeltaMany();
                                            }
                                            else
                                            {
                                                {
                                                    drag = false;
                                                    mp.Import(pullet.pos);

                                                }
                                            }
                                            break;

                                    }

                            }
                            else
                            {
                                drag = false;
                                mp.Import(pullet.pos);

                            }

                            break;
                    }


                }
                else
                {
                    switch (mattery.typeMattery)
                    {
                        case TypeMattery.Energy:
                            if (mattery.magic.init)
                            {

                                if (mattery.magic.spellActivator.TryGetParameter("MOD", out Param param))
                                    switch (param.value + "")
                                    {
                                        case "O":
                                            StartCoroutine(CoolDown(mattery.GetHashCode()));

                                            break;
                                        case "I":
                                            /*
                                            drag = false;
                                            SpellActivator cast = mattery.magic.spellActivator;

                                            if (cast.magic.Creater == Player.me)
                                            {
                                                if (ca != null)
                                                {
                                                    if ((int)cast.bo.extracted != 0)
                                                    {
                                                        Inventory.CreateSlot(new(1, new("", 1,(int)cast.bo.extracted/100)));
                                                        cast.bo.extracted = 0;
                                                        cast.bo = null;
                                                    }

                                                }

                                            }
                                            Destroy(mattery.gameObject);
                                            */
                                            break;
                                    }

                            }
                            break;
                    }
                }
        }
    }
}
public enum TypeTargetCast
{
    Player,
    Mouse,
    Custom
}

public class MagicPullet
{
    public class Pullet
    {
        public GameObject obj;
        public bool used;
        public int pos;
        public Pullet(GameObject obj, bool used, int pos)
        {
            this.obj = obj;
            this.used = used;
            this.pos = pos;
        }
        public void Update(GameObject obj, bool used, int pos)
        {
            this.obj = obj;
            this.used = used;
            this.pos = pos;
        }
    }
    public List<Pullet> chash = new();
    public static List<Pullet> globalchash = new();
    public int sizechash;
    public GameObject prefab;
    public void Flush()
    {
        for (int i = 0; i < chash.Count; i++)
        {
            chash[i].used = false;
            chash[i].obj.SetActive(false);
            chash[i].pos = globalchash.Count;
            globalchash.Add(chash[i]);
        }
    }
    public bool IsInit;

    public MagicPullet(int sizechash, GameObject prefab)
    {
        this.sizechash = sizechash;
        this.prefab = prefab;

    }
    public static bool GetFreeGlobalBase(GameObject prefab, out Pullet result)
    {
        result = null;
        for (int i = 0; i < globalchash.Count; i++)
        {
            if (globalchash[i].obj == null)
            {
                globalchash.Clear();
                break;
            }
            if (!globalchash[i].used & globalchash[i].obj.name == prefab.name)
            {
                Pullet pullet = globalchash[i];
                globalchash.Remove(pullet);
                result = pullet;
                return true;
            }
        }
        return false;
    }
    public void Init(Vector2 pos, Quaternion rot)
    {
        if (!IsInit)
        {
            for (int i = 0; i < sizechash; i++)
            {
                if (GetFreeGlobalBase(prefab, out Pullet pullet))
                {

                    pullet.Update(pullet.obj, false, i);
                    chash.Add(pullet);

                }
                else
                {
                    GameObject obj = MonoBehaviour.Instantiate(prefab, pos, rot, GameplayPublicField.BigFather());

                    obj.name = prefab.name;
                    obj.SetActive(false);

                    chash.Add(new(obj, false, i));
                }
            }
            IsInit = true;
        }
    }
    public Pullet Export(Vector2 pos, Quaternion rot)
    {
        for (int i = 0; i < chash.Count; i++)
        {

            if (!chash[i].used)
            {
                chash[i].obj.SetActive(true);
                chash[i].obj.transform.SetPositionAndRotation(pos, rot);
                chash[i].used = true;
                return chash[i];

            }
        }
       
        GameObject obj = MonoBehaviour.Instantiate(prefab, pos, rot, GameplayPublicField.BigFather());
        obj.SetActive(false);
        Pullet pull = new(obj, false, sizechash);
        chash.Add(pull);
        sizechash++;
        return pull;
    }
    public void Import(int i)
    {

        chash[i].obj.SetActive(false);
        chash[i].used = false;

    }
}
public class TargetCast
{
    public TypeTargetCast ttc;
    public Transform customTarget;
    public Transform me;
    public TargetCast(TypeTargetCast ttc, Transform me)
    {
        this.me = me;
        this.ttc = ttc;
    }
    public TargetCast(TypeTargetCast ttc, Transform me, Transform custom)
    {
        this.me = me;
        this.ttc = ttc;
        this.customTarget = custom;
    }
    public Vector2 GetTarget()
    {
        switch (ttc)
        {

            case TypeTargetCast.Mouse:
                return -BaseFunc.DirPointOfMouse(BaseFunc.GetScreenPos(me.position));
            case TypeTargetCast.Player:
                if (Player.TryGetPlayer())
                {
                    return (Player.me.transform.position - me.transform.position).normalized;
                }
                else
                {
                    return Vector2.zero;
                }
            case TypeTargetCast.Custom:
                return customTarget.position - me.position;
            default: return Vector2.one * float.NaN;
        };
    }

}
