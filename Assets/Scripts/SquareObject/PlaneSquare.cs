using UnityEngine;
using System.Collections;

public class PlaneSquare : AbstractSquare
{
    //コンストラクタ
    PlaneSquare() { type = Type.Normal; }

    //誰かが乗っているか
    public override bool isCharacterOn()
    {
        return base.isCharacterOn();
    }
    //アイテムが乗っているか
    public override bool isItemOn()
    {
        return base.isItemOn();
    }
    //罠があるか
    public override bool isTrapOn()
    {
        return base.isTrapOn();
    }
    //対象のオブジェクトが乗っているか（汎用）
    public override bool checkZeroDistance(GameObject obj)
    {
        return base.checkZeroDistance(obj);
    }

    //乗った時
    public override void enterThis()
    {
        Debug.Log("普通だ！");
    }
    //調べた時
    public override void checkThis() { }
}
