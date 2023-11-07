using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundController : MonoBehaviour
{
    public static SoundController Instanse;

    [SerializeField]
    private SwitchButton _buttonMusic;

    [SerializeField]
    private SwitchButton _buttonClip;

    [SerializeField]
    private PlayerDataManager _player;

    [SerializeField]
    private AudioSource _backgroundMusic;

    public Action<bool> OnClipSoundChange;

    public Action<bool> OnMusicSoundChange;

    private void Start()
    {

        if(Instanse == null)
        {
            Instanse = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _backgroundMusic.Play();
        
        Instanse.OnClipSoundChange += _buttonClip.SwitchSprite;

        Instanse.OnMusicSoundChange += _buttonMusic.SwitchSprite;

        _player.OnDataLoaded += Initialize;


    }

    public void Initialize()
    {
        OnClipSoundChange?.Invoke(FromIntToBool(_player.CurrentData.VolumeClip));
        OnMusicSoundChange?.Invoke(FromIntToBool(_player.CurrentData.VolumeMusic));
        _backgroundMusic.volume = _player.CurrentData.VolumeMusic;
    }

    public void PlayAudioClip(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Vector2.zero, _player.CurrentData.VolumeClip);
    }

    public void SetSound(int soundType)
    {
        switch((TypeSound)soundType)
        {
            case TypeSound.clip:
                {
                    SetMusicOpposite(ref _player.CurrentData.VolumeClip);
                    OnClipSoundChange?.Invoke(FromIntToBool(_player.CurrentData.VolumeClip));

                    break;
                }
            case TypeSound.music:
                {
                    SetMusicOpposite(ref _player.CurrentData.VolumeMusic);
                    _backgroundMusic.volume = _player.CurrentData.VolumeMusic;
                    OnMusicSoundChange?.Invoke(FromIntToBool(_player.CurrentData.VolumeMusic));
                    break;
                }
        }
    }
    
    public bool FromIntToBool(int i)
    {
        return i == 0 ? false : true;
    }


    public void SetMusicOpposite(ref int volume)
    {
        if (volume == 1)
        {
            volume = 0;
        }
        else
        {
            volume = 1;
        }

        //_defaultMusic.volume = IsMusic;

        //OnChangeVolume?.Invoke();
    }

}

public enum TypeSound
{
    clip,
    music
}