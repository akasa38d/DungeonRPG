using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerObject : AbstractCharacterObject
{
    //移動ボタンのプレハブ
    public GameObject moveButton;
	public GameObject attackButton;

    //周囲のキャラクター
	[SerializeField]
    List<GameObject> aroundCharacter = new List<GameObject>();

    //処理
    public override void operation()
    {
        base.operation();
    }

    //スタンバイフェイズ
    protected override void startOperation()
    {
        //周囲の敵を取得
        getAroundCharacter(aroundCharacter);

        //移動用ボタンを生成
        createMoveButton();

		createAttackButton ();

        //フェイズを進める
        process = Process.Main;
    }

    //周囲の敵を取得
    protected override void getAroundCharacter(List<GameObject> list)
    {
        list.Clear();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Character"))
        {
            if (obj != this.gameObject)
            {
                list.Add(obj);
            }
        }
    }

    //メインフェイズ
    protected override void mainOperation() { }

    //エンドフェイズ
    protected override void endOperation()
    {
        process = Process.Next;
    }

    protected override void nextOperation()
    {
        base.nextOperation();
    }

    //移動用ボタンを生成
    void createMoveButton()
    {
        foreach (GameObject floor in ObjectManager.Instance.square)
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


	void createAttackButton()
	{
		foreach (GameObject floor in ObjectManager.Instance.square)
		{
			if (checkOneDistance(this.gameObject, floor))
			{
				if(floor.GetComponent<AbstractSquare>().isCharacterOn()){
					var tmp = Instantiate(attackButton, new Vector3(floor.transform.position.x, floor.transform.position.y + 0.005f, floor.transform.position.z), Quaternion.identity) as GameObject;
					var tmpScript = tmp.GetComponent<AttackButton>();
					tmpScript.square = floor;
					tmpScript.attackDelegate = attackEnemy;
				}
			}
		}
	}



    //移動用ボタンを消去
    void deleteButton()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Button"))
        {
            Destroy(obj);
        }
    }


	public void attackEnemy(GameObject obj)
	{
		deleteButton();
		Instantiate(ObjectManager.Instance.explosion, obj.transform.position, Quaternion.identity);

		Destroy (obj);
		Debug.Log(obj + "を破壊した");

		process = Process.End;
	}



    //キューブの移動
    public void movePosition(GameObject obj)
    {
		deleteButton();
		
		this.transform.position = new Vector3(obj.transform.position.x, this.transform.position.y, obj.transform.position.z);
		
		Debug.Log(obj);
		
		obj.GetComponent<AbstractSquare>().enterThis();
		
		process = Process.End;
    }

    //********************ここからbool********************//

    //チェビシェフ距離１を確認（起点、対象）
    protected bool checkOneDistance(GameObject ob1, GameObject ob2)
    {
        //マンハッタン距離１なら
        if (checkOneDistance_M(ob1, ob2)) { return true; }

        //距離０なら
        if (checkZeroDistance(ob1, ob2)) { return false; }

        //カウント用変数
        int checkCount = 0;
        //オブジェクトを調べて
        foreach (GameObject obj in ObjectManager.Instance.square)
        {
            //ob1のマンハッタン距離が１かつ
            if (checkOneDistance_M(ob1, obj) && checkOneDistance_M(ob2, obj))
            {
                //カウントを増やす
                checkCount++;
            }
        }
        //カウントが２だったら
        if (checkCount == 2) { return true; }

        //ここまでの条件に当てはまらなかったら
        return false;
    }

    //マンハッタン距離１を確認（起点、対象）
    protected bool checkOneDistance_M(GameObject ob1, GameObject ob2)
    {
        Vector2 pos1 = new Vector2(ob1.transform.position.x, ob1.transform.position.z);
        Vector2 pos2 = new Vector2(ob2.transform.position.x, ob2.transform.position.z);

        if (Vector2.Distance(pos1, pos2) == 10) { return true; }
        return false;
    }

    //距離０を確認（起点、対象）
    protected bool checkZeroDistance(GameObject ob1, GameObject ob2)
    {
        Vector2 pos1 = new Vector2(ob1.transform.position.x, ob1.transform.position.z);
        Vector2 pos2 = new Vector2(ob2.transform.position.x, ob2.transform.position.z);
        if (pos1 == pos2) { return true; }
        return false;
    }
}
