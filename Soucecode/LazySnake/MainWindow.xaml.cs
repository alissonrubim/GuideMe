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
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MapCreatorWindow winCreator = new MapCreatorWindow();
            winCreator.ShowDialog();
        }

        private void MenuItemCommand_TurnUpLeft(object sender, RoutedEventArgs e)
        {
            player.TurnUpLeft();
        }

        private void MenuItemCommand_TurnUp(object sender, RoutedEventArgs e)
        {
            player.TurnUp();
        }
        private void MenuItemCommand_TurnUpRight(object sender, RoutedEventArgs e)
        {
            player.TurnUpRight();
        }

        private void MenuItemCommand_TurnBottomLeft(object sender, RoutedEventArgs e)
        {
            player.TurnBottomLeft();
        }

        private void MenuItemCommand_TurnBottom(object sender, RoutedEventArgs e)
        {
            player.TurnBottom();
        }
        private void MenuItemCommand_TurnBottomRight(object sender, RoutedEventArgs e)
        {
            player.TurnBottomRight();
        }

        private void MenuItemCommand_TurnLeft(object sender, RoutedEventArgs e)
        {
            player.TurnLeft();
        }

        private void MenuItemCommand_TurnRight(object sender, RoutedEventArgs e)
        {
            player.TurnRight();
        }

        private void MenuItemCommand_Walk(object sender, RoutedEventArgs e)
        {
            player.Walk();
        }

        private GamePlayer player;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GameEngine engine = new GameEngine(this.canvasGameMap);
            engine.LoadMap(ResourceMaps.Map01);
            player = engine.GetPlayer(0);
            player.TurnUp();
        }
    }
}
