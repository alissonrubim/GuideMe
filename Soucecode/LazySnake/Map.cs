using LazySnake.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace LazySnake
{
    class Map
    {
        internal enum MapItemType
        {
            None = 0,
            Obstacle = 1,
            Player = 2,
            Coin = 3
        }

        public MapItemType[,] Items;
        private StackPanel mainStackPanel;

        public const int BlockSize = 20;
        public int Width = 0;
        public int Height = 0;

        public Map(StackPanel mainStackPanel)
        {
            this.mainStackPanel = mainStackPanel;
        }

        internal void LoadMap(string map)
        {
            XmlDataDocument xmldoc = new XmlDataDocument();
            XmlNodeList xmlnode;
            xmldoc.LoadXml(map);
            xmlnode = xmldoc.GetElementsByTagName("map");
            XmlNode mapNode = xmlnode.Item(0);

            Width = Convert.ToInt32(mapNode.Attributes["width"].Value);
            Height = Convert.ToInt32(mapNode.Attributes["height"].Value);

            Items = new MapItemType[Width, Height];

            for (int i = 0; i <= mapNode.ChildNodes.Count - 1; i++)
            {
                XmlNode itemNode = mapNode.ChildNodes[i];

                int posX = Convert.ToInt32(itemNode.Attributes["posX"].Value);
                int posY = Convert.ToInt32(itemNode.Attributes["posY"].Value);

                Items[posX, posY] = MapItemType.None;
                
                if (itemNode.InnerText == "1")
                    Items[posX, posY] = MapItemType.Obstacle;
                else if (itemNode.InnerText == "2")
                    Items[posX, posY] = MapItemType.Player;
                    
            }

            renderMap();
        }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        private ImageSource imageSourceForBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        private void renderMap()
        {
            mainStackPanel.Orientation = Orientation.Vertical;


            ImageBrush brush = new ImageBrush();
            brush.ImageSource = imageSourceForBitmap(ResourceTextures.Grass);

            brush.TileMode = TileMode.Tile;
            brush.Viewport = new Rect(0, 0, BlockSize, BlockSize);
            brush.ViewportUnits = BrushMappingMode.Absolute;
            mainStackPanel.Background = brush;

            for (int i = 0; i < Items.GetLength(0); i++)
            {
                var stackPanelRow = new StackPanel();
                stackPanelRow.Orientation = Orientation.Horizontal;
                stackPanelRow.Height = BlockSize;
                mainStackPanel.Children.Add(stackPanelRow);


                for (int j = 0; j < Items.GetLength(1); j++)
                {
                    var texturePanel = new System.Windows.Controls.Image();
                    texturePanel.Width = BlockSize;
                    texturePanel.Height = BlockSize;

                    if (Items[i, j] == MapItemType.Player)
                    {

                    }
                    else if (Items[i, j] == MapItemType.Coin)
                    {
                        texturePanel.Source = imageSourceForBitmap(ResourceTextures.Apple);
                    }
                    else if (Items[i, j] == MapItemType.Obstacle)
                    {
                        texturePanel.Source = imageSourceForBitmap(ResourceTextures.Rock);
                    }

                    stackPanelRow.Children.Add(texturePanel);
                }
            }


        }
    }
}
