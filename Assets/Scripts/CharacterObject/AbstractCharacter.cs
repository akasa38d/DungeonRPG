using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MyUtility;

/// <summary>
/// キャラクターオブジェクトの抽象クラス
/// 基本処理とインターフェースを設定
/// </summary>
public abstract class AbstractCharacter : MonoBehaviour
{
    //キャラクターの種類
    public enum Type { Player, Friend, Enemy };
    public Type type;

    //ターン進行
    [SerializeField]
    public enum Process { Start, Main, PreEnd, End, Next };
    public Process process;

    //現在地
    public GameObject square;

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
            case Process.PreEnd:
                this.preEndOperation();
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
    protected virtual void preEndOperation()
    {
        if (additionalTurn > 0)
        {
            additionalTurn -= 1;
            process = Process.Start;
        }
        else
        {
            process = Process.End;
        }
    }

    //エンドフェイズ処理
    protected virtual void endOperation()
    {
       process = Process.Next;
    }

    //仮
    protected virtual void nextOperation() { }

    //攻撃
    public virtual void attackTarget(AttackWay attackWay, GameObject obj)
    {
		try
		{
	        Debug.Log(this.parameter.cName + "が" + attackWay.name + "で攻撃！");
			//攻撃のエフェクト
			if (attackWay.effect != null) { Instantiate(attackWay.effect, obj.transform.position, Quaternion.identity); }
			//ダメージ処理
			obj.GetComponent<AbstractCharacter>().beDameged(attackWay.power);
		}
		finally
		{
			this.process = Process.PreEnd;
		}
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
    public Action callBack;

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
            this.parameter.hp = 0;
            Debug.Log("グワーッ！！" + this.parameter.cName + "は爆発四散！");
            Instantiate(PrefabManager.Instance.explosion, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            yield return null;
        }
        else
        {
            this.parameter.hp -= damage;
        }
        yield return null;
    }

    //やられた時
    public virtual void beDefeated()
    {
        var player = GameObject.Find("Player");
        player.GetComponent<Player>().parameter.exp += this.parameter.exp;
    }

    //移動（メインフェイズ、床を指定すること）
    public virtual void movePosition(GameObject square)
    {
        //対象の場所まで移動
        this.transform.position = new Vector3(square.transform.position.x, this.transform.position.y, square.transform.position.z);

        //現在地を変更
        this.square = square;

        //床のイベント発生
        square.GetComponent<AbstractSquare>().enterThis();

        //フェイズ移行
        this.process = Process.PreEnd;
    }
}
