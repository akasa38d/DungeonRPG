using UnityEngine;
using System.Collections;
using System.Xml;

[System.Serializable]
public abstract class AbstractCharacterParameter
{
    //基本的なパラメータ
    public int id;
    public string cName;
    public int maxHp;
    public int hp;
    public int atk;
}
