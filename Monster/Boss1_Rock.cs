using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Rock : MonoBehaviour
{
    // Start is called before the first frame update

    public Monster_State M_State = null;

    void Start()
    {
        Destroy(gameObject, 2.0f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            M_State.M_Pos = transform.position;
            M_State.Monster_Attacked();
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "Wall" || other.gameObject.tag == "Ground") {
            Destroy(gameObject);
        }
    }
}
