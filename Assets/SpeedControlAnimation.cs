using UnityEngine;

public class SpeedControlAnimation : MonoBehaviour
{
    Animator animator;
    public float speed;
    BoxPacker<float> Speed=new();
    void OnGUI()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (Speed.SetValue(speed))
        {
            animator.SetFloat("speed", speed);
        }
    }

    // Update is called once per frame
   
}
