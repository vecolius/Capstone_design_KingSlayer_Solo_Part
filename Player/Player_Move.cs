using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_Move : MonoBehaviour
{
    public bool Right;
    public bool Left;
    public bool Isjump;
    public bool Ischeck;
    public bool jumpdelay;
    public bool doubleJump;
    public bool Nodamage = false;
    public float DashForce;


    private Vector2 P_pos;
    private Player_State State = null;
    private Rigidbody2D Rigi = null;
    private SpriteRenderer Sprite = null;
    private BoxCollider2D Box = null;
    public RaycastHit2D RayHit;
    public Player_aniator player_Aniator;
    
    public SPUM_Prefabs _prefabs;
    private float Hp;
    public bool OnDamage = false;
    public SpriteRenderer[] Color;
    public enum PlayerState
    {
        idle,
        run,
        attack_Normal,
        attack_Bow,
        attack_Magic,
        death,
    }
    public PlayerState _currentState;

    void Awake() {
        State = GetComponent<Player_State>();
        Rigi = GetComponent<Rigidbody2D>();
        Sprite = GetComponent<SpriteRenderer>();
        Box = GetComponent<BoxCollider2D>();
        player_Aniator = transform.GetChild(0).GetComponent<Player_aniator>();

        Isjump = false;
        Hp = DateManager.instance.nowPlayer.Player_Hp;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _prefabs = GameManager.instance.Player.GetComponent<SPUM_Prefabs>();
        jumpdelay=true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Sprite.flipX == false) {
            Right = true;
            Left = false;
        } else {
            Left = true;
            Right = false;
        }
        Move();
        Dash();
        Wcheck();
        Invoke("raycheck", 0.1f);
        Jump();
        if(player_Aniator.isDash) {
            Box.usedByEffector = false;
            Physics2D.IgnoreLayerCollision(8,3,true);
        } else {
            Box.usedByEffector = true;
            Physics2D.IgnoreLayerCollision(8,3,false);
        }
    }

    private void FixedUpdate() {
    }
    //moving
    void Move() {
        if (Input.GetKey(ControlManager.instance.inputs[0]) && OnDamage == false && player_Aniator.isDash == false) {
            //Rigi.MovePosition (Rigi.position + Vector2.right * State.Move_Speed * Time.deltaTime);
            Rigi.velocity = new Vector2(State.Move_Speed,Rigi.velocity.y);
            Sprite.flipX = false;
            Motionselect(2);
           _prefabs.transform.localScale = new Vector3(-1,1,1);
        }
        if (Input.GetKey(ControlManager.instance.inputs[1]) && OnDamage == false && player_Aniator.isDash == false ) {
            Rigi.velocity = new Vector2(-(State.Move_Speed),Rigi.velocity.y);
            Sprite.flipX = true;
            Motionselect(2);
           _prefabs.transform.localScale = new Vector3(1,1,1);
        }
        if ((Input.GetKeyUp(ControlManager.instance.inputs[0]) || Input.GetKeyUp(ControlManager.instance.inputs[1])) && OnDamage == false) {
            Rigi.velocity = new Vector2(0,Rigi.velocity.y);
            Motionselect(1);
        }
        
    }
    //jump
    void Jump() {
        if (Input.GetKey(ControlManager.instance.inputs[2])&&jumpdelay && OnDamage == false && player_Aniator.isDash == false)
        {
            if (!Isjump || doubleJump)
            {
                if(doubleJump)
                {
                Motionselect(7);
                Rigi.velocity = new Vector2(0f,0f);
                Rigi.AddForce(Vector2.up * State.Jumppower * State.Jumppower);
                doubleJump = !doubleJump;
                Isjump = true;
                jumpdelay = false;
                Invoke("Jumpdelay", 0.5f);
                }
                else
                {
                Motionselect(7);
                Rigi.AddForce(Vector2.up * State.Jumppower * State.Jumppower);
                doubleJump = !doubleJump;
                Isjump = true;
                jumpdelay = false;
                Invoke("Jumpdelay", 0.5f);
                }
            }
        }
    }

    void Jumpdelay()
    {
        jumpdelay=true;
    }


    void raycheck()
    {
        Vector2 frontVec = new Vector2(Rigi.position.x, Rigi.position.y); // 아래로 레이를 적용하여 place일때 작동
        Debug.DrawRay(Rigi.position, Vector3.down, new Color(0, 0.1f, 0));
        RaycastHit2D RayHit = Physics2D.Raycast(frontVec, Vector3.down, 0.1f, LayerMask.GetMask("place"));
        if (RayHit.collider != null)
        {
            Isjump = false;
            doubleJump = false;
        }
    }

    //dash
    void Dash() {
        if(DateManager.instance.nowPlayer.Stamina > 0) {
            if(Input.GetKeyDown(ControlManager.instance.inputs[3])&& OnDamage == false && player_Aniator.isDash == false) {
                Motionselect(6);
                if(Right) {
                    Rigi.velocity = new Vector2(1.0f * DashForce, Rigi.velocity.y);
                    //Rigi.AddForce(Vector2.right * DashForce, ForceMode2D.Force);
                    DateManager.instance.nowPlayer.Stamina --;
                    player_Aniator.isDash = true;
                }
                else {
                    Rigi.velocity = new Vector2(-1.0f * DashForce, Rigi.velocity.y);
                    //Rigi.AddForce(Vector2.left * DashForce, ForceMode2D.Force);
                    DateManager.instance.nowPlayer.Stamina --;
                    player_Aniator.isDash = true;
                }
            }
        }

/*        if (DateManager.instance.nowPlayer.Stamina > 0) {
            if(Input.GetKeyDown(ControlManager.instance.inputs[3])&& OnDamage == false) {
                Motionselect(6);
                RaycastHit2D[] Hit = null;
                if(Sprite.flipX == false) {
                    Hit = Physics2D.RaycastAll(transform.position,transform.right, 1.0f * DashForce);
                    for (int i = 0 ; i<Hit.Length;i++) {
                        if(Hit[i].collider.tag == "Wall") {
                            P_pos = transform.position;
                            P_pos.x = Hit[i].point.x - (Box.size.x/2 * transform.localScale.x);
                            //transform.position = P_pos;
                            Rigi.MovePosition(P_pos);
                        }
                        else {
                            Rigi.MovePosition (Rigi.position + Vector2.right * DashForce);
                        }
                    }
                    DateManager.instance.nowPlayer.Stamina --;
                }
                else if(Sprite.flipX == true) {
                    Hit = Physics2D.RaycastAll(transform.position,-transform.right, 1.0f * DashForce);
                    for (int i = 0 ; i<Hit.Length;i++) {
                        if(Hit[i].collider.tag == "Wall") {
                            P_pos = transform.position;
                            P_pos.x = Hit[i].point.x + (Box.size.x/2 * transform.localScale.x);
                            //transform.position = P_pos;
                            Rigi.MovePosition(P_pos);
                        }
                        else {
                            Rigi.MovePosition (Rigi.position + Vector2.left * DashForce);
                        }
                    }
                    DateManager.instance.nowPlayer.Stamina --;
                }
                
            }
        }
*/
    }

    //wcheck
    void Wcheck()
    {
        if (Input.GetKeyDown(ControlManager.instance.inputs[5]))
        {
            Ischeck = true;
        }
        if (Input.GetKeyUp(ControlManager.instance.inputs[5]))
        {
            Ischeck = false;
        }
        
    }
    //object interaction

    public void Motionselect(int i)
    {
        switch (i)
        {
            case 1:
                player_Aniator.Animationselect(i);//제자리
                break;
            case 2:
                player_Aniator.Animationselect(i);//달리기
                break;
            case 3:
                player_Aniator.Animationselect(i);//공격
                break;
            case 4:
                player_Aniator.Animationselect(i);//활 공격
                break;
            case 5:
                player_Aniator.Animationselect(i);//마법 공격
                break;
            case 6:
                player_Aniator.Animationselect(i);//대쉬
                break;
            case 7:
                player_Aniator.Animationselect(i);//점프
                break;
        }
    }

}


