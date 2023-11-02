using UnityEngine;

public class CreateBoard : MonoBehaviour
{
    public GameObject[] chips = new GameObject[16];
    public Vector3 board_position = new(-2f, -10f, -2f); //instantiate Point 
    public float split_x = 1.1f;
    public float split_z = 1.1f;
    [SerializeField] private float chip_scale_modifier;

    void Start()
    {
        GenerateBoard();
        ShowBoard();
    }

    void GenerateBoard()
    {
        for (int i = 1; i < 16; i++)
        {
            bool cancel = false;
            do
            {
                int row = Random.Range(0, 4);
                int col = Random.Range(0, 4);
                if (GlobalData.boardArray[row, col] == 0)
                {
                    cancel = true;
                    GlobalData.boardArray[row, col] = i;
                }
            } while (!cancel);
        }
    }

    void ShowBoard()
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
                    var chipGO = Instantiate(chips[chip], coardinate, transform.rotation);
                    if (chip_scale_modifier != 0)
                        chipGO.transform.localScale *= chip_scale_modifier;

                    chipGO.transform.parent = transform;
                    chipGO.name = chips[chip].name;
                }
            }
        }
    }
}