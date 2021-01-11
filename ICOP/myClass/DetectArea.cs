using System.Drawing;
using System.Windows.Forms;

namespace ICOP
{
    public class DetectArea
    {
        public RectangleF area = new RectangleF();

        public int sellectX;
        public int sellectY;
        public bool sellect = false;

        public bool resize = false;

        public string name = "abc";

        static Color nameColor = Color.Blue;
        SolidBrush brushName = new SolidBrush(nameColor);
        Font nameFont = new Font("Microsoft YaHei UI", 6);
        public Image ImageRef;
        public DetectArea()
        {
            this.area = new RectangleF();
        }
        public DetectArea(RectangleF detectRect)
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
                this.sellectX = (int)(Global.maxWidth * e.X / pictureBox.Width - area.X);
                this.sellectY = (int)(Global.maxHeight * e.Y / pictureBox.Height - area.Y);
            }
            return this.sellect;
        }
        public void move(MouseEventArgs e, PictureBox pictureBox)
        {
            area.X = Global.maxWidth * e.X / pictureBox.Width - sellectX;
            area.Y = Global.maxHeight * e.Y / pictureBox.Height - sellectY;
        }

        public RectangleF RectScale(RectangleF inputRect, float xScale, float yScale)
        {
            RectangleF outRect = new RectangleF();
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
            RectangleF RectangleF = RectScale(area, scaleX, scaleY);
            Bitmap b = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height);
            Graphics g = Graphics.FromImage(b);
            g.DrawRectangle(WorkingPen, RectangleF.X, RectangleF.Y, RectangleF.Width, RectangleF.Height);
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
                    RectangleF imageRec = RectScale(area, scaleX, scaleY);
                    bitmapBuffer = new Bitmap((int)imageRec.Width, (int)imageRec.Height);
                    using (Graphics g = Graphics.FromImage(bitmapBuffer))
                    {
                        g.DrawImage((Bitmap)this.ImageRef, new RectangleF(0, 0, bitmapBuffer.Width, bitmapBuffer.Height),
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
