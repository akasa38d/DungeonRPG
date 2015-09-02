using UnityEngine;
using System.Collections;

public class StairSquare : AbstractSquare
{

	// Use this for initialization
	void Start () {
        //コンストラクタ
        type = Type.Stair;
	}
	
    public override void enterThis()
    {
        if (isCharacterOn(AbstractCharacter.Type.Player))
        {
            StartCoroutine(enterThisCoroutine());
        }
    }
    
    IEnumerator enterThisCoroutine()
    {
        FadeManager.Instance.LoadLevel2 (1, "Title");
        
        yield return null;
    }

    public void pathEvent(DungeonManager.Floor floor)
    {

    }
}
