using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour
{
    [SerializeField]
    private PlayerDataManager _player;

    [SerializeField]
    private List<SkinButton> _skinList;

    [SerializeField]
    private SkinType _skinType; 


    private void Start()
    {
        _player.OnDataLoaded += Initialize;
    }

    public void Initialize()
    {

        Debug.Log(gameObject.name+" "+_skinType);

        if (_player.GetSkinList(_skinType).Count != _skinList.Count)
        {
            for (int i = 0; i <= _skinList.Count - _player.GetSkinList(_skinType).Count; i++)
            {
                _player.GetSkinList(_skinType).Add(false);
            }
        }

        Debug.Log(gameObject.name + " " + _skinType);

        _player.GetSkinList(_skinType)[0] = true;

        _skinList[0].Unlock();

        for (int i = 0; i < _player.GetSkinList(_skinType).Count; i++)
        {
            _skinList[i].Initialize(i);
            _skinList[i].OnBuyAndEquip += BuyAndEquip;
            
            if (_player.GetSkinList(_skinType)[i] == true)
            {
            
                _skinList[i].Unlock();

            }


        }

        _skinList[_player.GetSkinCurrent(_skinType, -1)].Equip(true);

    }

    public void BuyAndEquip(int number, ValueType type, int cost)
    {
            Debug.Log("_player.GetSkinList");

        if (_player.GetSkinList(_skinType)[number] == false)
        {

            if (!_player.TryChangeValue(ValueType.coin, -cost))
            {
                Debug.Log("TryChangeValue");

                return;
            }
            else
            {
                _player.GetSkinList(_skinType)[number] = true;
                _skinList[number].Unlock();
            }
        }

        _skinList[_player.GetSkinCurrent(_skinType, -1)].Equip(false);

        _skinList[number].Equip(true);

        _player.GetSkinCurrent(_skinType, number);

    }



}
