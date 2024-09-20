using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visual : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Element element;
    // Update is called once per frame
    public int GetLayer()
    {
        return spriteRenderer.sortingOrder;
    }
    
    public void SetLayer(int order)
    {
        spriteRenderer.sortingOrder=order;
    }
    public void InitElement(Element element)
    {
        this.element = element;
    }
    public void Init()
    {
        if (!animator)
        {
            animator = GetComponent<Animator>();
         
        }

        if (!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

        }
    }
   
}
