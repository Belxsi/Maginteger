using UnityEngine;
using System;
using System.Collections;
public class NpcVisual : Visual
{
    public Element npc;
    public Color in_color,out_color;
    public Coroutine colortine;
    public void LocalInit()
    {

        npc = element;

    }
    public void DrawColor()
    {
        if (colortine != null)
        {
            spriteRenderer.color = out_color;
        }
        else
        {
            spriteRenderer.color = in_color;
        }
    }
    public IEnumerator DamageColor(float time = 1)
    {
        
        Color current=in_color;
        for(int i = 0;i<60*time;i++)
        {
            yield return new WaitForSeconds(0.001f);
            float d = i / (60 * time);
            current = in_color*(1-d) + Color.red*(d);
            out_color = current;
        }
        for (int i = (int)(60 * time); i >0; i--)
        {
            yield return new WaitForSeconds(0.001f);
            float d = i / (60 * time);
            current = in_color * (1 - d) + Color.red * (d);
            out_color = current;
        }
    }
    public void SetAllFalse()
    {
        animator.SetBool(Anboly.none + "", false);
        animator.SetBool(Anboly.run + "", false);
        animator.SetBool(Anboly.foward + "", false);
        animator.SetBool(Anboly.back + "", false);
    }
    public void AnimatorBehaviour()
    {
        switch (npc.IsMove)
        {
            case Dir.none:
                SetAllFalse();
                animator.SetBool(Anboly.none + "", true);
                
                break;
            case Dir.X:
                SetAllFalse();
                
                animator.SetBool(Anboly.run + "", true);
                break;
            case Dir.F:
                SetAllFalse();
                
                animator.SetBool(Anboly.foward + "", true);
                break;
            case Dir.B:

                SetAllFalse();
                animator.SetBool(Anboly.back + "", true);
                break;
        }


    }
    public void FlipFacing(Element element)
    {

        spriteRenderer.flipX = BaseFunc.GetMousePos().x < element.GetElementScreenPos().x;

    }
    // Update is called once per frame
    public void Action()
    {
        FlipFacing(element);
        AnimatorBehaviour();
        DrawColor();
    }
  
}
public class CurrentActionIEnumenator{
    public IEnumerator enumerator;
    public bool currentState;
    

}
