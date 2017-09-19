using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazySnake.Engine
{
    class GameSpriteSheet
    {
        public Bitmap SpriteSheet;
        public Point SpriteArea;
        public GameSpriteSheet(Bitmap spriteSheet, Point spriteArea)
        {
            this.SpriteSheet = spriteSheet;
            this.SpriteArea = spriteArea;
        }

        public Bitmap GetSprite(int row, int col)
        {
            return cropImage(SpriteSheet, new Rectangle(col * SpriteArea.Y, row * SpriteArea.X, SpriteArea.X, SpriteArea.Y));
        }

        private static System.Drawing.Bitmap cropImage(System.Drawing.Bitmap img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }
    }

}
