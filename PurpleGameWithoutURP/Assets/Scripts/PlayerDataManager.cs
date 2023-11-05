using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDataManager : MonoBehaviour
{
    public Action OnPlayerDataChaged;

    public Action OnDataLoaded;

    [SerializeField] private Text _coinText;
    [SerializeField] private Text _gemText;

    [SerializeField]
    private PlayerData _currentData;


    public PlayerData CurrentData
    {
        get
        {
            return _currentData;
        }

        set
        {
            _currentData = value;

            OnPlayerDataChaged?.Invoke();

        }
    }




    private void Start()
    {
        if (_coinText != null)
            OnPlayerDataChaged += ChangeText;

        SceneManager.sceneLoaded += OnSceneLoaded;

        //Application.OpenURL(AppManager.Instance.AppSettings.PrivacyPolicyURL);

        Load();
    }

    public void ChangeText()
    {
        _coinText.text = CurrentData.Coins.ToString();

        _gemText.text = CurrentData.Gems.ToString();
    }

    public void Load()
    {
        CurrentData = SaveManager.Load<PlayerData>(StaticFields.PLAYER_DATA);
        CurrentData.LastDate = PlayerPrefs.GetString(StaticFields.PLAYER_DATE);
        OnDataLoaded?.Invoke();

    }

    public void Save()
    {
        SaveManager.Save(StaticFields.PLAYER_DATA, CurrentData);
        PlayerPrefs.SetString(StaticFields.PLAYER_DATE, CurrentData.LastDate);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public bool TryChangeValue(ValueType type, int amount)
    {
        //bool CanTake = true;

        switch (type)
        {
            case ValueType.coin:
                {
                    if (amount + _currentData.Coins < 0)
                    {
                        return false;
                        
                    }
                    _currentData.Coins += amount;


                    break;
                }
            case ValueType.gem:
                {
                    if (amount + _currentData.Gems < 0)
                    {
                        return false;
                        
                    }
                    _currentData.Gems += amount;
                    break;
                }
            case ValueType.spin:
                {
                    if (amount + _currentData.Spin < 0)
                    {
                        return false;
                        
                    }
                    _currentData.Spin += amount;
                    break;
                }
        }

        OnPlayerDataChaged?.Invoke();

        return true;
    }

    public void SetDate(DateTime updateDate)
    {
        CurrentData.LastDate = updateDate.ToString();
    }

    public DateTime GetDate()
    {
        DateTime transfer;
        try
        {
            transfer = DateTime.Parse(CurrentData.LastDate);
        }
        catch
        {
            SetDate(DateTime.MinValue);
            transfer = DateTime.Parse(CurrentData.LastDate);
        }

        return transfer;
    }

    public void MakeMinData()
    {
        Debug.Log(DateTime.MinValue.ToString());

        SetDate(DateTime.MinValue);
    }

    public void ShowDate()
    {
        Debug.Log(CurrentData.LastDate);
    }

    public List<bool> GetSkinList(SkinType type)
    {
        switch (type)
        {
            case SkinType.chip:
                {
                    return CurrentData.UnlockedChipSkin;
                }
            case SkinType.background:
                {
                    return CurrentData.UnlockedBackgroundSkin;
                }

        }

        return null;
    }

    public int GetSkinCurrent(SkinType type, int index)
    {
        switch (type)
        {
            case SkinType.chip:
                {
                    if (index >= 0)
                    {
                        CurrentData.CurrentChipSkin = index;
                    }

                    return CurrentData.CurrentChipSkin;

                }
            case SkinType.background:
                {
                    if (index >= 0)
                    {
                        CurrentData.CurrentBackgroundSkin = index;
                    }

                    return CurrentData.CurrentBackgroundSkin;
                }

        }

        return 0;
    }


}

public enum ValueType
{
    coin,
    gem,
    spin,
    date
}

public enum SkinType
{
    chip,
    background
}



[Serializable]
public class PlayerData
{
    public bool IsAcceptPrivacyPolicy;

    public int Coins;

    public int Gems;

    public int Spin;

    public int VolumeMusic;

    public int VolumeClip;

    public List<bool> Artifacts = new List<bool>();

    public List<bool> Levels = new List<bool>();

    public string LastDate;

    public List<bool> UnlockedChipSkin = new List<bool>();

    public int CurrentChipSkin;

    public List<bool> UnlockedBackgroundSkin = new List<bool>();

    public List<bool> ActivatedTreasure = new List<bool>();

    public int CurrentBackgroundSkin;


    public PlayerData()
    {

        Coins = 0;
        Gems = 0;
        Spin = 0;
    }

    public struct MyDate
    {

    }

}
