using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class Energy : Mattery
{
    // Start is called before the first frame update

   
    public string SpellEntry;
    public PhysicMove physic;

    public VisualEmitters visualEmitters;
    public Light2D light2d;
    public static int count_light=0;
    public EnergyManyImpuls emi;
    public CreaterMagic cm;
    public SpriteRenderer circle;
    public bool setLight;

    public void LocalAwake(CreaterMagic cm, MagicPullet mypuller, MagicPullet.Pullet pullet)
    {
        this.cm = cm;
        Speel = SpellEntry;
        EnergyInit();
        magic = new(matteryEnergy, this, Vector2.zero, cm.creater);
        Phys = physic;
        this.mypuller = mypuller;
        this.pullet = pullet;
        
    }
    public float ToOneZero(float num)
    {
        return 1f - Mathf.Clamp(Mathf.Pow (1f / num,1f/5f), 0, 1);
    }
    public void Update()
    {
        if (count_light+1 <= 10&!light2d.enabled)
        {
            light2d.enabled = true;
            count_light++;

        }
        else
        {
            if (count_light + 1 > 10 & light2d.enabled)
            {
                light2d.enabled = false;
                count_light--;
            }
        }
        if (!BaseFunc.Pause)
        {
           
            light2d.intensity = 1 / transform.localScale.x;
            if (!casting)
                if (cm.mattery != this) { mypuller.Import(pullet.pos); }
            Phys = physic;

            if (magic.spellActivator != null)       //if (magic.spellActivator.cast.Creater == null) casting = true;
                if (casting)
                    if (magic.spellActivator != null)
                    {
                        switch ((string)magic.spellActivator.MoreParameters["MOD"].value)
                        {
                            case "O":
                                float V = 0, E = 0, R = 0, LiqE = 0;
                                if (magic.spellActivator.TryGetParameter("V", out Param pv))
                                    V = Convert.ToSingle(pv.value);
                                if (magic.spellActivator.TryGetParameter("E", out Param pe))
                                {
                                    string s = pe.value.GetType().Name;
                                    E = Convert.ToSingle(pe.value);
                                }
                                if (magic.spellActivator.TryGetParameter("R", out Param pr))
                                    R = Convert.ToSingle(pr.value);
                                LiqE = magic.matteryEnergy.LiqEnergy;

                                magic.spellActivator.Use();
                                float im = ToOneZero( magic.spellActivator.GetImpulsMany());
                                im = Mathf.Clamp01(im);
                                circle.color = new Color((im+ToOneZero(R))/2, (im + ToOneZero(V))/2, (im + ToOneZero(E))/2, 1 / transform.localScale.x);
                                if (E < 1f / Mathf.Pow(10, 10))
                                {


                                    mypuller.Import(pullet.pos);

                                }
                                if (V != float.NaN)
                                {
                                    transform.localScale = Vector3.one * Mathf.Clamp(V, -10, 10);

                                }
                                if ((V != float.NaN) & (E != float.NaN) & (R != float.NaN))
                                {
                                    visualEmitters.Emit.maxParticles = Mathf.RoundToInt(E * (1 + LiqE));
                                    visualEmitters.Emit.emissionRate = E * (1 + LiqE);
                                    visualEmitters.Oreol.maxParticles = visualEmitters.Emit.particleCount * Mathf.RoundToInt(V);
                                    visualEmitters.Oreol.emissionRate = 5 * V;
                                    visualEmitters.Core.maxParticles = (int)(2 * Mathf.Log10(E * (1 + LiqE)));
                                    visualEmitters.Core.emissionRate = Mathf.Log10(E * (1 + LiqE));
                                    visualEmitters.Focus.maxParticles = (int)(V * R);
                                    visualEmitters.Focus.emissionRate = (int)(V * R);
                                    float A = visualEmitters.Focus.particleCount + visualEmitters.Core.particleCount + visualEmitters.Oreol.particleCount;
                                    light2d.pointLightOuterRadius = V;
                                    A /= 3;
                                    visualEmitters.SetAlpha(visualEmitters.Focus, 0.15F + 1F / A);
                                    visualEmitters.SetAlpha(visualEmitters.Core, 0.25F + 1F / A);
                                    visualEmitters.SetAlpha(visualEmitters.Oreol, 0.25F + 1F / A);
                                }
                                break;
                            case "I":
                                LiqE = magic.matteryEnergy.LiqEnergy;

                                magic.spellActivator.Use();


                                break;
                        }
                    }
        }


    }
    public void EnergyInit()
    {
        InitThis(this);
        Init();
    }
    public override bool UseMagic(Element creater)
    {
        magic.mattery.Creater = creater;
        if (!magic.IsReadSpelled)
        {
            if (magic.ReadSpeel(Speel) == null)
                if (magic.spellActivator.Use() == null)
                {
                    magic.spellActivator.Use();

                    return true;
                }

        }
        else return true;
        return false;


    }
}
