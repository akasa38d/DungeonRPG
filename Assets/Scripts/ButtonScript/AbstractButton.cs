using UnityEngine;
using System.Collections;

public abstract class AbstractButton : MonoBehaviour
{
    public GameObject square;

	public Color defaultColor;
	public Color onMouseEnterColor;
	public Color onMouseUpAsButtonCollor;

    public void Start()
    {
		this.defaultColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
		this.onMouseEnterColor = new Color (1.0f, 0.1f, 0.1f, 0.5f);
		this.onMouseUpAsButtonCollor = new Color(0.2f, 0.1f, 0.1f, 1.0f);
        this.GetComponent<Collider>().GetComponent<Renderer>().material.color = defaultColor;
	}

    public void OnMouseEnter()
    {
		this.GetComponent<Collider>().GetComponent<Renderer>().material.color = onMouseEnterColor;
    }

    public void OnMouseExit()
    {
		this.GetComponent<Collider>().GetComponent<Renderer>().material.color = defaultColor;
    }

    public void OnMouseDown()
    {
		this.GetComponent<Collider>().GetComponent<Renderer>().material.color = onMouseUpAsButtonCollor;
    }

    public void OnMouseUpAsButton()
    {
        this.GetComponent<Collider>().GetComponent<Renderer>().material.color = defaultColor;
    }
}
