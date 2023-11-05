using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TransferValue")]
public class TransferSO : ScriptableObject
{
    public ValueType FromValue;

    public ValueType ToValue;

    public int FromValueAmount;

    public int ToValueAmount;

}
