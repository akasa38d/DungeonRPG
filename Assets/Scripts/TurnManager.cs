using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnManager : SingletonMonoBehaviour<TurnManager>
{
    public int turnCount = 0;
	public bool inPreparation = false;

	public override void Awake()
	{
		base.Awake ();
	}

    //処理
    public void operation()
    {
		StartCoroutine (operationCoroutine ());
    }

	IEnumerator operationCoroutine()
	{
		var turnPlayer = ObjectManager.Instance.characterScript;

		turnPlayer [turnCount].operation ();

		//もしフェーディングしてるなら
		while (FadeManager.Instance.isFading)
		{
			yield return new WaitForEndOfFrame();
		}
		
		if (turnPlayer [turnCount].process == AbstractCharacterObject.Process.Next) {
			turnPlayer [turnCount].process = AbstractCharacterObject.Process.Start;
			ObjectManager.Instance.setCharacter();
			ObjectManager.Instance.setSquare();
			turnCount++;
		}
		//ループ
		if (turnCount >= turnPlayer.Count) { turnCount = 0;	}
	}


	public void makeEvent()
	{

	}

}
