using LazySnake.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LazySnake
{
    /// <summary>
    /// Interaction logic for MapCreatorWindow.xaml
    /// </summary>
    public partial class MapCreatorWindow : Window
    {
        public MapCreatorWindow()
        {
            InitializeComponent();

            GameEngine engine = new GameEngine(this.canvasGameMaker);
            GameMap currentMap = new GameMap(engine);
            currentMap.LoadMap(new GameObject[100, 100]);
        }
    }
}
