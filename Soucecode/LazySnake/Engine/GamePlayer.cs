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
        private bool isWalking = false;

        private Dictionary<GamePlayerDirection, Bitmap> textureDictionary = new Dictionary<GamePlayerDirection, Bitmap>();
        private Dictionary<GamePlayerDirection, GameAnimation> animationDictionary = new Dictionary<GamePlayerDirection, GameAnimation>();

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

        public void AddTexture(GamePlayerDirection direction, Bitmap texture)
        {
            if (textureDictionary.ContainsKey(direction))
                textureDictionary.Add(direction, texture);
            else
                textureDictionary[direction] = texture;
        }

        public void AddWalkAnimation(GamePlayerDirection direction, GameAnimation animation)
        {
            if (animationDictionary.ContainsKey(direction))
                animationDictionary.Add(direction, animation);
            else
                animationDictionary[direction] = animation;

            animation.OnStart += onAnimationStart;
            animation.OnFinish += onAnimationFinish;
        }

        public void Walk()
        {
            if (!this.isWalking)
            {
                if (currentTurnSide == GamePlayerDirection.Up)
                {
                    if (!gameEngine.IsColisionObject(gameObject.Neighbors.Top))
                        gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row - 1, gameObject.Coordinates.Col), animationDictionary[GamePlayerDirection.Up]);
                }
                else if (currentTurnSide == GamePlayerDirection.UpLeft)
                {
                    if (!gameEngine.IsColisionObject(gameObject.Neighbors.TopLeft) && (!gameEngine.IsColisionObject(gameObject.Neighbors.Top) || !gameEngine.IsColisionObject(gameObject.Neighbors.Left)))
                        gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row - 1, gameObject.Coordinates.Col - 1), animationDictionary[GamePlayerDirection.UpLeft]);
                }
                else if (currentTurnSide == GamePlayerDirection.UpRight)
                {
                    if (!gameEngine.IsColisionObject(gameObject.Neighbors.TopRight) && (!gameEngine.IsColisionObject(gameObject.Neighbors.Top) || !gameEngine.IsColisionObject(gameObject.Neighbors.Right)))
                        gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row - 1, gameObject.Coordinates.Col + 1), animationDictionary[GamePlayerDirection.UpRight]);
                }
                else if (currentTurnSide == GamePlayerDirection.Left)
                {
                    if (!gameEngine.IsColisionObject(gameObject.Neighbors.Left))
                        gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row, gameObject.Coordinates.Col - 1), animationDictionary[GamePlayerDirection.Left]);
                }
                else if (currentTurnSide == GamePlayerDirection.Right)
                {
                    if (!gameEngine.IsColisionObject(gameObject.Neighbors.Right))
                        gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row, gameObject.Coordinates.Col + 1), animationDictionary[GamePlayerDirection.Right]);
                }
                else if (currentTurnSide == GamePlayerDirection.Bottom)
                {
                    if (!gameEngine.IsColisionObject(gameObject.Neighbors.Bottom))
                        gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row + 1, gameObject.Coordinates.Col), animationDictionary[GamePlayerDirection.Bottom]);
                }
                else if (currentTurnSide == GamePlayerDirection.BottomLeft)
                {
                    if (!gameEngine.IsColisionObject(gameObject.Neighbors.BottomLeft) && (!gameEngine.IsColisionObject(gameObject.Neighbors.Bottom) || !gameEngine.IsColisionObject(gameObject.Neighbors.Left)))
                        gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row + 1, gameObject.Coordinates.Col - 1), animationDictionary[GamePlayerDirection.BottomLeft]);
                }
                else if (currentTurnSide == GamePlayerDirection.BottomRight)
                {
                    if (!gameEngine.IsColisionObject(gameObject.Neighbors.BottomRight) && (!gameEngine.IsColisionObject(gameObject.Neighbors.Bottom) || !gameEngine.IsColisionObject(gameObject.Neighbors.Right)))
                        gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.Row + 1, gameObject.Coordinates.Col + 1), animationDictionary[GamePlayerDirection.BottomRight]);
                }
            }
        }

        public void TurnUp()
        {
            turnSide(GamePlayerDirection.Up);
        }

        public void TurnUpLeft()
        {
            turnSide(GamePlayerDirection.UpLeft);
        }

        public void TurnUpRight()
        {
            turnSide(GamePlayerDirection.UpRight);
        }

        public void TurnLeft()
        {
            turnSide(GamePlayerDirection.Left);
        }

        public void TurnDown()
        {
            turnSide(GamePlayerDirection.Bottom);
        }

        public void TurnDownLeft()
        {
            turnSide(GamePlayerDirection.BottomLeft);
        }

        public void TurnDownRight()
        {
            turnSide(GamePlayerDirection.BottomRight);
        }

        public void TurnRight()
        {
            turnSide(GamePlayerDirection.Right);
        }

        private void turnSide(GamePlayerDirection direction)
        {
            if (!this.isWalking)
            {
                currentTurnSide = direction;
                gameObject.SetTexture(textureDictionary[currentTurnSide]);
            }
        }

        private void onAnimationFinish()
        {
            this.isWalking = false;
        }

        private void onAnimationStart()
        {
            this.isWalking = true;
        }
    }
}
