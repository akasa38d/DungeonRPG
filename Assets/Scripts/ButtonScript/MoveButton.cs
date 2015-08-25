using UnityEngine;
using System;
using System.Collections;

public class MoveButton : AbstractButton
{
    public Action<GameObject> moveDelegate;

    new void OnMouseEnter() { base.OnMouseEnter(); }

    new void OnMouseExit() { base.OnMouseExit(); }

    new void OnMouseDown() { base.OnMouseDown(); }

    new void OnMouseUpAsButton()
    {
        base.OnMouseUpAsButton();
        moveDelegate(square);
    }
}
