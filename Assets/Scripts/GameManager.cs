using UnityEngine;
using System.Collections;

namespace MyScript
{
    public class GameManager : MonoBehaviour
    {
        void Awake()
        {
            Application.targetFrameRate = 60;
        }

        void Start()
        {
            ObjectManager.Instance.setCharacter();
        }

        void Update()
        {
            TurnManager.Instance.operation();
        }
    }
}
