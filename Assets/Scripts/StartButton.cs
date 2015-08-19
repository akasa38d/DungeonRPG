using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour {

	public void startGame()
	{
		FadeManager.Instance.LoadLevel2(1, "Dungeon");
	}
}
