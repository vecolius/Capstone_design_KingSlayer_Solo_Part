using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerState {
    public float Max_Hp;
    public int Hp;
    public int Str;
    public int Dex;
    public int Int;
    public int Luck;
}
//복구용

public class Player_State : MonoBehaviour
{
    [Header ("데미지")]
    public float Phy_Damage = 1.0f;
    public float Magic_Damage = 1.0f;
    public float Critical_Per = 0;
    public float Critical_Damage = 1.5f;
    public float Attack_Speed = 1.0f;
    [Header ("이동 관련")]
    public float Move_Speed = 3.0f;
    public float Jumppower = 25.0f;
    [Header ("플레이어 재화")]
    public float Max_Exp = 0f;
    public float EXP = 0f;
    public float coin = 0f;
    public float Level = 0f;
    
    [Header ("플레이어 자체스탯")]
    public PlayerState P_State = new PlayerState();
    [Header ("무기 스탯")]
    public int W_Hp;
    public int W_Str;
    public int W_Dex;
    public int W_Int;
    public int W_Luck;
    [Header ("플레이어 총 스탯")]
    public int TotalHp;
    public int TotalStr;
    public int TotalDex;
    public int TotalInt;
    public int TotalLuck;    
    private Animator ani = null;

    public bool die;
    
    public SPUM_Prefabs _prefabs; //animation
    public enum MonsterState
    {
        idle,
        run,
        attack_Normal,
        attack_Bow,
        death,
    }
    public MonsterState _currentState;

    void Awake() {
        
    }
    
    void Start()
    {
        Max_Exp = DateManager.instance.nowPlayer.Max_Exp;
        EXP = DateManager.instance.nowPlayer.Exp;
        coin = DateManager.instance.nowPlayer.Player_Coin;
        Level = DateManager.instance.nowPlayer.Level;
        _prefabs = GameManager.instance.Player.GetComponent<SPUM_Prefabs>();
        ani = transform.GetChild(0).gameObject.GetComponent<Animator>();
        die = false;

        P_State.Hp = DateManager.instance.nowPlayer.Hp;
        P_State.Str = DateManager.instance.nowPlayer.Hp;
        P_State.Dex = DateManager.instance.nowPlayer.Hp;
        P_State.Int = DateManager.instance.nowPlayer.Hp;
        P_State.Luck = DateManager.instance.nowPlayer.Hp;

        W_Hp = DateManager.instance.nowPlayer.Weapon_Hp;
        W_Str = DateManager.instance.nowPlayer.Weapon_Str;
        W_Dex = DateManager.instance.nowPlayer.Weapon_Dex;
        W_Int = DateManager.instance.nowPlayer.Weapon_Int;
        W_Luck = DateManager.instance.nowPlayer.Weapon_Luck;


        TotalHp = P_State.Hp + W_Hp;
        TotalStr = P_State.Str + W_Str;
        TotalDex = P_State.Dex + W_Dex;
        TotalInt = P_State.Int + W_Int;
        TotalLuck = P_State.Luck + W_Luck;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(TotalHp != P_State.Hp + W_Hp) {
            TotalHp = P_State.Hp + W_Hp;
            P_State.Max_Hp = 20f + (TotalHp * 20f);
            P_State.Max_Hp = (P_State.Max_Hp <= 0) ? 10.0f : P_State.Max_Hp;
            if(P_State.Max_Hp <= DateManager.instance.nowPlayer.Player_Hp) {
                DateManager.instance.nowPlayer.Player_Hp = P_State.Max_Hp;
            }
        }

        if(TotalStr != P_State.Str + W_Str) {
            TotalStr = P_State.Str + W_Str;
            Phy_Damage = 1.0f + (1.0f * TotalStr);
            Phy_Damage = (Phy_Damage <= 0) ? 1.0f : Phy_Damage;
        }
        if(TotalDex != P_State.Dex + W_Dex) {
            TotalDex = P_State.Dex + W_Dex;
            Attack_Speed = 1.0f + (0.1f * TotalDex);
            Attack_Speed = (Attack_Speed <= 0 ) ? 0.6f : Attack_Speed;
            Attack_Speed = (Attack_Speed >= 2.0f ) ? 2.0f : Attack_Speed;
        }
        if(TotalInt != P_State.Int + W_Int) {
            TotalInt = P_State.Int + W_Int;
            Magic_Damage = 1.0f + (1.0f * TotalInt);
            Magic_Damage = (Magic_Damage <= 0) ? 1.0f : Magic_Damage;
        }
        if(TotalLuck != P_State.Luck + W_Luck) {
            TotalLuck = P_State.Luck + W_Luck;
            Critical_Per = 0f + (5.0f * TotalLuck);
            if(Critical_Per <= 0) {
                Critical_Per = 0;
            }
            else if(Critical_Per >= 50) {
                Critical_Per = 50;
            }
        }
        ani.SetFloat("AtkSpeed",Attack_Speed);
        if (EXP >= 100 + Level * 20){
            EXP -= 100 + Level*20;
            Level =  Level + 1;
        }
        

        if(DateManager.instance.nowPlayer.Player_Hp <= 0) {
            if(!die) {
                _currentState = MonsterState.death;
                die = true;
            }
        }
    }

}

