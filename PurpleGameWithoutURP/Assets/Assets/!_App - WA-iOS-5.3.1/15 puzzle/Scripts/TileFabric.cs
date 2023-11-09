using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileFabric")]
public class TileFabric : ScriptableObject
{
    [SerializeField]
    private ChipMove _chipPrefab;

    [SerializeField]
    private List<Sprite> _chipSprites;

    public ChipMove Get(int chipNumber, Vector3 pos, Quaternion rot)
    {
        if(chipNumber > _chipSprites.Count || chipNumber < 0)
        {
            return null;
        }

        ChipMove chipMove = Instantiate(_chipPrefab, pos, rot);

        chipMove.SpriteRender.sprite = _chipSprites[chipNumber];

        return chipMove;
    
    }



}
