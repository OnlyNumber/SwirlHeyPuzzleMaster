using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private ActivatePanel _losePanel;

    [SerializeField]
    private StarController _starController;

    [SerializeField]
    private TMP_Text _gemText;

    [SerializeField]
    private TMP_Text _coinText;

    [SerializeField]
    private int _similarOn1Star;

    [SerializeField]
    private int _similarOn2Star;

    [SerializeField]
    private int _similarOn3Star;

    [SerializeField]
    private Button _buttonX2;


    private void Start()
    {
        _player.OnDataLoaded += WhenDataLoaded;
    }

    public void WhenDataLoaded()
    {


        _background.sprite = _backgroundList[_player.CurrentData.CurrentBackgroundSkin];
        _board.InitializeBoard(_player.CurrentData.CurrentChipSkin);
        _board.OnEndGame += EndGame;



    }

    private int SetStars(int countSimilars)
    {
        if (countSimilars >= _similarOn3Star)
        {
            _starController.SetStars(3);
            return 3;
        }
        else if (countSimilars >= _similarOn2Star)
        {
            _starController.SetStars(2);
            return 2;
        }
        else if (countSimilars >= _similarOn1Star)
        {
            _starController.SetStars(1);
            return 1;
        }
        else
        {
            return 0;
        }

    }

    private void EndGame()
    {
        if (_board.CountSimilars < _similarOn1Star)
        {
            _gamePanel.ActivateNextScene(_losePanel.gameObject);

            return;

        }



        if (_player.CurrentData.Levels[GlobalLevelInfo.LevelNumber] > 0)
        {
            GlobalLevelInfo.LevelRewardGem = 0;
            GlobalLevelInfo.LevelRewardCoin = 0;
            _buttonX2.enabled = false;
            _buttonX2.image.CrossFadeAlpha(0.5f, 0.1f, false);
        }

        SetMoneyText(GlobalLevelInfo.LevelRewardGem.ToString(), GlobalLevelInfo.LevelRewardCoin.ToString());
        _player.TryChangeValue(ValueType.coin, GlobalLevelInfo.LevelRewardCoin);
        _player.TryChangeValue(ValueType.gem, GlobalLevelInfo.LevelRewardGem);


        if (_player.CurrentData.Levels[GlobalLevelInfo.LevelNumber] < SetStars(_board.CountSimilars))
        {
            _player.CurrentData.Levels[GlobalLevelInfo.LevelNumber] = SetStars(_board.CountSimilars);
        }

        _gamePanel.ActivateNextScene(_winPanel.gameObject);
    }

    public void ContinueLevel()
    {

        if (_player.TryChangeValue(ValueType.gem, -500))
        {
            _board.AddTime(60);
            _board.IsGame = true;
            _board.SetChipsCollidersActivity(true);
            _losePanel.ActivateNextScene(_gamePanel.gameObject);
        }

    }

    public void SetMoneyText(string gemText, string coinText)
    {
        _gemText.text = gemText;
        _coinText.text = coinText;
    }


}
