using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//All warning were verified 
#pragma warning disable CS0649

namespace Snake
{
    public enum BlockType
    {
        INACTIVE,
        GRAY,
        BLUE,
        GREEN,
        RED
    }


  
    public class BlocksSpawnManager : MonoBehaviour
    {

        [SerializeField] GameObject blockPrefab;
        [SerializeField] Transform BlocksContainer;

        private float greenChance;
        private float grayChance;
        private float blueChance;
        private float redChance;

        Block[] blocks;

        List<Block> activesBlocks = new List<Block>(); 


        public void CreateBlockPool(int maxBlocks, Vector2 tileSize)
        {
            blocks = new Block[maxBlocks];
            Block newBlock;
            for (int i = 0; i < maxBlocks; i++)
            {
                newBlock = Instantiate(blockPrefab, BlocksContainer).GetComponent<Block>();
                newBlock.SetBlockSize(tileSize);
                newBlock.SetBlock(BlockType.INACTIVE, Vector2.zero);
                blocks[i] = newBlock;

            }

        }

        public void SetBlockChances(float grayChance, float greenChance, float blueChance, float redChance)
        {
            var totalChance = grayChance + greenChance + blueChance + redChance;

            this.grayChance = grayChance / totalChance;
            this.blueChance = blueChance / totalChance;
            this.greenChance = greenChance / totalChance;
            this.redChance = redChance / totalChance;
        }

        public Block EnableBlock(Position position,Vector2 canvasPosition)
        {
            if (blocks == null || blocks.Length < 0) return null;

            //Debug.Log("positin" + position);
            float random = UnityEngine.Random.Range(0f, 1f);
            var theshold = grayChance;
//            Debug.Log(random);

            Block enabledBlock = null;

            for (int i = 0; i < blocks.Length; i++)
            {
                if(blocks[i].type == BlockType.INACTIVE)
                {
                    enabledBlock = blocks[i];
                    break;
                }
            }

            enabledBlock.SetPosition(position);
            activesBlocks.Add(enabledBlock);

            if(enabledBlock == null)
            {
                Debug.Log("activeBlock not found");
                return null;
            }

            if (random <= theshold)
            {
                enabledBlock.SetBlock(BlockType.GRAY, canvasPosition);
                return enabledBlock; 
            }

            theshold += blueChance;
            if (random <= theshold)
            {
                enabledBlock.SetBlock(BlockType.BLUE, canvasPosition);
                return enabledBlock;
            } 

            theshold += greenChance;
            if (random <= theshold)
            {
                enabledBlock.SetBlock(BlockType.GREEN, canvasPosition);
                return enabledBlock;
            }

            theshold += redChance;
            if (random <= theshold)
            {
                enabledBlock.SetBlock(BlockType.RED, canvasPosition);
                return enabledBlock;
            }

            return null;
        }

        internal void ResetBlocks()
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i].GetBlockType() != BlockType.INACTIVE) 
                    DesableBlock(blocks[i]);
            }
        }

        public void DesableBlock(Block block)
        {
            block.SetBlock(BlockType.INACTIVE, Vector2.zero);
            activesBlocks.Remove(block);
        }

        public Position GetNearBlock(Position position)
        {
            if (activesBlocks.Count == 0) {
                Debug.Log("No blocks");
            } else if (activesBlocks.Count == 1)
            {
                return activesBlocks[0].GetPosition();
            }

            var result = int.MaxValue;
            int dist;

            Position resultPosition = new Position();

            for (int i = 0; i < activesBlocks.Count; i++)
            {
                dist = BlocksDistance(position, blocks[i].GetPosition());
                if (dist < result) {
                    result = dist;
                    resultPosition = blocks[i].GetPosition(); 
                 }
            }

            if (result == int.MaxValue)
                Debug.LogError("Could not find near block");

            return resultPosition;

        }


        int BlocksDistance(Position p1,Position p2)
        {
            int result = 0;
            result = (Mathf.Abs(p1.x - p2.x)) + (Mathf.Abs(p1.y - p2.y));

            return result;
        }
    }
}
#pragma warning restore CS0649

