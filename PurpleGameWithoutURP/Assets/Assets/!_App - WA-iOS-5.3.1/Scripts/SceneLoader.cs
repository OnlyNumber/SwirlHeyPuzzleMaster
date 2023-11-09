using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(StaticFields.MAIN_SCENE);
    }

    public void GoToGameMenu()
    {
        

        SceneManager.LoadScene(StaticFields.GAME_SCENE);

    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
