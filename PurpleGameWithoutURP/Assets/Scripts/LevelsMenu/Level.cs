using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField]
    private int _levelNumber;

    [SerializeField]
    private Text _levelText;

    [SerializeField]
    private int _coinReward;

    [SerializeField]
    private int _gemReward;

    [SerializeField]
    private GameObject _lockImage;

    [SerializeField]
    private GameObject _starsWin;

    [SerializeField]
    private GameObject _starsLose;

    [SerializeField]
    private Button _levelButton;

    private void Start()
    {
    }

    public void WriteGlobalData()
    {
        GlobalLevelInfo.LevelNumber = _levelNumber;

        GlobalLevelInfo.LevelRewardCoin = _coinReward;

        GlobalLevelInfo.LevelRewardGem = _gemReward;

    }

    public void UnlockLevel(int levelNumber)
    {
        _levelText.text = (levelNumber + 1).ToString();
        _levelNumber = levelNumber;
        _lockImage.SetActive(false);
        _levelButton.enabled = true;
    }

    public void WinnedLevel()
    {

        _starsWin.SetActive(true);

    }

    public void LoseLevel()
    {
        _starsLose.SetActive(true);
    }


}
