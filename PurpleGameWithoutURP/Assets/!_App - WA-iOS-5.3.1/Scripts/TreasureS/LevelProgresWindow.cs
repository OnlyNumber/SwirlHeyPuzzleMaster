using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelProgresWindow : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    [SerializeField]
    private PlayerDataManager _player;

    [SerializeField]
    private ActivatePanel _levels;

    [SerializeField]
    private TMP_Text _preogressRewardText;

    public void ClaimReward(Treasure thisTreasure)
    {
        _preogressRewardText.text = "x" + thisTreasure.gems.ToString();

        _button.onClick.RemoveAllListeners();

        _button.onClick.AddListener(() => _player.TryChangeValue(ValueType.gem, thisTreasure.gems));

        _button.onClick.AddListener(() => GetComponent<ActivatePanel>().ActivateNextScene(_levels.gameObject));

        _button.onClick.AddListener(() => thisTreasure.Recicle());

    }


}
