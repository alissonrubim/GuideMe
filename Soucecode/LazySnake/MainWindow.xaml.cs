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
            player.TurnDownLeft();
        }

        private void MenuItemCommand_TurnBottom(object sender, RoutedEventArgs e)
        {
            player.TurnDown();
        }
        private void MenuItemCommand_TurnBottomRight(object sender, RoutedEventArgs e)
        {
            player.TurnDownRight();
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
        private GameEngine engine;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            engine = new GameEngine(this.canvasGameMap);
            engine.LoadMap(ResourceMaps.Map01);
            player = engine.GetPlayer(0);
            player.TurnDown();
        }

        private void ProcessAutoGame()
        {
            StarRoutine algoritmo = new StarRoutine();

            Vertex origem = null;
            Vertex meta = null;
            for (int i = 0; i < engine.GetMap().GetSize().Width; i++)
                for (int j = 0; j < engine.GetMap().GetSize().Height; j++) {
                    GameObject a = engine.GetMap().GetGameObjectAt(i, j);
                    if (a != null)
                    {
                        if(a.Type == GameObject.GameObjectType.Player)
                        {
                            origem = new Vertex(i, j);

                        }else if(a.Type == GameObject.GameObjectType.Target)
                        {
                            meta = new Vertex(i, j);

                        }

                    }
                }
            List<Vertex> caminho;
            if (algoritmo.Start(engine.GetMap(), origem, meta, new Heuristic(), out caminho))
                ///mostrarCaminho(caminho);
                MessageBox.Show("caminho encontrado!");
            else
                MessageBox.Show("caminho não encontrado!");
        }

        private void MenuItemCommand_AIExecute(object sender, RoutedEventArgs e)
        {
            ProcessAutoGame();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Left)
            {
                player.TurnLeft();
                player.Walk();
            }
            else if (e.Key == Key.Right)
            {
                player.TurnRight();
                player.Walk();
            }
            else if (e.Key == Key.Up)
            {
                player.TurnUp();
                player.Walk();
            }
            else if (e.Key == Key.Down)
            {
                player.TurnDown();
                player.Walk();
            }
            else if ((e.Key == Key.Right) && (e.Key == Key.Up))
            {
                player.TurnUpRight();
                player.Walk();
            }
            else if ((e.Key == Key.Right) && (e.Key == Key.Down))
            {
                player.TurnDownRight();
                player.Walk();
            }
            else if ((e.Key == Key.Left) && (e.Key == Key.Up))
            {
                player.TurnUpLeft();
                player.Walk();
            }
            else if ((e.Key == Key.Left) && (e.Key == Key.Down))
            {
                player.TurnDownLeft();
                player.Walk();
            }
        }
    }
}
