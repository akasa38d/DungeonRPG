using UnityEngine;

public class SubAttackButton : AbstractButton {

	public new void Start()
	{
		this.defaultColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		this.onMouseEnterColor = new Color (1.0f, 0.1f, 0.1f, 0.5f);
		this.onMouseUpAsButtonCollor = new Color(0.2f, 0.1f, 0.1f, 1.0f);
		getColor (defaultColor);
	}

	public new void OnMouseEnter(){}
	
	public new void OnMouseExit(){}
	
	public new void OnMouseDown(){}
	
	public new void OnMouseUpAsButton(){}
}
