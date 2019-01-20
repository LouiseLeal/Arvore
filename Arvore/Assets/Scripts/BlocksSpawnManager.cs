using UnityEngine;
using UnityEngine.UI;

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

        //todo Create block pool
        private float greenChance;
        private float grayChance;
        private float blueChance;
        private float redChance;



        Block[] blocks;


        public void CreateBlockPool(int maxBlocks, Vector2 tileSize)
        {
            blocks = new Block[maxBlocks];
            Block newBlock;
            for (int i = 0; i < maxBlocks; i++)
            {
                newBlock = Instantiate(blockPrefab, BlocksContainer).GetComponent<Block>();
                newBlock.SetBlockSize(tileSize);
                newBlock.SetBlcok(BlockType.INACTIVE, Vector2.zero);
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

        public void EnableBlock(Vector2 position)
        {
            Debug.Log("positin" + position);
            float random = Random.Range(0, 1);
            var theshold = grayChance;
            Debug.Log(random);

            Block activeBlock = null;

            for (int i = 0; i < blocks.Length; i++)
            {
                if(blocks[i].type == BlockType.INACTIVE)
                {
                    activeBlock = blocks[i];
                    continue;
                }
            }

            if(activeBlock == null)
            {
                Debug.Log("activeBlock not found");
                return;
            }

            if (random <= theshold)
            {
                activeBlock.SetBlcok(BlockType.GRAY, position);
                return;
            }

            theshold += blueChance;
            if (random <= theshold)
            {
                activeBlock.SetBlcok(BlockType.BLUE, position);
                return;
            }

            theshold += greenChance;
            if (random <= theshold)
            {
                activeBlock.SetBlcok(BlockType.GREEN, position);
                return;
            }

            theshold += redChance;
            if (random <= theshold)
            {
                activeBlock.SetBlcok(BlockType.RED, position);
                return;
            }
        }
    
        public void DesableBlock(Block block)
        {
            block.SetBlcok(BlockType.INACTIVE, Vector2.zero);
        }
    }
}
