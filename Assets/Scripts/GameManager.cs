using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        //TurnManagerクラスにオブジェクトを設定
//        TurnManager.Instance.setObject();
//		TurnManager.Instance.setComponent();
//		ObjectManager.Instance.setSquare();
		ObjectManager.Instance.setCharacter();
    }

    // Update is called once per frame
    void Update()
    {
		TurnManager.Instance.operation();
    }
}
