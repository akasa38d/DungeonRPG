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
	public int turnCount = 0;

    public override void Awake() { base.Awake(); }

    //処理
    public void operation()
    {
        StartCoroutine(operationCoroutine());
    }

    IEnumerator operationCoroutine()
    {
        var turnPlayer = ObjectManager.Instance.characterScript;

		turnPlayer[turnCharacter].operation();

        //もしフェーディングしているなら
        while (FadeManager.Instance.isFading)
        {
            yield return new WaitForEndOfFrame();
        }

		if (turnPlayer[turnCharacter].process == AbstractCharacterObject.Process.Next)
        {
			turnPlayer[turnCharacter].process = AbstractCharacterObject.Process.Start;
			turnCharacter++;
        }

        ObjectManager.Instance.setCharacter();
        ObjectManager.Instance.setSquare();

        //ループ
		if (turnCharacter >= turnPlayer.Count)
		{
//			foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Effect"))
//			{
//				Destroy(obj);
//			}

			//０番のキャラクタへターンプレイヤーを変更
			turnCharacter = 0;
			//ターン数増加
			turnCount++;

			Debug.Log (turnCount + "ターン目開始");

			//敵の出現
			if (turnCount % 2 == 0)
			{
				DungeonManager.Floor.Instance.prepareEnemy();
			}
		}
    }


}
