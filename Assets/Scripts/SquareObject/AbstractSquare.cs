using UnityEngine;
using System.Collections;
using MyUtility;
using System.Linq;

public abstract class AbstractSquare : MonoBehaviour
{
    //オブジェクト
    public GameObject character;
    public GameObject fieldItem;
    public GameObject trap;

    //位置
    [SerializeField]
    public MyVector2 sequence;

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

    //誰かが乗っているか（タイプ指定）
    public virtual bool isCharacterOn(AbstractCharacterObject.Type type)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Character"))
        {
            if (checkZeroDistance(obj))
            {
                if (obj.GetComponent<AbstractCharacterObject>().type == type)
                {
                    character = obj;
                    return true;
                }
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
                this.fieldItem = obj;
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
        if (obj.transform.position.x == this.transform.position.x
            && obj.transform.position.z == this.transform.position.z)
        {
            return true;
        }
        return false;
    }

    //乗った時
    public virtual void enterThis()
    {
        if (isCharacterOn(AbstractCharacterObject.Type.Player))
        {
            if (isItemOn())
            {
                fieldItem.GetComponent<FieldItem>().operation();
            }
        }
    }

    //調べた時
    public virtual void checkThis() { }

    public GameObject[] aroundSquare(int i, bool aroundOnly = false)
    {
        var aSquare = from n in ObjectManager.Instance.square
                      where Mathf.Abs(this.transform.position.x - n.transform.position.x) <= i * 10
                          && Mathf.Abs(this.transform.position.z - n.transform.position.z) <= i * 10
                      select n;
		if (!aroundOnly)
		{
			return aSquare.ToArray ();
		}

		return aSquare.Where((n) => n.transform.position != this.transform.position).ToArray ();
    }
}
