using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_creation : MonoBehaviour
{
    public int Rare;
    public GameObject[] Weapon;
    public int Rare_Maxstate = 0;//장비 스탯
    public int Unique_Maxstate = 0;
    public int Legend_Maxstate = 0;

    public void weapon_craft()
    {
        StartCoroutine(ItemSpawn());
    }


    public IEnumerator ItemSpawn()//장비 아이템 생성
    {
        yield return new WaitForSeconds(0.01f);
        Rare = Random.Range(0, 100);
        if (Rare < 60)
        {
            GameObject weapon = Instantiate(Weapon[Random.Range(0, 3)]);
            Weapon ws = weapon.GetComponent<Weapon>();
            switch (weapon.gameObject.tag)
            {
                case "Sword":
                    ws.itemstat.Hp = 0;
                    ws.itemstat.Str = 1;
                    ws.itemstat.Dex = 0;
                    ws.itemstat.Int = 0;
                    ws.itemstat.Luck = 0;
                    break;

                case "Bow":
                    ws.itemstat.Hp = 0;
                    ws.itemstat.Str = -2;
                    ws.itemstat.Dex = 2;
                    ws.itemstat.Int = 0;
                    ws.itemstat.Luck = 1;
                    break;

                case "Fan":
                    ws.itemstat.Hp = 0;
                    ws.itemstat.Str = 1;
                    ws.itemstat.Dex = 0;
                    ws.itemstat.Int = 3;
                    ws.itemstat.Luck = -2;
                    break;
            }
            weapon.transform.position = this.transform.position + new Vector3(0, 0.4f, 0);
        }
        else if (Rare < 90)
        {
            GameObject weapon = Instantiate(Weapon[Random.Range(3, 6)]);
            Weapon ws = weapon.GetComponent<Weapon>();
            switch (weapon.gameObject.tag)
            {
                case "Sword":
                    while (Rare_Maxstate != 3)
                    {
                        ws.itemstat.Hp = 0;
                        ws.itemstat.Str = Random.Range(-3, 4);
                        ws.itemstat.Dex = Random.Range(-3, 4);
                        ws.itemstat.Int = Random.Range(-3, 4);
                        ws.itemstat.Luck = Random.Range(-3, 4);
                        Rare_Maxstate = ws.itemstat.Str + ws.itemstat.Dex + ws.itemstat.Int + ws.itemstat.Luck;
                        if (Rare_Maxstate == 3)
                        {
                            break;
                        }
                    }
                    break;


                case "Bow":
                    while (Rare_Maxstate != 3)
                    {
                        ws.itemstat.Hp = 0;
                        ws.itemstat.Str = Random.Range(-3, 4);
                        ws.itemstat.Dex = Random.Range(-3, 4);
                        ws.itemstat.Int = Random.Range(-3, 4);
                        ws.itemstat.Luck = Random.Range(-3, 4);
                        Rare_Maxstate = ws.itemstat.Str + ws.itemstat.Dex + ws.itemstat.Int + ws.itemstat.Luck;
                        if (Rare_Maxstate == 3)
                        {
                            break;
                        }
                    }
                    break;

                case "Fan":
                    while (Rare_Maxstate != 3)
                    {
                        ws.itemstat.Hp = 0;
                        ws.itemstat.Str = Random.Range(-3, 4);
                        ws.itemstat.Dex = Random.Range(-3, 4);
                        ws.itemstat.Int = Random.Range(-3, 4);
                        ws.itemstat.Luck = Random.Range(-3, 4);
                        Rare_Maxstate = ws.itemstat.Str + ws.itemstat.Dex + ws.itemstat.Int + ws.itemstat.Luck;
                        if (Rare_Maxstate == 3)
                        {
                            break;
                        }
                    }
                    break;
            }
            weapon.transform.position = this.transform.position + new Vector3(0, 0.4f, 0);

        }
        else if (Rare < 99)
        {
            GameObject weapon = Instantiate(Weapon[Random.Range(6, 9)]);
            Weapon ws = weapon.GetComponent<Weapon>();
            switch (weapon.gameObject.tag)
            {
                case "Sword":
                    while (Unique_Maxstate != 5)
                    {
                        ws.itemstat.Hp = 0;
                        ws.itemstat.Str = Random.Range(-3, 4);
                        ws.itemstat.Dex = Random.Range(-3, 4);
                        ws.itemstat.Int = Random.Range(-3, 4);
                        ws.itemstat.Luck = Random.Range(-3, 4);
                        Unique_Maxstate = ws.itemstat.Str + ws.itemstat.Dex + ws.itemstat.Int + ws.itemstat.Luck;
                        if (Unique_Maxstate == 5)
                        {
                            break;
                        }
                    }
                    break;

                case "Bow":
                    while (Unique_Maxstate != 5)
                    {
                        ws.itemstat.Hp = 0;
                        ws.itemstat.Str = Random.Range(-3, 4);
                        ws.itemstat.Dex = Random.Range(-3, 4);
                        ws.itemstat.Int = Random.Range(-3, 4);
                        ws.itemstat.Luck = Random.Range(-3, 4);
                        Unique_Maxstate = ws.itemstat.Str + ws.itemstat.Dex + ws.itemstat.Int + ws.itemstat.Luck;
                        if (Unique_Maxstate == 5)
                        {
                            break;
                        }
                    }
                    break;

                case "Fan":
                    while (Unique_Maxstate != 5)
                    {
                        ws.itemstat.Hp = 0;
                        ws.itemstat.Str = Random.Range(-3, 4);
                        ws.itemstat.Dex = Random.Range(-3, 4);
                        ws.itemstat.Int = Random.Range(-3, 4);
                        ws.itemstat.Luck = Random.Range(-3, 4);
                        Unique_Maxstate = ws.itemstat.Str + ws.itemstat.Dex + ws.itemstat.Int + ws.itemstat.Luck;
                        if (Unique_Maxstate == 5)
                        {
                            break;
                        }
                    }
                    break;
            }
            weapon.transform.position = this.transform.position + new Vector3(0, 0.4f, 0);
        }
        else
        {
            GameObject weapon = Instantiate(Weapon[Random.Range(9, 12)]);
            Weapon ws = weapon.GetComponent<Weapon>();
            switch (weapon.gameObject.tag)
            {
                case "Sword":
                    while (Legend_Maxstate != 7)
                    {
                        ws.itemstat.Hp = 0;
                        ws.itemstat.Str = Random.Range(-3, 4);
                        ws.itemstat.Dex = Random.Range(-3, 4);
                        ws.itemstat.Int = Random.Range(-3, 4);
                        ws.itemstat.Luck = Random.Range(-3, 4);
                        Legend_Maxstate = ws.itemstat.Str + ws.itemstat.Dex + ws.itemstat.Int + ws.itemstat.Luck;
                        if (Legend_Maxstate == 7)
                        {
                            break;
                        }
                    }
                    break;

                case "Bow":
                    while (Legend_Maxstate != 7)
                    {
                        ws.itemstat.Hp = 0;
                        ws.itemstat.Str = Random.Range(-3, 4);
                        ws.itemstat.Dex = Random.Range(-3, 4);
                        ws.itemstat.Int = Random.Range(-3, 4);
                        ws.itemstat.Luck = Random.Range(-3, 4);
                        Legend_Maxstate = ws.itemstat.Str + ws.itemstat.Dex + ws.itemstat.Int + ws.itemstat.Luck;
                        if (Legend_Maxstate == 7)
                        {
                            break;
                        }
                    }
                    break;

                case "Fan":
                    while (Legend_Maxstate != 7)
                    {
                        ws.itemstat.Hp = 0;
                        ws.itemstat.Str = Random.Range(-3, 4);
                        ws.itemstat.Dex = Random.Range(-3, 4);
                        ws.itemstat.Int = Random.Range(-3, 4);
                        ws.itemstat.Luck = Random.Range(-3, 4);
                        Legend_Maxstate = ws.itemstat.Str + ws.itemstat.Dex + ws.itemstat.Int + ws.itemstat.Luck;
                        if (Legend_Maxstate == 7)
                        {
                            break;
                        }
                    }
                    break;
            }
            weapon.transform.position = this.transform.position + new Vector3(0, 0.4f, 0);
        }
    }
}