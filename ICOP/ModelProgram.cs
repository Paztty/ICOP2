using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICOP
{
    public class ModelProgram
    {
        public enum IcopFunction
        {
            [EnumMember(Value = "CODT")]
            CODT = 0,
            [EnumMember(Value = "CRDT")]
            SDDT = 1,
            [EnumMember(Value = "QRDT")]
            QRDT = 2,
        }
        public string ModelName { get; set; } = "ModelExample";
        public string MotherFolder { get; set; } = "";
        public string ModelImagePathCam0 { get; set; } = "";
        public string ModelImagePathCam1 { get; set; } = "";
        public string ModelImagePathCam2 { get; set; } = "";
        public string ModelImagePathCam3 { get; set; } = "";

        public Image ModelImageCam0;
        public Image ModelImageCam1;
        public Image ModelImageCam2;
        public Image ModelImageCam3;

        public class ModelStep
        {
            public string Name { get; set; } = "CON1";
            public string Position { get; set; } = "";
            public ModelProgram.IcopFunction Func { get; set; } = ModelProgram.IcopFunction.CODT;
            public string Spect { get; set; } = "0.00";
            public double Value { get; set; } = 0.00;
            public bool Result { get; set; } = false;
            public bool Skip { get; set; } = true;

            public Rectangle areaRect { get; set; } = new Rectangle(1000, 1000, 1000, 1000);
            public string areaImageName { get; set; } = "";
            public Bitmap areaBitmap;
            public ModelStep() { }
            public void addToDataView(int No,DataGridView ProgramStep)
            {
                ProgramStep.Rows.Add(No, this.Name, this.Position, this.Func.ToString(), this.Spect, this.Value.ToString("F2"), this.Result, this.Skip);
            }

            public void addToDataStepView(int No, DataGridView ProgramStep)
            {
                ProgramStep.Rows.Add(No, this.Name, this.Position, Func.ToString(), this.Spect, this.Skip);
            }
            public void addToControlView(TextBox Name, Label Position, ComboBox Func, TextBox Spect, CheckBox Skip)
            {
                Name.Text = this.Name;
                Position.Text = this.Position;
                Func.SelectedIndex = (int)this.Func;
                Spect.Text = this.Spect;
                Skip.Checked = this.Skip;
            }
            public void applyFromControlView(TextBox Name, Label Position, ComboBox Func, TextBox Spect, CheckBox Skip)
            {
                this.Name = Name.Text;
                this.Position = Position.Text;
                this.Func = (IcopFunction)Func.SelectedIndex;
                this.Spect = Spect.Text;
                this.Skip = Skip.Checked;
            }

            public Rectangle ForDraw(PictureBox pictureBox)
            {
                float scaleX = (float)pictureBox.Width / (float)Global.maxWidth;
                float scaleY = (float)pictureBox.Height / (float)Global.maxHeight;
                return RectScale(areaRect, scaleX, scaleY);
            }
            public void ForSave(PictureBox pictureBox, Rectangle rect)
            {
                float scaleX = (float)Global.maxWidth / (float)pictureBox.Width;
                float scaleY = (float)Global.maxHeight / (float)pictureBox.Height;
                this.areaRect = RectScale(rect, scaleX, scaleY);
            }
            public void getLocation(MouseEventArgs e, PictureBox pictureBox)
            {
                var Locations = new Vector2(Global.maxWidth * e.X / pictureBox.Width, Global.maxHeight * e.Y / pictureBox.Height);
                Rectangle rectangle = new Rectangle((int)Locations.X, (int)Locations.Y, 100, 100);
                this.areaRect = rectangle;
            }
            public void getSize(MouseEventArgs e, PictureBox pictureBox)
            {
                Rectangle rectangle = new Rectangle
                    (
                    this.areaRect.X,
                    this.areaRect.Y,
                    Global.maxWidth * e.X / pictureBox.Width - this.areaRect.X,
                    Global.maxHeight * e.Y / pictureBox.Height - this.areaRect.Y
                    );

                this.areaRect = rectangle;
            }
            public Rectangle RectScale(Rectangle inputRect, float xScale, float yScale)
            {
                Rectangle outRect = new Rectangle();
                outRect.X = (int)(inputRect.X * xScale);
                outRect.Y = (int)(inputRect.Y * yScale);
                outRect.Width = (int)(inputRect.Width * xScale);
                outRect.Height = (int)(inputRect.Height * yScale);
                return outRect;
            }

            public Bitmap getImageinArea(Bitmap modelImage)
            {
                Bitmap bitmapBuffer = new Bitmap(10, 10);
                if (modelImage != null)
                {

                    Bitmap bitmap;
                    bitmap = (Bitmap)modelImage.Clone();
                    Console.WriteLine(bitmap.Size);
                    float scaleX = bitmap.Size.Width / (float)Global.maxWidth;
                    float scaleY = bitmap.Size.Height / (float)Global.maxHeight;
                    Console.WriteLine(scaleX);
                    Console.WriteLine(scaleY);
                    Rectangle areaInImage = RectScale(areaRect, scaleX, scaleY);
                    Console.WriteLine(areaInImage);
                    if (areaInImage.Size.Width > 1 && areaInImage.Size.Height > 1)
                    {
                        bitmapBuffer = new Bitmap(areaInImage.Size.Width, areaInImage.Size.Height);
                        using (Graphics g = Graphics.FromImage(bitmapBuffer))
                        {
                            g.DrawImage(bitmap, new Rectangle(0, 0, bitmapBuffer.Width, bitmapBuffer.Height), areaInImage, GraphicsUnit.Pixel);
                        }
                        areaBitmap = bitmapBuffer;
                    }
                }
                return bitmapBuffer;
            }

            public void loadStepImage()
            {
                if (File.Exists(areaImageName))
                {
                    using (var fs = new System.IO.FileStream(areaImageName, System.IO.FileMode.Open))
                    {
                        var bmp = new Bitmap(fs);
                        areaBitmap = (Bitmap)bmp.Clone();
                    }
                }
                    
            }
        }

        public List<ModelStep> modelSteps { get; set; } = new List<ModelStep>(100);
        public ModelProgram(string test)
        {
            modelSteps.Add(new ModelStep()
            {
                Position = Global.Positions.ICam0,
                Result = true
            });
            modelSteps.Add(new ModelStep()
            {
                Position = Global.Positions.ICam1,
                Result = false
            });
            modelSteps.Add(new ModelStep()
            {
                Position = Global.Positions.ICam2,
                Result = false
            });
            modelSteps.Add(new ModelStep()
            {
                Position = Global.Positions.ICam3,
                Result = true
            });
        }
        public ModelProgram()
        {
            modelSteps.Add(new ModelProgram.ModelStep
            {
                Name = "new component",
                Position = Global.Positions.ICam0,
                Func = ModelProgram.IcopFunction.CODT,
                Spect = "0.05",
                Skip = false,
            });
        }


        protected virtual bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }
            return false;
        }
        public bool Save()
        {
            bool saveresult = true;
            //try
            //{
            Directory.Move(MotherFolder + ModelName + @"_Image\", MotherFolder + ModelName + @"_ImageOld\");
            Directory.Delete(MotherFolder + ModelName + @"_ImageOld\", true);
            Directory.CreateDirectory(MotherFolder + ModelName + @"_Image\");
            if (ModelImageCam0 != null)
            {
                using (Bitmap bmp = (Bitmap)ModelImageCam0.Clone())
                {
                    string fileNameSave = MotherFolder + ModelName + @"_Image\" + ModelName + "_cam0" + ".png";
                    Console.WriteLine(fileNameSave);
                    bmp.Save(MotherFolder + ModelName + @"_Image\" + ModelName + "_cam0" + ".png");
                    ModelImagePathCam0 = fileNameSave;
                }
            }
            if (ModelImageCam1 != null)
            {
                using (Bitmap bmp = (Bitmap)ModelImageCam1.Clone())
                {
                    string fileNameSave = MotherFolder + ModelName + @"_Image\" + ModelName + "_cam1" + ".png";
                    Console.WriteLine(fileNameSave);
                    //if (File.Exists(fileNameSave)) File.Delete(fileNameSave);
                    bmp.Save(fileNameSave);
                    ModelImagePathCam1 = fileNameSave;
                }
            }
            if (ModelImageCam2 != null)
            {
                using (Bitmap bmp = (Bitmap)ModelImageCam2.Clone())
                {
                    string fileNameSave = MotherFolder + ModelName + @"_Image\" + ModelName + "_cam2" + ".png";
                    Console.WriteLine(fileNameSave);
                    //if (File.Exists(fileNameSave)) File.Delete(fileNameSave);
                    bmp.Save(fileNameSave);
                    ModelImagePathCam2 = fileNameSave;
                }
            }
            if (ModelImageCam3 != null)
            {
                using (Bitmap bmp = (Bitmap)ModelImageCam3.Clone())
                {
                    string fileNameSave = MotherFolder + ModelName + @"_Image\" + ModelName + "_cam3" + ".png";
                    Console.WriteLine(fileNameSave);
                    if (File.Exists(fileNameSave)) File.Delete(fileNameSave);
                    bmp.Save(fileNameSave);
                    ModelImagePathCam3 = fileNameSave;
                }
            }

            for (int i = 0; i < modelSteps.Count; i++)
            {
                if (modelSteps[i].areaBitmap != null)
                {
                    using (Bitmap bmp = (Bitmap)modelSteps[i].areaBitmap.Clone())
                    {
                        string fileNameSave = MotherFolder + ModelName + @"_Image\" + modelSteps[i].Name + ".png";
                        Console.WriteLine(fileNameSave);
                        //if (File.Exists(fileNameSave)) File.Delete(fileNameSave);
                        bmp.Save(fileNameSave, ImageFormat.Png);
                        modelSteps[i].areaImageName = fileNameSave;
                    }
                }
                else
                    MessageBox.Show("Component: " + modelSteps[i].Name + " not have image.");
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            if (!Directory.Exists(MotherFolder)) Directory.CreateDirectory(MotherFolder);
            string ModelJson = JsonSerializer.Serialize(this, options);
            File.WriteAllText(MotherFolder + ModelName + ".imdl", ModelJson);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.ToString());
            //    saveresult = false;
            //}
            return saveresult;
        }
    }
}
