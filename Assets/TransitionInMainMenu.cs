using UnityEngine;

public class TransitionInMainMenu : MonoBehaviour
{
    public Animator animator,fon;
    public string autro = "autromenu";
    public void OnMove()
    {
        animator.Play("smoke_move",-1,0);
        fon.Play(autro, -1, 0);
    }
}
