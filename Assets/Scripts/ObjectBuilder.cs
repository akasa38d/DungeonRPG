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

	//IDから生成（出現率と合わせ、XMLから生成）
    public ItemContainer(int id)
    {
        this.item = Item.getItemData(id);
        this.vector3 = new Vector3(-1, -1, -1);
    }
	//一度出現させたデータを元に再生成
    public ItemContainer(Item item, Vector3 vector3)
    {
        this.item = item;
        this.vector3 = vector3;
    }
}
