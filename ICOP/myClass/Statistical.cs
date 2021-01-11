using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICOP
{
    class Statistical
    {
        public const string StatisticalFileName = "Statistical.ist";
        public UInt64 dailyFail { get; set; } = 0;
        public UInt64 dailyPass { get; set; } = 0;
        public UInt64 dailyToltal { get; set; } = 0;
        public double dailyPercentPass { get; set; } = 0.0;

        public Statistical()
        {
            if (!Directory.Exists(Global.ICOP_Statitic_path)) Directory.CreateDirectory(Global.ICOP_Statitic_path);
            if (!File.Exists(Global.ICOP_Statitic_path + StatisticalFileName))
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string StatisticalJson = JsonSerializer.Serialize(this, options);
                File.WriteAllText(Global.ICOP_Statitic_path + StatisticalFileName, StatisticalJson);
            }
        }

        public void Statistical_Update( string result, Label CounterFail, Label CounterPass, Label CounterTotal, Label CounterPercent)
        {
            if (result == Global.ICOP_tester_OK)
            {
                this.dailyPass++;
            }
            else
            {
                this.dailyFail++;
            }
            this.dailyToltal++;
            if (this.dailyToltal == 0)
                this.dailyPercentPass = 0.0;
            else
                this.dailyPercentPass =  this.dailyPass / (double)dailyToltal;

            CounterFail.Text = this.dailyFail.ToString();
            CounterPass.Text = this.dailyPass.ToString();
            CounterTotal.Text = this.dailyToltal.ToString();
            CounterPercent.Text = (this.dailyPercentPass * 100).ToString("f2");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string StatisticalJson = JsonSerializer.Serialize(this, options);
            File.WriteAllText(Global.ICOP_Statitic_path + StatisticalFileName, StatisticalJson);

        }
        public void Statistical_Init_Counter(Label CounterFail, Label CounterPass, Label CounterTotal, Label CounterPercent)
        {
            this.dailyToltal = this.dailyPass + this.dailyFail;
            if (this.dailyToltal == 0)
                this.dailyPercentPass = 0.0;
            else
                this.dailyPercentPass = this.dailyPass / (double)dailyToltal;

            CounterFail.Text = this.dailyFail.ToString();
            CounterPass.Text = this.dailyPass.ToString();
            CounterTotal.Text = this.dailyToltal.ToString();
            CounterPercent.Text = (this.dailyPercentPass * 100).ToString("f2");
        }
    }
}
