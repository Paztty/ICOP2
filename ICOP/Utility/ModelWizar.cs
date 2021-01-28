using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ICOP
{
    public partial class ModelWizar : Form
    {
        public ModelProgram model = new ModelProgram();
        User.Account user;
        public ModelWizar(User.Account user)
        {
            InitializeComponent();
            this.AcceptButton = btOK;
            this.user = user;
            lbUserCreater.Text =
                "Creater: " + user.userName + Environment.NewLine +
                "Permission : " + user.Permission.ToString();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (tbPCBCode.TextLength < 3)
            {
                tbPCBCode.SelectAll();
            }
            else if (tbModelName.TextLength < 4)
            {
                tbModelName.SelectAll();
            }
            else
            {
                model = new ModelProgram(this.user.userName, tbModelName.Text, tbPCBCode.Text, (int)nUD_PBA_Counter.Value, (int)nUD_QR_Length.Value);
                Console.WriteLine(tbModelName.Text);
                this.DialogResult = DialogResult.OK;
            }
            
        }

        private void tbModelName_TextChanged(object sender, EventArgs e)
        {
            model.ModelName = tbModelName.Text;
        }
    }
}
