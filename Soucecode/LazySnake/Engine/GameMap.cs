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
        public delegate GameObject ProcessXMLMapHandle(string nodeValue, int row, int col);

        public RenderGameObjectHandle OnRenderGameObject;
        public ProcessXMLMapHandle OnProcessXMLMap;

        private GameObject[,] mapItems;
        private GameEngine engine;
        

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

                if (OnProcessXMLMap != null)
                    mapItems[row, col] = OnProcessXMLMap(itemNode.InnerText, row, col);

                if (mapItems[row, col] != null)
                    mapItems[row, col].SetSize(new System.Windows.Size(engine.BlockSize, engine.BlockSize));
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

        public System.Windows.Size GetSize()
        {
            return new System.Windows.Size(mapItems.GetLength(0), mapItems.GetLength(1));
        }
        private void renderMap()
        {
            int rowCount = mapItems.GetLength(0);
            int colCount = mapItems.GetLength(1);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    if (mapItems[i, j] != null)
                    {
                        GameObject gameObject = mapItems[i, j];
                        gameObject.SetPosition(new System.Windows.Point(j * gameObject.GetSize().Height, i * gameObject.GetSize().Width));

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

        public bool MoveTo(GameObject gameObject, Coordinate coordinate, bool moveObject = true)
        {
            if (processColision(mapItems[coordinate.Row, coordinate.Col], gameObject))
            {
                int walkDistanceX = coordinate.Col - gameObject.Coordinates.Col;
                int walkDistanceY = coordinate.Row - gameObject.Coordinates.Row;

                mapItems[gameObject.Coordinates.Row, gameObject.Coordinates.Col] = null;
                mapItems[coordinate.Row, coordinate.Col] = gameObject;
                gameObject.Coordinates = coordinate;
                processNeighbors(gameObject);
                if (moveObject)
                    gameObject.SetPosition(new System.Windows.Point(gameObject.GetPosition().X + (walkDistanceX * engine.BlockSize), gameObject.GetPosition().Y + (walkDistanceY * engine.BlockSize)));
                return true;
            }
            return false;
        }

        private bool processColision(GameObject oldObject, GameObject newObject)
        {
            if(oldObject != null && oldObject.CanColideWithMe)
                if (oldObject.ColisionWithMeHandlers.ContainsKey(newObject.Type))
                    return oldObject.ColisionWithMeHandlers[newObject.Type](oldObject, newObject);

            return true;
        }

        public bool MoveTo(GameObject gameObject, Coordinate coordinate, GameAnimation animation)
        {
            if (MoveTo(gameObject, coordinate, moveObject: false))
            {
                gameObject.Animate(animation);
                return true;
            }
            return false;
        }

        private void processNeighbors(GameObject gameObject)
        {
            int i = gameObject.Coordinates.Row;
            int j = gameObject.Coordinates.Col;
            int rowCount = mapItems.GetLength(0);
            int colCount = mapItems.GetLength(1);

            //Remove my old neighbors with NULL
            Neighbor oldNeighbors = gameObject.Neighbors;
            if (oldNeighbors != null)
            {
                if (oldNeighbors.Top != null)
                    oldNeighbors.Top.Neighbors.Bottom = null;
                if (oldNeighbors.Bottom != null)
                    oldNeighbors.Bottom.Neighbors.Top = null;
                if (oldNeighbors.Left != null)
                    oldNeighbors.Left.Neighbors.Right = null;
                if (oldNeighbors.Right != null)
                    oldNeighbors.Right.Neighbors.Left = null;
                if (oldNeighbors.TopRight != null)
                    oldNeighbors.TopRight.Neighbors.BottomLeft = null;
                if (oldNeighbors.TopLeft != null)
                    oldNeighbors.TopLeft.Neighbors.BottomRight = null;
                if (oldNeighbors.BottomRight != null)
                    oldNeighbors.BottomRight.Neighbors.TopLeft = null;
                if (oldNeighbors.BottomLeft != null)
                    oldNeighbors.BottomLeft.Neighbors.TopRight = null;
            }

            gameObject.Neighbors = new Neighbor();
            //Update ME with my neighbors
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

            //Update my new neighbors with me
            if (gameObject.Neighbors.Top != null && gameObject.Neighbors.Top.Neighbors != null)
                gameObject.Neighbors.Top.Neighbors.Bottom = gameObject;
            if (gameObject.Neighbors.Bottom != null && gameObject.Neighbors.Bottom.Neighbors != null)
                gameObject.Neighbors.Bottom.Neighbors.Top = gameObject;
            if (gameObject.Neighbors.Left != null && gameObject.Neighbors.Left.Neighbors != null)
                gameObject.Neighbors.Left.Neighbors.Right = gameObject;
            if (gameObject.Neighbors.Right != null && gameObject.Neighbors.Right.Neighbors != null)
                gameObject.Neighbors.Right.Neighbors.Left = gameObject;
            if (gameObject.Neighbors.TopRight != null && gameObject.Neighbors.TopRight.Neighbors != null)
                gameObject.Neighbors.TopRight.Neighbors.BottomLeft = gameObject;
            if (gameObject.Neighbors.TopLeft != null && gameObject.Neighbors.TopLeft.Neighbors != null)
                gameObject.Neighbors.TopLeft.Neighbors.BottomRight = gameObject;
            if (gameObject.Neighbors.BottomRight != null && gameObject.Neighbors.BottomRight.Neighbors != null)
                gameObject.Neighbors.BottomRight.Neighbors.TopLeft = gameObject;
            if (gameObject.Neighbors.BottomLeft != null && gameObject.Neighbors.BottomLeft.Neighbors != null)
                gameObject.Neighbors.BottomLeft.Neighbors.TopRight = gameObject;
        }
    }
}
