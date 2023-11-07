using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Treasure : TreasureParent
{
    [SerializeField]
    public int gems;

    

    public override void DisableButton()
    {
        gameObject.SetActive(false);
    }

    public override void Work()
    {
        LevelPanel.ActivateNextScene(LevelProgressPopup.gameObject);

        LevelProgresWindow.ClaimReward(this);

    }
    public override void TryActivateTreasure(int level)
    {
        if (level >= this.NeedLevel)
            TreasureButton.enabled = true;
    }

}
