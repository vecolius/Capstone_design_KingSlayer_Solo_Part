using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Player_State State = null;
    private Player_Attack Atk = null;
    private BoxCollider2D Box = null;
    private Player_aniator Ani = null;
    // Start is called before the first frame update
    void Start()
    {
        State = GetComponent<Player_State>();
        Atk = GameManager.instance.Player.GetComponent<Player_Attack>();
        Box = GetComponent<BoxCollider2D>();
        Ani = GameManager.instance.Player.transform.GetChild(0).GetComponent<Player_aniator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Atk.My_Weapon != Player_Attack.Weapon_Types.Sword) {
            Box.isTrigger = false;
            Box.enabled = false;
        } else {
            Box.enabled = true;
            Box.isTrigger = true;
        }

    }

    private void OnTriggerStay2D(Collider2D collider) {
        if(collider.tag == "Monster") {
            if(Ani.Attacked) {
            collider.GetComponent<Monster_State>().P_Damage(2.0f);
            Ani.Attacked = false;
            }
        }
    }
}
