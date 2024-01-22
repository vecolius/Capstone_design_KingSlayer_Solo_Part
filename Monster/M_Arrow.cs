using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    public Monster_State M_State = null;
    // Update is called once per frame
    void Update() {
        if(M_State.die == true)
            Destroy(gameObject);
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

