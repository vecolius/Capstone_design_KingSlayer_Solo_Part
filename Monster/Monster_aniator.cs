using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_aniator : MonoBehaviour
{
    public Animator animator;
    public bool M_Attacked;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Animationselect(int i)
    {
        switch (i)
        {
            case 1:
                animator.SetBool("run", false);
                break;

            case 2:
                animator.SetBool("run", true);
                break;

            case 3:
                animator.SetTrigger("attack");
                break;
        }
    }
    public void Attackend()
    {
        animator.SetBool("New Bool", true);
    }

    void M_AttackTrue(){
        M_Attacked = true;
    }

    void M_AttackFalse(){
        M_Attacked = false;
    }
}
