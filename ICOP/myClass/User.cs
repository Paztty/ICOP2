using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ICOP
{
    public class User
    {
        public enum ICopPermision
        {
            [EnumMember(Value = "None")]
            None = -1,
            [EnumMember(Value = "OP")]
            OP = 0,
            [EnumMember(Value = "TE")]
            TE = 1,
            [EnumMember(Value = "MT")]
            MT = 2,
        }
        public class Account
        {
            public string userName { get; set; }
            public string passWord { get; set; }
            public ICopPermision Permission { get; set; }
            public bool InUsing { get; set; }
            public Account(string userName, string password, ICopPermision permision)
            {
                this.userName = userName;
                this.passWord = password;
                this.Permission = permision;
            }
            public Account() { }
        }

        public List<Account> accList { get; set; } = new List<Account>();
        public User(){
            accList.Add(new Account("admin", "1", ICopPermision.MT));
        }

        public void CreatUser(string userName, string password, ICopPermision permision)
        {
            accList.Add(new Account(userName, password, permision));
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
            File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + "cldys.ic", UserJson);
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
