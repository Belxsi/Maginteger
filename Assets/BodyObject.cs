

using System.Buffers;
using System.Collections;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static BodyObject;
using System;

public class BodyObject : Mattery
{
   
    Texture2D texture;
    public Texture2D GetTexture;
   
    public SpriteRenderer spriteRenderer;
    int x, y;
    public ParameterExtract parameter;
    public float extracted,savex;
    public float timeoutUS;
    public float[,] buffer,data;
    public bool getdated=false,setdated=false;
    public float getcounter, setcounter;
    public float GetState, SetState;
    public ExtractState currentState=ExtractState.End;
   // public const float delta = 5.3310953839169598595589559799376f;
    public float sumEX;
    public void Clear()
    {
        getcounter = 0;
        GetState = 0;
        data = new float[texture.width, texture.height];
        buffer = new float[texture.width, texture.height];
        sumEX = 0;
        extracted = 0;
        savex = 0;
        timeoutUS = 0;
        setcounter = 0;
        SetState = 0;
        x = 0; y = 0;
       
    }
    public void InitThisParameter(ParameterExtract pe)
    {
        parameter = pe;
    }
    public IEnumerator GetData()
    {
        Clear();
        currentState = ExtractState.Loaded;
        for (int x=0;x<texture.width;x++)
            for (int y = 0; y < texture.height; y++)
            {
                if(new System.Random().NextDouble()> 1)
                yield return new WaitForSeconds(0);
                Color color = texture.GetPixel(x, y);
                data[x,y] = ((color.r + color.g + color.b) / 3) * color.a;
                buffer[x, y] = data[x,y];
                sumEX += data[x, y];
                getcounter++;
        }
        currentState = ExtractState.Process;
        getdated = true;
    }
    public IEnumerator SetData()
    {
        setcounter = 0;
        SetState = 0;
        setdated = true;
        for (int x = 0; x < texture.width; x++)
            for (int y = 0; y < texture.height; y++)
            {
                if (new System.Random().NextDouble() > parameter.intensiveOfData)
                    yield return new WaitForSeconds(0);

                Color color=texture.GetPixel(x, y);



                texture.SetPixel(x, y, color * buffer[x,y]);
                setcounter++;
                texture.Apply();
            }
        setdated = false;
      
    }
    public float ExtractEnergy(float intensive, int iterat)
    {
        float result = 0;
        Vector2 scale =new ( texture.width,texture.height), pos = Vector2.zero;
        for (int i = 0; i < iterat; i++)
        {
         
            
            switch (parameter.type)
            {
                case TypeExtract.random:
                    pos = new Vector2(UnityEngine.Random.Range(0, scale.x), UnityEngine.Random.Range(0, scale.y));
                    break;
                case TypeExtract.consistently:
                    pos = new Vector2(x, y);
                    break;
                case TypeExtract.sinuality:
                    //pos = new Vector2(, );
                    break;
            }
            float b = buffer[(int)pos.x, (int)pos.y];
            buffer[(int)pos.x, (int)pos.y] *= 1 - intensive;
            result += b-buffer[(int)pos.x, (int)pos.y];
            x++;
            if (x >= texture.width) { x = 0; y++; }
            if (y >= texture.height) { x = 0; y = 0; }

        }
      
        extracted += result;
        savex += result;
        return result;
    }
    public void StartSprite()
    {       
        spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    }
    public void UpdateSprite()
    {
        if (timeoutUS <= 0)
        {
           
            if(!setdated)
            StartCoroutine(SetData());
            StartSprite();
            timeoutUS = parameter.timeUpdateSprite;
        }
        else timeoutUS -= Time.deltaTime;
    }
    public enum TypeExtract
    {
        random,
        consistently,
        sinuality
    }
    public Texture2D CopyTexture(Texture2D coped)
    {

        Texture2D texture = new(coped.width, coped.height);
        texture.SetPixels32(coped.GetPixels32());

        texture.Apply();
        return texture;
    }
    public void Awake()
    {
        texture = CopyTexture(spriteRenderer.sprite.texture);
        StartSprite();
    }
    public void AwakeExtract()
    {
     
        StartCoroutine(GetData());
    
    }
    public void ReturnResult()
    {
        getdated = false;
        setdated = false;
        currentState = ExtractState.Good;

    }

    public void UpdateExtract()
    {
        GetState=getcounter/(texture.width*texture.height);
        SetState = setcounter / (texture.width * texture.height);
       
        if (getdated)
        {
          //  if (extracted / sumEX<=SetState) 
            ExtractEnergy(parameter.inten, parameter.iter);
            UpdateSprite();
           
            if (Mathf.Round(sumEX*10)/10 <= Mathf.Round(savex*10)/10)
            {
                EndUpdateExtract();
                getdated = false;
                
                ReturnResult();
            }
            
        }
    }
    public void EndUpdateExtract()
    {
        GetState = getcounter / (texture.width * texture.height);
        SetState = setcounter / (texture.width * texture.height);

      
            //  if (extracted / sumEX<=SetState) 
            ExtractEnergy(parameter.inten, parameter.iter);
            UpdateSprite();

            

        
    }
    public void Update()
    {
        if ((currentState == ExtractState.Good)&(extracted==0))
        {
            Destroy(gameObject);
        }
      
    }
    public enum ExtractState
    {
        Loaded,
        Process,
        End,
        Good
    }
   
    public void BodyInit()
    {
        InitThis(this);
        Init();
    }
    public override bool UseMagic(Element creater)
    {
        return false;
    }
}
[Serializable]
public class ParameterExtract
{
    public float inten;
    public int iter;
    public float timeUpdateSprite, intensiveOfData;
    public TypeExtract type;

    public ParameterExtract(float inten, int iter, float timeUpdateSprite, float intensiveOfData, TypeExtract type)
    {
        this.inten = inten;
        this.iter = iter;
        this.timeUpdateSprite = timeUpdateSprite;
        this.intensiveOfData = intensiveOfData;
        this.type = type;
    }
}
