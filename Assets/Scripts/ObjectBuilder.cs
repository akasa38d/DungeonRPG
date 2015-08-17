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
        Bomb = 3
    }

    public ItemContainer(int id)
    {
        this.item = getItem(id);
        this.vector3 = new Vector3(-1, -1, -1);
    }

    public ItemContainer(Item item, Vector3 vector3)
    {
        this.item = item;
        this.vector3 = vector3;
    }

    public Item getItem(int id)
    {
        if (id == 1) { return new FlowerItem(); }
        if (id == 2) { return new SwordItem(); }
        if (id == 3) { return new BombItem(); }
        return new NullItem();
    }
}
