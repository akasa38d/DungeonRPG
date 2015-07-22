using UnityEngine;
using System.Collections;

public abstract class AbstractSquare : MonoBehaviour
{
    //位置
    [SerializeField]
    public Position position;
    [System.Serializable]
    public class Position
    {
        public int row;
        public int column;
        public Position(int x, int y)
        {
            row = x;
            column = y;
        }
        public static Position createPosition(int x, int y)
        {
            return new Position(x, y);
        }
    }

    //オブジェクト
    public GameObject character;
    public GameObject item;
    public GameObject trap;

    public enum Type { Normal, Path, Stair };
	[SerializeField]
    public Type type;

    //コンストラクタ
    public AbstractSquare() { }

    //誰かが乗っているか
    public virtual bool isCharacterOn()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Character"))
        {
            if (checkZeroDistance(obj))
            {
                character = obj;
                return true;
            }
        }
        return false;
    }
    //アイテムが乗っているか
    public virtual bool isItemOn()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Item"))
        {
            if (checkZeroDistance(obj))
            {
                this.item = obj;
                return true;
            }
        }
        return false;
    }
    //罠があるか
    public virtual bool isTrapOn()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Trap"))
        {
            if (checkZeroDistance(obj))
            {
                this.trap = obj;
                return true;
            }
        }
        return false;
    }
    //対象のオブジェクトが乗っているか（汎用）
    public virtual bool checkZeroDistance(GameObject obj)
    {
        if (obj.transform.position.x == this.transform.position.x)
        {
            if (obj.transform.position.z == this.transform.position.z)
            {
                return true;
            }
        }
        return false;
    }

    //乗った時
    public virtual void enterThis() { }
    //調べた時
    public virtual void checkThis() { }
}
