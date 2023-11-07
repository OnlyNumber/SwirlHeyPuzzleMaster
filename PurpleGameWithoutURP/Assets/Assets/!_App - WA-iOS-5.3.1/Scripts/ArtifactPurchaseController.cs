using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactPurchaseController : MonoBehaviour
{
    public static ArtifactPurchaseController Instance;

    [SerializeField]
    private PlayerDataManager _playerDataManager;

    [SerializeField]
    private Button _buyArtifact;

    [SerializeField]
    private List<SwitchButton> _artifacts;

    [SerializeField]
    private List<Image> _getArtifactsImage;

    [SerializeField]
    private ActivatePanel _shopPanel;

    [SerializeField]
    private ActivatePanel _getArtifactsPanel;

    [SerializeField]
    private ActivatePanel _watchArtifactsPanel;

    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _playerDataManager.OnDataLoaded += Initialize;

        //AppManager.Instance.AppSettings.PrivacyPolicyURL;
    }

    private void Initialize()
    {
        if (_playerDataManager.CurrentData.Artifacts.Count != _artifacts.Count)
        {
            for (int i = 0; i < _artifacts.Count - _playerDataManager.CurrentData.Artifacts.Count; i++)
            {
                _playerDataManager.CurrentData.Artifacts.Add(false);
            }
        }

        for (int i = 0; i < _playerDataManager.CurrentData.Artifacts.Count; i++)
        {
            if (_playerDataManager.CurrentData.Artifacts[i])
            {
                _artifacts[i].SwitchSprite(true);
            }
        }

        _buyArtifact.enabled = !CheckArtifactsCount();

    }

    public void ActivateArtifact()
    {
        int index;

        do
        {
            index = Random.Range(0, _playerDataManager.CurrentData.Artifacts.Count);

        } while (_playerDataManager.CurrentData.Artifacts[index] == true);

        foreach (var item in _getArtifactsImage)
        {
            item.gameObject.SetActive(false);
        }

        _getArtifactsImage[index].gameObject.SetActive(true);

        _shopPanel.ActivateNextScene(_getArtifactsPanel.gameObject);

        _playerDataManager.CurrentData.Artifacts[index] = true;

        _artifacts[index].SwitchSprite(true);

        _buyArtifact.enabled = !CheckArtifactsCount();


    }


    //True if all artifacts get and false if not
    public bool CheckArtifactsCount()
    {
        for (int i = 0; i < _playerDataManager.CurrentData.Artifacts.Count; i++)
        {
            if (_playerDataManager.CurrentData.Artifacts[i] == false)
            {
                return false;
            }
        }

        return true;

    }



}
