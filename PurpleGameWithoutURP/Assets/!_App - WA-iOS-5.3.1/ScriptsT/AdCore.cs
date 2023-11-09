using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdCore : MonoBehaviour, IUnityAdsLoadListener,IUnityAdsShowListener, IUnityAdsInitializationListener
{
    [SerializeField] private bool _testMode = true;

    [SerializeField] private GameManager _gameManager;

    [SerializeField] private PlayerDataManager _player;

    [SerializeField] private Button _buttonX2;

    public const string GameId = "5471165";

    private string _rewardVideo = "Interstitial_Android";

    public void Start()
    {
        Advertisement.Initialize(GameId, _testMode,this);
    }

    public void ShowAdVideo(string placementId)
    {
        Advertisement.Show(placementId, this);
    }

    public void ShowRewardedVideo()
    {
        ShowAdVideo(_rewardVideo);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("OnUnityAdsAdLoaded");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("OnUnityAdsFailedToLoad");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("OnUnityAdsShowClick");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        _player.TryChangeValue(ValueType.coin, GlobalLevelInfo.LevelRewardCoin);
        _player.TryChangeValue(ValueType.gem, GlobalLevelInfo.LevelRewardGem);
        _buttonX2.enabled = false;
        _buttonX2.image.CrossFadeAlpha(0.5f, 0.1f, false);
        _gameManager.SetMoneyText((GlobalLevelInfo.LevelRewardGem * 2).ToString(), (GlobalLevelInfo.LevelRewardCoin * 2).ToString());
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("OnUnityAdsShowFailure");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("OnUnityAdsShowStart");
    }

    public void OnInitializationComplete()
    {
        Debug.Log("OnInitializationComplete");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
    }
}
