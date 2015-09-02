using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MyUtility;

[System.Serializable]
public class MapManager : MonoBehaviour
{
    //Prefabs
    [SerializeField]
    GameObject mapRoomPrefab;

    [SerializeField]
    GameObject mapPathPrefab;

    [SerializeField]
    GameObject mapGoalPrefab;

    public bool[,] room;
    public int[,] roomDanger;

    GameObject[,] mapRoom;

    int mapSize = 20;
    int adjustX = 180;
    int adjustY = 20;

    public void getSequenceSize()
    {
        var floor = DungeonManager.Instance.floor;
        room = new bool[floor.sequenceSize.x, floor.sequenceSize.y];
        roomDanger = new int[floor.sequenceSize.x, floor.sequenceSize.y];
        mapRoom = new GameObject[floor.sequenceSize.x, floor.sequenceSize.y];
    }

    public void checkFloorDanger()
    {
        var floor = DungeonManager.Instance.floor;
        for(int n = 0; n < floor.sequenceSize.y; n++)
        {
            for(int m = 0; m < floor.sequenceSize.x; m++)
            {
                roomDanger[m, n] = checkRoomDanger(m, n);
                if(mapRoom[m,n] != null)
                {
                    mapRoom[m, n].GetComponent<Animator>().SetInteger("DangerState", roomDanger[m,n]);
                }
            }
        }
    }

    int checkRoomDanger(int m, int n)
    {
        var room = DungeonManager.Instance.room;
        if (room [m, n].isBuild)
        {
            if(ObjectManager.Instance.character.Count > 3) { return 3; }
            if(ObjectManager.Instance.character.Count > 2) { return 2; }
            if(ObjectManager.Instance.character.Count > 1) { return 1; }
            return 0;
        }
        if(DungeonManager.Instance.room[m,n].enemyList.Count > 2) { return 3; }
        if(DungeonManager.Instance.room[m,n].enemyList.Count > 1) { return 2; }
        if(DungeonManager.Instance.room[m,n].enemyList.Count > 0) { return 1; }
        return 0;
    }

    public void createMap(int x, int y)
    {
        createMapRoom(x, y);
        createMapPath(x, y);
        createMapGoal(x, y);
        checkFloorDanger();
    }

    void createMapRoom(int x, int y)
    {
        StartCoroutine(createMRCoruoutine(x, y));
    }

    IEnumerator createMRCoruoutine(int x, int y)
    {
        mapRoom[x, y] = Instantiate(mapRoomPrefab) as GameObject;
        mapRoom[x, y].transform.SetParent(this.gameObject.transform);
        yield return null;
        mapRoom[x, y].GetComponent<LayoutElement>().ignoreLayout = true;
        mapRoom[x, y].transform.position = new Vector3(mapRoom[x, y].transform.position.x-x*mapSize*4 + adjustX, mapRoom[x, y].transform.position.y-y*mapSize*4 + adjustY, mapRoom[x, y].transform.position.z);
    }

    void createMapPath(int x, int y)
    {
        StartCoroutine(createMPCoroutine(x,y));
    }

    IEnumerator createMPCoroutine(int x, int y)
    {
        var room = DungeonManager.Instance.room;
        GameObject obj;

        //up
        if (room[x,y].path.up >= 0)
        {
            obj = Instantiate(mapPathPrefab) as GameObject;
            obj.transform.SetParent(this.gameObject.transform);
            yield return null;
            obj.GetComponent<LayoutElement>().ignoreLayout = true;
            obj.transform.position = new Vector3(mapRoom[x, y].transform.position.x, mapRoom[x, y].transform.position.y - mapSize, mapRoom[x, y].transform.position.z);
        }
        
        //down
        if (room[x,y].path.down >= 0)
        {
            obj = Instantiate(mapPathPrefab) as GameObject;
            obj.transform.SetParent(this.gameObject.transform);
            yield return null;
            obj.GetComponent<LayoutElement>().ignoreLayout = true;
            obj.transform.position = new Vector3(mapRoom[x, y].transform.position.x, mapRoom[x, y].transform.position.y + mapSize, mapRoom[x, y].transform.position.z);
        }
        
        //left
        if (room[x,y].path.left >= 0)
        {
            obj = Instantiate(mapPathPrefab) as GameObject;
            obj.transform.SetParent(this.gameObject.transform);
            yield return null;
            obj.GetComponent<LayoutElement>().ignoreLayout = true;
            obj.transform.position = new Vector3(mapRoom[x, y].transform.position.x + mapSize, mapRoom[x, y].transform.position.y, mapRoom[x, y].transform.position.z);
        }
        
        //right
        if (room[x,y].path.right >= 0)
        {
            obj = Instantiate(mapPathPrefab) as GameObject;
            obj.transform.SetParent(this.gameObject.transform);
            yield return null;
            obj.GetComponent<LayoutElement>().ignoreLayout = true;
            obj.transform.position = new Vector3(mapRoom[x, y].transform.position.x - mapSize, mapRoom[x, y].transform.position.y, mapRoom[x, y].transform.position.z);
        }
        yield return null;
    }

    void createMapGoal(int x, int y)
    {
        StartCoroutine(createMGCoroutine(x, y));
    }

    IEnumerator createMGCoroutine(int x, int y)
    {
        var room = DungeonManager.Instance.room[x,y];
        GameObject obj;

        if (room.isGoal)
        {
            if (room.goalDir == Direction.up)
            {
                obj = Instantiate(mapGoalPrefab) as GameObject;
                obj.transform.SetParent(this.gameObject.transform);
                yield return null;
                obj.GetComponent<LayoutElement>().ignoreLayout = true;
                obj.transform.position = new Vector3(mapRoom[x, y].transform.position.x, mapRoom[x, y].transform.position.y + mapSize, mapRoom[x, y].transform.position.z);
            }
            if (room.goalDir == Direction.down)
            {
                obj = Instantiate(mapGoalPrefab) as GameObject;
                obj.transform.SetParent(this.gameObject.transform);
                yield return null;
                obj.GetComponent<LayoutElement>().ignoreLayout = true;
                obj.transform.position = new Vector3(mapRoom[x, y].transform.position.x, mapRoom[x, y].transform.position.y - mapSize, mapRoom[x, y].transform.position.z);
            }
            if (room.goalDir == Direction.left)
            {
                obj = Instantiate(mapGoalPrefab) as GameObject;
                obj.transform.SetParent(this.gameObject.transform);
                yield return null;
                obj.GetComponent<LayoutElement>().ignoreLayout = true;
                obj.transform.position = new Vector3(mapRoom[x, y].transform.position.x - mapSize, mapRoom[x, y].transform.position.y, mapRoom[x, y].transform.position.z);
            }
            if(room.goalDir == Direction.right)
            {
                obj = Instantiate(mapGoalPrefab) as GameObject;
                obj.transform.SetParent(this.gameObject.transform);
                yield return null;
                obj.GetComponent<LayoutElement>().ignoreLayout = true;
                obj.transform.position = new Vector3(mapRoom[x, y].transform.position.x + mapSize, mapRoom[x, y].transform.position.y, mapRoom[x, y].transform.position.z);
            }
            yield return null;
        }
    }
}
