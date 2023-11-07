using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CreateBoard : MonoBehaviour
{
    public GameObject[] chips = new GameObject[16];
    public List<GameObject> chipsPool = new List<GameObject>();
    public Vector3 board_position = new(-2f, -10f, -2f); //instantiate Point 
    public float split_x = 1.1f;
    public float split_z = 1.1f;
    [SerializeField] private float chip_scale_modifier;

    [SerializeField] private Text _scoreText;

    [SerializeField] private Text _currentScore;

    [SerializeField] private Text _best;

    public int skinNumber = 0;

    public Action OnEndGame;

    private float Timer;

    [SerializeField]
    private List<TileFabric> _skinsFabric;

    [SerializeField]
    private Text _timerText;

    public bool IsGame = true;

    [SerializeField,Tooltip("Определяет, какое колво тебе установить очков ")]
    private int ConsoleSetCountSimilars;

    [ContextMenu("SetCountSimilarsConsole")]
    public void SetCountSimilarsConsole()
    {
        CountSimilars = ConsoleSetCountSimilars;
        _scoreText.text = CountSimilars + "/15";
    }


    public int CountSimilars { get; private set; }

    void Start()
    {
        Timer = GlobalGameData.TimerCount;

        _best.text = PlayerPrefs.GetInt("best").ToString();

        OnEndGame += (() => SetChipsCollidersActivity(false));
    }

    private void Update()
    {


        if (IsGame)
        {
            if (Timer < 0)
            {
                IsGame = false;

                OnEndGame?.Invoke();
            }

            Timer -= Time.deltaTime;

            _timerText.text = FromIntToTime((int)Timer);
        }
    }

    public void InitializeBoard(int skin)
    {
        if (skin > 0 || skin < _skinsFabric.Count)
        {
            skinNumber = skin;
        }

        GenerateBoard();
        ShowBoard();

    }

    private void GenerateBoard()
    {
        for (int i = 1; i < 16; i++)
        {
            bool cancel = false;
            do
            {
                int row = UnityEngine.Random.Range(0, 4);
                int col = UnityEngine.Random.Range(0, 4);
                if (GlobalGameData.boardArray[row, col] == 0)
                {
                    cancel = true;
                    GlobalGameData.boardArray[row, col] = i;
                }
            } while (!cancel);
        }
    }

    private void ShowBoard()
    {
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (GlobalGameData.boardArray[row, col] != 0)
                {
                    Vector3 coardinate = new Vector3(board_position.x + row * split_x, board_position.y,
                        board_position.z + col * split_z);
                    int chip = GlobalGameData.boardArray[row, col] - 1;
                    var chipGO = _skinsFabric[skinNumber].Get(chip, coardinate, transform.rotation);
                    if (chip_scale_modifier != 0)
                        chipGO.transform.localScale *= chip_scale_modifier;

                    chipGO.OnMoveChip += CheckOnComplete;
                    chipGO.OnMoveChip += MotionCountPlus;


                    chipGO.transform.parent = transform;
                    chipGO.name = chips[chip].name;
                    chipsPool.Add(chipGO.gameObject);
                }
            }
        }
    }

    public void SetChipsCollidersActivity(bool activity)
    {
        foreach (var chip in chipsPool)
        {
            chip.GetComponent<BoxCollider>().enabled = activity;
        }
    }

    private void CheckOnComplete()
    {
        CountSimilars = 1;

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (GlobalGameData.boardArray[row, col] == CountSimilars)
                {
                    Debug.Log(CountSimilars);
                    _scoreText.text = CountSimilars + "/15";

                }
                else
                {
                    return;
                }
                CountSimilars++;
                if (CountSimilars == 16)
                {
                    OnEndGame?.Invoke();

                }
            }
        }
    }

    public void AddTime(float Time)
    {
        Timer += Time;
    }

    private void MotionCountPlus()
    {
        GlobalGameData.currentScore++;
        _currentScore.text = GlobalGameData.currentScore.ToString();
    }

    public string FromIntToTime(int value)
    {
        TimeSpan time = TimeSpan.FromSeconds(value);
        string text = time.ToString("mm':'ss");

        return text;
    }


}