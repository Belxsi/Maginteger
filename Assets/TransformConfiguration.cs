using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TransformConfiguration : MonoBehaviour
{
    public Waves waves;
    public RotationHis rotation;
    public List<ITransfiguration> list=new();
    public void Awake()
    {
        
        list.Add(waves);
        list.Add(rotation);
    }
    public void Update()
    {
        for(int i = 0; i < list.Count; i++)
        {
            if (list[i].enable)
            {
                list[i].Use();
            }
        }
    }

}
[Serializable]
public class Waves:ITransfiguration{
   
    public float x, y, z;
    public float xs, ys, zs;
   public float size=1;
    public Transform point;
    public bool rotatefix;
    public Waves(Transform transform,Transform point)
    {
        this.transform = transform;
        this.point = point;
    }

    public override void Use()
    {
        if(Time.timeScale!=0)
        if(point!=null){
            if (!loop) enable = false;

            transform.position = point.position + new Vector3(Mathf.Sin(Time.time * x / Time.timeScale) * xs, Mathf.Sin(Time.time * y / Time.timeScale) * ys, Mathf.Sin(Time.time * z / Time.timeScale) * zs) / size;
            if (rotatefix)
            {
                transform.rotation = Quaternion.Euler(0, 0, point.eulerAngles.z);
            }
        }
    }
   
}
[Serializable]
public class RotationHis : ITransfiguration
{

    public float x, y, z;
    public float angle;
    public Transform point;
    public RotationHis(Transform transform, Transform point)
    {
        this.transform = transform;
        this.point = point;
    }

    public override void Use()
    {
        if (!loop) enable = false;
        transform.RotateAround( point.position , new Vector3(x, y, z),angle);
    }

}
[Serializable]
public abstract class ITransfiguration
{
    public bool enable, loop;
    public Transform transform;
    public abstract void Use();
}
