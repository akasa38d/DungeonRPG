﻿using UnityEngine;
using System.Collections;

public class MoveButton : AbstractButton
{
    public delegate void MoveDelegate(GameObject obj);
    public MoveDelegate moveDelegate;

    new void Start()
    {
        base.Start();
    }

    new void OnMouseEnter()
    {
        base.OnMouseEnter();
    }

    new void OnMouseExit()
    {
        base.OnMouseExit();
    }

    new void OnMouseDown()
    {
        base.OnMouseDown();
    }

    new void OnMouseUpAsButton()
    {
        base.OnMouseUpAsButton();
        moveDelegate(square);
    }



}
