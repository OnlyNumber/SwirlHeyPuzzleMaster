using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip _clip;

    private void Start()
    {
        Button button = GetComponent<Button>();

        button.onClick.AddListener(() => SoundController.Instanse.PlayAudioClip(_clip));
    }
}
