using UnityEngine;


namespace Snake
{
    //public enum Sprites
    //{
    //    SnakeHead = 0,
    //    SnakeBody = 1,
    //    SnakeTail = 2,
    //    MapTile = 3,
    //    MapWall = 4
    //}

    [CreateAssetMenu(fileName = "SpriteData", menuName = "SpriteData", order = 1)]
    public class SpriteData : ScriptableObject
    {
       public Sprite SnakeHead;
       public Sprite SnakeBody;
       public Sprite SnakeTail;
       public Sprite MapTile;
       public Sprite MapWall;

        //public Sprite[] sprites;

        //public void Awake() =>
        //sprites = new Sprite[5]
        // {
        //     SnakeHead,
        //     SnakeBody,
        //     SnakeTail,
        //     MapTile,
        //     MapWall
        //};
    }

}