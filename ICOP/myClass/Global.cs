using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOP
{
    public static class Global
    {
        public static string ICOP_sw_path = @"D:\ICOP\";
        public static string ICOP_sw_path_log = @"D:\ICOP\LOG\";
        public static string ICOP_history_path = @"D:\ICOP\History\";
        public static string ICOP_model_path = @"D:\ICOP\Model\";
        public static string ICOP_setting_path = @"D:\ICOP\Setting\";
        public static string ICOP_sample_path = @"D:\ICOP\Sample\";
        public static string ICOP_Statitic_path = @"D:\ICOP\Statitics\";

        public static void DailyFolerCreat()
        {
            DateTime dateNew = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 30, 0);
            var moment = DateTime.Now;

            string today;
            int resultDay = DateTime.Compare(dateNew, moment);
            Console.WriteLine(dateNew);
            Console.WriteLine(DateTime.Now);
            Console.WriteLine(resultDay);
            if (resultDay > 0)
            {
                moment = moment.AddDays(-1);
            }
            today = moment.Year.ToString() + "\\" + moment.Month.ToString() + "\\" + moment.Day.ToString() + "\\";
            ICOP_sw_path_log += today;
            ICOP_history_path += today;
            ICOP_Statitic_path += today;
            if (!Directory.Exists(ICOP_sw_path_log)) Directory.CreateDirectory(ICOP_sw_path_log);
            if (!Directory.Exists(ICOP_history_path)) Directory.CreateDirectory(ICOP_history_path);
            if (!Directory.Exists(ICOP_Statitic_path)) Directory.CreateDirectory(ICOP_Statitic_path);
        }

        public const string ICOP_tester_OK = "PASS";
        public const string ICOP_tester_NG = "FAIL";

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
    }
}
