using UnityEngine;

public class SetColorSpriteMaterial : MonoBehaviour
{
    public SpriteRenderer sr;
   
    public void Awake()
    {
        BaseFunc.GetOfNull(ref sr, gameObject);
        
    }


    public void FixedUpdate()
    {
        sr.material.SetColor("_MainColor", sr.color);
    }
}
