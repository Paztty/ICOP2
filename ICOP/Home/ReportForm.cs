
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Data;
using System.Text.Json;
using System.Windows.Forms;

namespace ICOP
{
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();
            dateTimePickerStart.Value = DateTime.Now;
            dateTimePickerEnd.Value = DateTime.Now;
        }

        //Drag Form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        //Close-Maximize-Mini
        //Remove transparent border in maximized state
        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btMax_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                WindowState = FormWindowState.Maximized;
            else
                WindowState = FormWindowState.Normal;
        }

        private void btMin_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }


        #region Variable
        List<string> listReport = new List<string>();
        ModelProgram modelProgram = new ModelProgram();
        Image resultimage;
        #endregion

        private void dgwModelList_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            //if (e.Row.Index < listReport.Count)
            //{
            //    string modelJson = File.ReadAllText(listReport[e.Row.Index]);
            //    modelProgram = JsonSerializer.Deserialize<ModelProgram>(modelJson);
            //    dgwModelProgram.Rows.Clear();
            //    for (int i = 0; i < modelProgram.modelSteps.Count; i++)
            //    {
            //        modelProgram.modelSteps[i].addToReportDataView(i, dgwModelProgram);
            //    }
            //}
        }
        
        private void dgwModelList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(dgwModelList[0, e.RowIndex].Value) - 1 < listReport.Count)
                {
                    string modelJson = File.ReadAllText(listReport[Convert.ToInt32(dgwModelList[0, e.RowIndex].Value) - 1]);
                    modelProgram = JsonSerializer.Deserialize<ModelProgram>(modelJson);

                    if (pbResultImage.Image != null)
                    {
                        pbResultImage.Image.Dispose();
                    }

                    if (File.Exists(modelProgram.ModelImageResult))
                    {
                        using (var fs = new System.IO.FileStream(modelProgram.ModelImageResult, System.IO.FileMode.Open))
                        {
                            var bmp = new Bitmap(fs);
                            resultimage = (Bitmap)bmp.Clone();
                        }
                    }
                    pbResultImage.Image = resultimage;

                    lbModelName.Text = "Model: " + modelProgram.ModelName +"   PCB code: "+ modelProgram.PCB_Code  + "   Array: " + modelProgram.PBA_Count;
                    lbCreater.Text = modelProgram.AccCreater;

                    dgwModelProgram.Rows.Clear();
                    for (int i = 0; i < modelProgram.modelSteps.Count; i++)
                    {
                        modelProgram.modelSteps[i].addToReportDataView(i, dgwModelProgram);
                    }
                }
            }
            catch { }
        }
        private void btSearch_Click(object sender, EventArgs e)
        {
            SearchReport();
        }

        public void SearchReport()
        {
            listReport.Clear();
            cbbBarcode.Items.Clear();
            DateTime startFilterTime = dateTimePickerStart.Value;
            DateTime endFilterTime = dateTimePickerEnd.Value;
            int SN = 1;
            while (DateTime.Compare(endFilterTime, startFilterTime) >= 0)
            {
                string folderReport = Global.ICOP_history + startFilterTime.ToString("yyyy") + "\\" + startFilterTime.ToString("MMMM") + "\\" + startFilterTime.ToString("dd") + "\\";
                if (Directory.Exists(folderReport))
                {
                    string[] fileReport = Directory.GetFiles(folderReport, "*.ihf");
                    for (int i = 0; i < fileReport.Length; i++)
                    {
                        listReport.Add(fileReport[i]);
                        string fileName = Path.GetFileNameWithoutExtension(fileReport[i]);
                        string[] dataView = fileName.Split('_');
                        dataView[0] = dataView[0].Substring(0, 2) + ":" + dataView[0].Substring(2, 2) + ":" + dataView[0].Substring(4, 2);
                        dataView[0] = startFilterTime.ToString("dd/mm/yyyy ") + dataView[0];
                        dgwModelList.Rows.Add(SN, dataView[0], dataView[1], dataView[2], dataView[3]);
                        cbbBarcode.Items.Add(dataView[1]);
                        SN++;
                    }
                }
                startFilterTime = startFilterTime.AddDays(1);
            }
        }

        private void cbbBarcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string rowFilter = cbbBarcode.SelectedValue.ToString();
            string rowFilter = cbbBarcode.Text;
            SearchReport();
            for (int i = 0; i < dgwModelList.RowCount; i++)
            {
                if (!dgwModelList[2, i].Value.ToString().Contains(rowFilter))
                {
                    dgwModelList.Rows[i].Visible = false;
                }
            }
        }

        RectangleF watchArea = new RectangleF();
        SizeF offset = new SizeF();
        bool sellected = false;
        private void pbResultImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (sellected)
                {
                    sellected = false;
                    if (File.Exists(modelProgram.ModelImageResult))
                    {
                        using (var fs = new System.IO.FileStream(modelProgram.ModelImageResult, System.IO.FileMode.Open))
                        {
                            var bmp = new Bitmap(fs);
                            resultimage = (Bitmap)bmp.Clone();
                        }
                    }
                    pbResultImage.Image = resultimage;
                    pbResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                watchArea.Width = 1;
                watchArea.Height = 1;
                watchArea.X = e.X;
                watchArea.Y = e.Y;
                pbResultImage.Invalidate();
            }

            if (e.Button == MouseButtons.Right)
            {
                if (watchArea.Contains(e.X, e.Y))
                {
                    offset.Width = e.X - watchArea.X;
                    offset.Height = e.Y - watchArea.Y;
                }
            }
        }

        private void pbResultImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                watchArea.Width = e.X - watchArea.X;
                watchArea.Height = e.Y - watchArea.Y;
                pbResultImage.Invalidate();
            }
            
            if(e.Button == MouseButtons.Right && sellected)
            {
               watchArea.X = e.X - offset.Width;
               watchArea.Y = e.Y - offset.Height;
               pbResultImage.Invalidate();
            }
        }

        private void pbResultImage_MouseUp(object sender, MouseEventArgs e)
        {
            Image imageResult = (Bitmap)resultimage.Clone();
            if (imageResult != null)
            {
                Bitmap bitmap = (Bitmap)imageResult.Clone();
                if (watchArea.Size.Width > 10 && watchArea.Size.Height > 10)
                {
                    float scaleX = (float)bitmap.Size.Width / (float)pbResultImage.Size.Width;
                    float scaleY = (float)bitmap.Size.Height / (float)pbResultImage.Size.Height;
                    RectangleF rectangle = new RectangleF(watchArea.X * scaleX, watchArea.Y * scaleY, watchArea.Width * scaleX, watchArea.Height* scaleY);
                    Bitmap bitmapBuffer = new Bitmap((int)rectangle.Width, (int)rectangle.Height);

                    using (Graphics g = Graphics.FromImage(bitmapBuffer))
                    {
                        g.DrawImage(bitmap, new RectangleF(0, 0, bitmapBuffer.Width, bitmapBuffer.Height), rectangle, GraphicsUnit.Pixel);
                    }
                    pbResultImage.SizeMode = PictureBoxSizeMode.Zoom;
                    if (pbResultImage.Image != null)
                    {
                        pbResultImage.Image.Dispose();
                    }
                    pbResultImage.Image = bitmapBuffer;
                    sellected = true;
                }
                
            }
        }

        readonly Pen Pen = new Pen(Color.Cyan);
        private void PbResultImage_Paint(object sender, PaintEventArgs e)
        {
            if (watchArea.Width > 10 && watchArea.Height > 10 && !sellected)
            {
                e.Graphics.DrawRectangle(Pen,watchArea.X, watchArea.Y, watchArea.Width, watchArea.Height);
            }
        }
    }
}
