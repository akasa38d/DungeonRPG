using UnityEngine;
using System;
using System.Collections;

public class ExtraAttackButton : AbstractButton
{
	public Action<GameObject> attackDelegate;
	
	new void Start() { base.Start(); }
	
	new void OnMouseEnter() { base.OnMouseEnter(); }
	
	new void OnMouseExit() { base.OnMouseExit(); }
	
	new void OnMouseDown() { base.OnMouseDown(); }
	
	new void OnMouseUpAsButton()
	{
		base.OnMouseUpAsButton();
		attackDelegate(square.GetComponent<AbstractSquare>().character);
	}
}
