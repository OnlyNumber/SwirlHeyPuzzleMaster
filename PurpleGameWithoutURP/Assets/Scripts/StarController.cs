using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    [SerializeField]
    private List<SwitchButton> _starsList;

    public void SetStars(int starsCount)
    {
        if(starsCount > _starsList.Count || starsCount <= 0)
        {
            return;
        }

        for (int i = 0; i < starsCount; i++)
        {
            _starsList[i].SwitchSprite(true);
        }
    }

}
