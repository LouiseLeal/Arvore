using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "GameData", order = 1)]
public class GameData : ScriptableObject
{
    [Header("Blocks Spwan chance")]
    public float grayBlock = 0.5f;
    public float greenBlock = 0.15f;
    public float redBlock = 0.2f;
    public float blueBlock = 0.15f;

    [Header("Blocks specifics parameters")]
    public float SpeedIncrease = 1f;
    public float RangeConnon = 1f;

    [Header("Game parameters")]
    public int arenaHeight = 10;
    public int arenaWidth = 10;

    //Todo ainda não entendi como isso funciona
    //[Header("Preset parameters")]

}
