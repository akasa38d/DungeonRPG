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
    public int power;			//威力・効果
    public bool chain;			//チェインできるかどうか
    public bool expendable;		//消費するかどうか
    public bool magic;			//魔法であるかどうか

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



}

/// <summary>
/// 空の状態
/// </summary>
public class NullItem : Item
{
    public NullItem()
    {
        id = 0;
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
        power = 0;
        chain = false;
        expendable = false;
        magic = false;
    }

    public override void buttonEvent()
    {
        playerScript.process = AbstractCharacterObject.Process.PreEnd;
    }
}


/// <summary>
/// 近接攻撃アイテム（仮）
/// </summary>
public class SwordItem : Item
{
    public override Sprite sprite
    {
        get { return PrefabManager.Instance.swordCard; }
        set { }
    }

    public SwordItem()
    {
        //id(仮)
        id = 2;
        //変数のセット
        power = 99;
        chain = false;
        expendable = false;
        magic = false;
    }

    public SwordItem(int power)
    {
        //player関係の設定
        this.player = GameObject.Find("Player");
        this.playerScript = player.GetComponent<PlayerObject>();
        //変数のセット
        this.power = power;
        this.chain = false;
        this.expendable = false;
        this.magic = false;
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
                //キャラクターが乗っているなら
                if (floor.GetComponent<AbstractSquare>().isCharacterOn())
                {
                    var tmp = playerScript.pInstantiate(attackButton, new Vector3(floor.transform.position.x, floor.transform.position.y + 0.005f, floor.transform.position.z));
                    var tmpScript = tmp.GetComponent<AttackButton>();
                    tmpScript.square = floor;
                    tmpScript.attack = this.operation;
                }
            }
        }
    }

    public override void operation(GameObject obj)
    {
        Debug.Log("短剣で攻撃！");
        //ターンエンドのためのコールバックをセット
        obj.GetComponent<AbstractCharacterObject>().callBack = () => playerScript.process = AbstractCharacterObject.Process.PreEnd;
        //攻撃のエフェクト
        if (effect != null) { playerScript.pInstantiate(effect, obj.transform.position); }
        //ダメージ処理
        obj.GetComponent<AbstractCharacterObject>().beDameged(this.power);
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
        //id(仮)
        id = 2;
        //変数のセット
        power = 99;
        chain = false;
        expendable = false;
        magic = false;

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
