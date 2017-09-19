using LazySnake.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LazySnake.Engine.GameObject;

namespace LazySnake.Engine
{
    class GamePlayer
    {
        public enum GamePlayerDirection
        {
            Up = 1,
            UpLeft = 100,
            UpRight = 101,
            Left = 2,
            Bottom = 3,
            BottomLeft = 300,
            BottomRight = 301,
            Right = 4
        }

        private GamePlayerDirection currentTurnSide;
        private GameObject gameObject;
        private GameEngine gameEngine;

        public Dictionary<GamePlayerDirection, Bitmap> TextureDictionary = new Dictionary<GamePlayerDirection, Bitmap>();
        public Dictionary<GamePlayerDirection, GameAnimation> AnimationDictionary = new Dictionary<GamePlayerDirection, GameAnimation>();

        public GamePlayer(GameEngine gameEngine)
        {
            this.gameEngine = gameEngine;
        }

        public void SetGameObject(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public void Walk()
        {
            if(currentTurnSide == GamePlayerDirection.Up)
            {
                if(gameObject.Neighbors.Top == null || gameObject.Neighbors.Top.Type != GameObjectType.Wall)
                    gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row - 1, gameObject.Coordinates.Col), AnimationDictionary[GamePlayerDirection.Up]);
            }
            else if (currentTurnSide == GamePlayerDirection.Left)
            {
                if (gameObject.Neighbors.Left == null || gameObject.Neighbors.Left.Type != GameObjectType.Wall)
                    gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row, gameObject.Coordinates.Col - 1), AnimationDictionary[GamePlayerDirection.Left]);
            }
            else if (currentTurnSide == GamePlayerDirection.Right)
            {
                if (gameObject.Neighbors.Right == null || gameObject.Neighbors.Right.Type != GameObjectType.Wall)
                    gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row, gameObject.Coordinates.Col + 1), AnimationDictionary[GamePlayerDirection.Right]);
            }
            else if (currentTurnSide == GamePlayerDirection.Bottom)
            {
                if (gameObject.Neighbors.Bottom == null || gameObject.Neighbors.Bottom.Type != GameObjectType.Wall)
                    gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row + 1, gameObject.Coordinates.Col), AnimationDictionary[GamePlayerDirection.Bottom]);
            }
        }

        public void TurnUp()
        {
            currentTurnSide = GamePlayerDirection.Up;
            gameObject.SetTexture(TextureDictionary[currentTurnSide]);
        }

        public void TurnUpLeft()
        {
            currentTurnSide = GamePlayerDirection.UpLeft;
            gameObject.SetTexture(TextureDictionary[currentTurnSide]);
        }

        public void TurnUpRight()
        {
            currentTurnSide = GamePlayerDirection.UpRight;
            gameObject.SetTexture(TextureDictionary[currentTurnSide]);
        }

        public void TurnLeft()
        {
            currentTurnSide = GamePlayerDirection.Left;
            gameObject.SetTexture(TextureDictionary[currentTurnSide]);
        }

        public void TurnBottom()
        {
            currentTurnSide = GamePlayerDirection.Bottom;
            gameObject.SetTexture(TextureDictionary[currentTurnSide]);
        }

        public void TurnBottomLeft()
        {
            currentTurnSide = GamePlayerDirection.BottomLeft;
            gameObject.SetTexture(TextureDictionary[currentTurnSide]);
        }

        public void TurnBottomRight()
        {
            currentTurnSide = GamePlayerDirection.BottomRight;
            gameObject.SetTexture(TextureDictionary[currentTurnSide]);
        }

        public void TurnRight()
        {
            currentTurnSide = GamePlayerDirection.Right;
            gameObject.SetTexture(TextureDictionary[currentTurnSide]);
        }
    }
}
