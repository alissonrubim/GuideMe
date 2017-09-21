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
        private int energy = 0;

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

        public int GetEnergy()
        {
            return this.energy;
        }

        public void SetEnergy(int value)
        {
            this.energy = value;
        }

        public void Walk()
        {
            if(currentTurnSide == GamePlayerDirection.Up)
            {
                if(!gameEngine.IsColisionObject(gameObject.Neighbors.Top))
                    gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row - 1, gameObject.Coordinates.Col), AnimationDictionary[GamePlayerDirection.Up]);
            }
            else if (currentTurnSide == GamePlayerDirection.UpLeft)
            {
                if (!gameEngine.IsColisionObject(gameObject.Neighbors.TopLeft) && (!gameEngine.IsColisionObject(gameObject.Neighbors.Top) || !gameEngine.IsColisionObject(gameObject.Neighbors.Left)))
                    gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row - 1, gameObject.Coordinates.Col - 1), AnimationDictionary[GamePlayerDirection.UpLeft]);
            }
            else if (currentTurnSide == GamePlayerDirection.UpRight)
            {
                if (!gameEngine.IsColisionObject(gameObject.Neighbors.TopRight) && (!gameEngine.IsColisionObject(gameObject.Neighbors.Top) || !gameEngine.IsColisionObject(gameObject.Neighbors.Right)))
                    gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row - 1, gameObject.Coordinates.Col + 1), AnimationDictionary[GamePlayerDirection.UpRight]);
            }
            else if (currentTurnSide == GamePlayerDirection.Left)
            {
                if (!gameEngine.IsColisionObject(gameObject.Neighbors.Left))
                    gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row, gameObject.Coordinates.Col - 1), AnimationDictionary[GamePlayerDirection.Left]);
            }
            else if (currentTurnSide == GamePlayerDirection.Right)
            {
                if (!gameEngine.IsColisionObject(gameObject.Neighbors.Right))
                    gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row, gameObject.Coordinates.Col + 1), AnimationDictionary[GamePlayerDirection.Right]);
            }
            else if (currentTurnSide == GamePlayerDirection.Bottom)
            {
                if (!gameEngine.IsColisionObject(gameObject.Neighbors.Bottom))
                    gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row + 1, gameObject.Coordinates.Col), AnimationDictionary[GamePlayerDirection.Bottom]);
            }
            else if (currentTurnSide == GamePlayerDirection.BottomLeft)
            {
                if (!gameEngine.IsColisionObject(gameObject.Neighbors.BottomLeft) && (!gameEngine.IsColisionObject(gameObject.Neighbors.Bottom) || !gameEngine.IsColisionObject(gameObject.Neighbors.Left)))
                    gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row + 1, gameObject.Coordinates.Col - 1), AnimationDictionary[GamePlayerDirection.BottomLeft]);
            }
            else if (currentTurnSide == GamePlayerDirection.BottomRight)
            {
                if (!gameEngine.IsColisionObject(gameObject.Neighbors.BottomRight) && (!gameEngine.IsColisionObject(gameObject.Neighbors.Bottom) || !gameEngine.IsColisionObject(gameObject.Neighbors.Right)))
                    gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row + 1, gameObject.Coordinates.Col + 1), AnimationDictionary[GamePlayerDirection.BottomRight]);
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

        public void TurnDown()
        {
            currentTurnSide = GamePlayerDirection.Bottom;
            gameObject.SetTexture(TextureDictionary[currentTurnSide]);
        }

        public void TurnDownLeft()
        {
            currentTurnSide = GamePlayerDirection.BottomLeft;
            gameObject.SetTexture(TextureDictionary[currentTurnSide]);
        }

        public void TurnDownRight()
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
