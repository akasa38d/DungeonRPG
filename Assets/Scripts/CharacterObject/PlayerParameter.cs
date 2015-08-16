using UnityEngine;
using System.Collections;
using System.Xml;

[System.Serializable]
public class PlayerParameter : AbstractCharacterParameter
{
    //パラメータを作成
    public static PlayerParameter getPlayerParameter(string cName, int maxHp, int atk)
    {
        var parameter = new PlayerParameter();
		parameter.cName = cName;
		parameter.lv = 1;
		parameter.maxHp = maxHp;
		parameter.atk = atk;
		parameter.hp = parameter.maxHp;
		parameter.maxSp = 100;
		parameter.sp = parameter.maxSp;
        return parameter;
    }
}
