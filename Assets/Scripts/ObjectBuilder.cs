using UnityEngine;
using System.Collections;

/// <summary>
/// Enemy関連.
/// </summary>
public class EnemyContainer
{
    public AbstractCharacterParameter parameter;
    public GameObject prefab;

    //IDからパラメーターを生成
    public EnemyContainer(int ID)
    {
        this.parameter = EnemyParameter.getEnemyParameter(ID);
        this.prefab = PrefabManager.Instance.enemyList[ID];
    }
    //既存のパラメーターから生成（保存）
    public EnemyContainer(AbstractCharacterParameter enemyParameter)
    {
        this.parameter = enemyParameter;
        this.prefab = PrefabManager.Instance.enemyList[enemyParameter.id];
    }
}

/// <summary>
/// Item関連
/// </summary>
public class ItemContainer
{
    public Item item;
    public Vector3 vector3;

    public enum type
    {
        NullCard = 0,
        Flower = 1,
        Sword = 2,
        Bomb = 3,
		axe = 4
    }

	//IDから生成（出現率と合わせ、XMLから生成）
    public ItemContainer(int id)
    {
        this.item = getItem(id);
        this.vector3 = new Vector3(-1, -1, -1);
    }
	//一度出現させたデータを元に再生成
    public ItemContainer(Item item, Vector3 vector3)
    {
        this.item = item;
        this.vector3 = vector3;
    }

	//IDから対応するアイテムを格納
    public Item getItem(int ID)
    {
        if (ID == 1) { return new FlowerItem(); }
        if (ID == 2) { return new SwordItem(2); }
        if (ID == 3) { return new BombItem(); }
        if (ID == 4) { return new AxeItem(4); }
        if (ID == 12) { return new BreadItem(12); }
        return new NullItem();
    }
}
