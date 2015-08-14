using UnityEngine;
using System.Collections;

public class PlayerObject : AbstractCharacterObject
{
    public void Start()
    {
        this.type = Type.Player;
        this.parameter = PlayerParameter.getPlayerParameter("赤ずきん", 30, 30);
    }

    //基本処理
    public override void operation() { base.operation(); }

    //スタンバイフェイズ
    protected override void startOperation()
    {
		ObjectManager.Instance.setCharacter();
		ObjectManager.Instance.setSquare();

        //移動ボタンを生成
        createMoveButton();

        base.startOperation();
    }

    //メインフェイズ
    protected override void mainOperation() { }

    //エンドフェイズ
    protected override void endOperation() {
		this.gameObject.GetComponent<ItemManager> ().preTurnEnd ();

		preTurnEnd ();

		this.gameObject.GetComponent<ItemManager> ().turnEnd();

		process = Process.Next;
	}

    //ターン終了
    protected override void nextOperation() { }

	public void moveButtonEvent()
	{

	}

	//移動用ボタンを生成
    public void createMoveButton()
    {
		this.gameObject.GetComponent<ItemManager> ().usingNumber = -1;

		deleteButton ();

        //prefabsの設定
        GameObject moveButton = PrefabManager.Instance.moveButton;

        foreach (var floor in ObjectManager.Instance.square)
        {
            if (checkOneDistance(this.gameObject, floor))
            {
                if (!floor.GetComponent<AbstractSquare>().isCharacterOn())
                {
                    var tmp = Instantiate(moveButton, new Vector3(floor.transform.position.x, floor.transform.position.y + 0.005f, floor.transform.position.z), Quaternion.identity) as GameObject;
                    var tmpScript = tmp.GetComponent<MoveButton>();
                    tmpScript.square = floor;
                    tmpScript.moveDelegate = movePosition;
                }
            }
        }
    }

    //ボタンを消去
    public void deleteButton()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Button"))
        {
            Destroy(obj);
        }
    }

    public override IEnumerator DamagedCoroutine(int damage)
    {
        Debug.Log(this.parameter.cName + "は" + damage + "のダメージを受けた");
        if (damage >= this.parameter.hp)
        {
            Debug.Log("アバーッ！！" + this.parameter.cName + "は爆発四散！");
            Instantiate(PrefabManager.Instance.explosion, this.transform.position, Quaternion.identity);
            callBack();
            Debug.Log("なんちゃって");
            yield return null;
        }
        else
        {
            this.parameter.hp -= damage;
            callBack();
        }
        yield return null;
    }

    //移動
    public override void movePosition(GameObject obj)
    {
        //ボタンを消去
        deleteButton();

        base.movePosition(obj);
    }

	//ItemにInstantiateさせるためのもの
	public GameObject pInstantiate(GameObject obj)
	{
		var n = Instantiate(obj) as GameObject;
		return n;
	}
	public GameObject pInstantiate(GameObject obj, Vector3 vector3)
	{
		var n = Instantiate(obj, vector3, Quaternion.identity) as GameObject;
		return n;
	}
}
