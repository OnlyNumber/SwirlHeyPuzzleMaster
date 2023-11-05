using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private PlayerDataManager _playerDataManager;

    [SerializeField]
    private List<Level> _myLevels;

    private void Start()
    {
        _playerDataManager.OnDataLoaded += Initialize;
    }

    public void Initialize()
    {
        if (_playerDataManager.CurrentData.Levels.Count != _myLevels.Count)
        {
            for (int i = 0; i < _myLevels.Count - _playerDataManager.CurrentData.Levels.Count; i++)
            {
                _playerDataManager.CurrentData.Levels.Add(false);
            }



        }


        //_playerDataManager.CurrentData.Levels[0] = true;


        for (int i = 0; i < _playerDataManager.CurrentData.Levels.Count; i++)
        {

            _myLevels[i].UnlockLevel(i);

            if (_playerDataManager.CurrentData.Levels[i] == false)
            {
                _myLevels[i].LoseLevel();
                break;
            }
            else
            {
                _myLevels[i].WinnedLevel();
            }


        }
    }
}
