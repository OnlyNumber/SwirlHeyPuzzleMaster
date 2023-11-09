using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureShop : TreasureParent
{
    [SerializeField]
    protected TMP_Text TreasureText; 

    public override void DisableButton()
    {

    }

    public override void Work()
    {
    }

    public override void TryActivateTreasure(int level)
    {
        TreasureText.text = "Lv " + NeedLevel;

        if (level >= this.NeedLevel)
        {
            TreasureButton.enabled = true;
            TreasureText.enabled = false;
        }
    }

}
