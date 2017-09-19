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
                    mapItems[row, col] = new GameObject()
                    {
                        Type = GameObject.GameObjectType.Wall,
                        Size = new System.Windows.Size(blockSize, blockSize)
                    };
                else if (itemNode.InnerText == "2")
                    mapItems[row, col] = new GameObject()
                    {
                        Type = GameObject.GameObjectType.Player,
                        Size = new System.Windows.Size(blockSize, blockSize)
                    };
                else if (itemNode.InnerText == "3")
                    mapItems[row, col] = new GameObject()
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

            GameSpriteSheet playerSpriteSheet = new GameSpriteSheet(ResourceTextures.sprites_player, new System.Drawing.Point(32, 32));

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    if (mapItems[i, j] != null)
                    {
                        GameObject mapItem = mapItems[i, j];
                        mapItem.Position = new System.Windows.Point(j * mapItem.Size.Height, i * mapItem.Size.Width);
                        if (mapItems[i, j].Type == GameObject.GameObjectType.Wall)
                        {
                            mapItem.Texture = ResourceTextures.wall;

                            if (!hasBottom(i, j))
                                mapItem.SetTexture(ResourceTextures.wall_bottom);

                            if (!hasTop(i, j) && !hasRight(i, j))
                                mapItem.SetTexture(ResourceTextures.wall_top);

                            if (hasBottom(i, j) && hasRight(i, j) && !hasBottomRight(i, j))
                                mapItem.SetTexture(ResourceTextures.wall_3);

                            if (!hasBottom(i, j) && !hasRight(i, j) && !hasLeft(i, j))
                                mapItem.SetTexture(ResourceTextures.wall_bottom_right);

                            if (!hasBottom(i, j) && !hasRight(i, j) && hasTop(i, j))
                                mapItem.SetTexture(ResourceTextures.wall_7);

                            if (!hasBottom(i, j) && !hasRight(i, j) && hasTop(i, j) && hasLeft(i, j))
                                mapItem.SetTexture(ResourceTextures.wall_6);

                            if (hasBottom(i, j) && hasTop(i, j) && !hasRight(i, j))
                                mapItem.SetTexture(ResourceTextures.wall_side_right);

                            if (!hasBottom(i, j) && !hasRight(i, j) && !hasTop(i, j) && hasLeft(i, j))
                                mapItem.SetTexture(ResourceTextures.wall_5);

                            if (!hasBottom(i, j) && hasRight(i, j) && !hasLeft(i, j))
                                mapItem.SetTexture(ResourceTextures.wall_4);

                            mapItem.Render(engine.GetLayerByIndex(1));
                        }
                        else if (mapItems[i, j].Type == GameObject.GameObjectType.Player)
                        {
                            mapItem.Position = new System.Windows.Point(mapItem.Position.X, mapItem.Position.Y - 5);
                            mapItem.SetTexture(playerSpriteSheet.GetSprite(1, 1));
                            mapItem.Render(engine.GetLayerByIndex(2));

                            /*GameAnimation a = new GameAnimation("Walk_Left", new GameAnimation.AnimateStep[]
                             {
                                 new AnimateStep()
                                 {
                                     Texture = playerSpriteSheet.GetSprite(1,1),
                                     PositionDiff = new System.Windows.Point(-5, 0),
                                     Time = 200
                                 },
                                 new AnimateStep()
                                 {
                                     Texture = playerSpriteSheet.GetSprite(1,2),
                                     PositionDiff = new System.Windows.Point(-5, 0),
                                     Time = 200
                                 },
                                 new AnimateStep()
                                 {
                                     Texture = playerSpriteSheet.GetSprite(1,0),
                                     PositionDiff = new System.Windows.Point(-2, 0),
                                     Time = 200
                                 }
                             }, mapItem);

                            mapItem.Animate(a);*/

                            // mapItem.GoTo(new System.Windows.Point(mapItem.Position.X + 100, mapItem.Position.Y));
                        }
                        else if (mapItems[i, j].Type == GameObject.GameObjectType.Target)
                        {
                            mapItem.SetTexture(ResourceTextures.Diamond_1);
                            mapItem.Render(engine.GetLayerByIndex(2));
                        }
                    }
                }
            }
        }



        public bool hasTop(int i, int j)
        {
            return i > 0 && mapItems[i - 1, j]  != null && mapItems[i - 1, j].Type == GameObject.GameObjectType.Wall;
        }

        public bool hasBottom(int i, int j)
        {
            return i < mapItems.GetLength(0) - 1 && mapItems[i + 1, j] != null && mapItems[i + 1, j].Type == GameObject.GameObjectType.Wall;
        }

        public bool hasRight(int i, int j)
        {
            return j < mapItems.GetLength(1) - 1 && mapItems[i, j + 1] != null && mapItems[i, j + 1].Type == GameObject.GameObjectType.Wall;
        }

        public bool hasLeft(int i, int j)
        {
            return j > 0 && mapItems[i, j - 1] != null && mapItems[i, j - 1].Type == GameObject.GameObjectType.Wall;
        }

        public bool hasBottomRight(int i, int j)
        {
            return i < mapItems.GetLength(0) - 1 && j < mapItems.GetLength(1) - 1 && mapItems[i + 1, j + 1] != null && mapItems[i + 1, j + 1].Type == GameObject.GameObjectType.Wall;

        }
    }
}
