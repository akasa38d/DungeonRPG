using UnityEngine;
using System;
using System.Collections;

public class AttackButton : AbstractButton
{
    public override void OnMouseUpAsButton()
    {
        if (!isPointerOverGameObject())
        {
            try
            {
                effect(square);
            } finally
            {
                turnEnd();
            }
        }
    }
}
