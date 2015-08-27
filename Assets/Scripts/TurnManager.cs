using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// ターンを管理するシングルトンのクラス
/// </summary>
public class TurnManager : SingletonMonoBehaviour<TurnManager>
{
    //ターンプレイヤー
    public int turnCharacter = 0;
    //ターンカウント
    public int turnCount = 1;

    public override void Awake()
    {
        base.Awake();
    }

    //処理
    public void operation()
    {
        StartCoroutine(operationCoroutine());
    }

    IEnumerator operationCoroutine()
    {
        var turnPlayer = ObjectManager.Instance.characterScript;

        turnPlayer[turnCharacter].operation();
        yield return null;

        if (FadeManager.Instance.isFading)
        {
            turnPlayer[turnCharacter].process = AbstractCharacterObject.Process.Start;
            turnCharacter = 0;
            yield return 0;
        }

        //もしフェーディングしているなら
        while (FadeManager.Instance.isFading)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return null;

        if (turnPlayer[turnCharacter].process == AbstractCharacterObject.Process.Next)
        {
            turnPlayer[turnCharacter].process = AbstractCharacterObject.Process.Start;

            turnCharacter++;
        }
        yield return null;

        //ループ
        if (turnCharacter >= turnPlayer.Count)
        {
            endOperation();
        }        
        yield return null;
    }

    public void endOperation()
    {
        //０番のキャラクタへターンプレイヤーを変更
        turnCharacter = 0;
        //ターン数増加
        turnCount++;
        Debug.Log(turnCount + "ターン目開始");        
        //敵の出現
        if (turnCount % 3 == 0)
        {
            Debug.Log("敵の出現");
            DungeonManager.Floor.Instance.prepareEnemy();
        }
        DungeonManager.Instance.mapManager.checkFloorDanger();
    }
}
