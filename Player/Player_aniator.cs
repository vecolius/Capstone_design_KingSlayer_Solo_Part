using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_aniator : MonoBehaviour
{
    public Animator animator;
    private Attack Atk = null;
    public bool Attacked = false;
    public bool isDash = false;
    void Start() {
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
                animator.SetTrigger("normal");
                break;

            case 4:
                animator.SetTrigger("bow");
                break;

            case 5:
                animator.SetTrigger("magic");
                break;
            case 6:
                animator.SetTrigger("dash");
                break;
            case 7:
                animator.SetTrigger("jump");
                break;
        }
    }
    void AttackTrue() {
        Attacked = true;
    }

    void AttackFalse() {
        Attacked = false;
    }
    void DashFalse() {
        isDash = false;
    }

}
