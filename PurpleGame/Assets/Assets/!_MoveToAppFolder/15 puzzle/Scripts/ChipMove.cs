using UnityEngine;
using UnityEngine.UI;
public class ChipMove : MonoBehaviour
{
    private int number_chip;
    private int row_position;
    private int col_position;
    private int speed;
    private GameObject ui_motion;
    public Text buttonText;
    public Text ScoreText;
    public Text best;
    private Vector3 empty_position = new (0, 0, 0);

    private GameObject ui_completed;

    private bool canMoveChip;
    void Start()
    {
        speed = 4;
        number_chip = int.Parse(gameObject.name);
        ui_motion = GameObject.Find("Motion");
        ui_completed = GameObject.Find("Completed");
        buttonText = GameObject.Find("ShuffleText").GetComponent<Text>();
        ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        best = GameObject.Find("Best").GetComponent<Text>();
    }
    void Update()
    {
        if (canMoveChip)
            MoveChipOnBoard();
        
    }

    void OnMouseDown()
    {
        if (!canMoveChip)
        {
            FindOnBoard();
            CalculateDirection();
        }
    }

    void PlaySound()
    {
        GetComponent<AudioSource>().Play();
    }
    void MotionCountPlus()
    {
        GlobalData.currentScore++;
        ui_motion.GetComponent<Text>().text = GlobalData.currentScore.ToString();
    }
    void MoveChipOnBoard()
    {
        if (transform.position != empty_position)
        {
            transform.position = Vector3.MoveTowards(transform.position, empty_position, speed * Time.deltaTime);
        }
        else
        {
            canMoveChip = false;
            MotionCountPlus();
            CheckOnComplete();
        }
    }
    void CheckOnComplete()
    {
        int count = 1;

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (GlobalData.boardArray[row, col] == count)
                {
                    Debug.Log(count);
                    ScoreText.text = count + "/15";

                }
                else
                {
                    return;
                }
                count++;
                if (count == 16)
                {
                    Debug.Log("YOU'VE WON");
                    ui_completed.GetComponent<Text>().enabled = true;
                    buttonText.text = "TRY AGAIN";
                    if (GlobalData.currentScore < PlayerPrefs.GetInt("best") || PlayerPrefs.GetInt("best") == 0)
                    {
                        PlayerPrefs.SetInt("best", GlobalData.currentScore);
                        best.text = PlayerPrefs.GetInt("best").ToString();
                    }

                }
            }
        }
    }
    void CalculateDirection()
    {
        try
        {
            if (GlobalData.boardArray[row_position + 1, col_position] == 0)
            {
                empty_position = new Vector3(transform.position.x + 1.5f, 0, transform.position.z);
                GlobalData.boardArray[row_position, col_position] = 0;
                GlobalData.boardArray[row_position + 1, col_position] = number_chip;
                PlaySound();
                canMoveChip = true;
            }
        }
        catch { }

        try
        {
            if (GlobalData.boardArray[row_position - 1, col_position] == 0)
            {
                empty_position = new Vector3(transform.position.x - 1.5f, 0, transform.position.z);
                GlobalData.boardArray[row_position, col_position] = 0;
                GlobalData.boardArray[row_position - 1, col_position] = number_chip;
                PlaySound();
                canMoveChip = true;
            }
        }
        catch { }

        try
        {
            if (GlobalData.boardArray[row_position, col_position + 1] == 0)
            {
                empty_position = new Vector3(transform.position.x, 0, transform.position.z + 1.5f);
                GlobalData.boardArray[row_position, col_position] = 0;
                GlobalData.boardArray[row_position, col_position + 1] = number_chip;
                PlaySound();
                canMoveChip = true;
            }
        }
        catch { }

        try
        {
            if (GlobalData.boardArray[row_position, col_position - 1] == 0)
            {
                empty_position = new Vector3(transform.position.x, 0, transform.position.z - 1.5f);
                GlobalData.boardArray[row_position, col_position] = 0;
                GlobalData.boardArray[row_position, col_position - 1] = number_chip;
                PlaySound();
                canMoveChip = true;
            }
        }
        catch { }
    }
    void FindOnBoard()
    {
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (GlobalData.boardArray[row, col] == number_chip)
                {
                    row_position = row;
                    col_position = col;
                }
            }
        }
    }
}
