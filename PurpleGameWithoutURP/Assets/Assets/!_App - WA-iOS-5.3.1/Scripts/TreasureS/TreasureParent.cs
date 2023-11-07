using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TreasureParent : MonoBehaviour
{
    [SerializeField]
    protected Button TreasureButton;

    [SerializeField]
    protected int NeedLevel;

    protected LevelProgresWindow LevelProgresWindow;

    protected TreasureWatcher TreasureWatcher; 

    protected PlayerDataManager data;

    protected ActivatePanel LevelPanel;

    protected ActivatePanel LevelProgressPopup;

    private void Start()
    {
        TreasureButton.onClick.AddListener(Work);
    }

    public void Intialize(LevelProgresWindow window, PlayerDataManager data, ActivatePanel LvlPanel, ActivatePanel PopupPanel, TreasureWatcher TW)
    {
        TreasureWatcher = TW;

        LevelPanel = LvlPanel;

        LevelProgressPopup = PopupPanel;

        LevelProgresWindow = window;

        this.data = data;

    }

    public void Recicle()
    {
        TreasureWatcher.RemoveFromList(this);
    }

    public abstract void Work();

    public abstract void TryActivateTreasure(int level);

    public abstract void DisableButton();
}
