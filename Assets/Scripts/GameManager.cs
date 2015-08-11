using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public GameObject testButton; 

    void Start()
    {
        ObjectManager.Instance.setCharacter();

        //ダンジョン生成
        DungeonManager.Instance.getDungeonData(0);
		DungeonManager.Floor.Instance.setFloor(DungeonManager.sequenceSizeX, DungeonManager.sequenceSizeY);
        DungeonManager.Floor.Instance.createTest();
    }

    void Update()
    {
        TurnManager.Instance.operation();
    }
}
