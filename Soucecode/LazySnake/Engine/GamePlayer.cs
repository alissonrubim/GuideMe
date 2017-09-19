using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazySnake.Engine
{
    class GamePlayer
    {
        public enum GamePlayerTurnSide
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

        private GamePlayerTurnSide currentTurnSide;
        private GameObject playerObject;

        public Dictionary<GamePlayerTurnSide, Bitmap> TextureMap = new Dictionary<GamePlayerTurnSide, Bitmap>();
        public GamePlayer(GameObject playerObject)
        {
            this.playerObject = playerObject;
        }

        public void Walk()
        {

        }

        public void TurnUp()
        {
            currentTurnSide = GamePlayerTurnSide.Up;
            playerObject.SetTexture(TextureMap[currentTurnSide]);
        }

        public void TurnUpLeft()
        {
            currentTurnSide = GamePlayerTurnSide.UpLeft;
            playerObject.SetTexture(TextureMap[currentTurnSide]);
        }

        public void TurnUpRight()
        {
            currentTurnSide = GamePlayerTurnSide.UpRight;
            playerObject.SetTexture(TextureMap[currentTurnSide]);
        }

        public void TurnLeft()
        {
            currentTurnSide = GamePlayerTurnSide.Left;
            playerObject.SetTexture(TextureMap[currentTurnSide]);
        }

        public void TurnBottom()
        {
            currentTurnSide = GamePlayerTurnSide.Bottom;
            playerObject.SetTexture(TextureMap[currentTurnSide]);
        }

        public void TurnBottomLeft()
        {
            currentTurnSide = GamePlayerTurnSide.BottomLeft;
            playerObject.SetTexture(TextureMap[currentTurnSide]);
        }

        public void TurnBottomRight()
        {
            currentTurnSide = GamePlayerTurnSide.BottomRight;
            playerObject.SetTexture(TextureMap[currentTurnSide]);
        }

        public void TurnRight()
        {
            currentTurnSide = GamePlayerTurnSide.Right;
            playerObject.SetTexture(TextureMap[currentTurnSide]);
        }
    }
}
