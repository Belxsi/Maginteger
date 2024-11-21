using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
public class IntroPuttingText : MonoBehaviour
{
    public TextMeshPro tmp_intro;
    public List<string> scn = new();
    public string sum,split;
    public int currentStep=-1,oldstep=-1;
    public float speedText,timeout,allspeedwave;
    public Vector2 speedWaveColor,offsetwave,sizewave;
    public Transform target,loadbar,end;
    public Animator AniBar;
    public SpriteRenderer LoadBarRenderer;
    public RGBContrIntroCube CIC;
    public Exposuring exposuring;
    public OneSetIEnumerator blooming;
    public PlayAudioClip select, On,TextRead;
    public OneSetObject<int> stepObserver;
    public SceneMoveTimeOut SceneMoveTimeOut;
    void Awake()
    {
        stepObserver = new(currentStep);
        blooming= new OneSetIEnumerator(exposuring.Blomming(0.1f), this);
        
        
    }
    public IEnumerator Sumbito(string str)
    {
        split = "";
        AnimationSwitcher();
        for (int i = 0; i < str.Length; i++)
        {
            split += str[i];
            TextRead.play = true;
            yield return new WaitForSeconds(Time.deltaTime/speedText);
        }
        OnPlayAudioEffect(currentStep);
       
        yield return new WaitForSeconds(timeout);
        oldstep= currentStep;
        for (int i = 0; i < oldstep; i++)
        {
            sum += scn[i] + "\n";

        }
        
        split = "";
    }
    public void OnPlayAudioEffect(int step)
    {
        if(stepObserver.Set(step))
        switch (step)
        {
            case 0:
            case 2:
            case 4:
            case 5:
            case 7:
                select.play = true;
                break;
            case 1:
            case 3:
            case 6:
            case 8:           
                On.play = true;
                break;

        }
    }
    public void AnimationSwitcher()
    {
      
        switch (Mathf.Ceil( oldstep/2f)+1)
        {
            case 1:
                
                AniBar.SetBool("LoadBar", true);
                CIC.speedRotate = 1;
                CIC.PowColor = 5;
                break;
            case 2:
                AniBar.SetBool("ClearMind", true);
                AniBar.SetBool("LoadBar", false);
                CIC.speedRotate = 2;
                CIC.PowColor = 10;
                break;
            case 3:
                AniBar.SetBool("ConnectNet", true);
                AniBar.SetBool("ClearMind", false);
                CIC.speedRotate = 4;
                CIC.PowColor = 20;
                break;
            case 4:
                AniBar.SetBool("End", true);
                AniBar.SetBool("ConnectNet", false);
                CIC.speedRotate = 8;
                CIC.PowColor = 40;
                break;
            
            default:
                
                break;

        }
    }
    public IEnumerator SetSplit(int step)
    {
        currentStep =scn.Count- step;
        yield return Sumbito(scn[currentStep]);

    }
    // Update is called once per frame
   
    void Update()
    {

       
        sum = "";
        for(int i = 0; i <= oldstep; i++)
        {
            sum += scn[i]+"\n";
        }
        if (currentStep >= 2)
        {
            allspeedwave = Mathf.SmoothStep(allspeedwave, 1, 0.1f);
            float x = Mathf.Sin((Time.time+Random.Range(0,offsetwave.x))* speedWaveColor.x);
            float y = Mathf.Sin((Time.time + Random.Range(0, offsetwave.y)) * speedWaveColor.y);
            Camera.main.transform.position = new Vector3(x/sizewave.x * allspeedwave, y/sizewave.y * allspeedwave, -10);
        }
        float d = Vector2.Distance(end.position, target.position);
        loadbar.position = target.position + ((float)(currentStep+1)/scn.Count) * d * (end.position-target.position).normalized;
        AnimationSwitcher();
       
        tmp_intro.text = sum + split;
        if (currentStep == scn.Count - 1)
        {

            LoadBarRenderer.color = Color.green;
            CIC.PowColor = Mathf.SmoothStep(CIC.PowColor, 100000, 0.25f);
            CIC.speedRotate = Mathf.SmoothStep(CIC.speedRotate, 100000, 0.25f);
            CIC.transform.localScale = Vector3.one * Mathf.SmoothStep(1, 12, 0.25f);
            blooming.Set();
            SceneMoveTimeOut.work = true;
        }


    }
}
