using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public abstract class Item
{
    protected GameObject player;
    protected PlayerObject playerScript;

    public virtual Sprite sprite { get; set; }
    protected GameObject effect;

    //変数
    public int id;
	public string name;			//名前
    public int power = 0;			//威力・効果
    public bool chain = false;			//チェインできるかどうか
	public bool expendable = false;		//消費するかどうか
	public bool magic = false;			//魔法であるかどうか

    public Item()
    {
        //player関係の設定
        this.player = GameObject.Find("Player");
        this.playerScript = player.GetComponent<PlayerObject>();
    }

    //ボタンを押した時に発生するイベント
    public virtual void buttonEvent() { }
    //実際に行われる処理処理
    public virtual void operation() { }
    public virtual void operation(GameObject obj) { }
	public virtual void changeOperation()
	{
		playerScript.process = AbstractCharacterObject.Process.PreEnd;
	}
}

/// <summary>
/// 空の状態
/// </summary>
public class NullItem : Item
{
    public NullItem()
	{
		id = 0;
		name = "nullです";
	}

    public override Sprite sprite
    {
        get { return PrefabManager.Instance.nullCard; }
        set { }
    }
}

/// <summary>
/// 得点になるアイテム
/// 手札交換のみ可能
/// </summary>
public class FlowerItem : Item
{
    public override Sprite sprite
    {
        get { return PrefabManager.Instance.flowerCard; }
        set { }
    }

    public FlowerItem()
	{
		id = 1;
		name = "花束";
	}
}


/// <summary>
/// 近接攻撃アイテム（仮）
/// </summary>
public class SwordItem : Item
{
    public override Sprite sprite
    {
        get
		{
			if(id == 2)
			{
				return PrefabManager.Instance.swordCard;
			}
			return PrefabManager.Instance.nullCard;
		}
        set { }
    }

    public SwordItem(int id = 2)
    {
        //変数のセット
		this.id = id;
		if (id == 2)
		{
			this.name = "短剣";
			this.power = 99;
		}
    }

    public override void buttonEvent()
    {
        createButton();
    }

    //攻撃用ボタンを生成
    void createButton()
    {
        //prefabsの設定
        var attackButton = PrefabManager.Instance.attackButton;

        //既存のボタンを削除
        playerScript.deleteButton();

        foreach (var floor in ObjectManager.Instance.square)
        {
            //プレイヤからの距離が１の床で
            if (playerScript.checkOneDistance(player, floor))
            {
                    var tmp = playerScript.pInstantiate(attackButton, new Vector3(floor.transform.position.x, floor.transform.position.y + 0.005f, floor.transform.position.z));
                    var tmpScript = tmp.GetComponent<AttackButton>();
                    tmpScript.square = floor;
                    tmpScript.attack = this.operation;
            }
        }
    }

    public override void operation(GameObject square)
    {
		try
		{
			//キャラクターが乗っているなら
			if (square.GetComponent<AbstractSquare>().isCharacterOn())
			{
				var target = square.GetComponent<AbstractSquare>().character;
				Debug.Log("短剣で攻撃！");
				//ターンエンドのためのコールバックをセット
				target.GetComponent<AbstractCharacterObject>().callBack = () => {};
				//ダメージ処理
				target.GetComponent<AbstractCharacterObject>().beDameged(this.power);
			}			
		}
		finally
		{
			playerScript.process = AbstractCharacterObject.Process.PreEnd;
		}
    }
}

/// <summary>
/// 爆弾
/// </summary>
public class BombItem : Item
{
    public override Sprite sprite
    {
        get { return PrefabManager.Instance.bombCard; }
        set { }
    }

    public BombItem()
    {
        id = 3;
        power = 99;
		expendable = true;
        this.effect = PrefabManager.Instance.explosion;
    }

    public override void buttonEvent()
    {
        createButton();
    }

    void createButton()
    {
        //prefabsの設定
        var extraAttackButton = PrefabManager.Instance.extraAttackButton;
        var subAttackButton = PrefabManager.Instance.subAttackButton;

        //既存のボタンを削除
        playerScript.deleteButton();

        foreach (var floor in ObjectManager.Instance.square)
        {
            //プレイヤからの距離が３の床で
            if (playerScript.checkDistance(player, floor, 2))
            {
                var tmp = playerScript.pInstantiate(extraAttackButton, new Vector3(floor.transform.position.x, floor.transform.position.y + 0.005f, floor.transform.position.z));
                var tmpScript = tmp.GetComponent<ExtraAttackButton>();
                tmpScript.square = floor;
                tmpScript.attack = this.operation;
            }
            //プレイヤからの距離が３の床で
            if (playerScript.checkDistance(player, floor, 3) && !playerScript.checkDistance(player, floor, 2))
            {
                playerScript.pInstantiate(subAttackButton, new Vector3(floor.transform.position.x, floor.transform.position.y + 0.005f, floor.transform.position.z));
            }
        }
    }

    public override void operation(GameObject square)
    {
        try
        {
            foreach (var n in square.GetComponent<AbstractSquare>().aroundSquare(1))
            {
                if (effect != null) { playerScript.pInstantiate(effect, n.transform.position); }
            }

            var target = from n in square.GetComponent<AbstractSquare>().aroundSquare(1)
                         where n.GetComponent<AbstractSquare>().isCharacterOn()
                         select n;
            foreach (var n in target)
            {
                var c = n.GetComponent<AbstractSquare>().character;
                Debug.Log("爆弾で攻撃！");
                //ターンエンドのためのコールバックをセット
                c.GetComponent<AbstractCharacterObject>().callBack = () => { };
                //ダメージ処理
                c.GetComponent<AbstractCharacterObject>().beDameged(this.power);
            }
        }
        finally
        {
            playerScript.process = AbstractCharacterObject.Process.PreEnd;
        }

    }

}
