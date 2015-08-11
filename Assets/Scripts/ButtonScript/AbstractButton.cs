using UnityEngine;
using System.Collections;

public abstract class AbstractButton : MonoBehaviour
{
    public GameObject square;
    public delegate void SomeEvent(GameObject obj);

    public void Start()
    {
        this.GetComponent<Collider>().GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    }

    public void OnMouseEnter()
    {
        this.GetComponent<Collider>().GetComponent<Renderer>().material.color = new Color(1.0f, 0.1f, 0.1f, 0.5f);
    }

    public void OnMouseExit()
    {
        this.GetComponent<Collider>().GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    }

    public void OnMouseDown()
    {
        this.GetComponent<Collider>().GetComponent<Renderer>().material.color = new Color(0.2f, 0.1f, 0.1f, 1.0f);
    }

    public void OnMouseUpAsButton()
    {
        this.GetComponent<Collider>().GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    }
}
