using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureShop : TreasureParent
{
    [SerializeField]
    protected Text TreasureText; 

    public override void TryActivateTreasure(int level)
    {
        if(NeedLevel > level)
        {
            TreasureButton.enabled = true;
            TreasureText.enabled = false;

        }
    }

    public override void DisableButton()
    {

    }

    public override void Work()
    {
    }
}
