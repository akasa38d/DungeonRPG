using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
	protected GameObject player;
	protected PlayerObject playerScript;

	public virtual Sprite sprite { get; set; }
	protected GameObject effect;

	//変数
	public int id;
	public int power;			//威力・効果
	public bool chain;			//チェインできるかどうか
	public bool expendable;		//消費するかどうか
	public bool magic;			//魔法であるかどうか

	//ボタンを押した時に発生するイベント
	public virtual void buttonEvent (){}
	//実際に行われる処理処理
	public virtual void operation(){}
	public virtual void operation(GameObject obj){}
}


public class NullItem : Item
{
	public NullItem()
	{
		id = 0;
	}

	public override Sprite sprite {
		get { return PrefabManager.Instance.nullCard; }
		set{}
	}
}


public class FlowerItem : Item
{
	public FlowerItem()
	{
		id = 1;

		//player関係の設定
		this.player = GameObject.Find ("Player");
		this.playerScript = player.GetComponent<PlayerObject> ();

		power = 0;
		chain = false;
		expendable = false;
		magic = false;
	}

	public override Sprite sprite {
		get { return PrefabManager.Instance.flowerCard; }
		set{}
	}

	public override void buttonEvent()
	{
		playerScript.process = AbstractCharacterObject.Process.End;
	}
}


/// <summary>
/// 近接攻撃アイテム（仮）
/// </summary>
[System.Serializable]
public class SwordItem : Item
{
	public override Sprite sprite {
		get { return PrefabManager.Instance.swordCard; }
		set{}
	}

	public SwordItem()
	{
		id = 2;

		//player関係の設定
		this.player = GameObject.Find ("Player");
		this.playerScript = player.GetComponent<PlayerObject> ();

		power = 99;
		chain = false;
		expendable = false;
		magic = false;
	}

	public SwordItem(int power)
	{
		//player関係の設定
		this.player = GameObject.Find ("Player");
		this.playerScript = player.GetComponent<PlayerObject> ();
		
		this.power = power;
		this.chain = false;
		this.expendable = false;
		this.magic = false;
	}

	public override void buttonEvent()
	{
		createButton ();
	}

	//攻撃用ボタンを生成
	public void createButton()
	{
		//prefabsの設定
		var attackButton = PrefabManager.Instance.attackButton;

		//既存のボタンを削除
		playerScript.deleteButton ();
		
		foreach (var floor in ObjectManager.Instance.square)
		{
			//プレイヤからの距離が１の床で
			if (playerScript.checkOneDistance(player, floor))
			{
				//キャラクターが乗っているなら
				if (floor.GetComponent<AbstractSquare>().isCharacterOn())
				{
					var tmp = playerScript.pInstantiate(attackButton, new Vector3(floor.transform.position.x, floor.transform.position.y + 0.005f, floor.transform.position.z));
					var tmpScript = tmp.GetComponent<AttackButton>();
					tmpScript.square = floor;
					tmpScript.attackDelegate2 = this.operation;
				}
			}
		}
	}

	public override void operation(GameObject obj)
	{
		Debug.Log("短剣で攻撃！");
		//ターンエンドのためのコールバックをセット
		obj.GetComponent<AbstractCharacterObject>().callBack = () => playerScript.process = AbstractCharacterObject.Process.End;
		//攻撃のエフェクト
		if (effect != null) { playerScript.pInstantiate(effect, obj.transform.position); }
		//ダメージ処理
		obj.GetComponent<AbstractCharacterObject>().beDameged(this.power);
	}
}
