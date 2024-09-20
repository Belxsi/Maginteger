using UnityEngine;

public class EnergyManyImpuls : MonoBehaviour
{
    public CurrentTriggerCollision ctc;
    public int state;
    public BodyObject bo, bocreater;
    
    public Energy myclass;
    public float intensive;
    public Mattery me;
    public float timesafety = 3, timeoutsafety = 0;
    public bool nomagic;

    void Awake()
    {
        if (me != null)
        {
            state = Random.Range(int.MinValue, int.MaxValue);
            timeoutsafety = timesafety;
            myclass = me.energy;
        }

    }
    public void BaseEMI(Element Creater, float ligE)
    {
        if (ctc.state != state)
        {
            CurrentTriggerCollision.SearchInfo info = ctc.SearchBO(0);
            bo = info.bo;

            timeoutsafety -= Time.deltaTime;

            if (bo == Creater)
            {
                if (timeoutsafety > 0)
                {

                    bo = ctc.SearchBO(info.index + 1).bo;
                }
                else
                {
                    state = ctc.state;
                }

            }
            else
            {
                state = ctc.state;
            }

        }

        if (bo != null)
            if (bo != Creater)
                if (bo.TryGetComponent(out Element e))
                    if (e.frendlyCode != Creater.frendlyCode)
                        if (me.magic.spellActivator.TryGetParameter("E", out Param E))
                            if (System.Convert.ToSingle(E.value) > 1)

                                if (!float.IsNaN((me.magic.spellActivator.GetImpulsMany())))
                                    if (!float.IsInfinity((me.magic.spellActivator.GetImpulsMany())))
                                    {


                                    bo.matteryEnergy.LiqEnergy += ligE;
                                }
                                else
                                {
                                    if (timeoutsafety <= 0)
                                        if (me.magic.spellActivator.TryGetParameter("E", out Param E1))
                                            if (System.Convert.ToSingle(E1.value) > 1)

                                                if (!float.IsNaN((me.magic.spellActivator.GetImpulsMany())))
                                                    if (!float.IsInfinity((me.magic.spellActivator.GetImpulsMany())))
                                                    {

                                                    bo.matteryEnergy.LiqEnergy += ligE;
                                                }
                                }
    }
    public void MagicEMI()
    {
        if (ctc.state != state)
        {
            CurrentTriggerCollision.SearchInfo info = ctc.SearchBO(0);
            bo = info.bo;

            timeoutsafety -= Time.deltaTime;

            if (bo == me.magic.mattery.Creater.meBody)
            {
                if (timeoutsafety > 0)
                {

                    bo = ctc.SearchBO(info.index + 1).bo;
                }
                else
                {
                    state = ctc.state;
                }

            }
            else
            {
                state = ctc.state;
            }

        }

        if (bo != null)
            if (bo != me.magic.mattery.Creater.meBody)
                if (bo.TryGetComponent(out Element e))
                    if (e.frendlyCode != me.magic.mattery.Creater.frendlyCode)
                        if (me.magic.spellActivator.TryGetParameter("E", out Param E))
                            if (System.Convert.ToSingle(E.value) > 1)
                            {
                                if (!float.IsNaN((me.magic.spellActivator.GetImpulsMany())))
                                    if (!float.IsInfinity((me.magic.spellActivator.GetImpulsMany())))
                                    {

                                    float delta = me.magic.spellActivator.GetImpulsMany();
                                    bo.matteryEnergy.LiqEnergy += delta;
                                    me.matteryEnergy.LiqEnergy -= delta;
                                    Param p = E;
                                    p.value = 0;
                                }



                            }
                            else
                             if (!float.IsNaN((me.magic.spellActivator.GetImpulsMany())))
                                if (!float.IsInfinity((me.magic.spellActivator.GetImpulsMany())))
                                {
                                if (timeoutsafety <= 0)
                                {
                                    float delta = intensive * Time.deltaTime * me.magic.spellActivator.GetImpulsMany();
                                    bo.matteryEnergy.LiqEnergy += delta;
                                    me.matteryEnergy.LiqEnergy -= delta;
                                    Param p = E;
                                    p -= delta;

                                }
                            }
    }

    void LateUpdate()
    {
       
        if (ctc.SearchWall().ob != null)
        {
            myclass.mypuller.Import(myclass.pullet.pos);
        }
        if (!nomagic)
        {
            MagicEMI();
        }
        else
        {

            BaseEMI(bocreater.Creater, bocreater.matteryEnergy.LiqEnergy);
        }


    }
}
