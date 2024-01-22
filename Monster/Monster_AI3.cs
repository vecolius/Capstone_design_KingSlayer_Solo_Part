using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_AI3 : MonoBehaviour
{
    public GameObject Monster;
    private Rigidbody2D rigi = null;
    private SpriteRenderer Sprite = null;
    private Monster_State State = null;
    public Transform Pos;
    public int nextMove;
    private float nextTinkTime;
    public bool cool;
    public float speed = 1.0f;

    public float distance; //player를 감지하는 거리
    public LayerMask isLayer; //탐색할 Layer
    public float atkDistance; //몬스터 공격 사거리
    public bool free;
    public bool distancebool;
    public GameObject animators;
    public Monster_aniator monster_Aniator;

    public SPUM_Prefabs _prefabs; //animation

    private Player_State Player = null;
    public GameObject Arrow;

    void Awake()
    {
        rigi = GetComponent<Rigidbody2D>();
        Sprite = GetComponent<SpriteRenderer>();
        State = GetComponent<Monster_State>();
        animators = transform.GetChild(0).gameObject;
        monster_Aniator = animators.GetComponent<Monster_aniator>();
        Invoke("Monster_Think", nextTinkTime);
    }

    void Start()
    {
        _prefabs = Monster.GetComponent<SPUM_Prefabs>();
        Player = GameManager.instance.Player.GetComponent<Player_State>();
        cool = true; // 공격 쿨타임
        distancebool = false; // 공격 모션을 유지하기 위해 사용
    }

    void FixedUpdate()
    {
        if(!State.die){
        rigi.velocity = new Vector2(nextMove, rigi.velocity.y);

        //낭떠러지를 만났을 때
        Vector2 frontVec = new Vector2(rigi.position.x + nextMove * 0.2f, rigi.position.y);
        Debug.DrawRay(rigi.position, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("place"));
        if (rayHit.collider == null)
        {
            if (rayHit.distance < 0.5f)
            {
                nextMove = nextMove * -1;
                if (nextMove == 1)
                {
                    _prefabs.transform.localScale = new Vector3(-1, 1, 1);
                }
                if (nextMove == -1)
                {
                    _prefabs.transform.localScale = new Vector3(1, 1, 1);
                }
                CancelInvoke();
                Invoke("Monster_Think", nextTinkTime);
            }
        }
        //원거리 몬스터 공격
        RaycastHit2D raycast_left = Physics2D.Raycast(transform.position, transform.right * -1, distance, isLayer);
        RaycastHit2D raycast_right = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);
        if (raycast_left.collider != null)
        {  //왼쪽에서 player 인식
            if (Vector2.Distance(Player.transform.position, Monster.transform.position) > atkDistance)
            {//공격사거리 밖에서 플레이어를 인식했을때 플레이어 쪽으로 이동
                Motionselect(2);
                _prefabs.transform.localScale = new Vector3(1, 1, 1);
                transform.position = Vector3.MoveTowards(transform.position, raycast_left.collider.transform.position, Time.deltaTime * speed);
            }
            else
            {//left공격 사거리안에 player 확인
                Motionselect(1);
                if (cool)
                {
                    CancelInvoke();
                    nextMove = 0;
                    free = true;
                    _prefabs.transform.localScale = new Vector3(1, 1, 1);
                    Motionselect(3);
                    //화살 생성
                    Invoke("arrowattackleft", 0.5f);// 화살 날리는 모션에 자연스럽게 화살이 날라가기 위해 사용
                    cool = false;
                    Invoke("attack_delay", 4f);
                }
            }
        }
        else if (raycast_right.collider != null)
        { //오른쪽에서 player 인식
            if (Vector2.Distance(Player.transform.position, Monster.transform.position) > atkDistance)
            { 
                  
            }
            else
            {//right공격 사거리에 player 확인
                Motionselect(1);
                if (cool)
                {
                    CancelInvoke();
                    nextMove = 0;
                    free = true;
                    _prefabs.transform.localScale = new Vector3(-1, 1, 1);
                    Motionselect(3);
                    //화살 생성
                    Invoke("arrowattackright", 0.5f);
                    cool = false;
                    Invoke("attack_delay", 4f);
                }   
            }
        }

        if (raycast_right.collider == null && raycast_left.collider == null)// 플레이어가 인식 범위안에 없을때 자동 이동
        {
            if (free == true)
            {
                Motionselect(1);
                Invoke("Monster_Think", nextTinkTime);
                free = false;
            }
        }
        }else{//die == true
            CancelInvoke();
        }
    }

    //몬스터의 움직임 생각
    void Monster_Think()
    {
        nextMove = Random.Range(-1, 2);
        nextTinkTime = Random.Range(2f, 3f);
        if (nextMove != 0)
        {
            if (nextMove == 1)
            {// 오른쪽
                _prefabs.transform.localScale = new Vector3(-1, 1, 1);
                Motionselect(2);
            }
            if (nextMove == -1)
            {// 왼쪽
                _prefabs.transform.localScale = new Vector3(1, 1, 1);
                Motionselect(2);
            }
        }
        if (nextMove == 0)
            Motionselect(1);

        Invoke("Monster_Think", nextTinkTime);
    }

    void attack_delay()// 공격 쿨타임
    {
        cool = true;
    }

    public void arrowattackleft()//왼쪽 화살 생성
    {
        GameObject arrow = Instantiate(Arrow);
        arrow.GetComponent<M_Arrow>().M_State = GetComponent<Monster_State>();
        arrow.transform.position = Pos.GetComponent<Transform>().position + new Vector3(0f, 0.1f, 0f);
        arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(-4, arrow.GetComponent<Rigidbody2D>().velocity.y);
        arrow.GetComponent<SpriteRenderer>().flipX = true;
    }
    
    public void arrowattackright()//오른쪽 화살 생성
    {
        GameObject arrow = Instantiate(Arrow);
        arrow.GetComponent<M_Arrow>().M_State = GetComponent<Monster_State>();
        arrow.transform.position = Pos.GetComponent<Transform>().position + new Vector3(0f, 0.1f, 0f);
        arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(4, arrow.GetComponent<Rigidbody2D>().velocity.y);
        arrow.GetComponent<SpriteRenderer>().flipX = false;
    }
    
    public void Motionselect(int i)
    {
        switch (i)
        {
            case 1:
                monster_Aniator.Animationselect(i);
                break;
            case 2:
                monster_Aniator.Animationselect(i);
                break;
            case 3:
                monster_Aniator.Animationselect(i);
                break;
        }
    }
}