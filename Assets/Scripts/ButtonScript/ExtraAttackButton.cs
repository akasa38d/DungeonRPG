using UnityEngine;
using System;
using System.Linq;
using MyUtility;

public class ExtraAttackButton : AbstractButton
{
    public override void OnMouseEnter()
    {
        this.GetComponent<Collider>().GetComponent<Renderer>().material.color = onMouseEnterColor;
        foreach (var n in GameObject.FindGameObjectsWithTag("Button").Where((a) => this.gameObject.checkDistanceCE(a, 1)))
        {
            var a = n.GetComponent<AbstractButton>();
            a.getColor(a.onMouseEnterColor);
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
        this.GetComponent<Collider>().GetComponent<Renderer>().material.color = onMouseUpAsButtonCollor;
        foreach (var n in GameObject.FindGameObjectsWithTag("Button").Where((a) => this.gameObject.checkDistanceCE(a, 1)))
        {
            var a = n.GetComponent<AbstractButton>();
            a.getColor(a.onMouseUpAsButtonCollor);
        }
    }

    public override void OnMouseUpAsButton()
    {
        try
        {
            this.GetComponent<Collider>().GetComponent<Renderer>().material.color = defaultColor;
            
            foreach (var n in GameObject.FindGameObjectsWithTag("Button").Where((a) => this.gameObject.checkDistanceCE(a, 1)))
            {
                var a = n.GetComponent<AbstractButton>();
                a.getColor(a.defaultColor);
                a.effect(a.square);
            }
        } finally
        {
            turnEnd();
        }
    }
}
