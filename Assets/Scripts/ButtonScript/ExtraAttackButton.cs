using UnityEngine;
using System;
using System.Linq;

public class ExtraAttackButton : AbstractButton
{
	public Action<GameObject> attack;

	public override void Start() {
		base.Start();
	}
	
	public override void OnMouseEnter() {
		this.GetComponent<Collider>().GetComponent<Renderer>().material.color = onMouseEnterColor;
		foreach (var n in GameObject.FindGameObjectsWithTag ("Button").Where((a) => checkDistance(a)))
		{
			var a = n.GetComponent<AbstractButton>();
			a.getColor(a.onMouseEnterColor);
		}
	}
	
	public override void OnMouseExit() {
		this.GetComponent<Collider>().GetComponent<Renderer>().material.color = defaultColor;
		foreach (var n in GameObject.FindGameObjectsWithTag ("Button").Where((a) => checkDistance(a)))
		{
			var a = n.GetComponent<AbstractButton>();
			a.getColor(a.defaultColor);
		}
	}
	
	public override void OnMouseDown() {
		this.GetComponent<Collider>().GetComponent<Renderer>().material.color = onMouseUpAsButtonCollor;
		foreach (var n in GameObject.FindGameObjectsWithTag ("Button").Where((a) => checkDistance(a)))
		{
			var a = n.GetComponent<AbstractButton>();
			a.getColor(a.onMouseUpAsButtonCollor);
		}
	}
	
	public override void OnMouseUpAsButton()
	{
		this.GetComponent<Collider>().GetComponent<Renderer>().material.color = defaultColor;
		foreach (var n in GameObject.FindGameObjectsWithTag ("Button").Where((a) => checkDistance(a)))
		{
			var a = n.GetComponent<AbstractButton>();
			a.getColor(a.defaultColor);
		}
		attack(square);
	}

	public bool checkDistance(GameObject obj)
	{
		if (Mathf.Abs (this.gameObject.transform.position.x - obj.transform.position.x) <= 10) {
			if (Mathf.Abs (this.gameObject.transform.position.z - obj.transform.position.z) <= 10) {
				return true;
			}
		}
		return false;
	}
}
