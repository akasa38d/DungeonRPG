using UnityEngine;
using System.Collections;

public class CureButton : AbstractButton
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
