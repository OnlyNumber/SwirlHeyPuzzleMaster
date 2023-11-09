using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivacyPolicyController : MonoBehaviour
{
    [SerializeField]
    private PlayerDataManager _playerDataManager;

    [SerializeField]
    private ActivatePanel _privacyPolicyPanel;

    [SerializeField]
    private ActivatePanel _mainMenuPanel;

    private void Start()
    {
        _playerDataManager.OnDataLoaded += Initialize;

    }

    public void Initialize()
    {
        if (_playerDataManager.CurrentData.IsAcceptPrivacyPolicy)
        {
            _privacyPolicyPanel.ActivateNextScene(_mainMenuPanel.gameObject);
        }
    }

    public void CheckPrivacyPolicy()
    {
        Application.OpenURL(AppManager.Instance.AppSettings.PrivacyPolicyURL);
    }

    public void AcceptPrivacyPolicy()
    {
        _playerDataManager.CurrentData.IsAcceptPrivacyPolicy = true;
    }


}
