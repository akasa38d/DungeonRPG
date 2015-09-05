using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Xml;
using MyUtility;

[System.Serializable]
public abstract class Item
{
    protected GameObject player;
    protected Player playerScript;
    
    public virtual Sprite sprite { get; set; }
    protected GameObject effect;
    
    //parameter（readonlyにした方がいいか？）
    public int id;
    public string name;         //名前
    public string text;             //説明文
    public int power = 0;           //威力・効果
    public bool chain = false;          //再行動できるかどうか
    public bool expendable = false;     //消費するかどうか
    public bool magic = false;          //魔法であるかどうか（仮）
    
    //管理用
    public bool changed = false;        //カード交換を使用したか（一時的）
    
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
        this.changed = true;
        playerScript.process = AbstractCharacter.Process.PreEnd;
    }
    
    //loadTest
    public static Item getItemData(int number)
    {
        XmlNodeList nodes = XMLReader.Instance.itemNodes;
        if(number == 0)
        {
            return new NullItem();
        }
        XmlNode tempNode = searchNode(number, nodes);
        Item item = getItemType(number);
        item.id = number;
        foreach (XmlNode node in tempNode.ChildNodes)
        {
            if (node.Name == "iName") { item.name = node.InnerText; }
            if (node.Name == "iText") { item.text = node.InnerText; }
            if (node.Name == "iPower") { item.power = int.Parse(node.InnerText); }
            if (node.Name == "iChain")
            {
                if(int.Parse(node.InnerText) != 0)
                {
                    item.chain = true;
                }
            }
            if (node.Name == "iExpendable") 
            {
                if(int.Parse(node.InnerText) != 0)
                {
                    item.expendable = true;
                }
            }
            if (node.Name == "iMagic")
            {
                if(int.Parse(node.InnerText) != 0)
                {
                    item.magic = true;
                }
            }
        }
        return item;
    }
    
    static XmlNode searchNode(int number, XmlNodeList nodes)
    {
        foreach (XmlNode n in nodes)
        {
            if(int.Parse(n.Attributes.GetNamedItem("id").Value) == number)
            {
                return n;
            }
        }
        return null;
    }
    
    static Item getItemType(int id)
    {
        if (id > 70)
            return new NullItem();
        if (id > 60)
            return new FlowerItem();
        if (id > 50)
            return new BreadItem();
        if (id > 40)
            return new BombItem();
        if (id > 30)
            return new KnuckleItem();
        if (id > 20)
            return new SpearItem();
        if (id > 10)
            return new AxeItem();
        return new SwordItem();
    }
}

/// <summary>
/// 空の状態
/// </summary>
public class NullItem : Item
{
    public NullItem() { }
    
    public override Sprite sprite
    {
        get { return PrefabManager.Instance.nullCard; }
        set { }
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
            if(id == 1)
            {
                return PrefabManager.Instance.knifeCard;
            }
            if(id == 2)
            {
                return PrefabManager.Instance.swordCard;
            }
            return PrefabManager.Instance.nullCard;
        }
        set { }
    }
    
    public SwordItem()
    {
        this.effect = PrefabManager.Instance.explosion;
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
/// 斧
/// </summary>
public class AxeItem : Item
{
    public override Sprite sprite
    {
        get { return PrefabManager.Instance.axeCard; }
        set { }
    }
    
    public AxeItem()
    {
        this.effect = PrefabManager.Instance.explosion;
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
        if (effect != null) { playerScript.pInstantiate(effect, square.transform.position); }
        
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
        get { return PrefabManager.Instance.knuckleCard; }
        set { }
    }
    
    public KnuckleItem()
    {
        this.effect = PrefabManager.Instance.explosion;
    }

    public override void buttonEvent()
    {
        createButton();
    }
    
    void createButton()
    {
        //prefabsの設定
        var extraMoveButton = PrefabManager.Instance.extraMoveButton;
        var subAttackButton = PrefabManager.Instance.subAttackButton;
        
        //既存のボタンを削除
        playerScript.deleteButton();

        foreach (var floor in ObjectManager.Instance.square)
        {
            if (player.checkOneDistanceE(floor))
            {
                if (!floor.GetComponent<AbstractSquare>().isCharacterOn())
                {
                    var tmp = playerScript.pInstantiate(extraMoveButton, new Vector3(floor.transform.position.x, floor.transform.position.y + 0.005f, floor.transform.position.z)) as GameObject;
                    var tmpScript = tmp.GetComponent<ExtraMoveButton>();
                    tmpScript.square = floor;
                    tmpScript.effect = this.operation;
                    tmpScript.turnEnd = () => playerScript.process = AbstractCharacter.Process.PreEnd;

                    var target = floor.GetComponent<AbstractSquare>().extendSquare(player);
                    if(target != null)
                    {
                        var tmp2 = playerScript.pInstantiate(subAttackButton, new Vector3(target.transform.position.x, target.transform.position.y + 0.005f, target.transform.position.z)) as GameObject;
                        var tmp2Script = tmp2.GetComponent<SubAttackButton>();
                        tmp2Script.square = target;
                        tmp2Script.effect = this.operation2;

                        tmpScript.subAttackButton = tmp2;
                    }
                }
            }
        }
    }

    public override void operation(GameObject square)
    {
        //対象の場所まで移動
        player.transform.position = new Vector3(square.transform.position.x, player.transform.position.y, square.transform.position.z);
        
        //現在地を変更
        playerScript.square = square;
        
        //床のイベント発生
        square.GetComponent<AbstractSquare>().enterThis();      
    }

    public void operation2(GameObject square)
    {
        if (effect != null) { playerScript.pInstantiate(effect, square.transform.position); }
        
        if (square.GetComponent<AbstractSquare> ().isCharacterOn())
        {
            var target = square.GetComponent<AbstractSquare>().character;
            Debug.Log(this.name + "で攻撃！");
            target.GetComponent<AbstractCharacter>().beDameged(this.power);
        }
    }
}

public class SpearItem : Item
{
}

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
        
        if (square.GetComponent<AbstractSquare>().isCharacterOn())
        {
            var target = square.GetComponent<AbstractSquare>().character;
            Debug.Log("爆弾で攻撃！");
            target.GetComponent<AbstractCharacter>().beDameged(this.power);
        }
    }
}

public class BreadItem : Item
{
    public override Sprite sprite
    {
        get
        {
            if (id == 52)
            {
                return PrefabManager.Instance.breadMCard;
            }
            return PrefabManager.Instance.nullCard;
        }
        set { }
    }
    
    public BreadItem() { }
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
        
        var floor = playerScript.square;
        var tmp = playerScript.pInstantiate(cureButton, new Vector3(floor.transform.position.x, floor.transform.position.y + 0.005f, floor.transform.position.z));
        var tmpScript = tmp.GetComponent<CureButton>();
        tmpScript.square = floor;
        
        //デリゲート
        tmpScript.effect = this.operation;
        tmpScript.turnEnd = () => playerScript.process = AbstractCharacter.Process.PreEnd;
        
    }
    
    public override void operation(GameObject square)
    {
        if (effect != null) { playerScript.pInstantiate(effect, square.transform.position); }
        
        if (square.GetComponent<AbstractSquare>().isCharacterOn())
        {
            var target = square.GetComponent<AbstractSquare>().character;
            
            if (target.GetComponent<AbstractCharacter>().parameter.sp + this.power > 100)
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
    
    public FlowerItem() { }
    
    public override void buttonEvent()
    {
        //既存のボタンを削除
        playerScript.deleteButton();
    }
}
