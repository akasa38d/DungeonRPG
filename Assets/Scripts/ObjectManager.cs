using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// オブジェクトを一括して管理するためのクラス
/// </summary>
public class ObjectManager : SingletonMonoBehaviour<ObjectManager>
{
    //仮
    [SerializeField]
    public GameObject explosion;

    public List<GameObject> character = new List<GameObject>();
    public List<AbstractCharacterObject> characterScript = new List<AbstractCharacterObject>();
    public List<GameObject> square = new List<GameObject>();

    public override void Awake() { base.Awake(); }

    public void setCharacter()
    {
        //一旦クリアー
        character.Clear();
        //playerを優先してセット
        character.Add(GameObject.Find("Player"));
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Character"))
        {
            if (obj.GetComponent<AbstractCharacterObject>().type == AbstractCharacterObject.Type.Enemy)
            {
                if(obj.GetComponent<AbstractCharacterObject>().parameter.hp > 0)
                {
                    character.Add(obj);
                }
            }
        }

        //各スクリプトをセット
        characterScript.Clear();
        characterScript.Add(character[0].GetComponent<PlayerObject>());
        characterScript[0].id = 0;
        for (int i = 1; i < character.Count; i++)
        {
            characterScript.Add(character[i].GetComponent<EnemyObject>());
            characterScript[i].id = i;
        }
    }

    public void setSquare()
    {
        square.Clear();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Square"))
        {
            square.Add(obj);
        }
    }
}
