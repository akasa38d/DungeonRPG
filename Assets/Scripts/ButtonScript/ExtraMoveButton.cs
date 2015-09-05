using UnityEngine;
using System.Collections;

public class ExtraMoveButton : AbstractButton {

    public GameObject subAttackButton;

    public override void OnMouseEnter()
    {
        if (!isPointerOverGameObject())
        {
            this.GetComponent<Collider>().GetComponent<Renderer>().material.color = onMouseEnterColor;

            if(subAttackButton != null)
            {
                var a = subAttackButton.GetComponent<AbstractButton>();
                a.getColor(a.onMouseEnterColor);
            }
        }
    }
    
    public override void OnMouseExit()
    {
        this.GetComponent<Collider>().GetComponent<Renderer>().material.color = defaultColor;

        if(subAttackButton != null)
        {
            var a = subAttackButton.GetComponent<AbstractButton>();
            a.getColor(a.defaultColor);
        }
    }
    
    public override void OnMouseDown()
    {
        if (!isPointerOverGameObject())
        {
            this.GetComponent<Collider>().GetComponent<Renderer>().material.color = onMouseUpAsButtonCollor;

            if(subAttackButton != null)
            {
                var a = subAttackButton.GetComponent<AbstractButton>();
                a.getColor(a.onMouseUpAsButtonCollor);
            }
        }
    }

    public override void OnMouseUpAsButton()
    {
        if (!isPointerOverGameObject())
        {
            try
            {
                effect(square);
                if(subAttackButton != null)
                {
                    var target = subAttackButton.GetComponent<SubAttackButton>().square;
                    subAttackButton.GetComponent<SubAttackButton>().getColor(defaultColor);
                    subAttackButton.GetComponent<SubAttackButton>().effect(target);
                }
            }
            finally
            {
                turnEnd();
            }
        }
    }
}
