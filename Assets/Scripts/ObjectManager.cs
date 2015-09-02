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
    public List<AbstractCharacter> characterScript = new List<AbstractCharacter>();
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
            if (obj.GetComponent<AbstractCharacter>().type == AbstractCharacter.Type.Enemy)
            {
                if(obj.GetComponent<AbstractCharacter>().parameter.hp > 0)
                {
                    character.Add(obj);
                }
            }
        }

        //各スクリプトをセット
        characterScript.Clear();
        characterScript.Add(character[0].GetComponent<Player>());
        for (int i = 1; i < character.Count; i++)
        {
            characterScript.Add(character[i].GetComponent<Enemy>());
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
