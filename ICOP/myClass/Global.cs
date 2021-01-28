using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ICOP
{
    public static class Global
    {
        public const string ICOP_Version = "V2.0.0";

        public const string ICOP_sw = @"D:\ICOP\";
        public const string ICOP_sw_log = @"D:\ICOP\Log\";
        public const string ICOP_history = @"D:\ICOP\History\";
        public const string ICOP_history_image = @"D:\ICOP\History\";
        public const string ICOP_model = @"D:\ICOP\Model\";
        public const string ICOP_setting = @"D:\ICOP\Setting\";
        public const string ICOP_sample= @"D:\ICOP\Sample\";
        public const string ICOP_Statitic = @"D:\ICOP\Statitics\";
        public const string ICOP_MES = @"D:\ICOP\MES\";

        public static string ICOP_sw_path = @"D:\ICOP\";
        public static string ICOP_sw_path_log = @"D:\ICOP\Log\";
        public static string ICOP_history_path = @"D:\ICOP\History\";
        public static string ICOP_history_path_image = @"D:\ICOP\History\";
        public static string ICOP_model_path = @"D:\ICOP\Model\";
        public static string ICOP_setting_path = @"D:\ICOP\Setting\";
        public static string ICOP_sample_path = @"D:\ICOP\Sample\";
        public static string ICOP_Statitic_path = @"D:\ICOP\Statitics\";

        public static string MachineLine = "MI_S1";

        public static void DailyFolerCreat()
        {
            DateTime dateNew = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 30, 0);
            var moment = DateTime.Now;

            string today;
            int resultDay = DateTime.Compare(dateNew, moment);
            if (resultDay > 0)
            {
                moment = moment.AddDays(-1);
            }
            today = moment.ToString("yyyy") + "\\" + moment.ToString("MMMM") + "\\" + moment.ToString("dd") + "\\";
            ICOP_sw_path_log = @"D:\ICOP\Log\" + today;
            ICOP_history_path = @"D:\ICOP\History\"+ today;
            ICOP_Statitic_path = @"D:\ICOP\Statitics\" + today;
            ICOP_history_path_image = @"D:\ICOP\History\" + today + "Image\\";
            if (!Directory.Exists(ICOP_sw_path_log)) Directory.CreateDirectory(ICOP_sw_path_log);
            if (!Directory.Exists(ICOP_history_path)) Directory.CreateDirectory(ICOP_history_path);
            if (!Directory.Exists(ICOP_Statitic_path)) Directory.CreateDirectory(ICOP_Statitic_path);
            if (!Directory.Exists(ICOP_history_path_image)) Directory.CreateDirectory(ICOP_history_path_image);
            if (!Directory.Exists(ICOP_MES)) Directory.CreateDirectory(ICOP_MES);
        }

        public static void addToLog(string messenger)
        {
            File.AppendAllText(Global.ICOP_sw_path_log + "log.txt", DateTime.Now + ": " + messenger + Environment.NewLine);
        }

        public const string ICOP_tester_OK = "PASS";
        public const string ICOP_tester_NG = "FAIL";
        public const string ICOP_tester_OP_PASS = "OP PASS";

        public const int maxWidth = 100000;
        public const int maxHeight = 80000;
        public static class Positions
        {
            public const string ICam0 = "ICam0";
            public const string ICam1 = "ICam1";
            public const string ICam2 = "ICam2";
            public const string ICam3 = "ICam3";
        }
        public static class PBA
        {
            public const string PBA0 = "PBA0";
            public const string PBA1 = "PBA1";
            public const string PBA2 = "PBA2";
            public const string PBA3 = "PBA3";
        }

        public static Setting ICOP_setting_data = new Setting();
        public static void GetSetting()
        {
            if (File.Exists(Setting.Path))
            {
                string settingJson = File.ReadAllText(Setting.Path);
                ICOP_setting_data = JsonSerializer.Deserialize<Setting>(settingJson);
            }
        }

        public class Paint
        {
            public SolidBrush brush = new SolidBrush(Color.Cyan);
            public Font Font = new Font("Microsoft YaHei UI", 8, FontStyle.Bold);
            public Pen pen = new Pen(Color.Cyan);
            public Paint()
            {
                pen.Width = 2;
            }
        }

        public static User users = new User();

        public static string convayerStatus = "0000000000";
        public static IO_Port IO_Port = new IO_Port();
        public static void ICOP_messenger(string messenger)
        {
            IcopMessenger icopMessenger = new IcopMessenger(messenger);
            icopMessenger.ShowDialog();
        }
    }
}
