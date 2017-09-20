using LazySnake.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using static LazySnake.Engine.GameAnimation;
using static LazySnake.Engine.GameObject;

namespace LazySnake.Engine
{
    class GameMap
    {
        public delegate void RenderGameObjectHandle(GameObject gameObject);

        public RenderGameObjectHandle OnRenderGameObject;

        private GameObject[,] mapItems;
        private GameEngine engine;
        private const int blockSize = 25;

        public GameMap(GameEngine engine)
        {
            this.engine = engine;
        }

        public void LoadMap(string mapStruct)
        {
            XmlDataDocument xmldoc = new XmlDataDocument();
            XmlNodeList xmlnode;
            xmldoc.LoadXml(mapStruct);
            xmlnode = xmldoc.GetElementsByTagName("map");
            XmlNode mapNode = xmlnode.Item(0);
            int cols = Convert.ToInt32(mapNode.Attributes["cols"].Value);
            int rows = Convert.ToInt32(mapNode.Attributes["rows"].Value);
            mapItems = new GameObject[rows, cols];
            for (int i = 0; i <= mapNode.ChildNodes.Count - 1; i++)
            {
                XmlNode itemNode = mapNode.ChildNodes[i];
                int row = Convert.ToInt32(itemNode.Attributes["row"].Value);
                int col = Convert.ToInt32(itemNode.Attributes["col"].Value);
                mapItems[row, col] = null;
                if (itemNode.InnerText == "1")
                    mapItems[row, col] = new GameObject(new Coordinate(row, col))
                    {
                        Type = GameObject.GameObjectType.Wall,
                        Size = new System.Windows.Size(blockSize, blockSize),
                        MakeColision = true
                    };
                else if (itemNode.InnerText == "2")
                    mapItems[row, col] = new GameObject(new Coordinate(row, col))
                    {
                        Type = GameObject.GameObjectType.Player,
                        Size = new System.Windows.Size(blockSize, blockSize)
                    };
                else if (itemNode.InnerText == "3")
                    mapItems[row, col] = new GameObject(new Coordinate(row, col))
                    {
                        Type = GameObject.GameObjectType.Target,
                        Size = new System.Windows.Size(blockSize, blockSize)
                    };
            }

            LoadMap(mapItems);
        }

        public void LoadMap(GameObject[,] mapStruct)
        {
            mapItems = mapStruct;
            renderMap();
        }

        public void Reload()
        {
            renderMap();
        }

        private void renderMap()
        {
            int rowCount = mapItems.GetLength(0);
            int colCount = mapItems.GetLength(1);

            Canvas backgroundLayer = engine.GetLayerByIndex(0);
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = GameObject.ImageSourceForBitmap(ResourceTextures.Grass);
            brush.TileMode = TileMode.Tile;
            brush.Viewport = new Rect(0, 0, blockSize, blockSize);
            brush.ViewportUnits = BrushMappingMode.Absolute;
            backgroundLayer.Background = brush;

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    if (mapItems[i, j] != null)
                    {
                        GameObject gameObject = mapItems[i, j];
                        gameObject.Position = new System.Windows.Point(j * gameObject.Size.Height, i * gameObject.Size.Width);

                        processNeighbors(gameObject);

                        if (OnRenderGameObject != null)
                            OnRenderGameObject(gameObject);
                    }
                }
            }
        }

        public GameObject GetGameObjectAt(int row, int col)
        {
            return mapItems[row, col];
        }

        public void MoveTo(GameObject gameObject, Coordinate coordinate, bool moveObject = true)
        {
            int walkDistanceX = coordinate.Col - gameObject.Coordinates.Col;
            int walkDistanceY = coordinate.Row - gameObject.Coordinates.Row;

            mapItems[gameObject.Coordinates.Row, gameObject.Coordinates.Col] = null;
            mapItems[coordinate.Row, coordinate.Col] = gameObject;
            gameObject.Coordinates = coordinate;
            processNeighbors(gameObject);
            if(moveObject)
                gameObject.SetPosition(new System.Windows.Point(gameObject.GetPosition().X + (walkDistanceX * blockSize) , gameObject.GetPosition().Y + (walkDistanceY * blockSize)));
        }

        public void MoveTo(GameObject gameObject, Coordinate coordinate, GameAnimation animation)
        {
            MoveTo(gameObject, coordinate, moveObject: false);
            gameObject.Animate(animation);
        }

        private void processNeighbors(GameObject gameObject)
        {
            int i = gameObject.Coordinates.Row;
            int j = gameObject.Coordinates.Col;
            int rowCount = mapItems.GetLength(0);
            int colCount = mapItems.GetLength(1);

            gameObject.Neighbors = new Neighbor();

            if (i > 0)
                gameObject.Neighbors.Top = mapItems[i - 1, j];
            if (i > 0 && j > 0)
                gameObject.Neighbors.TopLeft = mapItems[i - 1, j - 1];
            if (i > 0 && j < colCount - 1)
                gameObject.Neighbors.TopRight = mapItems[i - 1, j + 1];
            if (j > 0)
                gameObject.Neighbors.Left = mapItems[i, j - 1];
            if (j < colCount - 1)
                gameObject.Neighbors.Right = mapItems[i, j + 1];
            if (i < rowCount - 1)
                gameObject.Neighbors.Bottom = mapItems[i + 1, j];
            if (i < rowCount - 1 && j > 0)
                gameObject.Neighbors.BottomLeft = mapItems[i + 1, j - 1];
            if (i < rowCount - 1 && j < colCount - 1)
                gameObject.Neighbors.BottomRight = mapItems[i + 1, j + 1];
        }
    }
}
