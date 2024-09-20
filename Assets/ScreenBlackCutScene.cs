using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBlackCutScene : MonoBehaviour
{
    public Animator animator;
    public static ScreenBlackCutScene me;
    void Awake()
    {
        me = this;
    }

    // Update is called once per frame
    public void SetBlack(bool trfl)
    {
        animator.SetBool("Blacking", trfl);

    }
    
}
