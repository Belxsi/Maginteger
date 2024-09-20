using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;
using JetBrains.Annotations;
public class VisualCircleMagic : MonoBehaviour
{
    public Animator animator;
    public ObjMagicGroup o;
    public LightMagic p;
    public ParticleSystem p_main, p_focus;
    public SpriteMagic s;
    public Mattery mattery;
    public static List<GameObject> mws=new();
    public void SettingUp(VisualCircleMagicParameter vcmp)
    {
        o.tr.SetActive(vcmp.tr);
        o.sq.SetActive(vcmp.sq);
        o.ug5.SetActive(vcmp.ug5);
        o.ug6.SetActive(vcmp.ug6);
        o.c_base.SetActive(vcmp.c_base);
        o.c_U.SetActive(vcmp.c_U);
        GameObject focus;
        switch (vcmp.type)
        {
            case TypeMagicCircle.MIE:

                ParticleSystem.ShapeModule shape = p_focus.shape;
                shape.sprite = s.tr;
                focus = o.tr;
               

                break;
        }
        if (vcmp.I)
        {
            o.tr.transform.localScale = Vector3.one * 2;
        }
        else
        {
            o.tr.transform.localScale = Vector3.one * 2;
        } 

    }
   
    public void Awake()
    {
        if (mws.Count <= 10)
        {
            mws.Add(gameObject);
        }
        else
        {
            mws.Add(gameObject);
            GameObject o = mws[0];
            mws.Remove(o);
            DestroyImmediate (o);
        }
    }
    public void Update()
    {
        if (mattery == null)
        {
            mws.Remove(gameObject);
            Destroy(gameObject);
          
        }
        
    }
    public void WakeUp()
    {
        animator.SetBool("WakeUp", true);
    }
    public IEnumerator AutoDie(float ofSecond)
    {
        yield return new WaitForSeconds(ofSecond);
        mws.Remove(gameObject);
        Destroy(gameObject);
    }
    public void WakeDown()
    {
        animator.SetBool("WakeUp", false);
        StartCoroutine(AutoDie(3));
    }
    
}
[Serializable]
public class ObjMagicGroup
{
    public GameObject tr,sq, ug5, ug6, c_base, c_U;
}
[Serializable]
public class LightMagic
{
    public Light2D tr,sq, ug5, ug6, c_base, c_U;
}
[Serializable]
public class SpriteMagic
{
    public Sprite tr,sq, ug5, ug6, c_base, c_U;
}
public class VisualCircleMagicParameter
{
    public bool tr,sq,ug5,ug6,c_base,c_U;
    public bool I;
    public TypeMagicCircle type;

    public VisualCircleMagicParameter(TypeMagicCircle type, bool tr, bool sq, bool ug5, bool ug6, bool c_base, bool c_U, bool i)
    {
        this.tr = tr;
        this.sq = sq;
        this.ug5 = ug5;
        this.ug6 = ug6;
        this.c_base = c_base;
        this.c_U = c_U;
        I = i;
        this.type = type;
    }
}
