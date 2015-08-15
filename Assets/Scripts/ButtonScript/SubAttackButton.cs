using UnityEngine;
using System.Collections;

public class SubAttackButton : MonoBehaviour {

	public Color defaultColor;
	public Color onMouseEnterColor;
	public Color onMouseUpAsButtonCollor;
	
	public void Start()
	{
		this.defaultColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		this.onMouseEnterColor = new Color (1.0f, 0.1f, 0.1f, 0.5f);
		this.onMouseUpAsButtonCollor = new Color(0.2f, 0.1f, 0.1f, 1.0f);
		this.GetComponent<Collider>().GetComponent<Renderer>().material.color = defaultColor;
	}
}
