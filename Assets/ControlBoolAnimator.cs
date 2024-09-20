using UnityEngine;

public class ControlBoolAnimator : MonoBehaviour
{

    Animator animator;
    public string name_bool;
    public bool value,active;
    
    void Update()
    {
        if (active)
        {
            active = false;
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
          
                animator.SetBool(name_bool, value);
            
        }
    }
}
