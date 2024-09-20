using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEmitters : MonoBehaviour
{
    public ParticleSystem Emit, Premit, Oreol, Core, Focus;
   public void SetAlpha(ParticleSystem ps,float a)
    {
        ps.startColor = new Color(ps.startColor.r, ps.startColor.g, ps.startColor.b, a);
    }
   public void SetAllAplha(float a)
    {
        SetAlpha(Emit, a);
        SetAlpha(Premit, a);
        SetAlpha(Core, a);
        SetAlpha(Oreol, a);
        SetAlpha(Focus, a);
    }

}
