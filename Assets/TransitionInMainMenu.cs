using UnityEngine;

public class TransitionInMainMenu : MonoBehaviour
{
    public Animator animator;
    public void OnMove()
    {
        animator.Play("smoke_move",-1,0);
    }
}
