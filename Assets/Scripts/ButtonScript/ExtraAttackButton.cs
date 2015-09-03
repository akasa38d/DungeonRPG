using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using MyUtility;

public class ExtraAttackButton : AbstractButton
{
    public override void OnMouseEnter()
    {
        if (!isPointerOverGameObject())
        {
            this.GetComponent<Collider>().GetComponent<Renderer>().material.color = onMouseEnterColor;
            foreach (var n in GameObject.FindGameObjectsWithTag("Button").Where((a) => this.gameObject.checkDistanceCE(a, 1)))
            {
                var a = n.GetComponent<AbstractButton>();
                a.getColor(a.onMouseEnterColor);
            }
        }
    }

    public override void OnMouseExit()
    {
        this.GetComponent<Collider>().GetComponent<Renderer>().material.color = defaultColor;
        foreach (var n in GameObject.FindGameObjectsWithTag("Button").Where((a) => this.gameObject.checkDistanceCE(a, 1)))
        {
            var a = n.GetComponent<AbstractButton>();
            a.getColor(a.defaultColor);
        }
    }

    public override void OnMouseDown()
    {
        if (!isPointerOverGameObject())
        {
            this.GetComponent<Collider>().GetComponent<Renderer>().material.color = onMouseUpAsButtonCollor;
            foreach (var n in GameObject.FindGameObjectsWithTag("Button").Where((a) => this.gameObject.checkDistanceCE(a, 1)))
            {
                var a = n.GetComponent<AbstractButton>();
                a.getColor(a.onMouseUpAsButtonCollor);
            }
        }
    }

    public override void OnMouseUpAsButton()
    {
        if (!isPointerOverGameObject())
        {
            StartCoroutine("test");
        }
    }

    IEnumerator test()
    {
        this.GetComponent<Collider>().GetComponent<Renderer>().material.color = defaultColor;

        yield return null;
        foreach (var n in GameObject.FindGameObjectsWithTag("Button").Where((a) => this.gameObject.checkDistanceCE(a, 1)))
        {
            var a = n.GetComponent<AbstractButton>();
            a.getColor(a.defaultColor);
            a.effect(a.square);
            yield return null;
        }
        turnEnd();
        turnEnd = () => { };
        yield return null;
    }
}
