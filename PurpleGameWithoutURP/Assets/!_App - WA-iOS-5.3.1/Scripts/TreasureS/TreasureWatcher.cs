using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureWatcher : MonoBehaviour
{
    [SerializeField]
    private LevelController _levelController;

    [SerializeField]
    private List<TreasureParent> _treasures;

    [SerializeField]
    private PlayerDataManager _playerDataManager;

    [SerializeField]
    private LevelProgresWindow _levelProgresWindow;

    [SerializeField]
    private ActivatePanel _levelPanel;

    [SerializeField]
    private ActivatePanel _levelProgressPopup;

    private void Start()
    {
        _playerDataManager.OnDataLoaded += Initialize;
    }

    public void Initialize()
    {
        if (_playerDataManager.CurrentData.Levels.Count != _treasures.Count)
        {
            for (int i = 0; i < _treasures.Count - _playerDataManager.CurrentData.ActivatedTreasure.Count; i++)
            {
                _playerDataManager.CurrentData.ActivatedTreasure.Add(false);
            }
        }


        int lastLevel = LatestLevel();

        for (int i = 0; i < _playerDataManager.CurrentData.ActivatedTreasure.Count; i++)
        {
            if (_playerDataManager.CurrentData.ActivatedTreasure[i] == false)

            {
                _treasures[i].Intialize(_levelProgresWindow, _playerDataManager, _levelPanel,_levelProgressPopup,this);
                _treasures[i].TryActivateTreasure(lastLevel);
            }
            else
            {
                _treasures[i].DisableButton();
            }
        }

        

    }

    public int LatestLevel()
    {
        int index = 0;

        for (int i = 0; i < _playerDataManager.CurrentData.Levels.Count; i++)
        {
            if (_playerDataManager.CurrentData.Levels[i] < 0)
            {
                index = i;
                break;
            }

        }

        return index;

    }

    public void RemoveFromList(TreasureParent treasure)
    {
        int index = _treasures.IndexOf(treasure);

        _playerDataManager.CurrentData.ActivatedTreasure[index] = true;
        _treasures[index].DisableButton();

    }


}
