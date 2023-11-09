using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePanel : MonoBehaviour
{
    public void ActivateNextScene(GameObject nextScene)
    {


        gameObject.SetActive(false);

        nextScene.SetActive(true);
    }
}
