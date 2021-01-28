using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICOP
{
    class Statistical
    {
        public const string StatisticalFileName = "Statistical.ist";
        public int dailyFail { get; set; } = 0;
        public int dailyPass { get; set; } = 0;
        public int dailyToltal { get; set; } = 0;
        public double dailyPercentPass { get; set; } = 0.0;

        public Statistical()
        {
            if (!Directory.Exists(Global.ICOP_Statitic_path)) Directory.CreateDirectory(Global.ICOP_Statitic_path);
            if (!File.Exists(Global.ICOP_Statitic_path + StatisticalFileName))
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string StatisticalJson = JsonSerializer.Serialize(this, options);
                File.WriteAllText(Global.ICOP_Statitic_path + StatisticalFileName, StatisticalJson);
            }
        }
        public void Statistical_Save()
        {
            if (!Directory.Exists(Global.ICOP_Statitic_path)) Directory.CreateDirectory(Global.ICOP_Statitic_path);
            if (!File.Exists(Global.ICOP_Statitic_path + StatisticalFileName))
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string StatisticalJson = JsonSerializer.Serialize(this, options);
                File.WriteAllText(Global.ICOP_Statitic_path + StatisticalFileName, StatisticalJson);
            }
        }
        public void Statistical_Update(int arrayNumber, string result, Label CounterFail, Label CounterPass, Label CounterTotal, Label CounterPercent)
        {
            if (result == Global.ICOP_tester_OK)
            {
                this.dailyPass = dailyPass + arrayNumber;
            }
            else
            {
                this.dailyFail = dailyFail + arrayNumber;
            }
            this.dailyToltal = this.dailyPass + this.dailyFail;
            if (this.dailyToltal == 0)
                this.dailyPercentPass = 0.0;
            else
                this.dailyPercentPass =  this.dailyPass / (double)dailyToltal;

            CounterFail.Text = this.dailyFail.ToString();
            CounterPass.Text = this.dailyPass.ToString();
            CounterTotal.Text = this.dailyToltal.ToString();
            CounterPercent.Text = (this.dailyPercentPass * 100).ToString("f2");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string StatisticalJson = JsonSerializer.Serialize(this, options);
            File.WriteAllText(Global.ICOP_Statitic_path + StatisticalFileName, StatisticalJson);

        }
        public void Statistical_Init_Counter(Label CounterFail, Label CounterPass, Label CounterTotal, Label CounterPercent)
        {
            this.dailyToltal = this.dailyPass + this.dailyFail;
            if (this.dailyToltal == 0)
                this.dailyPercentPass = 0.0;
            else
                this.dailyPercentPass = this.dailyPass / (double)dailyToltal;

            CounterFail.Text = this.dailyFail.ToString();
            CounterPass.Text = this.dailyPass.ToString();
            CounterTotal.Text = this.dailyToltal.ToString();
            CounterPercent.Text = (this.dailyPercentPass * 100).ToString("f2");
        }
        public void DrawChart(PictureBox pBChar, float charCicle)
        {
           
            float okRadian = (float)charCicle / dailyToltal * dailyPass;
            float ngRadian = (float)charCicle - okRadian;

            int startRectY = pBChar.Size.Height / 2 - pBChar.Size.Width / 2;
            int startRectX = pBChar.Size.Width / 2 - pBChar.Size.Height / 2;
            int rectDimemtions = pBChar.Size.Width;

            if (startRectY < 0)
            {
                startRectY = 0;
                rectDimemtions = pBChar.Size.Height;
            }
            if (startRectX < 0)
            {
                startRectX = 0;
                rectDimemtions = pBChar.Size.Width;
            }

            if (pBChar.Size.Width > 30 && pBChar.Size.Height > 30)
            {
                Rectangle rect = new Rectangle(startRectX, startRectY, rectDimemtions, rectDimemtions);
                Rectangle rectInside = new Rectangle(startRectX + rectDimemtions / 4, startRectY + rectDimemtions / 4, rectDimemtions / 2, rectDimemtions / 2);
                Bitmap custormChart = new Bitmap(pBChar.Size.Width, pBChar.Size.Height);
                Graphics g = Graphics.FromImage(custormChart);

                Color okColor = Color.FromArgb(30, 136, 221);
                Color bacgroudColor = Color.FromArgb(240, 240, 240);
                SolidBrush brush = new SolidBrush(okColor);
                SolidBrush brushNumber = new SolidBrush(Color.FromArgb(2, 52, 93));
                SolidBrush brushInside = new SolidBrush(bacgroudColor);

                g.FillPie(brush, rect, 0, okRadian);
                g.FillPie(Brushes.Red, rect, okRadian, ngRadian);
                g.FillPie(brushInside, rectInside, 0, 360);

                string persenOkString = (dailyPercentPass*100).ToString("F1") + " %";
                Font persentOkFont = new Font("Microsoft YaHei UI", rectDimemtions / 14, FontStyle.Bold);
                g.DrawString(persenOkString, persentOkFont, brushNumber, startRectX + rectDimemtions / 2 - (persenOkString.Length * 4 * rectDimemtions / 14 / 10), startRectY + rectDimemtions / 2 - rectDimemtions / 14);

                if (pBChar.Image != null)
                {
                    pBChar.Image.Dispose();
                }
                pBChar.Image = custormChart;

                brush.Dispose();
                brushInside.Dispose();
                brushNumber.Dispose();
                g.Dispose();
                //custormChart.Dispose();
            }
        }
    }
}
