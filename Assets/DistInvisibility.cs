using System.Collections;
using UnityEngine;
public class DistInvisibility : MonoBehaviour
{
    public Vector2 offsetTrigger;
    public float min_a = 0.5f;
    public SpriteRenderer sr;
    public GameObject point;
    public bool ishide;
    public GlobalDistInvisibility gdi;
    public StateAlphing state;
    public void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

    }
    public StateAlphing Alphing(float sum)
    {

        if ((sr.color.a + sum > min_a) & (sr.color.a + sum <= 1))
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a + sum);
            state = StateAlphing.Process;
        }
        else
        {
            if (sr.color.a + sum < min_a)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, min_a);
                state = StateAlphing.Off;
            }
            if (sr.color.a + sum > 1)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
                state = StateAlphing.On;
            }
        }
        return state;
    }
    public void Invisibility()
    {
        //Vector3 pos = BaseFunc.AbsVector3(Player.me.transform.position);
        if (Player.TryGetPlayer())
            if (Player.me.transform.position.y > point.transform.position.y + offsetTrigger.y)
            {
                float xr = Mathf.Abs(Player.me.transform.position.x - point.transform.position.x);
                if (xr < offsetTrigger.x)
                {
                    Alphing(-Time.deltaTime);
                }
                else Alphing(Time.deltaTime);
            }
            else Alphing(Time.deltaTime);
    }
   
    void Update()
    {
       


        //Invisibility();
        if (gdi != null)
            if (gdi.control)
            {
                if (ishide)
                {
                    Alphing(-Time.deltaTime);

                }
                else Alphing(Time.deltaTime);
            }
        

    }
}
public enum StateAlphing { 
    Off,
    Process,
    On

}
