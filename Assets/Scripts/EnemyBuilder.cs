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
		this.parameter = EnemyParameter.getEnemyParameter (ID);
		this.prefab = PrefabManager.Instance.enemyList [ID];
	}
	//既存のパラメーターから生成（保存）
	public EnemyContainer(AbstractCharacterParameter enemyParameter)
	{
		this.parameter = enemyParameter;
		this.prefab = PrefabManager.Instance.enemyList [enemyParameter.id];
	}
}
