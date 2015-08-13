using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// プレハブを管理するシングルトンのクラス
/// </summary>
public class PrefabManager : SingletonMonoBehaviour<PrefabManager>
{
    //Square系
    public GameObject square;
    public GameObject pathSquare;

    //Button系
    public GameObject attackButton;
    public GameObject moveButton;

    //effect系
    public GameObject explosion;

	//Enemy系
	public Dictionary<int, GameObject> enemyList = new Dictionary<int, GameObject>();

    //起動時の処理
    public override void Awake() {
		base.Awake();

		enemyList.Clear ();
		enemyList.Add (0, Resources.Load ("EnemyPrefabs/Enemy") as GameObject);
		enemyList.Add (2, Resources.Load ("EnemyPrefabs/Enemy mk2") as GameObject);
		enemyList.Add (3, Resources.Load ("EnemyPrefabs/Enemy mk3") as GameObject);
//		Instantiate (enemyList [2]);
	}

	public Sprite test;

	public Sprite sword;
	public Sprite flower;
	public Sprite nullCard;
}
