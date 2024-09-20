using UnityEngine;

public class CustomScrollRect : MonoBehaviour
{
    public RectTransform viewport;
    public bool smooth;
    public float seventi = 0.5f;
    
    

    // Update is called once per frame
    public void ViewControl(int s,float offset,float start=0)
    {
        if (!smooth)
        {
            viewport.offsetMin = new(s * offset + start, 0);
        }
        else
        {
            viewport.offsetMin = new(Mathf.SmoothStep(viewport.offsetMin.x, s * offset + start,seventi), 0);
        }
    }
}
