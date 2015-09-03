using UnityEngine;
using System;
using System.Collections;

public class MoveButton : AbstractButton
{
    public override void OnMouseUpAsButton()
    {
        if (!isPointerOverGameObject())
        {
            effect(square);
        }
    }
}
