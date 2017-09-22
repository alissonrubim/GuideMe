using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LazySnake.Engine
{
    public class GameObject: GameSprite
    {
        public delegate bool GameObjectColisionHandle(GameObject currentObject, GameObject colisorObject);

        public class Coordinate
        {
            public int Col;
            public int Row;

            public Coordinate(int row, int col)
            {
                this.Col = col;
                this.Row = row;
            }
        }

        public class Neighbor
        {
            public GameObject Top;
            public GameObject TopLeft;
            public GameObject TopRight;
            public GameObject Left;
            public GameObject Right;
            public GameObject Bottom;
            public GameObject BottomLeft;
            public GameObject BottomRight;
        }

        public enum GameObjectType
        {
            Wall = 1,
            Player = 2,
            Target = 3,
            Batery = 4
        }

        public bool CanColideWithMe { get; set; }
        public Dictionary<GameObjectType, GameObjectColisionHandle> ColisionWithMeHandlers;
        public Neighbor Neighbors { get; set; }
        public GameObjectType Type { get; set; }
        public Coordinate Coordinates { get; set; }

        public GameObject(Coordinate mapCoordinates):base()
        {
            this.Coordinates = mapCoordinates;
        }
    }
}
