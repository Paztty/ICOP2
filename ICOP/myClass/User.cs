using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICOP
{
    public class User
    {
        public enum ICopPermision
        {
            [EnumMember(Value = "None")]
            None = 0,
            [EnumMember(Value = "OP")]
            OP = 1,
            [EnumMember(Value = "Technical")]
            Technical = 2,
            [EnumMember(Value = "Master")]
            Master = 3,
            [EnumMember(Value = "Manager")]
            Manager = 4,
        }
        public class Account
        {
            public string userName { get; set; }
            public string passWord { get; set; }
            public ICopPermision Permission { get; set; }
            public string Name { get; set; }
            public string EmployerCode { get; set; }
            public string userImagefile { get; set; }

            public Bitmap userImage;
            public bool InUsing { get; set; }
            public Account(string userName, string password, ICopPermision permision, string Name, string EmployerCode)
            {
                this.userName = userName;
                this.passWord = password;
                this.Permission = permision;
            }
            public Account() { }

            public void getFromControl(TextBox userName, TextBox password, ComboBox Permission, TextBox Name, TextBox EmployerCode)
            {
                this.userName = userName.Text;
                this.passWord = password.Text;
                this.Permission = (ICopPermision)Permission.SelectedIndex;
                this.Name = Name.Text;
                this.EmployerCode = EmployerCode.Text;
            }
            public void LoadToControl(TextBox userName, TextBox password, ComboBox Permission, TextBox Name, TextBox EmployerCode)
            {
                userName.Text = this.userName ;
                password.Text = this.passWord ;
                Permission.SelectedIndex = (int)this.Permission;
                Name.Text = this.Name;
                EmployerCode.Text = this.EmployerCode;
            }

        }

        public List<Account> accList { get; set; } = new List<Account>();
        public User(){
            accList.Add(new Account("admin", "1", ICopPermision.Master,"Le Minh Nhut", "7207905"));
            accList.Add(new Account("manager", "0346809230", ICopPermision.Manager, "Ho Van Tan", "7209115"));
        }

        public void CreatUser(string userName, string password, ICopPermision permision, string name, string EmployerCode)
        {
            accList.Add(new Account(userName, password, permision, name, EmployerCode));
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string UserJson = JsonSerializer.Serialize(this, options);
            File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + "cldys.ic", UserJson);
        }

        public void update_change()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string UserJson = JsonSerializer.Serialize(this, options);
            File.WriteAllText(Global.ICOP_sw_path + "cldys.ic", UserJson);
            Console.WriteLine(UserJson);
        }

        public void loadToView(DataGridView userView)
        {
            userView.Rows.Clear();
            for (int i = 0; i < accList.Count; i++)
            {
                userView.Rows.Add( i+1, accList[i].userName, accList[i].passWord, accList[i].Permission, accList[i].Name, accList[i].EmployerCode);

            }
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string UserJson = JsonSerializer.Serialize(this, options);
            Console.WriteLine(UserJson);
        }

        public void updateChaneToView(DataGridView userView, int positionUpdate)
        {
            if (positionUpdate < userView.Rows.Count && positionUpdate < accList.Count)
            {
                userView[0, positionUpdate].Value = positionUpdate + 1;
                userView[1, positionUpdate].Value = accList[positionUpdate].userName;
                userView[2, positionUpdate].Value = accList[positionUpdate].passWord;
                userView[3, positionUpdate].Value = accList[positionUpdate].Permission;
                userView[4, positionUpdate].Value = accList[positionUpdate].Name;
                userView[5, positionUpdate].Value = accList[positionUpdate].EmployerCode;
            }
        }
        public ICopPermision Login(string userName, string password)
        {
            ICopPermision permision = ICopPermision.None;
            for (int i = 0; i < accList.Count; i++)
            {
                if (userName == accList[i].userName && password == accList[i].passWord)
                {
                    permision = accList[i].Permission;
                }
            }
            return permision;
        }
    }
}
