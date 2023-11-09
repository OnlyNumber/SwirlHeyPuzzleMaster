using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScrip : MonoBehaviour
{
    [SerializeField]
    private PlayerDataManager _player;

    [SerializeField]
    private ActivatePanel _tutorialPanel;

    [SerializeField]
    private ActivatePanel _mainPanel;
    private void Start()
    {
        _player.OnDataLoaded += Initialize;
    }

    public void Initialize()
    {
        if (_player.CurrentData.IsFirstLog)
        {
            _tutorialPanel.ActivateNextScene(_mainPanel.gameObject);
            
        }
        else
        {
            _player.CurrentData.IsFirstLog = true;
        }
    }


}
