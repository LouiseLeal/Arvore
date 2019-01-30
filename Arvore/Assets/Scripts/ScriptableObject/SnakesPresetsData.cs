using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SnakesPresetsData", menuName = "SnakesPresetsDataDefault", order = 1)]
public class SnakesPresetsData : ScriptableObject
{
    //considering the possibles keys the number of snakes
    //in arena will be 36 (18 players and 18 pc-controller)
    public Color[] colors = new Color[36];
}
