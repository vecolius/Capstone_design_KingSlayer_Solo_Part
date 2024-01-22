using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    private Player_State State = null;
    private Player_Move Move = null;
    public GameObject Wind;

    public GameObject Arrow;
    public GameObject BladeStrom;

    public GameObject Powered_Arrow;
    public GameObject Thunder;
    public GameObject Kill;
    public Transform Pos;
    private SpriteRenderer Sprite = null;
    public Player_aniator player_Aniator;
    public bool isAtk = false;
    public bool isSkill = false;
    public float Skill_cool = 0;
    public int ArrowCount = 0; //특수화살 개수

    public enum Weapon_Types {
        Sword, Bow, Fan, God
    }

    public Weapon_Types My_Weapon = Weapon_Types.Sword;
    public AudioClip clip;

    void Awake() {
        
        State = GetComponent<Player_State>();
        Move = GetComponent<Player_Move>();
        Sprite = GetComponent<SpriteRenderer>();
        player_Aniator = transform.GetChild(0).GetComponent<Player_aniator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetMouseButton(0) && player_Aniator.isDash == false) {
            if (isAtk == false) {
                switch(My_Weapon) {
                    case Weapon_Types.Sword:
                    Move.Motionselect(3);
                    break;

                    case Weapon_Types.Bow:
                        //Move._currentState = Player_Move.PlayerState.attack_Bow;
                        Move.Motionselect(4);
                        if(ArrowCount != 0){//특수화살
                            if(Move.Right){
                                GameObject powered_arrow = Instantiate(Powered_Arrow);
                                powered_arrow.GetComponent<Range_Attack>().isForce = true;
                                powered_arrow.transform.position = Pos.transform.position;
                                powered_arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(5,powered_arrow.GetComponent<Rigidbody2D>().velocity.y);
                                powered_arrow.GetComponent<SpriteRenderer>().flipX = false;
                            }
                            else if(Move.Left){
                                GameObject powered_arrow = Instantiate(Powered_Arrow);
                                powered_arrow.transform.position = Pos.transform.position;
                                powered_arrow.GetComponent<Range_Attack>().isForce = true;
                                powered_arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(-5,powered_arrow.GetComponent<Rigidbody2D>().velocity.y);
                                powered_arrow.GetComponent<SpriteRenderer>().flipX = true;
                            }
                            ArrowCount -= 1;
                            }
                            else{//일반화살
                                if(Move.Right) {
                                GameObject arrow = Instantiate(Arrow);
                                arrow.transform.position = Pos.transform.position;
                                arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(5,arrow.GetComponent<Rigidbody2D>().velocity.y);
                                arrow.GetComponent<SpriteRenderer>().flipX = false;
                            }
                            else if(Move.Left) {
                                GameObject arrow = Instantiate(Arrow);
                                arrow.transform.position = Pos.transform.position;
                                arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(-5,arrow.GetComponent<Rigidbody2D>().velocity.y);
                                arrow.GetComponent<SpriteRenderer>().flipX = true;
                            }        
                        }           
                        break;

                    case Weapon_Types.Fan :
                        //Move._currentState = Player_Move.PlayerState.attack_Magic;
                        Move.Motionselect(5);
                        if(Move.Right) {
                            GameObject wind = Instantiate(Wind);
                            wind.transform.position = Pos.transform.position + new Vector3(0,0.1f,0);
                            wind.GetComponent<Rigidbody2D>().velocity = new Vector2(5,wind.GetComponent<Rigidbody2D>().velocity.y);
                        }
                        else if(Move.Left) {
                            GameObject wind = Instantiate(Wind);
                            wind.transform.position = Pos.transform.position + new Vector3(0,0.1f,0);
                            wind.GetComponent<Rigidbody2D>().velocity = new Vector2(-5,wind.GetComponent<Rigidbody2D>().velocity.y);
                        }
                        SoundManager.instance.SFXplay("03. Clean Whoosh", clip);
                        break;
                    case Weapon_Types.God :
                        Move.Motionselect(5);
                        if(Move.Right) {
                            GameObject wind = Instantiate(Wind);
                            wind.transform.position = Pos.transform.position + new Vector3(0,0.1f,0);
                            wind.GetComponent<Rigidbody2D>().velocity = new Vector2(5,wind.GetComponent<Rigidbody2D>().velocity.y);
                        }
                        else if(Move.Left) {
                            GameObject wind = Instantiate(Wind);
                            wind.transform.position = Pos.transform.position + new Vector3(0,0.1f,0);
                            wind.GetComponent<Rigidbody2D>().velocity = new Vector2(-5,wind.GetComponent<Rigidbody2D>().velocity.y);
                        }
                        SoundManager.instance.SFXplay("03. Clean Whoosh", clip);
                        break;                        
                }
                StartCoroutine(Weapon_Cooltime());

            }
        }
        
        if(Input.GetMouseButtonUp(1) && player_Aniator.isDash == false){ // 무기 전용 스 킬
            if(isSkill == false) {
                switch(My_Weapon) {
                    case Weapon_Types.Sword :
                    if(Move.Right) {
                        GameObject BS = Instantiate(BladeStrom);
                        BS.transform.position = Pos.transform.position + new Vector3(0,0.35f,0);
                        BS.GetComponent<Rigidbody2D>().velocity = new Vector2(5,BS.GetComponent<Rigidbody2D>().velocity.y);
                        BS.GetComponent<SpriteRenderer>().flipX = false;                        
                    }
                    else if(Move.Left) {
                        GameObject BS = Instantiate(BladeStrom);
                        BS.transform.position = Pos.transform.position + new Vector3(0,0.35f,0);
                        BS.GetComponent<Rigidbody2D>().velocity = new Vector2(-5,BS.GetComponent<Rigidbody2D>().velocity.y);
                        BS.GetComponent<SpriteRenderer>().flipX = true;                               
                    }
                    Move.Motionselect(3);
                    Skill_cool = 10.0f ;
                    StartCoroutine(Skill_Cooldown());
                    break;

                    case Weapon_Types.Bow:
                    if(ArrowCount <= 3)
                    ArrowCount = 3;
                    Skill_cool = 5.0f;
                    StartCoroutine(Skill_Cooldown());
                    break;

                    case Weapon_Types.Fan:
                    if(Move.Right) {
                        GameObject Th = Instantiate(Thunder);
                        Th.transform.position = Pos.transform.position + new Vector3(5.0f,0.7f,0);
                        Th.GetComponent<Range_Attack>().isThunder = true;
                        Th.GetComponent<SpriteRenderer>().flipX = false;
                    }
                    else if(Move.Left) {       
                        GameObject Th = Instantiate(Thunder);
                        Th.transform.position = Pos.transform.position + new Vector3(-5.0f,0.7f,0);
                        Th.GetComponent<Range_Attack>().isThunder = true;
                        Th.GetComponent<SpriteRenderer>().flipX = true;                                             
                    }
                    Skill_cool = 20.0f;
                    StartCoroutine(Skill_Cooldown());                 
                    break;
                    case Weapon_Types.God:
                    if(Move.Right) {
                        GameObject km = Instantiate(Kill);
                        km.transform.position = Pos.transform.position;
                    }
                    else if(Move.Left) {       
                        GameObject km = Instantiate(Kill);
                        km.transform.position = Pos.transform.position;                                          
                    }
                    Skill_cool = 1.0f;
                    StartCoroutine(Skill_Cooldown());                 
                    break;                    
                }
            }
        }
    }

    IEnumerator Weapon_Cooltime() {
        isAtk = true;
        yield return new WaitForSeconds(1.0f/State.Attack_Speed);
        isAtk = false;
        //Move._currentState = Player_Move.PlayerState.idle;
        Move.Motionselect(1);
    }
    IEnumerator Skill_Cooldown() {
        isSkill = true;
        yield return new WaitForSeconds(Skill_cool/1.0f);
        isSkill = false;
    }
}
