using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICOP_V2.Class
{
    public class DetectArea
    {
        public Rectangle area = new Rectangle();

        public int sellectX;
        public int sellectY;
        public bool sellect = false;

        public bool resize = false;

        public string name = "abc";

        static Color nameColor = Color.Blue;
        SolidBrush brushName = new SolidBrush(nameColor);
        Font nameFont = new Font("Microsoft YaHei UI", 6);
        public Image ImageRef;
        public DetectArea() { }
        public DetectArea(Rectangle detectRect)
        {
            this.area = detectRect;
        }

        public void getLocation(MouseEventArgs e, PictureBox pictureBox)
        {
            this.area.X = Global.maxWidth * e.X / pictureBox.Width;
            this.area.Y = Global.maxHeight * e.Y / pictureBox.Height;
        }
        public void getSize(MouseEventArgs e, PictureBox pictureBox)
        {
            this.area.Width = Global.maxWidth * e.X / pictureBox.Width - this.area.X;
            this.area.Height = Global.maxHeight * e.Y / pictureBox.Height - this.area.Y;
        }
        public bool checkSellect(MouseEventArgs e, PictureBox pictureBox)
        {
            this.sellect = false;
            var mousepoint = new System.Drawing.Point(Global.maxWidth * e.X / pictureBox.Width, Global.maxHeight * e.Y / pictureBox.Height);
            this.sellect = area.Contains(mousepoint);
            if (this.sellect)
            {
                this.sellectX = Global.maxWidth * e.X / pictureBox.Width - area.X;
                this.sellectY = Global.maxHeight * e.Y / pictureBox.Height - area.Y;
            }
            return this.sellect;
        }
        public void move(MouseEventArgs e, PictureBox pictureBox)
        {
            area.X = Global.maxWidth * e.X / pictureBox.Width - sellectX;
            area.Y = Global.maxHeight * e.Y / pictureBox.Height - sellectY;
        }

        public Rectangle RectScale(Rectangle inputRect, float xScale, float yScale)
        {
            Rectangle outRect = new Rectangle();
            outRect.X = (int)(inputRect.X * xScale);
            outRect.Y = (int)(inputRect.Y * yScale);
            outRect.Width = (int)(inputRect.Width * xScale);
            outRect.Height = (int)(inputRect.Height * yScale);
            return outRect;
        }

        public Image draw(Pen WorkingPen, Image image)
        {
            this.ImageRef = null;
            if (image != null)
            {
                this.ImageRef = (Image)image.Clone();
            }
            else
            {
                this.ImageRef = new Bitmap(1366, 768);
            }
            float scaleX = Global.maxWidth / this.ImageRef.Width;
            float scaleY = Global.maxHeight / this.ImageRef.Height;
            Bitmap b = new Bitmap(ImageRef, ImageRef.Width, ImageRef.Height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.DrawRectangle(WorkingPen, RectScale(area, scaleX, scaleY));
                g.DrawString(this.name, nameFont, brushName, area.X, area.Y - 10);
            }
            return b;
        }
        public Bitmap GetImage(Image image)
        {
            Bitmap bitmapBuffer = new Bitmap(10, 10);
            if (!this.area.IsEmpty)
            {
                this.ImageRef = (Image)image.Clone();
                if (this.ImageRef != null)
                {
                    float scaleX = Global.maxWidth / this.ImageRef.Width;
                    float scaleY = Global.maxHeight / this.ImageRef.Height;
                    Rectangle imageRec = RectScale(area, scaleX, scaleY);
                    bitmapBuffer = new Bitmap(imageRec.Width, imageRec.Height);
                    using (Graphics g = Graphics.FromImage(bitmapBuffer))
                    {
                        g.DrawImage((Bitmap)this.ImageRef, new Rectangle(0, 0, bitmapBuffer.Width, bitmapBuffer.Height),
                                         imageRec,
                                         GraphicsUnit.Pixel);
                    }
                }
            }
            this.ImageRef = null;
            return bitmapBuffer;
        }
    }
}
