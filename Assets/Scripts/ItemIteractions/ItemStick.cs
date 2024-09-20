using System.Collections;
using UnityEngine;

public class ItemStick : IItemIteraction
{
    public CurrentTriggerCollision ctc;
    float seconds=1;
    public override void Click()
    {
    }

    public override IEnumerator RepeatUse()
    {
        float timeout=seconds;
        while (true)
        {
            
            BodyObject bo = BaseFunc.GetEnemyCollision(ctc, ref state, Player.me);
            if (bo != null)
            {
                bo.matteryEnergy.LiqEnergy += 1;
                break;
            }
            timeout -= Time.deltaTime;
            if (timeout <= 0) break;
            if(Player.me.HandAnimator.GetCurrentAnimatorClipInfo(0).Length>0)
            if (Player.me.HandAnimator.GetCurrentAnimatorClipInfo(0)[^1].clip.name == "none")
            {
                break;
            }
            yield return new WaitForSeconds(0);
        }

        Player.me.AtackItem(false);
    }

    public override string ToStringInfo()
    {
        return "Просто палка, которой можно махать";
    }

    public override void Use(Element Creater)
    {
        if (Player.TryGetPlayer())
        {
            Player.me.AtackItem(true);
          

            StartCoroutine(RepeatUse());
            
        }
    }
}
