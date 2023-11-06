using UnityEngine;
using UnityEngine.UI;
using System;

public class ChipMove : MonoBehaviour
{
    private int number_chip;
    private int row_position;
    private int col_position;
    [SerializeField] private int speed;
    [SerializeField] private float scale;
    
    private Vector3 empty_position = new (0, 0, 0);

    private bool canMoveChip;

    public SpriteRenderer SpriteRender ;

    public Action OnMoveChip;

    void Start()
    {
        number_chip = int.Parse(gameObject.name);
        

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

    void MoveChipOnBoard()
    {
        if (transform.position != empty_position)
        {
            transform.position = Vector3.MoveTowards(transform.position, empty_position, speed * Time.deltaTime);
        }
        else
        {
            canMoveChip = false;
            OnMoveChip?.Invoke();
            
            
        }
    }
    
    void CalculateDirection()
    {
        try
        {
            SwapDirection(1, 0);
        }
        catch { }

        try
        {
            SwapDirection(-1,0 );
        }
        catch { }

        try
        {
            SwapDirection(0, 1);
        }
        catch { }

        try
        {
            SwapDirection(0, -1);
        }
        catch { }
    }

    private void SwapDirection(int x, int y)
    {
        if (GlobalGameData.boardArray[row_position + x, col_position + y] == 0)
        {
            empty_position = new Vector3(transform.position.x + x * scale, 0, transform.position.z + y * scale);
            GlobalGameData.boardArray[row_position, col_position] = 0;
            GlobalGameData.boardArray[row_position + x, col_position + y] = number_chip;
            PlaySound();
            canMoveChip = true;
        }
    }


    void FindOnBoard()
    {
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (GlobalGameData.boardArray[row, col] == number_chip)
                {
                    row_position = row;
                    col_position = col;
                }
            }
        }
    }
}
