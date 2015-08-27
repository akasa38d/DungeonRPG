using UnityEngine;
using System.Collections;
using MyUtility;

[System.Serializable]
public class MapManager : MonoBehaviour  {
    //Prefabs
    [SerializeField]
    GameObject mapRoomPrefab;

    [SerializeField]
    GameObject mapPathPrefab;

    [SerializeField]
    GameObject mapGoalPrefab;

    GameObject[,] mapRoom;
    public bool[,] room;
    public int[,] roomDanger;

    MyVector2 sequenceSize;
    public void getSequenceSize(int x, int y)
    {
        sequenceSize = new MyVector2(x, y);
        room = new bool[sequenceSize.x, sequenceSize.y];
        roomDanger = new int[sequenceSize.x, sequenceSize.y];
        Debug.Log(room.Length);
    }

    public void checkFloorDanger()
    {
        for(int n = 0; n < sequenceSize.y; n++)
        {
            for(int m = 0; m < sequenceSize.x; m++)
            {
                roomDanger[m, n] = checkRoomDanger(m, n);
                Debug.Log(m + "," + n + "部屋の危険度は" + roomDanger[m,n]);
            }
        }
    }

    int checkRoomDanger(int m, int n)
    {
        if(DungeonManager.Instance.room[m,n].enemyList.Count > 2) { return 3; }
        if(DungeonManager.Instance.room[m,n].enemyList.Count > 1) { return 2; }
        if(DungeonManager.Instance.room[m,n].enemyList.Count > 0) { return 1; }
        return 0;
    }
}
