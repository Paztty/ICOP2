using ICOP.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows.Forms;

namespace ICOP
{
    public partial class MainSetting : Form
    {
        #region Form init
        public MainSetting()
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
        #endregion


        #region Varialble

        //tab setting varialble
        public List<bool> Sensors = new List<bool>(4)
            {
                false,
                false,
                false,
                false
            };
        public List<bool> Stopers = new List<bool>(2)
            {
                false,
                false
            };
        public List<bool> TowerLight = new List<bool>(3)
            {
                false,
                false,
                false,
            };
        //end tab setting

        #endregion
        private void Main_Load(object sender, EventArgs e)
        {
        }

        private void activerGroubSetting(GroupBox group)
        {
            gbOtherSetting.Visible = false;
            gbSettingCamera.Visible = false;
            gbSettingIO.Visible = false;
            gbUserManager.Visible = false;
            group.Visible = true;
        }
        private void lbSetingOther_Click(object sender, EventArgs e)
        {
            timerUpdateConvayer.Stop();
            string objectName = ((Label)sender).Name;
            switch (objectName)
            {
                case "lbSetingCamera":
                    activerGroubSetting(gbSettingCamera);
                    break;
                case "lbSetingIO":
                    activerGroubSetting(gbSettingIO);
                    timerUpdateConvayer.Start();
                    break;
                case "lbSetingUser":
                    addNew = true;
                    accActiver = 0;
                    activerGroubSetting(gbUserManager);
                    cbbUserPermissions.Items.Clear();
                    foreach (var item in Enum.GetValues(typeof(User.ICopPermision)))
                    {
                        cbbUserPermissions.Items.Add(item);
                    }
                    cbbUserPermissions.SelectedIndex = 0;
                    Global.users.loadToView(dgwUsers);
                    if (Global.users.accList.Count >= 1)
                    {
                        Global.users.accList[0].LoadToControl(tbUserName, tbPass, cbbUserPermissions, tbName, tbEmployerCode);
                    }
                    addNew = false;
                    break;
                case "lbSetingOther":
                    activerGroubSetting(gbOtherSetting);
                    break;
                default:
                    break;
            }
        }
        #region Setting IO
        private void IO_Port_ManualTestEvent(object sender, EventArgs e)
        {
            string ctrl = ((Button)sender).Name;
            switch (ctrl)
            {
                case "btModeIn":
                    Global.IO_Port.IO_COM_Port.Write(((int)IO_Port.StatusCommand.ModeIn).ToString());
                    Console.WriteLine(((int)IO_Port.StatusCommand.ModeIn).ToString());
                    break;
                case "btModeOut":
                    Global.IO_Port.IO_COM_Port.Write(((int)IO_Port.StatusCommand.ModeOut).ToString());
                    Console.WriteLine(((int)IO_Port.StatusCommand.ModeOut).ToString());
                    break;
                case "btModePass":
                    Global.IO_Port.IO_COM_Port.Write(((int)IO_Port.StatusCommand.ModePass).ToString());
                    Console.WriteLine(((int)IO_Port.StatusCommand.ModePass).ToString());
                    break;
                case "btModeNG":
                    Global.IO_Port.IO_COM_Port.Write(((int)IO_Port.StatusCommand.ModeNG).ToString());
                    Console.WriteLine(((int)IO_Port.StatusCommand.ModeNG).ToString());
                    break;
                case "btModeUp":
                    Global.IO_Port.IO_COM_Port.Write(((int)IO_Port.StatusCommand.ModeUp).ToString());
                    Console.WriteLine(((int)IO_Port.StatusCommand.ModeUp).ToString());
                    break;
                case "btUpdateCom":
                    cbbSerialPortName.DataSource = System.IO.Ports.SerialPort.GetPortNames(); ;
                    break;

                default:
                    break;
            }
        }

        private void cbbSerialPortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Global.IO_Port.IO_COM_Port.IsOpen)
                Global.IO_Port.IO_COM_Port.Close();

            Global.IO_Port.IO_COM_Port.PortName = cbbSerialPortName.SelectedItem.ToString();
            Global.IO_Port.IO_COM_Port.Open();
        }
        string last_convayer_status;
        private void timerUpdateConvayer_Tick(object sender, EventArgs e)
        {
            if (last_convayer_status != Global.convayerStatus)
            {
                Console.WriteLine(Global.convayerStatus);
                last_convayer_status = Global.convayerStatus;
                clearOldImage();
                //ss1
                if (last_convayer_status[0] == '0')
                {
                    btSensor1.BackgroundImage = Resources.SensorOFF;
                }
                else
                {
                    btSensor1.BackgroundImage = Resources.SensorON;
                }
                //ss2
                if (last_convayer_status[6] == '0')
                {
                    btSensor2.BackgroundImage = Resources.SensorOFF;
                }
                else
                {
                    btSensor2.BackgroundImage = Resources.SensorON;
                }
                //ss3
                if (last_convayer_status[2] == '0')
                {
                    btSensor3.BackgroundImage = Resources.SensorOFF;
                }
                else
                {
                    btSensor3.BackgroundImage = Resources.SensorON;
                }
                //ss4
                if (last_convayer_status[5] == '0')
                {
                    btSensor4.BackgroundImage = Resources.SensorOFF;
                }
                else
                {
                    btSensor4.BackgroundImage = Resources.SensorON;
                }
                //stopper1
                if (last_convayer_status[1] == '1')
                {
                    lbStopper1.Image = Resources.CylinderOFF;
                }
                else
                {
                    lbStopper1.Image = Resources.CylinderOn;
                }
                //stoper2
                if (last_convayer_status[3] == '1')
                {
                    lbStopper2.Image = Resources.CylinderOFF;
                }
                else
                {
                    lbStopper2.Image = Resources.CylinderOn;
                }
                //led tower
                if (last_convayer_status[0] == '0')
                {
                    btSensor1.BackgroundImage = Resources.SensorOFF;
                }
                else
                {
                    btSensor1.BackgroundImage = Resources.SensorON;
                }
                GC.Collect();
            }
        }
        private void clearOldImage()
        {

            if (btSensor1.BackgroundImage != null) { btSensor1.BackgroundImage.Dispose(); }
            if (btSensor2.BackgroundImage != null) { btSensor1.BackgroundImage.Dispose(); }
            if (btSensor3.BackgroundImage != null) { btSensor1.BackgroundImage.Dispose(); }
            if (btSensor4.BackgroundImage != null) { btSensor1.BackgroundImage.Dispose(); }

            if (lbStopper1.Image != null) { lbStopper2.Image.Dispose(); }
            if (lbStopper2.Image != null) { lbStopper2.Image.Dispose(); }
        }
        #endregion

        #region User setting
        int accActiver = 0;
        bool addNew = false;
        private void UserManagerButtonEvent(object sender, EventArgs e)
        {
            var ctrButton = ((Button)sender).Name;
            switch (ctrButton)
            {
                case "btConfirmEdit":
                    string informationsText = "";
                    if (tbUserName.TextLength < 4)
                    {
                        informationsText += Environment.NewLine + "User name must has more than 4 character.";
                    }
                    else if (tbPass.Text != tbRePass.Text)
                    {
                        informationsText += Environment.NewLine + "Password not match.";
                    }
                    else if (cbbUserPermissions.SelectedIndex == (int)User.ICopPermision.None)
                    {
                        informationsText += Environment.NewLine + "Permission can't be 'none'.";
                    }
                    else if (tbName.TextLength < 4)
                    {
                        informationsText += Environment.NewLine + "Name so short.";
                    }
                    else if (tbEmployerCode.TextLength != 7) //7209115
                    {
                        informationsText += Environment.NewLine + "Employer code not correct.";
                    }

                    if (informationsText != "")
                    {
                        Global.users.accList[accActiver].getFromControl(tbUserName, tbPass, cbbUserPermissions, tbName, tbEmployerCode);
                        Global.users.updateChaneToView(dgwUsers, accActiver);
                    }
                    else
                    {
                        lbInformations.Text = "Informations :" + informationsText;
                    }
                    
                    break;
                case "btClearUser":
                    addNew = true;
                    Global.users.accList.RemoveAt(accActiver);
                    Global.users.loadToView(dgwUsers);
                    addNew = false;
                    break;
                case "btEditUser":
                    break;
                case "btAddUser":
                    addNew = true;
                    Global.users.accList.Add(new User.Account("user name", "", User.ICopPermision.None, "name", "employer code"));
                    Global.users.loadToView(dgwUsers);
                    addNew = false;
                    break;
                case "btApply":
                        Global.users.update_change();
                    break;
                case "btCancle":
                    addNew = true;
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
                    Global.users.loadToView(dgwUsers);
                    addNew = false;
                    break;
                default:
                    break;
            }
        }

        private void dgwUsers_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (!addNew)
            {
                if (e.Row.Index != accActiver)
                {
                    accActiver = e.Row.Index;
                    if (accActiver < Global.users.accList.Count)
                    {
                        Global.users.accList[accActiver].LoadToControl(tbUserName, tbPass, cbbUserPermissions, tbName, tbEmployerCode);
                    }
                }
            }
        }

        #endregion

        private void lbInformations_Click(object sender, EventArgs e)
        {

        }
    }
}
