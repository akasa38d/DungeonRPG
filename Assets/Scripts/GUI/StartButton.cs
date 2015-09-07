using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour {

	public void startGame()
	{
		StartCoroutine ("startGameCoroutine");
	}

	IEnumerator startGameCoroutine()
	{
		yield return new WaitForSeconds (1);

		FadeManager.Instance.LoadLevel2(1, "Dungeon");
	}
}
