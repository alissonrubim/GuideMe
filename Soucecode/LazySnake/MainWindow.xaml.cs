using LazySnake.Engine;
using LazySnake.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace LazySnake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            GameEngine engine = new GameEngine(this.canvasGameMap);
            GameMap currentMap = new GameMap(engine);
            currentMap.LoadMap(ResourceMaps.Map01);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MapCreatorWindow winCreator = new MapCreatorWindow();
            winCreator.ShowDialog();
        }
    }
}
