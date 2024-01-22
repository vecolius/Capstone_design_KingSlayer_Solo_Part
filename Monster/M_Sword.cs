using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Sword : MonoBehaviour
{
    private Monster_State M_State = null;
    private Monster_aniator Ani = null;
    private BoxCollider2D BoxCollider2D = null;

    // Start is called before the first frame update
    void Start()
    {
        M_State = GetComponentInParent<Monster_State>();
        Ani = GetComponentInParent<Monster_aniator>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(M_State.die == true)
            BoxCollider2D.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collider) {
        if(collider.tag == "Player") {
            if(Ani.M_Attacked) {
                M_State.M_Pos = transform.position;
                M_State.Monster_Attacked();
                Ani.M_Attacked = false;
            }
        }
    }

}
