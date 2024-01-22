using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_AI : MonoBehaviour
{
    public GameObject Monster;
    private Rigidbody2D rigi = null;
    private SpriteRenderer Sprite = null;
    private Monster_State State = null;
    public Transform Pos;
    private float nextTinkTime;
    public bool cool;
    public bool dashcool;
    public bool isDashing;
    public float speed = 1f;

    public float distance; //player를 감지하는 거리
    public LayerMask isLayer; //탐색할 Layer
    public float atkDistance; //몬스터 공격 사거리
    public bool free;
    public bool distancebool;
    public GameObject animators;
    public Monster_aniator monster_Aniator;

    public SPUM_Prefabs _prefabs; //animation

    private Player_State Player = null;
    public GameObject Rock;

    private float dashTime;
    private float dashingPower;
    private float horizontal;
    private bool isdash;
    
    void Awake()
    {
        rigi = GetComponent<Rigidbody2D>();
        Sprite = GetComponent<SpriteRenderer>();
        State = GetComponent<Monster_State>();
        animators = transform.GetChild(0).gameObject;
        monster_Aniator = animators.GetComponent<Monster_aniator>();
    }

    void Start()
    {
        _prefabs = Monster.GetComponent<SPUM_Prefabs>();
        Player = GameManager.instance.Player.GetComponent<Player_State>();
        cool = true; // 공격 쿨타임
        dashcool=true;// 대쉬 공격 쿨타임
        distancebool = false; // 공격 모션을 유지하기 위해 사용
        isDashing=false;
        dashingPower = 350f;
    }

    void FixedUpdate()
    {
        if(!State.die){
        horizontal = Input.GetAxisRaw("Horizontal");

        //보스 몬스터 공격
        RaycastHit2D raycast_left = Physics2D.Raycast(transform.position, transform.right * -1, distance, isLayer);
        RaycastHit2D raycast_right = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);
        if (raycast_left.collider != null)
        {  //왼쪽에서 player 인식
            if (dashcool)
            {
                int dash = Random.Range(0, 99);
                if(dash < 50)
                {
                    dashcool=false;
                    isDashing=true;
                    _prefabs.transform.localScale = new Vector3(2, 2, 1);
                    rigi.velocity = new Vector2(0f, rigi.velocity.y);
                    Motionselect(3);
                    rigi.AddForce(-1*transform.right * dashingPower);
                    Invoke("isDash", 2f);
                    Invoke("dash_delay", 4f);
                }
            }
            else if(!isDashing)
            {
                if (Vector2.Distance(Player.transform.position, Monster.transform.position) > atkDistance)
                { 

                }
                else
                {//left공격 사거리안에 player 확인
                    if (cool)
                    {
                        //CancelInvoke();
                        _prefabs.transform.localScale = new Vector3(2, 2, 1);
                        Motionselect(3);
                        //돌 생성
                        Rockattackleft();
                        cool = false;
                        Invoke("attack_delay", 4f);
                    }
                }
            }
        }
        else if (raycast_right.collider != null)
        { //오른쪽에서 player 인식
            if (dashcool)
            {
                int dash = Random.Range(0, 99);
                if (dash < 50)
                {
                    dashcool = false;
                    isDashing = true;
                    _prefabs.transform.localScale = new Vector3(-2, 2, 1);
                    rigi.velocity = new Vector2(0f, rigi.velocity.y);
                    Motionselect(3);
                    rigi.AddForce(transform.right * dashingPower);
                    Invoke("isDash", 2f);
                    Invoke("dash_delay", 4f);
                }
            }
            if (Vector2.Distance(Player.transform.position, Monster.transform.position) > atkDistance)
            { 

            }
            else
            {//right공격 사거리에 player 확인
                if (cool)
                {
                    //CancelInvoke();
                    _prefabs.transform.localScale = new Vector3(-2, 2, 1);
                    Motionselect(3);
                    //돌 생성
                    Rockattackright();
                    cool = false;
                    Invoke("attack_delay", 4f);
                }
            }
        }

        if (raycast_right.collider == null && raycast_left.collider == null)// 플레이어가 인식 범위안에 없을때 자동 이동
        {
            
        }
        }else{ //die == true
            CancelInvoke();
        }
    }


    void attack_delay()// 공격 쿨타임
    {
        cool = true;
    }

    void dash_delay()// 대쉬 공격 쿨타임
    {
        dashcool = true;
        
    }
    void isDash()// 대쉬 공격 쿨타임
    {
        isDashing = false;
    }

    public void Rockattackleft()//왼쪽 바위 생성
    {
        GameObject rock = Instantiate(Rock);
        rock.GetComponent<Boss1_Rock>().M_State = GetComponent<Monster_State>();
        rock.transform.position = Pos.GetComponent<Transform>().position;
        rock.transform.localScale = new Vector3(4, 4, 1);
        rock.GetComponent<Rigidbody2D>().velocity = new Vector2(-4, 4);
    }
    public void Rockattackright()//오른쪽 바위 생성
    {
        GameObject rock = Instantiate(Rock);
        rock.GetComponent<Boss1_Rock>().M_State = GetComponent<Monster_State>();
        rock.transform.position = Pos.GetComponent<Transform>().position;
        rock.transform.localScale = new Vector3(4, 4, 1);
        rock.GetComponent<Rigidbody2D>().velocity = new Vector2(4, 4);
    }
  
    public void Motionselect(int i)
    {
        switch (i)
        {
            case 1:
                monster_Aniator.Animationselect(i);//제자리
                break;
            case 2:
                monster_Aniator.Animationselect(i);//달리기
                break;
            case 3:
                monster_Aniator.Animationselect(i);//공격
                break;
        }
    }
}