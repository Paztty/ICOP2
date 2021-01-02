using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICOP
{
    public partial class MainForm : Form
    {
        #region Form control
        public MainForm()
        {
            InitializeComponent();
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
        //Close-Maximize-Minimize
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        //Remove transparent border in maximized state
        private void FormMainMenu_Resize(object sender, EventArgs e)
        {
        }
        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
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
        #endregion


        #region Variable
        ModelProgram modelProgram = new ModelProgram();
        #endregion



        private void Main_Load(object sender, EventArgs e)
        {
            Main.StartingForm startingForm = new Main.StartingForm();
            startingForm.Show();
            //Thread.Sleep(5000);
            startingForm.Hide();
            Console.WriteLine("adsasdsa");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ModelForm modelForm = new ModelForm();
            if (modelForm.ShowDialog() == DialogResult.OK)
            {
                
                string modelJson = File.ReadAllText(modelForm.pathModel);
                modelProgram = JsonSerializer.Deserialize<ModelProgram>(modelJson);
                lbModelName.Text = modelProgram.ModelName;
                using (var fs = new System.IO.FileStream(modelProgram.ModelImagePathCam0, System.IO.FileMode.Open))
                {
                    var bmp = new Bitmap(fs);
                    pbICam0.Image = (Bitmap)bmp.Clone();
                    modelProgram.ModelImageCam0 = (Bitmap)bmp.Clone();
                }
                using (var fs = new System.IO.FileStream(modelProgram.ModelImagePathCam1, System.IO.FileMode.Open))
                {
                    var bmp = new Bitmap(fs);
                    pbICam1.Image = (Bitmap)bmp.Clone();
                    modelProgram.ModelImageCam1 = (Bitmap)bmp.Clone();
                }
                using (var fs = new System.IO.FileStream(modelProgram.ModelImagePathCam2, System.IO.FileMode.Open))
                {
                    var bmp = new Bitmap(fs);
                    pbICam2.Image = (Bitmap)bmp.Clone();
                    modelProgram.ModelImageCam2 = (Bitmap)bmp.Clone();
                }
                using (var fs = new System.IO.FileStream(modelProgram.ModelImagePathCam3, System.IO.FileMode.Open))
                {
                    var bmp = new Bitmap(fs);
                    pbICam3.Image = (Bitmap)bmp.Clone();
                    modelProgram.ModelImageCam3 = (Bitmap)bmp.Clone();
                }

                dgwProgram.Rows.Clear();
                for (int i = 0; i < modelProgram.modelSteps.Count; i++)
                {
                    modelProgram.modelSteps[i].addToDataView(i,dgwProgram);
                    modelProgram.modelSteps[i].loadStepImage();
                }
                dgwProgram.Refresh();
                lbModelName.Text = modelProgram.ModelName;

                pbICam0.Invalidate();
                pbICam1.Invalidate();
                pbICam2.Invalidate();
                pbICam3.Invalidate();
            }
        }

        private void button3_MouseUp(object sender, MouseEventArgs e)
        {

        }
    }

    public static class Global
    {
        public const int maxWidth = 100000;
        public const int maxHeight = 80000;
        public class Positions
        {
            public const string ICam0 = "ICam0";
            public const string ICam1 = "ICam1";
            public const string ICam2 = "ICam2";
            public const string ICam3 = "ICam3";
        }
    }
}
