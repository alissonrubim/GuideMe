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

        public Dictionary<GamePlayerDirection, Bitmap> TextureMap = new Dictionary<GamePlayerDirection, Bitmap>();

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
                GameSpriteSheet playerSpriteSheet = new GameSpriteSheet(ResourceTextures.player_sheet, new System.Drawing.Point(32, 32));
                GameAnimation walkUp = new GameAnimation("Player_WalkUp", new GameAnimation.AnimateStep[]
                {
                    new GameAnimation.AnimateStep()
                    {
                        PositionDiff = new System.Windows.Point(0, -5),
                        Texture = playerSpriteSheet.GetSprite(3, 1),
                        Time = 200
                    },
                    new GameAnimation.AnimateStep()
                    {
                        PositionDiff = new System.Windows.Point(0, -5),
                        Texture = playerSpriteSheet.GetSprite(3, 2),
                        Time = 200
                    },
                    new GameAnimation.AnimateStep()
                    {
                        PositionDiff = new System.Windows.Point(0, -5),
                        Texture = playerSpriteSheet.GetSprite(3, 3),
                        Time = 200
                    },
                    new GameAnimation.AnimateStep()
                    {
                        PositionDiff = new System.Windows.Point(0, -5),
                        Texture = playerSpriteSheet.GetSprite(3, 4),
                        Time = 200
                    },
                    new GameAnimation.AnimateStep()
                    {
                        PositionDiff = new System.Windows.Point(0, -5),
                        Texture = playerSpriteSheet.GetSprite(3, 5),
                        Time = 200
                    }
                }, runForever: false);
                gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.X, gameObject.Coordinates.Y - 1), walkUp);
            }
            else if (currentTurnSide == GamePlayerDirection.Left)
            {
                gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.X - 1, gameObject.Coordinates.Y));
            }
            else if (currentTurnSide == GamePlayerDirection.Right)
            {
                gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.X + 1, gameObject.Coordinates.Y));
            }
            else if (currentTurnSide == GamePlayerDirection.Bottom)
            {
                gameEngine.MoveTo(gameObject, new Coordinate(gameObject.Coordinates.X, gameObject.Coordinates.Y + 1));
            }
        }

        public void TurnUp()
        {
            currentTurnSide = GamePlayerDirection.Up;
            gameObject.SetTexture(TextureMap[currentTurnSide]);
        }

        public void TurnUpLeft()
        {
            currentTurnSide = GamePlayerDirection.UpLeft;
            gameObject.SetTexture(TextureMap[currentTurnSide]);
        }

        public void TurnUpRight()
        {
            currentTurnSide = GamePlayerDirection.UpRight;
            gameObject.SetTexture(TextureMap[currentTurnSide]);
        }

        public void TurnLeft()
        {
            currentTurnSide = GamePlayerDirection.Left;
            gameObject.SetTexture(TextureMap[currentTurnSide]);
        }

        public void TurnBottom()
        {
            currentTurnSide = GamePlayerDirection.Bottom;
            gameObject.SetTexture(TextureMap[currentTurnSide]);
        }

        public void TurnBottomLeft()
        {
            currentTurnSide = GamePlayerDirection.BottomLeft;
            gameObject.SetTexture(TextureMap[currentTurnSide]);
        }

        public void TurnBottomRight()
        {
            currentTurnSide = GamePlayerDirection.BottomRight;
            gameObject.SetTexture(TextureMap[currentTurnSide]);
        }

        public void TurnRight()
        {
            currentTurnSide = GamePlayerDirection.Right;
            gameObject.SetTexture(TextureMap[currentTurnSide]);
        }
    }
}
