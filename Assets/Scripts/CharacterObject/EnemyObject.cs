using UnityEngine;
using System.Collections;
using System.Linq;
using MyUtility;

public class EnemyObject : AbstractCharacterObject
{
    AttackWay myAttack;

    public void Start()
    {
        this.type = Type.Enemy;
        myAttack = new AttackWay("キック", 10, PrefabManager.Instance.explosion);
    }

    //基本処理
    public override void operation() { base.operation(); }

    //スタンバイフェイズ
    protected override void startOperation()
    {
        Debug.Log("ステンバーイ");
        ObjectManager.Instance.setCharacter();
        ObjectManager.Instance.setSquare();
        base.startOperation();
    }

    //メインフェイズ
    protected override void mainOperation()
    {
        Debug.Log("メイーン");
        searchTarget();
    }

    //プリエンドフェイズ
    protected override void preEndOperation() { base.preEndOperation(); }

    //エンドフェイズ
    protected override void endOperation() { base.endOperation(); }

	//仮
    protected override void nextOperation() { base.nextOperation(); }


    //ターゲットを探して行動（メインフェイズ）
    void searchTarget()
    {
        var target = ObjectManager.Instance.character[0];

        if (this.gameObject.checkOneDistanceE(target))
        {
            attackToTarget(target);
        }
        else
        {
            moveTowardTarget(target);
        }
    }

    //攻撃（仮）
    void attackToTarget(GameObject target)
    {
        base.attackTarget(myAttack, target);
    }

    //移動
    void moveTowardTarget(GameObject target)
    {
		//現在地を取得
		var squares = from n in ObjectManager.Instance.square
			where n.transform.position.x == this.gameObject.transform.position.x
			&& n.transform.position.z == this.gameObject.transform.position.z
				select n;

		//現在地から範囲１以内のマスを取得（現在地含まない）
		var aroundSquare = squares.ElementAt (0).GetComponent<AbstractSquare> ().aroundSquare (1, true);

		//targetに一番近いマスを取得
		var positions = from n in aroundSquare
			where n.GetComponent<AbstractSquare>().isCharacterOn() != true
			orderby Vector3.Distance (n.transform.position, target.transform.position)
				select n;

		//移動できるマスがあれば
		if (positions.Count () != 0)
		{
			var position = positions.ElementAt (0);
			movePosition(position);
		}

		//ターン終了
		process = Process.PreEnd;
	}
}
