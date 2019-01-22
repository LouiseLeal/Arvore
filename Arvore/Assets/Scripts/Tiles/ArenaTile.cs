﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Snake
{
    public enum ArenaTileState
    {
        EMPTY,
        WALL,
        SNAKE,
        BLOCK

    }

    public class ArenaTile : Tile
    {
        ArenaTileState arenaTileState;

        //Store reference;
        public Block block;

        private void Awake()
        {
            image.sprite = spriteData.MapTile;
            arenaTileState = ArenaTileState.EMPTY;
            image.color = Color.blue;
        }

        public void SetWall()
        {
            arenaTileState = ArenaTileState.WALL;
            image.sprite = spriteData.MapWall;
            //gameObject.AddComponent<Collider2D>();
            //Todo set size if needed

        }

        public Vector2 GetCanvasPosition() => rectTransform.anchoredPosition;

        public void SetCanvasPosiiton(Vector2 position) => rectTransform.anchoredPosition = position;

        public ArenaTileState GetArenaTileState() => arenaTileState;

        public void ChangeArenaTileState(ArenaTileState state, Block block = null) {
            if (state == ArenaTileState.WALL) return;

            arenaTileState = state;
            if (block != null)
            {
                this.block = block;
            }
            else
            {
                block = null;
            }

            //For DEBUG
            switch (arenaTileState)
            {
                case ArenaTileState.EMPTY:
                    image.color = Color.blue;
                    break;
                case ArenaTileState.WALL:
                    break;
                case ArenaTileState.SNAKE:
                    image.color = Color.red;
                    break;
                case ArenaTileState.BLOCK:
                    image.color = Color.white;
                    break;
            }
        }
    }
}