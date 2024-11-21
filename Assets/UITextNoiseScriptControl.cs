using UnityEngine;

public class UITextNoiseScriptControl : MonoBehaviour
{
    public Material material;
    public Color color;
    public float Pow;
   
    public void Update()
    {
        material.SetColor("m_Color",color);
        material.SetFloat("_Pow", Pow);
    }
}
