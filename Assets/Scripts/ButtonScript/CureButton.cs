using UnityEngine;
using System.Collections;

public class CureButton : AbstractButton {
    new void OnMouseEnter() { base.OnMouseEnter(); }
    
    new void OnMouseExit() { base.OnMouseExit(); }
    
    new void OnMouseDown() { base.OnMouseDown(); }
    
    new void OnMouseUpAsButton()
    {
        base.OnMouseUpAsButton();
        try
        {
            effect(square);
        }
        finally
        {
            turnEnd();
        }
    }
}
