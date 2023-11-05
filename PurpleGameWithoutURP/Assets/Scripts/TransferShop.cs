using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferShop : MonoBehaviour
{
    [SerializeField]
    private PlayerDataManager _playerDataManager;

    [SerializeField]
    private GameObject _messagePanel;

    [SerializeField]
    private GameObject _messageSuccessText;

    [SerializeField]
    private GameObject _messageFailText;

    public void TryBuySomeCoins(TransferSO transfer)
    {
        if(_playerDataManager.TryChangeValue(transfer.FromValue, -transfer.FromValueAmount))
        {
            _playerDataManager.TryChangeValue(transfer.ToValue, transfer.ToValueAmount);
            IsSuccessMessage(true);
        }
        else
        {
            IsSuccessMessage(false);
        }    
    }

    public void IsSuccessMessage(bool isSucces)
    {
        _messagePanel.SetActive(true);
        _messageSuccessText.SetActive(isSucces);
        _messageFailText.SetActive(!isSucces);
    }
}