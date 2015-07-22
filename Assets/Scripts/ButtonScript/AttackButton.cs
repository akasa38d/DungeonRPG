using UnityEngine;
using System.Collections;

public class AttackButton : AbstractButton
{
	public delegate void AttackDelegate(GameObject obj);
	public AttackDelegate attackDelegate;
	
	new void Start()
	{
		base.Start();
	}
	
	new void OnMouseEnter()
	{
		base.OnMouseEnter();
	}
	
	new void OnMouseExit()
	{
		base.OnMouseExit();
	}
	
	new void OnMouseDown()
	{
		base.OnMouseDown();
	}
	
	new void OnMouseUpAsButton()
	{
		base.OnMouseUpAsButton();
		attackDelegate(square.GetComponent<AbstractSquare>().character);
	}

}
