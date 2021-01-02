using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICOP
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
        public DetectArea() {
            this.area = new Rectangle();
        }
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

        public void draw(Pen WorkingPen, PictureBox pictureBox, string name)
        {
            float scaleX = pictureBox.Size.Width / Global.maxWidth;
            float scaleY = pictureBox.Size.Height / Global.maxHeight;
            Rectangle rectangle = RectScale(area, scaleX, scaleY);
            Bitmap b = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height);
            Graphics g = Graphics.FromImage(b);
            g.DrawRectangle(WorkingPen, rectangle);
            g.DrawString(name, nameFont, brushName, area.X, area.Y - 10);
            if (pictureBox.Image != null) pictureBox.Image.Dispose();
            pictureBox.Image = b;
            g.Dispose();
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
