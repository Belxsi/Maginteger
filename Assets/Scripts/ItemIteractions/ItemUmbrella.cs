using System.Collections;
using UnityEngine;

public class ItemUmbrella : IItemIteraction
{
    public Animator ItemAnimator;
    public bool open;
    public CurrentTriggerCollision ctc;
    float seconds=0.25f;
    public GameObject Shield;
    public override void Click()
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator RepeatUse()
    {
        float timeout = seconds;
        while (true)
        {

            BodyObject bo = BaseFunc.GetEnemyCollision(ctc, ref state, Player.me);
            if (bo != null)
            {
                bo.matteryEnergy.LiqEnergy += 2.5f;
                break;
            }
            timeout -= Time.deltaTime;
            if (timeout <= 0) break;
            
            yield return new WaitForSeconds(0);
        }

        Shield.SetActive(open);

    }

    public override string ToStringInfo()
    {
       return " огда раскрываетс€, атакует врагов вокруг себ€. ѕосле раскрыти€ становитс€ щитом.";
    }

    public override void Use(Element Creater)
    {
        if (!open)
        {
            open = true;
           
            StartCoroutine(RepeatUse());
            
        }
        else
        {
            open = false;
            Shield.SetActive(open);
        }
        ItemAnimator.SetBool("open", open);
    }

}
