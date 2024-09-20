using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDistInvisibility : MonoBehaviour
{
    public List<DistInvisibility> invis = new();
    public StoperIEnumerator invisib;
    public bool control=true;
    public IEnumerator SetInvisibility(float intensive)
    {
        invisib = new StoperIEnumerator(true);
        int hash = invisib.GetHashCode();
        float inv = intensive;
        while (invisib.active)
        {
            if (invisib.GetHashCode() != hash) break;
            yield return new WaitForSeconds(Time.deltaTime);

            for(int i = 0; i < invis.Count; i++)
            {
                StateAlphing state = invis[i].Alphing(inv);
                if (state == StateAlphing.On )
                {
                    invisib.active = false;
                }
            }
        }
    }
}
