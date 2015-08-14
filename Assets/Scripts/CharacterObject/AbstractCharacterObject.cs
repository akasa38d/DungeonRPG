using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// キャラクターオブジェクトの抽象クラス
/// 基本処理とインターフェースを設定
/// </summary>
public abstract class AbstractCharacterObject : MonoBehaviour
{
    //キャラクターの種類
    public enum Type { Player, Friend, Enemy };
    public Type type;

    //ターン進行
    [SerializeField]
    public enum Process { Start, Main, End, Next };
    public Process process;

    //ID（仮）
    public int id;

	//追加ターン
	public int additionalTurn = 0;

	//パラメーター
	[SerializeField]
	public AbstractCharacterParameter parameter;


    //基本処理
    public virtual void operation()
    {
        switch (this.process)
        {
            //スタンバイフェイズ
            case Process.Start:
                this.startOperation();
                break;
            //メインフェイズ
            case Process.Main:
                this.mainOperation();
                break;
            //エンドフェイズ
            case Process.End:
                this.endOperation();
                break;
            case Process.Next:
                this.nextOperation();
                break;
            //例外
            default:
                this.endOperation();
                break;
        }
    }

    //スタンバイフェイズ処理
    protected virtual void startOperation()
	{
		process = Process.Main;
	}

    //メインフェイズ処理
    protected virtual void mainOperation() { }

	//エンドフェイズ前確認
	protected virtual void preTurnEnd()
	{
		if (additionalTurn > 0) {
			Debug.Log ("追加ターンだぜ！");
			additionalTurn -= 1;
			process = Process.Start;
		}
	}

    //エンドフェイズ処理
    protected virtual void endOperation()
    {
		preTurnEnd ();

		process = Process.Next;
    }


    //仮
    protected virtual void nextOperation() { }



    //攻撃
    public virtual void attackTarget(AttackWay attackWay, GameObject obj)
    {
        Debug.Log(this.parameter.cName + "が" + attackWay.name + "で攻撃！");
        //ターンエンドのためのコールバックをセット
        obj.GetComponent<AbstractCharacterObject>().callBack = () => this.process = Process.End;
        //攻撃のエフェクト
        if (attackWay.effect != null) { Instantiate(attackWay.effect, obj.transform.position, Quaternion.identity); }
        //ダメージ処理
        obj.GetComponent<AbstractCharacterObject>().beDameged(attackWay.power);
    }

    //攻撃手段
    public class AttackWay
    {
        public string name;
        public int power;
        public GameObject effect;

        public AttackWay(string name, int power, GameObject effect)
        {
            this.name = name;
            this.power = power;
            this.effect = effect;
        }
    }

    //ターン処理させるためのコールバック
    public delegate void TestEvent();
    public TestEvent callBack;

    //ダメージを受けた時
    public virtual void beDameged(int damage)
    {
        StartCoroutine(DamagedCoroutine(damage));
    }

    public virtual IEnumerator DamagedCoroutine(int damage)
    {
        Debug.Log(this.parameter.cName + "は" + damage + "のダメージを受けた");
        if (damage >= this.parameter.hp)
        {
            Debug.Log("グワーッ！！" + this.parameter.cName + "は爆発四散！");
            Instantiate(PrefabManager.Instance.explosion, this.transform.position, Quaternion.identity);
            callBack();
            Destroy(this.gameObject);
            yield return null;
        }
        else
        {
            this.parameter.hp -= damage;
            callBack();
        }
        yield return null;
    }

    //やられた時
    public virtual void beDefeated()
    {

    }


    //移動（メインフェイズ、床を指定すること）
    public virtual void movePosition(GameObject obj)
    {
        //対象の場所まで移動
        this.transform.position = new Vector3(obj.transform.position.x, this.transform.position.y, obj.transform.position.z);

        //床のイベント発生
        obj.GetComponent<AbstractSquare>().enterThis();

        //フェイズ移行
        this.process = Process.End;
    }

    //********************ここからbool********************//

    //チェビシェフ距離１を確認（起点、対象）
    public bool checkOneDistance(GameObject ob1, GameObject ob2)
    {
        //マンハッタン距離１なら
        if (checkOneDistance_M(ob1, ob2)) { return true; }

        //距離０なら
        if (checkZeroDistance(ob2)) { return false; }

        //カウント用変数
        int count = 0;
        //オブジェクトを調べて
        foreach (GameObject obj in ObjectManager.Instance.square)
        {
            //ob1のマンハッタン距離が１かつ
            if (checkOneDistance_M(ob1, obj) && checkOneDistance_M(ob2, obj))
            {
                //カウントを増やす
                count++;
            }
        }
        //カウントが２だったら
        if (count == 2) { return true; }

        //ここまでの条件に当てはまらなかったら
        return false;
    }

    //マンハッタン距離１を確認
    protected bool checkOneDistance_M(GameObject obj1, GameObject obj2)
    {
        Vector2 pos1 = new Vector2(obj1.transform.position.x, obj1.transform.position.z);
        Vector2 pos2 = new Vector2(obj2.transform.position.x, obj2.transform.position.z);
        if (Vector2.Distance(pos1, pos2) == 10) { return true; }
        return false;
    }

    //距離０を確認（対象）
    protected bool checkZeroDistance(GameObject obj)
    {
        Vector2 pos1 = new Vector2(this.transform.position.x, this.transform.position.z);
        Vector2 pos2 = new Vector2(obj.transform.position.x, obj.transform.position.z);
        if (pos1 == pos2) { return true; }
        return false;
    }
}
