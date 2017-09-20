using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LazySnake.Engine
{
    public class GameObject
    {
        public class Coordinate
        {
            public int Col;
            public int Row;

            public Coordinate(int row, int col)
            {
                this.Col = col;
                this.Row = row;
            }
        }

        public class Neighbor
        {
            public GameObject Top;
            public GameObject TopLeft;
            public GameObject TopRight;
            public GameObject Left;
            public GameObject Right;
            public GameObject Bottom;
            public GameObject BottomLeft;
            public GameObject BottomRight;
        }

        public enum GameObjectType
        {
            Wall = 0,
            Player = 1,
            Target = 2
        }

        public bool MakeColision { get; set; }
        public Neighbor Neighbors { get; set; }
        public string Identifier { get; set; }
        public GameObjectType Type { get; set; }
        public Bitmap Texture { get; set; }
        public System.Windows.Size Size { get; set; }
        public System.Windows.Point Position { get; set; }
        public Coordinate Coordinates { get; set; }

        private System.Windows.Controls.Image renderPanel;
        public GameObject(Coordinate mapCoordinates)
        {
            this.Coordinates = mapCoordinates;
            this.renderPanel = new System.Windows.Controls.Image();
        }

        public void SetTexture(Bitmap texture)
        {
            this.Texture = texture;
            renderPanel.Source = ImageSourceForBitmap(Texture);
        }

        public void SetPosition(System.Windows.Point point)
        {
            this.Position = point;
            Canvas.SetTop(renderPanel, point.Y);
            Canvas.SetLeft(renderPanel, point.X);
        }

        public System.Windows.Point GetPosition()
        {
            return new System.Windows.Point(Canvas.GetLeft(this.renderPanel), Canvas.GetTop(this.renderPanel));
        }

        public UIElement Render(Canvas layer)
        {
            renderPanel.Width = Size.Width;
            renderPanel.Height = Size.Height;
            renderPanel.HorizontalAlignment = HorizontalAlignment.Center;
            renderPanel.VerticalAlignment = VerticalAlignment.Center;
            renderPanel.Stretch = Stretch.Uniform;
            layer.Children.Add(renderPanel);
            if(Position != null)
                SetPosition(Position);
            if(Texture != null)
                SetTexture(Texture);
            return renderPanel;
        }

        public void Animate(GameAnimation gameAnimation)
        {
            gameAnimation.SetGameObject(this);
            gameAnimation.Run();
        }
        

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);
        public static ImageSource ImageSourceForBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }
    }
}
