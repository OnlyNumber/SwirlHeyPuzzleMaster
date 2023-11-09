using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{

    [SerializeField]
    private Image _image;

    [SerializeField]
    private Sprite _buttonSpriteOn;

    [SerializeField]
    private Sprite _buttonSpriteOff;

    public void SwitchSprite(bool sprite)
    {
        if(sprite)
        {
            _image.sprite = _buttonSpriteOn;
        }
        else
        {
            _image.sprite = _buttonSpriteOff;
        }
    }


}
