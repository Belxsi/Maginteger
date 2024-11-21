using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class Exposuring : MonoBehaviour
{
    public Volume volume;
    public LiftGammaGain cm;
    public Vignette vignette;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        BaseFunc.GetOfNull(ref volume, gameObject);
        var list = volume.profile.components;
        cm = BaseFunc.SearchList<LiftGammaGain, VolumeComponent>(list);
        vignette = BaseFunc.SearchList<Vignette, VolumeComponent>(list);
    }
    public IEnumerator Blomming(float dir)
    {
        float md = Mathf.Abs(dir);
        float normal = md / dir;
        if (normal > 0)
        {
            while (true)
            {

                yield return new WaitForSeconds(Time.deltaTime);
                if (BaseFunc.SmoothStep(cm.lift.value.w, 2, md, out float outed))
                {
                    break;
                }
                cm.lift.value = new Vector4(0, 0, 0, outed);

            }
        }
        else
        {
            while (true)
            {

                yield return new WaitForSeconds(Time.deltaTime);
                if (BaseFunc.SmoothStep(cm.lift.value.w, 0, md, out float outed))
                {
                    break;
                }
                cm.lift.value = new Vector4(0, 0, 0, outed);

            }

        }
    }
    public IEnumerator Viging(float dir)
    {
        float md = Mathf.Abs(dir);
        float normal = md / dir;
        if (normal > 0)
        {
            while (true)
            {

                yield return new WaitForSeconds(Time.deltaTime);
                if (BaseFunc.SmoothStep(vignette.intensity.value, 1, md, out float outed))
                {
                    break;
                }
                vignette.intensity.value =  outed;

            }
        }
        else
        {
            while (true)
            {

                yield return new WaitForSeconds(Time.deltaTime);
                if (BaseFunc.SmoothStep(vignette.intensity.value, 0.283f, md, out float outed))
                {
                    break;
                }
                vignette.intensity.value = outed;

            }

        }
    }
    public IEnumerator Blacking(float dir)
    {
        float md = Mathf.Abs(dir);
        float normal = md / dir;
        if (normal > 0)
        {
            while (true)
            {


                

                    if (BaseFunc.SmoothStep(cm.gain.value.w, 0, md, out float outed))
                    {
                    break;

                    }
                    cm.gain.value = new Vector4(0, 0, 0, outed);
                   
                
                
                yield return new WaitForSeconds(Time.fixedDeltaTime);
                Debug.Log("Start");
            }
        }
        else
        {
            while (true)
            {

               
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
