using UnityEngine;
using System;

[System.Serializable]
public abstract class AbstractCharacterParameter
{
    //基本的なパラメータ
    public int id;
    public int lv;
    public string cName;
    public int maxHp;
    public int hp;
    public int maxSp;
    public int sp;
    public int atk;
}
