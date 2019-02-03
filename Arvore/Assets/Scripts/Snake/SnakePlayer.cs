using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//All warning were verified 
#pragma warning disable CS0649
namespace Snake
{


    public class SnakePlayer : Snake
    {


        //public static List<int> UsedSnakesPresets = new List<int>();
        KeyCode[] input;

        private void Awake()
        {
            input = new KeyCode[2];
            input[0] = KeyCode.None;
            input[1] = KeyCode.None;
        }


        protected override void Update()
        {
            if (snakeTiles == null || !isActive)
                return;

            CheckForInput();

            nextMoveTime -= Time.deltaTime;
            if (nextMoveTime < 0)
            {
                CheckForMove();
            }
        }


        void CheckForInput()
        {

            if (canCheckInput )
            {
                if (Input.GetKeyDown(input[0]))
                {
                    Rotate(clockwise: true);
                    canCheckInput = false;
                }
                else if (Input.GetKeyDown(input[1]))
                {
                    Rotate(clockwise: false);
                    canCheckInput = false;
                }
            }
        }


        public override void CreateSnake(int initialTileCount, int arenaHeight, Vector2 TileSize, float speed)
        {
            base.CreateSnake(initialTileCount, arenaHeight, TileSize, speed);
            ChoosePresetCoroutine = StartCoroutine(CyclingPresets());
        }

        public void SetInput(KeyCode[] input)
        {
            this.input = input;
            canCheckInput = true;
        }


        #region SnakePreset

        //Variables for the snakes preset method
        Coroutine ChoosePresetCoroutine;

        //Avoid garbage colector
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        IEnumerator CyclingPresets()
        {
            indexSelectedColor = 0;
            while (true)
            {
                if (IsColorIndexUsed(indexSelectedColor))
                {
                    indexSelectedColor++;
                    continue;
                }

                //Considering the conbinations of avaiables keys
                //we only have 36 colors
                if (indexSelectedColor >= 36)
                    indexSelectedColor = 0;

                TintSnake(indexSelectedColor);
                indexSelectedColor++;

                yield return wait;
            }
        }

        public override void  SelectSnakePreset()
        {
            if (ChoosePresetCoroutine != null)
                StopCoroutine(ChoosePresetCoroutine);
        }

        #endregion
    }
}
#pragma warning restore CS0649