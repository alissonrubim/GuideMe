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
        

        private const int blockSize = 25;

       // private EGameItemType[,] gameMap;

        public MainWindow()
        {
            InitializeComponent();

            /*gameMap = new EGameItemType[7, 6]
            {
                {EGameItemType.Obstacle,EGameItemType.None,EGameItemType.None,EGameItemType.None,EGameItemType.None, EGameItemType.Obstacle },
                {EGameItemType.Obstacle,EGameItemType.Obstacle, EGameItemType.Obstacle, EGameItemType.None,EGameItemType.None,EGameItemType.None },
                {EGameItemType.Obstacle,EGameItemType.Obstacle, EGameItemType.Obstacle, EGameItemType.None,EGameItemType.None,EGameItemType.None },
                {EGameItemType.Obstacle,EGameItemType.None, EGameItemType.Obstacle, EGameItemType.Coin,EGameItemType.None,EGameItemType.None },
                {EGameItemType.Obstacle,EGameItemType.None, EGameItemType.None, EGameItemType.None,EGameItemType.None,EGameItemType.None },
                {EGameItemType.None,EGameItemType.None, EGameItemType.None, EGameItemType.None,EGameItemType.None,EGameItemType.None },
                {EGameItemType.Obstacle,EGameItemType.None, EGameItemType.None, EGameItemType.None,EGameItemType.None,EGameItemType.None }
            };*/


            Map currentMap = new Map(stackPanelGameMap);
            currentMap.LoadMap(ResourceMaps.Map01);


        }

        

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
