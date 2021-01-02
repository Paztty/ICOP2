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

namespace ICOP_V2.Class
{
    public class ModelProgram
    {
        public enum IcopFunction
        {
            [EnumMember(Value = "CODT")]
            CODT = 1,
            [EnumMember(Value = "CRDT")]
            CRDT = 2,
            [EnumMember(Value = "QRDT")]
            QRDT = 3,
        }
        public string ModelName { get; set; } = "ModelExample";
        public class ModelStep
        {
            public string Name { get; set; } = "CON1";
            public int PCB { get; set; } = 1;
            public ModelProgram.IcopFunction Func { get; set; } = ModelProgram.IcopFunction.CODT;
            public string Spect { get; set; } = "0.00";
            public double Value { get; set; } = 0.00;
            public bool Result { get; set; } = false;
            public bool Skip { get; set; } = true;

            public Rectangle areaRect { get; set; } = new Rectangle(1000, 1000, 1000, 1000);
            public string areaimageName { get; set; } = "";

            public DetectArea detectArea = new DetectArea();
            public ModelStep() {}
            public void addToDataView(DataGridView ProgramStep)
            {
                ProgramStep.Rows.Add(this.Name, this.PCB.ToString(), this.Spect, this.Value.ToString("F2"), this.Result, this.Skip);
            }

            public void addToDataStepView(DataGridView ProgramStep)
            {
                ProgramStep.Rows.Add(this.Name, this.PCB.ToString(), Func.ToString() , this.Spect, this.Skip);
            }

            public Rectangle ForDraw(PictureBox pictureBox)
            {
                float scaleX = (float)pictureBox.Width / (float)Global.maxWidth;
                float scaleY = (float)pictureBox.Height / (float)Global.maxHeight;
                Rectangle onPictureBox = detectArea.RectScale(areaRect, scaleX, scaleY);
                return onPictureBox;
            }
            public void ForSave(PictureBox pictureBox)
            {
                float scaleX = (float)Global.maxWidth / (float)pictureBox.Width;
                float scaleY = (float)Global.maxHeight / (float)pictureBox.Height;    
                areaRect = detectArea.RectScale(areaRect, scaleX, scaleY);
            }
        }

        public List<ModelStep> modelSteps { get; set; } = new List<ModelStep>(100);
        public ModelProgram(string test)
        {
            modelSteps.Add(new ModelStep()
            {
                PCB = 1,
                Result = true
            });
            modelSteps.Add(new ModelStep()
            {
                PCB = 2,
                Result = false
            });
            modelSteps.Add(new ModelStep()
            {
                PCB = 3,
                Result = false
            });
            modelSteps.Add(new ModelStep()
            {
                PCB = 4,
                Result = true
            });
        }
        public ModelProgram() { }
        public bool Save(string Path)
        {
            bool saveresult = true;
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);
                string ModelJson = JsonSerializer.Serialize(this, options);
                File.WriteAllText(Path + ModelName + ".imdl", ModelJson);
            }
            catch (Exception err)
            {
                saveresult = false;
                Console.WriteLine("Save error:" + err.ToString());
            }
            return saveresult;
        }
    }
}
