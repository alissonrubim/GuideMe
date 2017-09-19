using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LazySnake.Engine
{
    class GameEngine
    {
        private Canvas[] layers = new Canvas[5];
        private Canvas parentCanvas = null;
        private GameMap currentMap = null;

        public GameEngine(Canvas parentCanvas)
        {
            this.parentCanvas = parentCanvas;
            setupLayers();
        }

        public void LoadMap(string map)
        {
            currentMap = new GameMap(this);
            currentMap.LoadMap(map);
        }

        public Canvas GetLayerByIndex(int layerIndex)
        {
            return layers[layerIndex];
        }

        private void setupLayers()
        {
            Canvas parent = this.parentCanvas;
            for (int i = 0; i < layers.Length; i++)
            {
                Canvas layer = new Canvas();
                layer.Visibility = Visibility.Visible;
                layer.Background = System.Windows.Media.Brushes.Transparent;
                layer.Width = parent.ActualWidth;
                layer.Height = parent.ActualHeight;
                layer.Name = "Layer" + (i).ToString();
                layers[i] = layer;
                parent.Children.Add(layer);
                parent = layer;
            }
        }
    }
}
