using UnityEngine;
using System.Collections;

public class AttackButton : AbstractButton
{
	public delegate void AttackEvent2(GameObject obj);
	public AttackEvent2 attackDelegate2;   



    public AbstractCharacterObject.AttackWay attackWay;

    new void Start() { base.Start(); }

    new void OnMouseEnter() { base.OnMouseEnter(); }

    new void OnMouseExit() { base.OnMouseExit(); }

    new void OnMouseDown() { base.OnMouseDown(); }

    new void OnMouseUpAsButton()
    {
        base.OnMouseUpAsButton();
        attackDelegate2(square.GetComponent<AbstractSquare>().character);
    }
}
