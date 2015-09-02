using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using MyUtility;

[System.Serializable]
public abstract class Item
{
    protected GameObject player;
    protected Player playerScript;

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
        this.playerScript = player.GetComponent<Player>();
    }

    //ボタンを押した時に発生するイベント
    public virtual void buttonEvent() { }
    //実際に行われる処理処理
    public virtual void operation() { }
    public virtual void operation(GameObject obj) { }
	public virtual void changeOperation()
	{
		playerScript.process = AbstractCharacter.Process.PreEnd;
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

    public override void buttonEvent()
    {
        //既存のボタンを削除
        playerScript.deleteButton();
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
				return PrefabManager.Instance.knifeCard;
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
			this.power = 9;
            this.chain = true;
            this.effect = PrefabManager.Instance.explosion;
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
            if (player.checkOneDistanceE(floor))
            {
                var tmp = playerScript.pInstantiate(attackButton, new Vector3(floor.transform.position.x, floor.transform.position.y + 0.005f, floor.transform.position.z));
                var tmpScript = tmp.GetComponent<AttackButton>();
                tmpScript.square = floor;
                tmpScript.effect = this.operation;
                tmpScript.turnEnd = () => playerScript.process = AbstractCharacter.Process.PreEnd;
            }
        }
    }

    public override void operation(GameObject square)
    {
        if (effect != null) { playerScript.pInstantiate(effect, square.transform.position); }

		//キャラクターが乗っているなら
		if (square.GetComponent<AbstractSquare>().isCharacterOn())
		{
			var target = square.GetComponent<AbstractSquare>().character;
			Debug.Log(this.name + "で攻撃！");
			//ダメージ処理
			target.GetComponent<AbstractCharacter>().beDameged(this.power);
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
        this.name = "爆弾";
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
            if (player.checkDistanceCE(floor, 2))
            {
                var tmp = playerScript.pInstantiate(extraAttackButton, new Vector3(floor.transform.position.x, floor.transform.position.y + 0.005f, floor.transform.position.z));
                var tmpScript = tmp.GetComponent<ExtraAttackButton>();
                tmpScript.square = floor;
                tmpScript.effect = this.operation;
                tmpScript.turnEnd = () => playerScript.process = AbstractCharacter.Process.PreEnd;

            }
            //プレイヤからの距離が３の床で
            if (player.checkDistanceCE(floor, 3) && !player.checkDistanceCE(floor, 2))
            {
                var tmp = playerScript.pInstantiate(subAttackButton, new Vector3(floor.transform.position.x, floor.transform.position.y + 0.005f, floor.transform.position.z));
                var tmpScript = tmp.GetComponent<SubAttackButton>();
                tmpScript.square = floor;
                tmpScript.effect = this.operation;
            }
        }
    }

    public override void operation(GameObject square)
    {
        if (effect != null) { playerScript.pInstantiate(effect, square.transform.position); }

        if (square.GetComponent<AbstractSquare> ().isCharacterOn())
        {
            var target = square.GetComponent<AbstractSquare>().character;
            Debug.Log("爆弾で攻撃！");
            target.GetComponent<AbstractCharacter>().beDameged(this.power);
        }
    }
}

/// <summary>
/// 斧れ
/// </summary>
public class AxeItem : Item
{
	public override Sprite sprite
	{
		get { return PrefabManager.Instance.axeCard; }
		set { }
	}
	
	public AxeItem(int id = 4)
	{
		//変数のセット
		this.id = id;
		if (id == 4)
		{
			this.name = "手斧";
			this.power = 12;
			this.effect = PrefabManager.Instance.explosion;
		}
	}
	
	public override void buttonEvent()
	{
		createButton();
	}
	
	void createButton()
	{
		//prefabsの設定
		var wideAttackButton = PrefabManager.Instance.wideAttackButton;

		//既存のボタンを削除
		playerScript.deleteButton();
		
		foreach (var floor in ObjectManager.Instance.square)
		{
			//プレイヤからの距離が１の床で
			if (player.checkOneDistanceE(floor))
			{
				var tmp = playerScript.pInstantiate(wideAttackButton, new Vector3(floor.transform.position.x, floor.transform.position.y + 0.005f, floor.transform.position.z));
				var tmpScript = tmp.GetComponent<WideAttackButton>();
				tmpScript.square = floor;

				//デリゲート
				tmpScript.effect = this.operation;
				tmpScript.turnEnd = () => playerScript.process = AbstractCharacter.Process.PreEnd;
			}
		}
	}
	
	public override void operation(GameObject square)
	{
		if (effect != null) { playerScript.pInstantiate(effect, square.transform.position);	}

		if (square.GetComponent<AbstractSquare> ().isCharacterOn())
		{
			var target = square.GetComponent<AbstractSquare>().character;
			Debug.Log(this.name + "で攻撃！");
			target.GetComponent<AbstractCharacter>().beDameged(this.power);
		}
	}
}

public class KnuckleItem : Item
{
    public override Sprite sprite
    {
        get { return PrefabManager.Instance.axeCard; }
        set { }
    }
    
    public KnuckleItem(int id = 5)
    {
        //変数のセット
        this.id = id;
        if (id == 5)
        {
            this.name = "こぶし";
            this.power = 99;
            this.effect = PrefabManager.Instance.explosion;
        }
    }
}

public class BreadItem : Item
{
    public override Sprite sprite
    {
        get
        {
            if(id == 12) 
            {
                return PrefabManager.Instance.breadMCard;
            }
            return PrefabManager.Instance.nullCard;
        }
        set { }
    }
    
    public BreadItem(int id = 12)
    {
        //変数のセット
        this.id = id;
        if (id == 11)
        {
            this.name = "パン（小）";
            this.power = 25;
        }
        if (id == 12)
        {
            this.name = "パン（中）";
            this.power = 50;
        }
        if (id == 13)
        {
            this.name = "パン（大）";
            this.power = 100;
        }
        this.expendable = true;
    }
    public override void buttonEvent()
    {
        createButton();
    }

    void createButton()
    {
        //prefabsの設定
        var cureButton = PrefabManager.Instance.cureButton;

        //既存のボタンを削除
        playerScript.deleteButton();

        foreach (var floor in ObjectManager.Instance.square
                 .Where((n) => n.GetComponent<AbstractSquare>().isCharacterOn(AbstractCharacter.Type.Player)))
        {
            var tmp = playerScript.pInstantiate(cureButton, new Vector3(floor.transform.position.x, floor.transform.position.y + 0.005f, floor.transform.position.z));
            var tmpScript = tmp.GetComponent<CureButton>();
            tmpScript.square = floor;

            //デリゲート
            tmpScript.effect = this.operation;
            tmpScript.turnEnd = () => playerScript.process = AbstractCharacter.Process.PreEnd;
        }
    }

    public override void operation(GameObject square)
    {
        if (effect != null) { playerScript.pInstantiate(effect, square.transform.position); }

        if (square.GetComponent<AbstractSquare> ().isCharacterOn())
        {
            var target = square.GetComponent<AbstractSquare>().character;

            if(target.GetComponent<AbstractCharacter>().parameter.sp + this.power > 100)
            {
                target.GetComponent<AbstractCharacter>().parameter.sp = 100;
                Debug.Log("SPが満タンになった！");
            }
            else
            {
                target.GetComponent<AbstractCharacter>().parameter.sp += this.power;
                Debug.Log("SPが" + power + "回復！");
            }
        }
    }
}


