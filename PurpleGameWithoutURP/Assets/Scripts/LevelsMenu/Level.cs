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
    private Button _levelButton;

    [SerializeField]
    private StarController _starController;

    [SerializeField]
    private float _LevelTimer;

    private void Start()
    {
    }

    public void WriteGlobalData()
    {
        GlobalLevelInfo.LevelNumber = _levelNumber;

        GlobalLevelInfo.LevelRewardCoin = _coinReward;

        GlobalLevelInfo.LevelRewardGem = _gemReward;

        GlobalGameData.TimerCount = _LevelTimer;


    }

    public void UnlockLevel(int levelNumber, int starsCount)
    {
        _levelText.text = (levelNumber + 1).ToString();
        _levelNumber = levelNumber;
        _lockImage.SetActive(false);
        _levelButton.enabled = true;

        if (starsCount <= 0)
        {
            return;
        }
        _starController.gameObject.SetActive(true);
        _starController.SetStars(starsCount);

    }

    
}
