using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
	protected GameObject player;
	protected PlayerObject playerScript;

	public GameObject button;

	protected GameObject graphic;

	protected GameObject effect;

	protected int power;
	protected bool chain;
	protected bool expendable;
	protected bool magic;

	public Item()
	{
	}

	public virtual void buttonEvent ()
	{
		//ボタンを作成
		createButton ();
	}

	public virtual void createButton(){}
	
	public virtual void operation(){}
	public virtual void operation(GameObject obj){}
}

/// <summary>
/// 近接攻撃アイテム（仮）
/// </summary>
public class SwordItem : Item
{
	public SwordItem()
	{
		//player関係の設定
		this.player = GameObject.Find ("Player");
		this.playerScript = player.GetComponent<PlayerObject> ();

		power = 99;
		chain = false;
		expendable = false;
		magic = false;

		var n = GameObject.Find ("ItemButton");
		var image = n.GetComponent<Image> ();
		image.sprite = Resources.Load<Sprite>("無題") as Sprite;
	}

	//攻撃用ボタンを生成
	public override void createButton()
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
