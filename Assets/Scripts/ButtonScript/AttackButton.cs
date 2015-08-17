using UnityEngine;
using System;
using System.Collections;

public class AttackButton : AbstractButton
{
    public Action<GameObject> attack;

    new void Start() { base.Start(); }

    new void OnMouseEnter() { base.OnMouseEnter(); }

    new void OnMouseExit() { base.OnMouseExit(); }

    new void OnMouseDown() { base.OnMouseDown(); }

    new void OnMouseUpAsButton()
    {
        base.OnMouseUpAsButton();
        attack(square.GetComponent<AbstractSquare>().character);
    }
}
