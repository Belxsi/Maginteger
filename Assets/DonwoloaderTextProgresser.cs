using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class DonwoloaderTextProgresser : MonoBehaviour
{
    public List<ProgressVisitor> aos = new();
    public TextMeshProUGUI informbar, progress;
    public MapLevel ml;
    public ProgressVisitor GetActiveProgress()
    {

        for (int i = 0; i < aos.Count; i++)
        {
            if (aos[i].active)
            {
                return aos[i];
            }
        }
        return null;
    }
    ProgressVisitor actived = null;
    // Update is called once per frame
    void Update()
    {
       
        if (ml != null)
            if (aos != null)
                if (aos.Count > 0)
                {
                    if (actived == null)
                    {
                        actived = GetActiveProgress();

                    }
                    if (actived != null)
                        if (actived.active)
                        {
                            
                            informbar.text = actived.name;
                            if (informbar.text != "Верификация")
                            {
                                progress.text = (int)(actived.progress * 100) + "%";
                            }
                            else
                            {
                                progress.text = actived.progress + " / " + (ml.sum_rooms + ml.sum_tonel);
                            }
                            if (actived.IsDone)
                            {
                                aos.Remove(actived);
                                actived = null;
                            }
                        }
                        else
                        {
                            actived = GetActiveProgress();
                        }
                }
    }
}
