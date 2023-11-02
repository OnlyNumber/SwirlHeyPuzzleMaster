using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShuffleChips : MonoBehaviour
{
    public void Shuffle() // For Shuffle Button
    {
        GlobalData.currentScore = 0;
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                GlobalData.boardArray[row, col] = 0;
            }
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        GetComponentInChildren<Text>().text = "SHUAFFLE";
    }
}
