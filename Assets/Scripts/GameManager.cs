using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        ObjectManager.Instance.setCharacter();
	}

    void Update()
    {
        TurnManager.Instance.operation();
    }
}
