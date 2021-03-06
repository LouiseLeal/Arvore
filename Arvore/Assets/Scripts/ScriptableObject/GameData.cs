﻿using System.Collections;
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
    public float speedIncrease = 1f;
    //Range has to be in tiles
    public int rangeConnon = 2;
    public float cannonThreshold = 0.3f;
    public float cannonSpeed = 0.5f;

    [Header("Game parameters")]
    [HideInInspector]public float tileSize = 20;
    public int arenaHeight = 10;
    public int arenaWidth = 10;
    [HideInInspector] public Vector2 tileOffSet;

    [Header("Snake parameters")]
    public int initialSnakeSize = 3;
    public float snakeSpeed = 1;
    public int snakesPlayerAmount = 1;
}
