using UnityEngine;

public class ActivateAnimator : MonoBehaviour
{
    public bool active;
    Animator animator;
    void Update()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animator.enabled = active;

    }
}
