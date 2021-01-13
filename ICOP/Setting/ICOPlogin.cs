using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ICOP
{
    
    public partial class IcopLogin : Form
    {
        public User.ICopPermision permision = User.ICopPermision.None;
        private bool isChangingPassword = false;
        public User.Account Acc = new User.Account();
        public IcopLogin()
        {
            InitializeComponent();

            Acc.userName = "none";
            for (int i = 0; i < Global.users.accList.Count; i++)
            {
                cbbAcc.Items.Add(Global.users.accList[i].userName);
            }
            pnChangePassword.BringToFront();
            pnChangePassword.Hide();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btCancle_Click(object sender, EventArgs e)
        {
            if (isChangingPassword)
            {
                pnChangePassword.Hide();
                isChangingPassword = false;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
            
        }

        private void lbChangePass_Click(object sender, EventArgs e)
        {
            if (Acc.userName != "none")
            {
                isChangingPassword = true;
                pnChangePassword.Show();
            }
            else
            {
                lbNotification.Text = "Please select user name fist!";
            }

        }

        private void cbbAcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            Acc = Global.users.accList[cbbAcc.SelectedIndex];
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (!isChangingPassword)
            {
                if (Acc.passWord == tbPassword.Text)
                {
                    permision = Acc.Permission;
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    lbNotification.Text = "Wrong password !";
                }
            }
            else
            {
                if (tbPasswordOld.Text == Acc.passWord)
                {
                    if (tbPasswordNew.Text == tbRePassNew.Text)
                    {
                        Acc.passWord = tbRePassNew.Text;
                        for (int i = 0; i < Global.users.accList.Count; i++)
                        {
                            if (Acc.userName == Global.users.accList[i].userName)
                            {
                                Global.users.accList[i] = Acc;
                                Global.users.update_change();
                            }
                        }
                        pnChangePassword.Hide();
                        isChangingPassword = false;
                    }
                    else
                    {
                        lbNotification.Text = "New password not match !";
                    }

                }
                else
                {
                    lbNotification.Text = "Wrong password !";
                }
            }
        }
    }

}
