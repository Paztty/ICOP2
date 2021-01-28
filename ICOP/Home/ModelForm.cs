using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows.Forms;

namespace ICOP
{
    public partial class ModelForm : Form
    {
        public string pathToProgram = "";
        User.Account GetUser;
        public ModelForm(string pathToProgram, User.Account user)
        {
            InitializeComponent();
            this.AcceptButton = btApplyStep;
            this.ActiveControl = tbName;
            this.pathToProgram = pathToProgram;
            this.GetUser = user;
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
        private void FormMainMenu_Resize(object sender, EventArgs e)
        {
        }
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

        DetectArea detectArea = new DetectArea();
        ModelProgram modelProgram = new ModelProgram();
        public int activeStep = 0;
        static Color nameColor = Color.Cyan;
        SolidBrush brushName = new SolidBrush(nameColor);
        Font nameFont = new Font("Microsoft YaHei UI", 10, FontStyle.Bold);

        public void messengerIcop(string messenger)
        {
            IcopMessenger icopMessenger = new IcopMessenger(messenger);
            icopMessenger.ShowDialog();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Console.WriteLine(pathToProgram);
            if (pathToProgram != ".imdl")
            {
                try
                {
                    FileInfo fileModel = new FileInfo(pathToProgram);
                    String Path = fileModel.DirectoryName + @"\";
                    string modelJson = File.ReadAllText(pathToProgram);
                    try
                    {
                        modelProgram = JsonSerializer.Deserialize<ModelProgram>(modelJson);
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }
                    modelProgram.MotherFolder = Path;
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

                    dgwStepsProgram.Rows.Clear();
                    for (int i = 0; i < modelProgram.modelSteps.Count; i++)
                    {
                        modelProgram.modelSteps[i].addToDataStepView(i, dgwStepsProgram);
                        modelProgram.modelSteps[i].loadStepImage();
                    }
                    dgwStepsProgram.Refresh();
                    lbModelName.Text = modelProgram.ModelName;
                    pbICam0.Invalidate();
                    pbICam1.Invalidate();
                    pbICam2.Invalidate();
                    pbICam3.Invalidate();
                }
                catch (Exception err) {
                    Console.WriteLine(err.Message);
                    Console.WriteLine(pathModel);
                }
            }
            else
            {
                for (int i = 0; i < modelProgram.modelSteps.Count; i++)
                {
                    modelProgram.modelSteps[i].addToDataStepView(i, dgwStepsProgram);
                }
                modelProgram.modelSteps[modelProgram.modelSteps.Count - 1].addToControlView(tbName, lbPosition, cbbPBA, cbbFunc, tbSpect, cbSkip);
                activeStep = modelProgram.modelSteps.Count - 1;
            }
        }


        private void btCreatModel_Click(object sender, EventArgs e)
        {
            ModelWizar modelWizar = new ModelWizar(GetUser);
            modelWizar.ShowDialog();
            if (modelWizar.DialogResult == DialogResult.OK)
            {
                dgwStepsProgram.Rows.Clear();
                modelProgram = modelWizar.model;
                lbModelName.Text = modelProgram.ModelName;
                Directory.CreateDirectory(Global.ICOP_model_path + modelProgram.PCB_Code + @"\" + modelProgram.ModelName + @"\" + modelProgram.ModelName + "_Image");
                string Path = Global.ICOP_model_path + modelProgram.PCB_Code + @"\" + modelProgram.ModelName + @"\";
                modelProgram.MotherFolder = Path;
                activeStep = 0;
                cbbPBA.Items.Clear();
                for (int i = 0; i < modelProgram.PBA_Count; i++)
                {
                    cbbPBA.Items.Add("PBA " + (i + 1));
                }
                for (int i = 0; i < modelProgram.modelSteps.Count; i++)
                {
                    modelProgram.modelSteps[i].addToDataStepView(i, dgwStepsProgram);
                }
                modelProgram.modelSteps[modelProgram.modelSteps.Count - 1].addToControlView(tbName, lbPosition, cbbPBA, cbbFunc, tbSpect, cbSkip);
                activeStep = modelProgram.modelSteps.Count - 1;
                modelProgram.Save();
                pbICam0.Invalidate();
                pbICam1.Invalidate();
                pbICam2.Invalidate();
                pbICam3.Invalidate();
            }
        }

        private void btAddStep_Click(object sender, EventArgs e)
        {
            int newComponentCount = 0;
            for (int i = 0; i < modelProgram.modelSteps.Count; i++)
            {
                if (modelProgram.modelSteps[i].Name.Contains("new component"))
                    newComponentCount++;
            }
            string newComponentCountString = newComponentCount.ToString();
            if (newComponentCount == 0) newComponentCountString = "";
            modelProgram.modelSteps.Add(new ModelProgram.ModelStep
            {
                Name = "new component " + newComponentCountString,
                Position = Global.Positions.ICam0,
                Func = ModelProgram.IcopFunction.CODT,
                Spect = "0.5",
                Skip = false,
            });
            modelProgram.modelSteps[modelProgram.modelSteps.Count - 1].addToDataStepView(modelProgram.modelSteps.Count - 1, dgwStepsProgram);
            modelProgram.modelSteps[modelProgram.modelSteps.Count - 1].addToControlView(tbName, lbPosition, cbbPBA, cbbFunc, tbSpect, cbSkip);
            if (dgwStepsProgram.Rows.Count > 0)
                dgwStepsProgram.CurrentCell = dgwStepsProgram[0, activeStep];
        }

        private void btSaveModel_Click(object sender, EventArgs e)
        {
            Console.WriteLine(modelProgram.MotherFolder);
            IcopConfirm icopConfirm = new IcopConfirm("Do you want to update this model ?");
            if (icopConfirm.ShowDialog() == DialogResult.OK)
            {
                modelProgram.ModelImageCam0 = pbICam0.Image;
                modelProgram.ModelImageCam1 = pbICam1.Image;
                modelProgram.ModelImageCam2 = pbICam2.Image;
                modelProgram.ModelImageCam3 = pbICam3.Image;
                modelProgram.Save();
            }

        }
        private void btApplyStep_Click(object sender, EventArgs e)
        {
            bool isExitComponent = false;
            for (int i = 0; i < activeStep; i++)
            {
                if (tbName.Text == dgwStepsProgram[1,i].Value.ToString() && (cbbPBA.SelectedIndex+1) == Convert.ToInt32(dgwStepsProgram[3, i].Value.ToString()))
                {
                    isExitComponent = true;
                }
            }
            if (isExitComponent)
            {
                messengerIcop(tbName.Text + " is aready exit !");
            }
            else
            {
                modelProgram.modelSteps[activeStep].applyFromControlView(tbName, lbPosition, cbbPBA, cbbFunc, tbSpect, cbSkip);
                modelProgram.modelSteps[activeStep].editDataStepView(activeStep, dgwStepsProgram);
                if (activeStep < modelProgram.modelSteps.Count - 1)
                {
                    activeStep++;
                    dgwStepsProgram.Rows[activeStep].Selected = true;
                    if (dgwStepsProgram.Rows.Count > 0)
                        dgwStepsProgram.CurrentCell = dgwStepsProgram[0, activeStep];
                    modelProgram.modelSteps[activeStep].addToControlView(tbName, lbPosition, cbbPBA, cbbFunc, tbSpect, cbSkip);
                    modelProgram.modelSteps[activeStep].imageSources[0].bitmap = (Bitmap)pbViewArea.Image;
                    for (int i = 0; i < modelProgram.modelSteps[activeStep].imageSources.Count; i++)
                    {
                        modelProgram.modelSteps[activeStep].imageSources.RemoveAt(i);
                    }
                    modelProgram.modelSteps[activeStep].imageSources.Add(new ModelProgram.ModelStep.imageSource());
                    modelProgram.modelSteps[activeStep].imageSources[0].bitmap = (Bitmap)pbViewArea.Image;
                }
                pbICam0.Invalidate();
                pbICam1.Invalidate();
                pbICam2.Invalidate();
                pbICam3.Invalidate();
            }
        }

        private void dgwStepsProgram_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dgwStepsProgram.Rows[e.RowIndex].Selected = true;
        }
        private void DgwStepsProgram_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            this.ActiveControl = dgwStepsProgram;
            // For any other operation except, StateChanged, do nothing
            if (e.StateChanged != DataGridViewElementStates.Selected) return;
            if (e.Row.Index != activeStep)
            {
                if (e.Row.Index < modelProgram.modelSteps.Count)
                {
                    activeStep = e.Row.Index;
                }
                else
                {
                    activeStep = modelProgram.modelSteps.Count - 1;
                }
                Console.WriteLine(activeStep);
                modelProgram.modelSteps[activeStep].addToControlView(tbName, lbPosition, cbbPBA, cbbFunc, tbSpect, cbSkip);
                tbName.Text = dgwStepsProgram[1, activeStep].Value.ToString();
                //DisponsePB(pbViewArea);
                for (int i = 0; i < modelProgram.modelSteps[activeStep].imageSources.Count; i++)
                {
                    if (modelProgram.modelSteps[activeStep].imageSources[i].bitmap != null)
                    {
                        try
                        {
                            pbViewArea.Image = modelProgram.modelSteps[activeStep].imageSources[i].bitmap;
                            break;
                        }
                        catch (Exception)
                        {
                        }

                    }
                }
            }
        }

        private void btClearStep_Click(object sender, EventArgs e)
        {
            if (activeStep >= 0)
            {
                try
                {
                    if (modelProgram.modelSteps[activeStep].Func != ModelProgram.IcopFunction.QRDT)
                    {
                        modelProgram.modelSteps.RemoveAt(activeStep);
                        if (dgwStepsProgram.Rows.Count > 0)
                            dgwStepsProgram.Rows.RemoveAt(activeStep);
                        for (int i = activeStep; i < dgwStepsProgram.Rows.Count; i++)
                        {
                            dgwStepsProgram[0, i].Value = i;
                        }
                    }
                    else
                    {
                        IcopMessenger messenger = new IcopMessenger("Can't delete QR code detect. If no need QR, Please skip it.");
                        messenger.ShowDialog();
                    }
                    pbICam0.Invalidate();
                    pbICam1.Invalidate();
                    pbICam2.Invalidate();
                    pbICam3.Invalidate();
                    activeStep = activeStep - 1;
                    if (activeStep < 0)
                        activeStep = 0;
                }
                catch (Exception) { }
            }
        }

        private void btOpenModel_Click(object sender, EventArgs e)
        {
            openFileModel.ShowDialog();
        }

        private void openFileModel_FileOk(object sender, CancelEventArgs e)
        {
            FileInfo fileModel = new FileInfo(openFileModel.FileName);
            String Path = fileModel.DirectoryName + @"\";
            string modelJson = File.ReadAllText(openFileModel.FileName);
            try
            {
                modelProgram = JsonSerializer.Deserialize<ModelProgram>(modelJson);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            cbbPBA.Items.Clear();
            for (int i = 0; i < modelProgram.PBA_Count; i++)
            {
                cbbPBA.Items.Add("PBA " + (i + 1));
            }
            modelProgram.MotherFolder = Path;
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

            dgwStepsProgram.Rows.Clear();
            for (int i = 0; i < modelProgram.modelSteps.Count; i++)
            {
                modelProgram.modelSteps[i].addToDataStepView(i, dgwStepsProgram);
                modelProgram.modelSteps[i].loadStepImage();
            }
            dgwStepsProgram.Refresh();
            lbModelName.Text = modelProgram.ModelName;
            pbICam0.Invalidate();
            pbICam1.Invalidate();
            pbICam2.Invalidate();
            pbICam3.Invalidate();
        }
        private void Icam_Mousedown(object sender, MouseEventArgs e)
        {
            string pictureBoxIcam = ((PictureBox)sender).Name;
            lbPosition.Text = pictureBoxIcam.Remove(0, 2);
            dgwStepsProgram[2, activeStep].Value = pictureBoxIcam.Remove(0, 2);
            pbICam0.Invalidate();
            pbICam1.Invalidate();
            pbICam2.Invalidate();
            pbICam3.Invalidate();
            switch (pictureBoxIcam)
            {
                case "pbICam0":
                    modelProgram.modelSteps[activeStep].Position = Global.Positions.ICam0;
                    modelProgram.modelSteps[activeStep].getLocation(e, pbICam0);
                    break;
                case "pbICam1":
                    modelProgram.modelSteps[activeStep].Position = Global.Positions.ICam1;
                    modelProgram.modelSteps[activeStep].getLocation(e, pbICam1);
                    break;
                case "pbICam2":
                    modelProgram.modelSteps[activeStep].Position = Global.Positions.ICam2;
                    modelProgram.modelSteps[activeStep].getLocation(e, pbICam2);
                    break;
                case "pbICam3":
                    modelProgram.modelSteps[activeStep].Position = Global.Positions.ICam3;
                    modelProgram.modelSteps[activeStep].getLocation(e, pbICam3);
                    break;
                default:
                    break;
            }
        }
        private void Icam_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                string pictureBoxIcam = ((PictureBox)sender).Name;
                Pen drawPen = new Pen(Color.Green, 2);
                switch (pictureBoxIcam)
                {
                    case "pbICam0":
                        modelProgram.modelSteps[activeStep].getSize(e, pbICam0);
                        pbICam0.Invalidate();
                        break;
                    case "pbICam1":
                        modelProgram.modelSteps[activeStep].getSize(e, pbICam1);
                        pbICam1.Invalidate();
                        break;
                    case "pbICam2":
                        modelProgram.modelSteps[activeStep].getSize(e, pbICam2);
                        pbICam2.Invalidate();
                        break;
                    case "pbICam3":
                        modelProgram.modelSteps[activeStep].getSize(e, pbICam3);
                        pbICam3.Invalidate();
                        break;
                    default:
                        break;
                }
            }
        }
        private void Icam_MouseUp(object sender, MouseEventArgs e)
        {
            string pictureBoxIcam = ((PictureBox)sender).Name;
            Pen drawPen = new Pen(Color.Green, 2);
            if (!modelProgram.modelSteps[activeStep].Skip)
            {
                switch (pictureBoxIcam)
                {
                    case "pbICam0":
                        DisponsePB(pbViewArea);
                        pbViewArea.Image = modelProgram.modelSteps[activeStep].getImageinArea((Bitmap)pbICam0.Image);
                        break;
                    case "pbICam1":
                        DisponsePB(pbViewArea);
                        pbViewArea.Image = modelProgram.modelSteps[activeStep].getImageinArea((Bitmap)pbICam1.Image);
                        break;
                    case "pbICam2":
                        DisponsePB(pbViewArea);
                        pbViewArea.Image = modelProgram.modelSteps[activeStep].getImageinArea((Bitmap)pbICam2.Image);
                        break;
                    case "pbICam3":
                        DisponsePB(pbViewArea);
                        pbViewArea.Image = modelProgram.modelSteps[activeStep].getImageinArea((Bitmap)pbICam3.Image);
                        break;
                    default:
                        break;
                }
            }
            this.ActiveControl = tbName;
            tbName.SelectAll();
        }
        private void pbICam_Paint(object sender, PaintEventArgs e)
        {
            string pictureBoxIcam = ((PictureBox)sender).Name;
            Pen drawPen = new Pen(Color.Cyan, 1);
            switch (pictureBoxIcam)
            {
                case "pbICam0":
                    for (int i = 0; i < modelProgram.modelSteps.Count; i++)
                    {
                        if (modelProgram.modelSteps[i].Position == Global.Positions.ICam0 && modelProgram.modelSteps[i].Skip == false)
                        {
                            RectangleF rectangleF = modelProgram.modelSteps[i].ForDraw(pbICam0);
                            e.Graphics.DrawRectangle(drawPen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
                            e.Graphics.DrawString
                                (
                                modelProgram.modelSteps[i].Name,
                                nameFont,
                                brushName,
                                modelProgram.modelSteps[i].ForDraw(pbICam0).X,
                                modelProgram.modelSteps[i].ForDraw(pbICam0).Y - (nameFont.Height)
                                );
                        }
                    }
                    break;
                case "pbICam1":
                    for (int i = 0; i < modelProgram.modelSteps.Count; i++)
                    {
                        if (modelProgram.modelSteps[i].Position == Global.Positions.ICam1 && modelProgram.modelSteps[i].Skip == false)
                        {
                            RectangleF rectangleF = modelProgram.modelSteps[i].ForDraw(pbICam1);
                            e.Graphics.DrawRectangle(drawPen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
                            e.Graphics.DrawString
                                (
                                modelProgram.modelSteps[i].Name,
                                nameFont,
                                brushName,
                                modelProgram.modelSteps[i].ForDraw(pbICam1).X,
                                modelProgram.modelSteps[i].ForDraw(pbICam1).Y - (nameFont.Height)
                                );
                        }
                    }
                    break;
                case "pbICam2":
                    for (int i = 0; i < modelProgram.modelSteps.Count; i++)
                    {
                        if (modelProgram.modelSteps[i].Position == Global.Positions.ICam2 && modelProgram.modelSteps[i].Skip == false)
                        {
                            RectangleF rectangleF = modelProgram.modelSteps[i].ForDraw(pbICam2);
                            e.Graphics.DrawRectangle(drawPen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
                            e.Graphics.DrawString
                                (
                                modelProgram.modelSteps[i].Name,
                                nameFont,
                                brushName,
                                modelProgram.modelSteps[i].ForDraw(pbICam2).X,
                                modelProgram.modelSteps[i].ForDraw(pbICam2).Y - (nameFont.Height)
                                );
                        }
                    }
                    break;
                case "pbICam3":
                    for (int i = 0; i < modelProgram.modelSteps.Count; i++)
                    {
                        if (modelProgram.modelSteps[i].Position == Global.Positions.ICam3 && modelProgram.modelSteps[i].Skip == false)
                        {
                            RectangleF rectangleF = modelProgram.modelSteps[i].ForDraw(pbICam3);
                            e.Graphics.DrawRectangle(drawPen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
                            e.Graphics.DrawString(modelProgram.modelSteps[i].Name,
                                nameFont,
                                brushName,
                                modelProgram.modelSteps[i].ForDraw(pbICam3).X,
                                modelProgram.modelSteps[i].ForDraw(pbICam3).Y - (nameFont.Height));
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        private void tbName_TextChanged(object sender, EventArgs e)
        {
            modelProgram.modelSteps[activeStep].Name = tbName.Text;
            switch (lbPosition.Text)
            {
                case "ICam0":
                    pbICam0.Invalidate();
                    break;
                case "ICam1":
                    pbICam1.Invalidate();
                    break;
                case "ICam2":
                    pbICam2.Invalidate();
                    break;
                case "ICam3":
                    pbICam3.Invalidate();
                    break;
                default:
                    break;
            }
        }

        private void btLoadImage_Click(object sender, EventArgs e)
        {
            openImageModel.ShowDialog();
        }

        private void openImageModel_FileOk(object sender, CancelEventArgs e)
        {
            if (pbICam0.Image != null)
                pbICam0.Image.Dispose();
            if (pbICam1.Image != null)
                pbICam1.Image.Dispose();
            if (pbICam2.Image != null)
                pbICam2.Image.Dispose();
            if (pbICam3.Image != null)
                pbICam3.Image.Dispose();
            string imagePath = openImageModel.FileName;
            pbICam0.Image = Image.FromFile(imagePath);
            pbICam1.Image = Image.FromFile(imagePath.Replace("cam1", "cam2"));
            pbICam2.Image = Image.FromFile(imagePath.Replace("cam1", "cam3"));
            pbICam3.Image = Image.FromFile(imagePath.Replace("cam1", "cam4"));
            pbICam0.Invalidate();
            pbICam1.Invalidate();
            pbICam2.Invalidate();
            pbICam3.Invalidate();
        }

        public void DisponsePB(PictureBox pictureBox)
        {
            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
            }
        }

        public string pathModel
        {
            get
            {
                return modelProgram.MotherFolder + modelProgram.ModelName + ".imdl";
            }
        }
        private void btUserModel_Click(object sender, EventArgs e)
        {
            if (modelProgram.ModelName != "ModelExample")
                this.DialogResult = DialogResult.OK;
            else
            {
                IcopMessenger messenger = new IcopMessenger("You need load model or creat new fist.");
                messenger.ShowDialog();
            }

        }
        private void btSaveAs_Click(object sender, EventArgs e)
        {
            saveModelAs.ShowDialog();
        }

        private void saveModelAs_FileOk(object sender, CancelEventArgs e)
        {
            FileInfo fileModel = new FileInfo(saveModelAs.FileName);
            lbModelName.Text = fileModel.Name;
            Directory.CreateDirectory(fileModel.DirectoryName + @"\" + fileModel.Name + @"\" + fileModel.Name + "_Image");
            String Path = fileModel.DirectoryName + @"\" + fileModel.Name + @"\";
            modelProgram.MotherFolder = Path;
            modelProgram.ModelName = lbModelName.Text;
            modelProgram.Save();
            pbICam0.Invalidate();
            pbICam1.Invalidate();
            pbICam2.Invalidate();
            pbICam3.Invalidate();
        }

        private void cbSkip_CheckedChanged(object sender, EventArgs e)
        {
            modelProgram.modelSteps[activeStep].Skip = cbSkip.Checked;
        }


    }
}
