using UnityEngine;
using UnityEngine.EventSystems;
using System;

public abstract class AbstractButton : MonoBehaviour
{
    public Action<GameObject> effect;
    public Action turnEnd;

    //連動するマス
    public GameObject square;
    public Color defaultColor;
    public Color onMouseEnterColor;
    public Color onMouseUpAsButtonCollor;

    public virtual void Start()
    {
        this.defaultColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        this.onMouseEnterColor = new Color(1.0f, 0.1f, 0.1f, 0.5f);
        this.onMouseUpAsButtonCollor = new Color(0.2f, 0.1f, 0.1f, 1.0f);
        getColor(defaultColor);
    }

    public virtual void OnMouseEnter()
    {
        if (!isPointerOverGameObject())
        {
            getColor(onMouseEnterColor);
        }
    }

    public virtual void OnMouseExit()
    {
        if (!isPointerOverGameObject())
        {
            getColor(defaultColor);
        }
    }

    public virtual void OnMouseDown()
    {
        if (!isPointerOverGameObject())
        {
            getColor(onMouseUpAsButtonCollor);
        }
    }

    public virtual void OnMouseUpAsButton()
    {
        if (!isPointerOverGameObject())
        {
            getColor(defaultColor);
        }
    }

    public virtual void getColor(Color color)
    {
        GetComponent<Collider>().GetComponent<Renderer>().material.color = color;
    }

    //uGUIとの競合を防ぐ
    protected bool isPointerOverGameObject()
    {
        EventSystem current = EventSystem.current;
        if (current != null)
        {
            if (current.IsPointerOverGameObject())
            {
                return true;
            }
        }
        return false;
    }
}
