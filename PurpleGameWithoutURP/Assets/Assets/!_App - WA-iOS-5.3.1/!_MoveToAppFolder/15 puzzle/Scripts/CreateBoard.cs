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

    public Action OnWinGame;
    
    [SerializeField]
    private List<TileFabric> _skinsFabric;

    void Start()
    {
        _best.text = PlayerPrefs.GetInt("best").ToString();

        OnWinGame += (() => SetChipsCollidersActivity(false));

        //GenerateBoard();
        //ShowBoard();
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
                if (GlobalData.boardArray[row, col] == 0)
                {
                    cancel = true;
                    GlobalData.boardArray[row, col] = i;
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
                if (GlobalData.boardArray[row, col] != 0)
                {
                    Vector3 coardinate = new Vector3(board_position.x + row * split_x, board_position.y,
                        board_position.z + col * split_z);
                    int chip = GlobalData.boardArray[row, col] - 1;
                    var chipGO = _skinsFabric[skinNumber].Get(chip, coardinate, transform.rotation).gameObject;
                    if (chip_scale_modifier != 0)
                        chipGO.transform.localScale *= chip_scale_modifier;

                    chipGO.GetComponent<ChipMove>().OnMoveChip += CheckOnComplete;
                    chipGO.GetComponent<ChipMove>().OnMoveChip += MotionCountPlus;


                    chipGO.transform.parent = transform;
                    chipGO.name = chips[chip].name;
                    chipsPool.Add(chipGO);
                }
            }
        }
    }

    public void SetChipsCollidersActivity(bool activity)
    {
        foreach (var chip in chipsPool)
        {
            chip.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void CheckOnComplete()
    {
        int count = 1;

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (GlobalData.boardArray[row, col] == count)
                {
                    Debug.Log(count);
                    _scoreText.text = count + "/15";

                }
                else
                {
                    return;
                }
                count++;
                count = 16;
                if (count == 16)
                {
                    if (GlobalData.currentScore < PlayerPrefs.GetInt("best") || PlayerPrefs.GetInt("best") == 0)
                    {
                        PlayerPrefs.SetInt("best", GlobalData.currentScore);
                        _best.text = PlayerPrefs.GetInt("best").ToString();
                    }

                    OnWinGame?.Invoke();

                }
            }
        }
    }

    private void MotionCountPlus()
    {
        GlobalData.currentScore++;
        _currentScore.text = GlobalData.currentScore.ToString();
    }


}