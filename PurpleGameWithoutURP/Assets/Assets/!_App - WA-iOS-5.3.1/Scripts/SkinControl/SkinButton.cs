using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class SkinButton : MonoBehaviour
{


    [SerializeField]
    private int _skinNumber;

    [SerializeField]
    private GameObject _lockImage;

    [SerializeField]
    private GameObject _equipImage;

    [SerializeField]
    private GameObject _costGO;

    [SerializeField]
    private TMP_Text _costText;

    [SerializeField]
    private Button _buyButton;

    public Action<int, ValueType, int> OnBuyAndEquip;

    [SerializeField]
    private int _cost;

    [SerializeField]
    private ValueType _type;

    private void Start()
    {
        _costText.text = _cost.ToString();
    }

    public void BuyAndEquip()
    {
        OnBuyAndEquip?.Invoke(_skinNumber, _type, _cost);
    }

    public void Initialize(int number)
    {
        _skinNumber = number;
    }

    public void Unlock()
    {
        _lockImage.SetActive(false);
        _costGO.SetActive(false);

    }

    public void Equip(bool equipState)
    {
        _equipImage.SetActive(equipState);
    }



}
