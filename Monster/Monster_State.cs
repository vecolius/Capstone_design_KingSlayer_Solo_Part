using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_State : MonoBehaviour
{

    private void Awake()
    {
        _prefabs = Monster.GetComponent<SPUM_Prefabs>();

    }
    public Monster_count monster_Count;
    public GameObject Monster;
    private Player_State P_State = null;
    private Rigidbody2D rigi;
    private Rigidbody2D P_Rigi = null;
    private BoxCollider2D BoxCollider2D = null; 
    public float Monster_Maxhp;
    public float Monster_Hp;
    public float Monster_Attack;
    public float Monster_Speed;
    public float Monster_Exp;

    public Vector3 M_Pos;
    private float Dir;
    private Player_Move P_Move = null;

    public SPUM_Prefabs _prefabs; //animation
    public bool die;
    public Weapon_creation weapon_Creation;//무기 랜덤드랍
    public GameObject coin_gold;
    public GameObject coin_silver;
    public GameObject coin_bronze;
    public GameObject posion;

    public enum MonsterState
    {
        idle,
        run,
        attack_Normal,
        attack_Bow,
        death,
    }

    public enum Monstertype
    {
        normal,
        boss,
    }

    public MonsterState _currentState;
    public Monstertype monstertype;
    public AudioClip clip;
    
    // Start is called before the first frame update
    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        P_Move = GameManager.instance.Player.GetComponent<Player_Move>();
        P_Rigi = GameManager.instance.Player.GetComponent<Rigidbody2D>();
        P_State = GameManager.instance.Player.GetComponent<Player_State>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        monster_Count = transform.parent.GetComponent<Monster_count>();// 부모객체의 스크립트 연결
        weapon_Creation=Monster.GetComponent<Weapon_creation>();
        die = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case MonsterState.death:
                PlayStateAnimation(_currentState);
                break;
        }
        if(Monster_Hp <= 0) {
            if(!die){
                die = true;
                rigi.constraints = RigidbodyConstraints2D.FreezeAll;
                _currentState = MonsterState.death;
                BoxCollider2D.enabled = false;
                P_State.EXP += Monster_Exp;
                this.transform.position=this.transform.position+ new Vector3(0, 0, -0.5f);
                if (monstertype == Monstertype.boss) //죽는 몬스터가 보스일 경우
                {
                    weapon_Creation.weapon_craft();
                }else if(monstertype == Monstertype.normal){//일반 몹일경우
                    Coin_spawn();
                }
                Invoke("dead", 1.5f);
            }
        }
    }
    public void dead(){ //몬스터 사망 시
        monster_Count.monster_count();// 부모객체에 연결된 스크립트 변수 변경(각 방마다 개체 카운트 감소)
        Destroy(gameObject);
    }

    public void P_Damage(float Damage) { //몬스터 물리 피격 연산
        SoundManager.instance.SFXplay("02. Bloody Hit", clip);
        if(P_State.Critical_Per >= Random.Range(0,100)) {
            Monster_Hp -= P_State.Phy_Damage * P_State.Critical_Damage * Damage;
        }
        else Monster_Hp -= P_State.Phy_Damage * Damage;
        rigi.AddForce(new Vector2(0,2.0f),ForceMode2D.Impulse);
    }

    public void M_Damage(float Damage) { // 몬스터 마법 피격 연산
        if(P_State.Critical_Per >= Random.Range(0,100)) {
            Monster_Hp -= P_State.Magic_Damage * P_State.Critical_Damage * Damage; 
        }
        else Monster_Hp -= P_State.Magic_Damage * Damage;
        rigi.AddForce(new Vector2(0,2.0f),ForceMode2D.Impulse);
    }

    public void Monster_Attacked() { //몬스터 공격 데미지 연산
        if(P_Move.Nodamage == false && GameManager.instance.Player.transform.GetChild(0).GetComponent<Player_aniator>().isDash == false) {
            P_Move.Nodamage = true;
            DateManager.instance.nowPlayer.Player_Hp -= Monster_Attack;
            //DateManager.instance.SaveData();
            Dir = (M_Pos.x <= GameManager.instance.Player.transform.position.x) ? 1 : -1;
            P_Rigi.AddForce(new Vector2(Dir * 3.0f,5.0f),ForceMode2D.Impulse);
            P_Move.OnDamage = true;
            P_Move.Motionselect(1);
            StartCoroutine(Unbeat());         
        }
    }
    //spum 함수
    public void PlayStateAnimation(MonsterState state)
    {
        _prefabs.PlayAnimation(state.ToString());
    }

    IEnumerator Unbeat() { //몬스터 공격으로 인한 Player 피격시 무적
        for(int i = 0; i < 3; i++) {
            for (int j = 0; j < P_Move.Color.Length; j++) {
                P_Move.Color[j].color = new Color(P_Move.Color[j].color.r,P_Move.Color[j].color.g,P_Move.Color[j].color.b,0.8f);
            }
            yield return new WaitForSeconds(0.1f);
            for (int j = 0; j < P_Move.Color.Length; j++) {
                P_Move.Color[j].color = new Color(P_Move.Color[j].color.r,P_Move.Color[j].color.g,P_Move.Color[j].color.b,1.0f);
            }
            yield return new WaitForSeconds(0.1f);
        }
        P_Move.OnDamage = false;
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < P_Move.Color.Length; j++) {
                P_Move.Color[j].color = new Color(P_Move.Color[j].color.r,P_Move.Color[j].color.g,P_Move.Color[j].color.b,0.8f);
            }
            yield return new WaitForSeconds(0.1f);
            for (int j = 0; j < P_Move.Color.Length; j++) {
                P_Move.Color[j].color = new Color(P_Move.Color[j].color.r,P_Move.Color[j].color.g,P_Move.Color[j].color.b,1.0f);
            }
            yield return new WaitForSeconds(0.1f);            
        }
        rigi.constraints = RigidbodyConstraints2D.None; //몬스터의 위치 고정 해제
        rigi.constraints = RigidbodyConstraints2D.FreezeRotation;
        if(die){
            rigi.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        P_Move.Nodamage = false;
    }
    private void OnCollisionStay2D(Collision2D other) {
        if(!die){
            if(other.gameObject.tag == "Player") { 
                M_Pos = GetComponent<Transform>().position;
                rigi.constraints = RigidbodyConstraints2D.FreezeAll; //몬스터의 위치 고정
                Monster_Attacked();
            }
        }
    }

    public void Coin_spawn(){//코인 생성
        int coin_drop = Random.Range(0,100);
        if(coin_drop < 45){
            GameObject coin = Instantiate(coin_bronze);
            coin.transform.position = this.transform.position + new Vector3(0, 0.2f, 0);
        }else if(coin_drop < 65){
            GameObject coin = Instantiate(coin_silver);
            coin.transform.position = this.transform.position + new Vector3(0, 0.2f, 0);
        }else if(coin_drop < 75){
            GameObject coin = Instantiate(coin_gold);
            coin.transform.position = this.transform.position + new Vector3(0, 0.2f, 0);
        }else if(coin_drop < 85){
            GameObject coin = Instantiate(posion);
            posion.transform.position = this.transform.position + new Vector3(0, 0.2f, 0);
        }
    }
}
