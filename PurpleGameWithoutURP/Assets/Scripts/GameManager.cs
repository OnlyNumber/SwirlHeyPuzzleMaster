using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private CreateBoard _board;

    [SerializeField]
    private List<Sprite> _backgroundList;

    [SerializeField]
    private Image _background;

    [SerializeField]
    private PlayerDataManager _player;

    [SerializeField]
    private ActivatePanel _gamePanel;

    [SerializeField]
    private GameObject _winPanel;

    [SerializeField]
    private Text _gemText;

    [SerializeField]
    private Text _coinText;

    private void Start()
    {
        _player.OnDataLoaded += WhenDataLoaded;
    }

    public void WhenDataLoaded()
    {
        if (!_player.CurrentData.Levels[GlobalLevelInfo.LevelNumber])
        {
            _gemText.text = GlobalLevelInfo.LevelRewardGem.ToString();
            _coinText.text = GlobalLevelInfo.LevelRewardCoin.ToString();

        }
        else
        {
            _gemText.text = "0";
            _coinText.text = "0";
        }

        _background.sprite = _backgroundList[_player.CurrentData.CurrentBackgroundSkin];
        _board.InitializeBoard(_player.CurrentData.CurrentChipSkin);
        _board.OnWinGame += WinMethod;



    }


    private void WinMethod()
    {
        _board.SetChipsCollidersActivity(false);

        if (!_player.CurrentData.Levels[GlobalLevelInfo.LevelNumber])
        {
            _player.TryChangeValue(ValueType.coin, GlobalLevelInfo.LevelRewardCoin);
            _player.TryChangeValue(ValueType.gem, GlobalLevelInfo.LevelRewardGem);
        }
        
        _player.CurrentData.Levels[GlobalLevelInfo.LevelNumber] = true;

        _gamePanel.ActivateNextScene(_winPanel.gameObject);


    }
}
