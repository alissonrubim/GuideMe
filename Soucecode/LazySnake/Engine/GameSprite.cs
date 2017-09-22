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

namespace LazySnake.Engine
{
    public class GameSprite
    {
        private Bitmap texture { get; set; }
        private System.Windows.Size size { get; set; }
        private System.Windows.Point position { get; set; }
        private System.Windows.Controls.Image renderPanel;

        public GameSprite()
        {
            this.renderPanel = new System.Windows.Controls.Image();
        }

        public void SetTexture(Bitmap texture)
        {
            this.texture = texture;
            if (texture != null)
                renderPanel.Source = ImageSourceForBitmap(texture);
            else
                renderPanel.Source = null;
        }

        public void SetPosition(System.Windows.Point point)
        {
            this.position = point;
            Canvas.SetTop(renderPanel, point.Y);
            Canvas.SetLeft(renderPanel, point.X);
        }

        public void SetSize(System.Windows.Size size)
        {
            this.size = size;
        }

        public System.Windows.Size GetSize()
        {
            return this.size;
        }

        public System.Windows.Point GetPosition()
        {
            return new System.Windows.Point(Canvas.GetLeft(this.renderPanel), Canvas.GetTop(this.renderPanel));
        }

        public UIElement Render(Canvas layer)
        {
            renderPanel.Width = size.Width;
            renderPanel.Height = size.Height;
            renderPanel.HorizontalAlignment = HorizontalAlignment.Center;
            renderPanel.VerticalAlignment = VerticalAlignment.Center;
            renderPanel.Stretch = Stretch.Uniform;
            layer.Children.Add(renderPanel);
            if (position != null)
                SetPosition(position);
            if (texture != null)
                SetTexture(texture);
            return renderPanel;
        }

        public void Animate(GameAnimation gameAnimation)
        {
            gameAnimation.SetGameSprite(this);
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
