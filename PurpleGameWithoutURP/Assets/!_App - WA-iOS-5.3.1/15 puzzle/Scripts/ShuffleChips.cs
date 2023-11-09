using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShuffleChips : MonoBehaviour
{
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Shuffle() // For Shuffle Button
    {
        GlobalGameData.currentScore = 0;
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                GlobalGameData.boardArray[row, col] = 0;
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Shuffle();


    }

}
