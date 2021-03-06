﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
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
        Global.Paint paint = new Global.Paint
        {
            brush = new SolidBrush(Color.Cyan),
            Font = new Font("Microsoft YaHei UI", 8, FontStyle.Bold),
            pen = new Pen(Color.Cyan, 2)
        };
        Global.Paint paint_OK = new Global.Paint
        {
            brush = new SolidBrush(Color.Green),
            Font = new Font("Microsoft YaHei UI", 8, FontStyle.Bold),
            pen = new Pen(Color.Green, 2)
        };
        Global.Paint paint_NG = new Global.Paint
        {
            brush = new SolidBrush(Color.Red),
            Font = new Font("Microsoft YaHei UI", 8, FontStyle.Bold),
            pen = new Pen(Color.Red, 2)
        };

        private int activeStep = 0;
        private int failStepCount = 0;
        string[] fileImageArrays = Directory.GetFiles(@"D:\DEV_PJT\ICOPv2\ICOPExample", "*cam1.bmp");
        int imageCount = 0;
        private bool IsTesting = false;

        Statistical Statistical;

        public User.Account OP_Acc = new User.Account() {userName = "Default OP"};
        public User.Account TE_Acc;
        public User.Account MT_Acc;
        public User.Account activeAcc;
        public int CharCircle = 0;

        Image ICAM0;
        Image ICAM1;
        Image ICAM2;
        Image ICAM3;

        DateTime finishTest = DateTime.Now;
        bool testTurnChecker = true;
        #endregion

        public void messengerIcop(string messenger)
        {
            IcopMessenger icopMessenger = new IcopMessenger(messenger);
            icopMessenger.ShowDialog();
        }

        public User.ICopPermision Login(bool isStart)
        {
            User.ICopPermision permision = User.ICopPermision.None;
            IcopLogin icopLogin = new IcopLogin(isStart);
            icopLogin.ShowDialog();
            if (icopLogin.DialogResult == DialogResult.OK)
            {
                permision = icopLogin.permision;
                activeAcc = icopLogin.Acc;
                switch (permision)
                {
                    case User.ICopPermision.None:
                        break;
                    case User.ICopPermision.OP:
                        break;
                    case User.ICopPermision.Technical:
                        TE_Acc = icopLogin.Acc;
                        break;
                    case User.ICopPermision.Master:
                        MT_Acc = icopLogin.Acc;
                        break;
                    default:
                        break;
                }
            }
            return permision;
        }

        private void Main_Load(object sender, EventArgs e)
        {

            Main.StartingForm startingForm = new Main.StartingForm();
            startingForm.Show();
            // Creat folder

            Global.DailyFolerCreat();

            if (!Directory.Exists(Global.ICOP_sw_path)) Directory.CreateDirectory(Global.ICOP_sw_path);
            if (!Directory.Exists(Global.ICOP_history_path)) Directory.CreateDirectory(Global.ICOP_history_path);
            if (!Directory.Exists(Global.ICOP_sample_path)) Directory.CreateDirectory(Global.ICOP_sample_path);
            if (!Directory.Exists(Global.ICOP_model_path)) Directory.CreateDirectory(Global.ICOP_model_path);
            if (!Directory.Exists(Global.ICOP_setting_path)) Directory.CreateDirectory(Global.ICOP_setting_path);
            if (!Directory.Exists(Global.ICOP_Statitic_path)) Directory.CreateDirectory(Global.ICOP_Statitic_path);

            Global.IO_Port.IO_COM_Port.DataReceived += IO_COM_Port_DataReceived;

            Global.GetSetting();
            string[] PortName = System.IO.Ports.SerialPort.GetPortNames();
            for (int i = 0; i < PortName.Length; i++)
            {
                if (PortName[i] == Global.ICOP_setting_data.IO_Port.IO_COM_Port.PortName)
                {
                    Global.IO_Port.Set_IO_Port(PortName[i]);
                    break;
                }
            }
            

            if (File.Exists(Global.ICOP_sw_path + "cldys.ic"))
            {
                string userAdded = File.ReadAllText(Global.ICOP_sw_path + "cldys.ic");
                Global.users = JsonSerializer.Deserialize<User>(userAdded);
            }
            else
            {
                Global.users = new User();
                Global.users.update_change();
            }
            // encread folder

            // check statitis file
            Statistical = new Statistical();

            if (File.Exists(Global.ICOP_Statitic_path + Statistical.StatisticalFileName))
            {
                string statisticJson = File.ReadAllText(Global.ICOP_Statitic_path + Statistical.StatisticalFileName);
                try
                {
                    Statistical = JsonSerializer.Deserialize<Statistical>(statisticJson);
                    Statistical.Statistical_Init_Counter(lbCounterFail, lbCounterPass, lbCounterTotal, lbCounterPercent);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
            else
            {
                Statistical.Statistical_Save();
            }

            startingForm.Hide();
            lbSupport.Hide();
            btManualPass.Enabled = false;

            //Login(true);

            timerUpdateChar.Start();
            pbChart.BackgroundImage = null;
        }

        private void IO_COM_Port_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (Global.IO_Port.IO_COM_Port.IsOpen)
            {
                try
                {
                    Global.convayerStatus = Global.IO_Port.IO_COM_Port.ReadLine();
                    //Console.WriteLine(Global.convayerStatus);
                }
                catch (Exception ER) {
                    Console.WriteLine(ER);
                }
            }
        }

        private void btModel_Click(object sender, EventArgs e)
        {
            User.ICopPermision permision = Login(false);

            if (permision == User.ICopPermision.Technical || permision == User.ICopPermision.Master)
            {
                ModelForm modelForm = new ModelForm(modelProgram.MotherFolder + modelProgram.ModelName + ".imdl", activeAcc);
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
                    Global.addToLog("load step image of model " + modelProgram.ModelName);
                    for (int i = 0; i < modelProgram.modelSteps.Count; i++)
                    {
                        modelProgram.modelSteps[i].loadStepImage();
                        modelProgram.modelSteps[i].addToDataView(i, dgwProgram);
                    }
                    dgwProgram.Refresh();
                    lbModelName.Text = modelProgram.ModelName;
                    tlpCamview.BackgroundImage = null;
                    modelProgram.loaded = true;
                }
            }
        }

        private void pbICam_Paint(object sender, PaintEventArgs e)
        {
            if (modelProgram.loaded)
            {
                string pictureBoxIcam = ((PictureBox)sender).Name;
                switch (pictureBoxIcam)
                {
                    case "pbICam0":
                        for (int i = 0; i < modelProgram.modelSteps.Count; i++)
                        {
                            if (modelProgram.modelSteps[i].Result == Global.ICOP_tester_OK)
                                paint = paint_OK;
                            else
                                paint = paint_NG;
                            if (modelProgram.modelSteps[i].Position == Global.Positions.ICam0 && modelProgram.modelSteps[i].Skip == false)
                            {
                                RectangleF rectangleF = modelProgram.modelSteps[i].ForDraw(pbICam0);
                                e.Graphics.DrawRectangle(paint.pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
                                e.Graphics.DrawString
                                    (
                                    modelProgram.modelSteps[i].Name,
                                    paint.Font,
                                    paint.brush,
                                    modelProgram.modelSteps[i].ForDraw(pbICam0).X,
                                    modelProgram.modelSteps[i].ForDraw(pbICam0).Y - (paint.Font.Height)
                                    );
                            }
                        }
                        break;
                    case "pbICam1":
                        for (int i = 0; i < modelProgram.modelSteps.Count; i++)
                        {
                            if (modelProgram.modelSteps[i].Result == Global.ICOP_tester_OK)
                                paint = paint_OK;
                            else
                                paint = paint_NG;
                            if (modelProgram.modelSteps[i].Position == Global.Positions.ICam1 && modelProgram.modelSteps[i].Skip == false)
                            {
                                RectangleF rectangleF = modelProgram.modelSteps[i].ForDraw(pbICam1);
                                e.Graphics.DrawRectangle(paint.pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
                                e.Graphics.DrawString
                                    (
                                    modelProgram.modelSteps[i].Name,
                                    paint.Font,
                                    paint.brush,
                                    modelProgram.modelSteps[i].ForDraw(pbICam1).X,
                                    modelProgram.modelSteps[i].ForDraw(pbICam1).Y - (paint.Font.Height)
                                    );
                            }
                        }
                        break;
                    case "pbICam2":
                        for (int i = 0; i < modelProgram.modelSteps.Count; i++)
                        {
                            if (modelProgram.modelSteps[i].Result == Global.ICOP_tester_OK)
                                paint = paint_OK;
                            else
                                paint = paint_NG;
                            if (modelProgram.modelSteps[i].Position == Global.Positions.ICam2 && modelProgram.modelSteps[i].Skip == false)
                            {
                                RectangleF rectangleF = modelProgram.modelSteps[i].ForDraw(pbICam2);
                                e.Graphics.DrawRectangle(paint.pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
                                e.Graphics.DrawString
                                    (
                                    modelProgram.modelSteps[i].Name,
                                    paint.Font,
                                    paint.brush,
                                    modelProgram.modelSteps[i].ForDraw(pbICam2).X,
                                    modelProgram.modelSteps[i].ForDraw(pbICam2).Y - (paint.Font.Height)
                                    );
                            }
                        }
                        break;
                    case "pbICam3":
                        for (int i = 0; i < modelProgram.modelSteps.Count; i++)
                        {
                            if (modelProgram.modelSteps[i].Result == Global.ICOP_tester_OK)
                                paint = paint_OK;
                            else
                                paint = paint_NG;
                            if (modelProgram.modelSteps[i].Position == Global.Positions.ICam3 && modelProgram.modelSteps[i].Skip == false)
                            {
                                RectangleF rectangleF = modelProgram.modelSteps[i].ForDraw(pbICam3);
                                e.Graphics.DrawRectangle(paint.pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
                                e.Graphics.DrawString
                                    (
                                    modelProgram.modelSteps[i].Name,
                                    paint.Font,
                                    paint.brush,
                                    modelProgram.modelSteps[i].ForDraw(pbICam3).X,
                                    modelProgram.modelSteps[i].ForDraw(pbICam3).Y - (paint.Font.Height)
                                    );
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Interval = 3000;
            lbResult.Text = "TESTTING";
            lbResult.BackColor = Color.Blue;
            IsTesting = true;
            try
            {
                RunTest();
            }
            catch {
                RunTest();
            }
            
            IsTesting = false;
            lbResult.Text = modelProgram.result;
            CharCircle = 0;
            timerUpdateChar.Start();
            if (modelProgram.result == Global.ICOP_tester_NG)
            {
                lbResult.BackColor = Color.Red;
                this.ActiveControl = btLoadNextFailStep;
            }
            else
            {
                lbResult.BackColor = Color.Green;
                
                timer1.Start();
            }
        }

        public void RunTest()
        {
            if (testTurnChecker)
            {
                testTurnChecker = false;
            }
            else
            {
                if (modelProgram.result == Global.ICOP_tester_NG)
                {
                    Statistical.Statistical_Update(modelProgram.PBA_Count, modelProgram.result, lbCounterFail, lbCounterPass, lbCounterTotal, lbCounterPercent);
                    modelProgram.saveResult(finishTest, ICAM0, ICAM1, ICAM2, ICAM3);
                    modelProgram.SaveMESReport(finishTest, (int)Statistical.dailyToltal);
                    modelProgram.SaveTextReport(finishTest, OP_Acc);

                }
                testTurnChecker = true;
            }
            var startTime = DateTime.Now;

            pbSourceImage.Image = null;
            pbGetImage.Image = null;

            modelProgram.result = Global.ICOP_tester_OK;
            activeStep = 0;
            if (imageCount < fileImageArrays.Length - 1)
            {
                releasePictureBox();
                imageCount++;
                pbICam0.Image = Image.FromFile(fileImageArrays[imageCount]);
                pbICam1.Image = Image.FromFile(fileImageArrays[imageCount].Replace("cam1", "cam2"));
                pbICam2.Image = Image.FromFile(fileImageArrays[imageCount].Replace("cam1", "cam3"));
                pbICam3.Image = Image.FromFile(fileImageArrays[imageCount].Replace("cam1", "cam4"));

                ICAM0 = Image.FromFile(fileImageArrays[imageCount]);
                ICAM1 = Image.FromFile(fileImageArrays[imageCount].Replace("cam1", "cam2"));
                ICAM2 = Image.FromFile(fileImageArrays[imageCount].Replace("cam1", "cam3"));
                ICAM3 = Image.FromFile(fileImageArrays[imageCount].Replace("cam1", "cam4"));
            }

            for (int i = 0; i < modelProgram.modelSteps.Count; i++)
            {
                if (modelProgram.modelSteps[i].Skip)
                {
                    dgwProgram[6, i].Value = "Skip";
                    dgwProgram[7, i].Value = "Skip";
                    modelProgram.modelSteps[i].Value = "SKIP";
                    modelProgram.modelSteps[i].Result = "SKIP";
                }
                else
                {
                    switch (modelProgram.modelSteps[i].Func)
                    {
                        case ModelProgram.IcopFunction.CODT:
                            if (!modelProgram.modelSteps[i].Skip)
                            {
                                switch (modelProgram.modelSteps[i].Position)
                                {
                                case Global.Positions.ICam0:
                                    modelProgram.CODT(modelProgram.modelSteps[i], modelProgram.modelSteps[i].getImage((Bitmap)ICAM0));
                                    break;
                                case Global.Positions.ICam1:
                                    modelProgram.CODT(modelProgram.modelSteps[i], modelProgram.modelSteps[i].getImage((Bitmap)ICAM1));
                                    break;
                                case Global.Positions.ICam2:
                                    modelProgram.CODT(modelProgram.modelSteps[i], modelProgram.modelSteps[i].getImage((Bitmap)ICAM2));
                                    break;
                                case Global.Positions.ICam3:
                                    modelProgram.CODT(modelProgram.modelSteps[i], modelProgram.modelSteps[i].getImage((Bitmap)ICAM3));
                                    break;
                                default:
                                    break;
                                }
                            }
                            break;
                        case ModelProgram.IcopFunction.SDDT:
                            break;
                        case ModelProgram.IcopFunction.QRDT:
                            if (!modelProgram.modelSteps[i].Skip)
                            {
                                switch (modelProgram.modelSteps[i].Position)
                                {
                                    case Global.Positions.ICam0:
                                        modelProgram.QRDT(modelProgram.modelSteps[i], modelProgram.modelSteps[i].getImage((Bitmap)ICAM0));
                                        break;
                                    case Global.Positions.ICam1:
                                        modelProgram.QRDT(modelProgram.modelSteps[i], modelProgram.modelSteps[i].getImage((Bitmap)ICAM1));
                                        break;
                                    case Global.Positions.ICam2:
                                        modelProgram.QRDT(modelProgram.modelSteps[i], modelProgram.modelSteps[i].getImage((Bitmap)ICAM2));
                                        break;
                                    case Global.Positions.ICam3:
                                        modelProgram.QRDT(modelProgram.modelSteps[i], modelProgram.modelSteps[i].getImage((Bitmap)ICAM3));
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    dgwProgram[6, i].Value = modelProgram.modelSteps[i].Value;
                    dgwProgram[7, i].Value = modelProgram.modelSteps[i].Result;
                    if (modelProgram.modelSteps[i].Result == Global.ICOP_tester_NG)
                    {
                        modelProgram.result = Global.ICOP_tester_NG;
                    }
                }
            }

            finishTest = DateTime.Now;

            if (modelProgram.result == Global.ICOP_tester_OK)
            {
                Statistical.Statistical_Update(modelProgram.PBA_Count, modelProgram.result, lbCounterFail, lbCounterPass, lbCounterTotal, lbCounterPercent);
                modelProgram.saveResult(finishTest, ICAM0, ICAM1, ICAM2, ICAM3);
                modelProgram.SaveMESReport(finishTest, (int)Statistical.dailyToltal);
                modelProgram.SaveTextReport(finishTest, OP_Acc);
            }
            GC.Collect();
            failStepCount = 0;
        }

        private void DgwStepsProgram_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (!IsTesting)
            {
                // For any other operation except, StateChanged, do nothing
                if (e.StateChanged != DataGridViewElementStates.Selected) return;
                int i = e.Row.Index;
                if (dgwProgram.Rows.Count > 0 && activeStep != i)
                {
                    dgwProgram.CurrentCell = dgwProgram[0, i];
                    activeStep = i;
                    switch (modelProgram.modelSteps[i].Func)
                    {
                        case ModelProgram.IcopFunction.CODT:
                            if (!modelProgram.modelSteps[i].Skip)
                            {
                                switch (modelProgram.modelSteps[i].Position)
                                {
                                    case Global.Positions.ICam0:
                                        pbSourceImage.Image = modelProgram.CODT(modelProgram.modelSteps[i], modelProgram.modelSteps[i].getImage((Bitmap)ICAM0));
                                        pbGetImage.Image = modelProgram.modelSteps[i].getImage((Bitmap)pbICam0.Image);
                                        break;
                                    case Global.Positions.ICam1:
                                        pbSourceImage.Image = modelProgram.CODT(modelProgram.modelSteps[i], modelProgram.modelSteps[i].getImage((Bitmap)ICAM1));
                                        pbGetImage.Image = modelProgram.modelSteps[i].getImage((Bitmap)pbICam1.Image);
                                        break;
                                    case Global.Positions.ICam2:
                                        pbSourceImage.Image = modelProgram.CODT(modelProgram.modelSteps[i], modelProgram.modelSteps[i].getImage((Bitmap)ICAM2));
                                        pbGetImage.Image = modelProgram.modelSteps[i].getImage((Bitmap)pbICam2.Image);
                                        break;
                                    case Global.Positions.ICam3:
                                        pbSourceImage.Image = modelProgram.CODT(modelProgram.modelSteps[i], modelProgram.modelSteps[i].getImage((Bitmap)ICAM3));
                                        pbGetImage.Image = modelProgram.modelSteps[i].getImage((Bitmap)pbICam3.Image);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case ModelProgram.IcopFunction.QRDT:
                            pbSourceImage.Image = null;
                            if (!modelProgram.modelSteps[i].Skip)
                            {
                                switch (modelProgram.modelSteps[i].Position)
                                {
                                    case Global.Positions.ICam0:
                                        pbGetImage.Image = modelProgram.modelSteps[i].getImage((Bitmap)ICAM0);
                                        break;
                                    case Global.Positions.ICam1:
                                        pbGetImage.Image = modelProgram.modelSteps[i].getImage((Bitmap)ICAM1);
                                        break;
                                    case Global.Positions.ICam2:
                                        pbGetImage.Image = modelProgram.modelSteps[i].getImage((Bitmap)ICAM2);
                                        break;
                                    case Global.Positions.ICam3:
                                        pbGetImage.Image = modelProgram.modelSteps[i].getImage((Bitmap)ICAM3);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                    }
                }
            }
        }

        private void releasePictureBox()
        {
            if (pbICam0.Image != null)
                pbICam0.Image.Dispose();
            if (pbICam1.Image != null)
                pbICam1.Image.Dispose();
            if (pbICam2.Image != null)
                pbICam2.Image.Dispose();
            if (pbICam3.Image != null)
                pbICam3.Image.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!modelProgram.loaded)
            {
                messengerIcop("No model loaded!");
            }
            else
            {
                timer1.Interval = 50;
                timer1.Start();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
        private void dgwProgram_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgwProgram.Rows[e.RowIndex].Selected = true;
        }

        private void btAddReference_Click(object sender, EventArgs e)
        {
            modelProgram.AddStepImageReference(modelProgram.modelSteps[activeStep], (Bitmap)pbGetImage.Image);
            messengerIcop("Add a new image to referent source.");
            this.ActiveControl = btLoadNextFailStep;
        }

        private void NextFailStep_Click(object sender, EventArgs e)
        {
            failStepCount++;
            if (failStepCount < modelProgram.modelSteps.Count)
            {
                while (modelProgram.modelSteps[failStepCount].Result == Global.ICOP_tester_OK && failStepCount < modelProgram.modelSteps.Count - 1 || modelProgram.modelSteps[failStepCount].Skip)
                {
                    failStepCount++;
                }
                if (modelProgram.modelSteps[failStepCount].Result == Global.ICOP_tester_NG)
                {
                    activeStep = failStepCount;
                    dgwProgram.Rows[failStepCount].Selected = true;
                    pbSourceImage.Image = modelProgram.modelSteps[failStepCount].sourceImage;
                    pbGetImage.Image = modelProgram.modelSteps[failStepCount].templateImage;
                }
            }
            else if (failStepCount >= modelProgram.modelSteps.Count - 1)
            {
                failStepCount = 0;
                btManualPass.Enabled = true;
                this.ActiveControl = btManualPass;
            }
        }
        private void btManualPass_Click(object sender, EventArgs e)
        {
            if (modelProgram.result == Global.ICOP_tester_NG)
            {
                modelProgram.result = Global.ICOP_tester_OK;
                Statistical.Statistical_Update(modelProgram.PBA_Count, modelProgram.result, lbCounterFail, lbCounterPass, lbCounterTotal, lbCounterPercent);
                modelProgram.result = Global.ICOP_tester_OP_PASS;
                modelProgram.saveResult(finishTest, ICAM0, ICAM1, ICAM2, ICAM3);
                modelProgram.SaveMESReport(finishTest, (int)Statistical.dailyToltal);
                modelProgram.SaveTextReport(finishTest, OP_Acc);
                timer1.Start();
            }
            btManualPass.Enabled = false;
        }

        private void btChangeSpect_Click(object sender, EventArgs e)
        {
            modelProgram.ChangerSpectvalue(modelProgram.modelSteps[activeStep], modelProgram.modelSteps[activeStep].Value);
            dgwProgram[5, activeStep].Value = modelProgram.modelSteps[activeStep].Spect;
            this.ActiveControl = btLoadNextFailStep;
        }

        private void KeyDown_Event_Preview(object sender, PreviewKeyDownEventArgs e)
        {
            
        }

        private void lbSupport_Click(object sender, EventArgs e)
        {
            lbSupport.Hide();
        }

        private void btCallSupport_Click(object sender, EventArgs e)
        {
            lbSupport.BringToFront();
            lbSupport.Show();
        }

        private void timerUpdateChar_Tick(object sender, EventArgs e)
        {
            if (CharCircle <= 360)
            {
                Statistical.DrawChart(pbChart, CharCircle);
                CharCircle = CharCircle + (360 - CharCircle) / 50 + 1;
                timerUpdateChar.Start();
            }
            else
            {
                timerUpdateChar.Stop();
                //timerUpdateChar.Dispose();
            }
        }

        public bool IsFormOpen(Form form)
        {
            FormCollection fc = Application.OpenForms;
            bool bFormNameOpen = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == form.Name)
                {
                    bFormNameOpen = true;
                }
            }
            return bFormNameOpen;
        }
        private void btReportManager_Click(object sender, EventArgs e)
        {
            ReportForm reportForm = new ReportForm();
            if (!IsFormOpen(reportForm))
            {
                reportForm.Show();
            }  
        }

        private void btOpenSetting_Click(object sender, EventArgs e)
        {
            MainSetting mainSetting = new MainSetting();
            if (!IsFormOpen(mainSetting))
            {
                mainSetting.Show();
            }
        }
    }
}
