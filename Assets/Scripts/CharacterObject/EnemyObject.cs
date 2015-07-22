using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyObject : AbstractCharacterObject
{
    //行動回数カウント
    int testCount = 0;

    //周囲の床
    //    List<GameObject> aroundFloor = new List<GameObject>();
    //周囲のキャラクター
    List<GameObject> aroundCharacter = new List<GameObject>();

    //処理
    public override void operation()
    {
        base.operation();
    }

    //スタンバイフェイズ
    protected override void startOperation()
    {
        getAroundCharacter(aroundCharacter);
        process = Process.Main;
    }

    //周囲の敵を取得
    protected override void getAroundCharacter(List<GameObject> list)
    {
        list.Clear();
        GameObject obj = GameObject.Find("Player");
        {
            list.Add(obj);
        }
    }

    //メインフェイズ
    protected override void mainOperation()
    {
        testCount++;
        Debug.Log("Enemy" + id + "は" + testCount + "回行動しました");
        process = Process.End;
    }

    //エンドフェイズ
    protected override void endOperation()
    {
        //フェイズを戻す
        process = Process.Next;
    }

    protected override void nextOperation()
    {
        base.nextOperation();
    }
}
