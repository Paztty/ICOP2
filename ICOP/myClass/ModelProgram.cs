using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Windows.Forms;
using ZXing;
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
        public enum IcopPBA
        {
            [EnumMember(Value = "PBA0")]
            PBA0 = 0,
            [EnumMember(Value = "PBA1")]
            PBA1 = 1,
            [EnumMember(Value = "PBA2")]
            PBA2 = 2,
            [EnumMember(Value = "PBA3")]
            PBA3 = 3,
        }
        public string ModelName { get; set; } = "ModelExample";
        public string MotherFolder { get; set; } = "";
        public string ModelImagePathCam0 { get; set; } = "";
        public string ModelImagePathCam1 { get; set; } = "";
        public string ModelImagePathCam2 { get; set; } = "";
        public string ModelImagePathCam3 { get; set; } = "";

        public System.Drawing.Image ModelImageCam0;
        public System.Drawing.Image ModelImageCam1;
        public System.Drawing.Image ModelImageCam2;
        public System.Drawing.Image ModelImageCam3;

        public string result = "TESTTING";
        public bool loaded = false;
        public class ModelStep
        {
            public string Name { get; set; } = "CON1";
            public string Position { get; set; } = "";
            public IcopPBA PBA { get; set; } = IcopPBA.PBA0;
            public ModelProgram.IcopFunction Func { get; set; } = ModelProgram.IcopFunction.CODT;
            public string Spect { get; set; } = "0.00";
            public string Value { get; set; } = "";
            public string Result { get; set; } = Global.ICOP_tester_NG;
            public bool Skip { get; set; } = true;
            public int block { get; set; } = 11;
            public int gama { get; set; } = 3;


            public RectangleF areaRect { get; set; } = new RectangleF(1000, 1000, 1000, 1000);


            public class imageSource
            {
                public string ImageName { get; set; }
                public Bitmap bitmap = new Bitmap(10, 10);
                public imageSource() { }
            }
            public List<imageSource> imageSources { get; set; } = new List<imageSource>();


            public Bitmap sourceImage;
            public Bitmap templateImage;

            public ModelStep()
            {
                imageSources.Add(new imageSource());
            }
            public void addToDataView(int No, DataGridView ProgramStep)
            {
                ProgramStep.Rows.Add(No, this.Name, this.Position, this.PBA, this.Func.ToString(), this.Spect, this.Value, this.Result, this.Skip);
            }

            public void addToDataStepView(int No, DataGridView ProgramStep)
            {
                ProgramStep.Rows.Add(No, this.Name, this.Position, this.PBA, Func.ToString(), this.Spect, this.Skip);
            }
            public void editDataStepView(int No, DataGridView ProgramStep)
            {
                ProgramStep[0, No].Value = No;
                ProgramStep[1, No].Value = this.Name;
                ProgramStep[2, No].Value = this.Position;
                ProgramStep[3, No].Value = this.PBA;
                ProgramStep[4, No].Value = Func.ToString();
                ProgramStep[5, No].Value = this.Spect;
                ProgramStep[6, No].Value = this.Skip;
            }

            public void addToControlView(TextBox Name, Label Position, ComboBox PBA, ComboBox Func, TextBox Spect, CheckBox Skip)
            {
                Name.Text = this.Name;
                Position.Text = this.Position;
                Func.SelectedIndex = (int)this.Func;
                PBA.SelectedIndex = (int)this.PBA;
                Spect.Text = this.Spect;
                Skip.Checked = this.Skip;
            }
            public void applyFromControlView(TextBox Name, Label Position, ComboBox PBA, ComboBox Func, TextBox Spect, CheckBox Skip)
            {
                if (this.Name.Contains("QR"))
                {
                    this.Spect = Spect.Text;
                    this.Skip = Skip.Checked;
                    this.PBA = (IcopPBA)PBA.SelectedIndex;
                }
                else
                {
                    this.Name = Name.Text;
                    this.Position = Position.Text;
                    this.Func = (IcopFunction)Func.SelectedIndex;
                    this.PBA = (IcopPBA)PBA.SelectedIndex;
                    this.Spect = Spect.Text;
                    this.Skip = Skip.Checked;
                }
            }

            public RectangleF ForDraw(PictureBox pictureBox)
            {
                float scaleX = (float)pictureBox.Width / (float)Global.maxWidth;
                float scaleY = (float)pictureBox.Height / (float)Global.maxHeight;
                return RectScale(areaRect, scaleX, scaleY);
            }
            public RectangleF ForDrawResult(Image image)
            {
                float scaleX = (float)image.Width / (float)Global.maxWidth;
                float scaleY = (float)image.Height / (float)Global.maxHeight;
                return RectScale(areaRect, scaleX, scaleY);
            }
            public void ForSave(PictureBox pictureBox, RectangleF rect)
            {
                float scaleX = (float)Global.maxWidth / (float)pictureBox.Width;
                float scaleY = (float)Global.maxHeight / (float)pictureBox.Height;
                this.areaRect = RectScale(rect, scaleX, scaleY);
            }
            public void getLocation(MouseEventArgs e, PictureBox pictureBox)
            {
                var Locations = new Vector2(Global.maxWidth * e.X / pictureBox.Width, Global.maxHeight * e.Y / pictureBox.Height);
                RectangleF RectangleF = new RectangleF((int)Locations.X, (int)Locations.Y, 100, 100);
                this.areaRect = RectangleF;
            }
            public void getSize(MouseEventArgs e, PictureBox pictureBox)
            {
                RectangleF RectangleF = new RectangleF
                    (
                    this.areaRect.X,
                    this.areaRect.Y,
                    Global.maxWidth * e.X / pictureBox.Width - this.areaRect.X,
                    Global.maxHeight * e.Y / pictureBox.Height - this.areaRect.Y
                    );

                this.areaRect = RectangleF;
            }
            public RectangleF RectScale(RectangleF inputRect, float xScale, float yScale)
            {
                RectangleF outRect = new RectangleF();
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
                    RectangleF areaInImage = RectScale(areaRect, scaleX, scaleY);
                    Console.WriteLine(areaInImage);
                    if (areaInImage.Size.Width > 1 && areaInImage.Size.Height > 1)
                    {
                        bitmapBuffer = new Bitmap((int)areaInImage.Size.Width, (int)areaInImage.Size.Height);
                        using (Graphics g = Graphics.FromImage(bitmapBuffer))
                        {
                            g.DrawImage(bitmap, new RectangleF(0, 0, bitmapBuffer.Width, bitmapBuffer.Height), areaInImage, GraphicsUnit.Pixel);
                        }
                        imageSources[0].bitmap = bitmapBuffer;
                    }
                }
                return bitmapBuffer;
            }

            public Bitmap getImage(Bitmap modelImage)
            {
                Bitmap bitmapBuffer = new Bitmap(10, 10);
                if (modelImage != null)
                {
                    Bitmap bitmap;
                    bitmap = (Bitmap)modelImage.Clone();
                    float scaleX = bitmap.Size.Width / (float)Global.maxWidth;
                    float scaleY = bitmap.Size.Height / (float)Global.maxHeight;
                    RectangleF areaInImage = RectScale(areaRect, scaleX, scaleY);
                    if (areaInImage.Size.Width > 1 && areaInImage.Size.Height > 1)
                    {
                        bitmapBuffer = new Bitmap((int)areaInImage.Size.Width, (int)areaInImage.Size.Height);
                        using (Graphics g = Graphics.FromImage(bitmapBuffer))
                        {
                            g.DrawImage(bitmap, new RectangleF(0, 0, bitmapBuffer.Width, bitmapBuffer.Height), areaInImage, GraphicsUnit.Pixel);
                        }
                    }
                }
                return bitmapBuffer;
            }
            public void loadStepImage()
            {
                for (int i = 0; i < imageSources.Count; i++)
                {
                    if (File.Exists(imageSources[i].ImageName))
                    {
                        using (var fs = new System.IO.FileStream(imageSources[i].ImageName, System.IO.FileMode.Open))
                        {
                            var bmp = new Bitmap(fs);
                            imageSources[i].bitmap = (Bitmap)bmp.Clone();
                        }
                    }
                }
            }

        }

        public List<ModelStep> modelSteps { get; set; } = new List<ModelStep>(100);
        public ModelProgram()
        {
            modelSteps.Add(new ModelProgram.ModelStep
            {
                Name = "QR1",
                Position = Global.Positions.ICam0,
                PBA = IcopPBA.PBA0,
                Func = ModelProgram.IcopFunction.QRDT,
                Spect = "23",
                Skip = true,
            });
            modelSteps.Add(new ModelProgram.ModelStep
            {
                Name = "QR2",
                Position = Global.Positions.ICam1,
                PBA = IcopPBA.PBA1,
                Func = ModelProgram.IcopFunction.QRDT,
                Spect = "23",
                Skip = true,
            });
            modelSteps.Add(new ModelProgram.ModelStep
            {
                Name = "QR3",
                Position = Global.Positions.ICam2,
                PBA = IcopPBA.PBA2,
                Func = ModelProgram.IcopFunction.QRDT,
                Spect = "23",
                Skip = true,
            });
            modelSteps.Add(new ModelProgram.ModelStep
            {
                Name = "QR4",
                Position = Global.Positions.ICam3,
                PBA = IcopPBA.PBA3,
                Func = ModelProgram.IcopFunction.QRDT,
                Spect = "23",
                Skip = true,
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

            if (Directory.Exists(MotherFolder + ModelName))
            {
                Directory.Move(MotherFolder + ModelName + @"_Image\", MotherFolder + ModelName + @"_ImageOld\");
                Directory.Delete(MotherFolder + ModelName + @"_ImageOld\", true);
                Directory.CreateDirectory(MotherFolder + ModelName + @"_Image\");
            }
            else
            {
                Directory.CreateDirectory(MotherFolder + ModelName + @"_Image\");
            }
            if (ModelImageCam0 != null)
            {
                using (Bitmap bmp = (Bitmap)ModelImageCam0.Clone())
                {
                    string fileNameSave = MotherFolder + ModelName + @"_Image\" + ModelName + "_cam0" + ".png";
                    Console.WriteLine(fileNameSave);
                    bmp.Save(fileNameSave);
                    ModelImagePathCam0 = fileNameSave;
                }
            }
            if (ModelImageCam1 != null)
            {
                using (Bitmap bmp = (Bitmap)ModelImageCam1.Clone())
                {
                    string fileNameSave = MotherFolder + ModelName + @"_Image\" + ModelName + "_cam1" + ".png";
                    Console.WriteLine(fileNameSave);
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
                    bmp.Save(fileNameSave);
                    ModelImagePathCam3 = fileNameSave;
                }
            }

            for (int i = 0; i < modelSteps.Count; i++)
            {
                // if(modelSteps[i].Func != IcopFunction.QRDT)
                //     modelSteps[i].Spect = "0.5";
                {
                    if (modelSteps[i].imageSources[0].bitmap != null)
                    {
                        using (Bitmap bmp = (Bitmap)modelSteps[i].imageSources[0].bitmap.Clone())
                        {
                            string fileNameSave = MotherFolder + ModelName + @"_Image\" + modelSteps[i].Name + "_" + 0 + "_" + modelSteps[i].PBA.ToString() + "_" + modelSteps[i].Func.ToString() + ".png";
                            Console.WriteLine(fileNameSave);
                            bmp.Save(fileNameSave, ImageFormat.Png);
                            modelSteps[i].imageSources[0].ImageName = fileNameSave;
                        }
                    }
                }
            }
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            if (!Directory.Exists(MotherFolder)) Directory.CreateDirectory(MotherFolder);
            string ModelJson = JsonSerializer.Serialize(this, options);
            File.WriteAllText(MotherFolder + ModelName + ".imdl", ModelJson);
            return saveresult;
        }

        public void AddStepImageReference(ModelStep modelStep, Bitmap referenceBitmap)
        {
            modelStep.imageSources.Add(new ModelStep.imageSource());
            modelStep.imageSources[modelStep.imageSources.Count - 1].bitmap = referenceBitmap;
            if (modelStep.imageSources[modelStep.imageSources.Count - 1].bitmap != null)
            {
                using (Bitmap bmp = (Bitmap)modelStep.imageSources[modelStep.imageSources.Count - 1].bitmap.Clone())
                {
                    string fileNameSave = this.MotherFolder + this.ModelName + @"_Image\" + modelStep.Name + "_" + (modelStep.imageSources.Count - 1) + "_" + modelStep.PBA.ToString() + "_" + modelStep.Func.ToString() + ".png";
                    Console.WriteLine(fileNameSave);
                    bmp.Save(fileNameSave, ImageFormat.Png);
                    modelStep.imageSources[modelStep.imageSources.Count - 1].ImageName = fileNameSave;
                }
            }
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            if (!Directory.Exists(MotherFolder)) Directory.CreateDirectory(MotherFolder);
            string ModelJson = JsonSerializer.Serialize(this, options);
            File.WriteAllText(MotherFolder + ModelName + ".imdl", ModelJson);
        }

        public void ChangerSpectvalue(ModelStep modelStep, string value)
        {
            modelStep.Spect = value;
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            if (!Directory.Exists(MotherFolder)) Directory.CreateDirectory(MotherFolder);
            string ModelJson = JsonSerializer.Serialize(this, options);
            File.WriteAllText(MotherFolder + ModelName + ".imdl", ModelJson);
        }

        public void saveResultImage(Image ICam0, Image ICam1, Image ICam2, Image ICam3)
        {
            Global.Paint paint_NG = new Global.Paint
            {
                brush = new SolidBrush(Color.Red),
                Font = new Font("Microsoft YaHei UI", 20, FontStyle.Bold),
                pen = new Pen(Color.Red, 5)
            };
            Pen pen = new Pen(Color.Red);

            Bitmap resultBitmap = new Bitmap(ICam0.Size.Width * 2, ICam0.Size.Height * 2);
            using (var g = Graphics.FromImage(resultBitmap))
            {
                g.DrawImage(ICam0, 0, 0);
                g.DrawImage(ICam1, ICam0.Width, 0);
                g.DrawImage(ICam2, 0, ICam0.Height);
                g.DrawImage(ICam3, ICam0.Width, ICam0.Height);
                RectangleF[] rectangleF = new RectangleF[1];
                for (int i = 0; i < modelSteps.Count; i++)
                {
                    if (modelSteps[i].Result == Global.ICOP_tester_NG)
                    {
                        switch (modelSteps[i].Position)
                        {
                            case Global.Positions.ICam0:
                                rectangleF[0] = modelSteps[i].ForDrawResult(ICam0);
                                rectangleF[0].X = rectangleF[0].X;
                                rectangleF[0].Y = rectangleF[0].Y;
                                g.DrawRectangles(paint_NG.pen, rectangleF);
                                g.DrawString
                                            (
                                            modelSteps[i].Name,
                                            paint_NG.Font,
                                            paint_NG.brush,
                                            rectangleF[0].X,
                                            rectangleF[0].Y - (paint_NG.Font.Height)
                                            );
                                break;
                            case Global.Positions.ICam1:
                                rectangleF[0] = modelSteps[i].ForDrawResult(ICam1);
                                rectangleF[0].X = rectangleF[0].X + ICam0.Width;
                                rectangleF[0].Y = rectangleF[0].Y;
                                g.DrawRectangles(paint_NG.pen, rectangleF);
                                g.DrawString
                                            (
                                            modelSteps[i].Name,
                                            paint_NG.Font,
                                            paint_NG.brush,
                                            rectangleF[0].X,
                                            rectangleF[0].Y - (paint_NG.Font.Height)
                                            );
                                break;
                            case Global.Positions.ICam2:
                                rectangleF[0] = modelSteps[i].ForDrawResult(ICam2);
                                rectangleF[0].X = rectangleF[0].X;
                                rectangleF[0].Y = rectangleF[0].Y + ICam0.Height;
                                g.DrawRectangles(paint_NG.pen, rectangleF);
                                g.DrawString
                                            (
                                            modelSteps[i].Name,
                                            paint_NG.Font,
                                            paint_NG.brush,
                                            rectangleF[0].X,
                                            rectangleF[0].Y - (paint_NG.Font.Height)
                                            );
                                break;
                            case Global.Positions.ICam3:
                                rectangleF[0] = modelSteps[i].ForDrawResult(ICam3);
                                rectangleF[0].X = rectangleF[0].X + ICam0.Width;
                                rectangleF[0].Y = rectangleF[0].Y + ICam0.Height;
                                g.DrawRectangles(paint_NG.pen, rectangleF);
                                g.DrawString
                                            (
                                            modelSteps[i].Name,
                                            paint_NG.Font,
                                            paint_NG.brush,
                                            rectangleF[0].X,
                                            rectangleF[0].Y - (paint_NG.Font.Height)
                                            );
                                break;
                        }
                    }
                }
            }
            resultBitmap.Save(@"D:\ICOP\result.png");
        }




        public string QRDT(ModelStep modelStep, Bitmap bitmap)
        {
            string QRcode = "";
            Mat matImage = new Mat();
            Mat outImage = matImage;
            bitmap.SetResolution(500, 500);
            matImage = bitmap.ToMat();
            bitmap.Dispose();
            var codeDetector = new BarcodeReader();
            Result result = codeDetector.Decode(matImage.ToBitmap());
            if (result == null || result.Text.Length != Convert.ToInt32(modelStep.Spect))
            {
                if (matImage.Width > 10 && matImage.Height > 10)
                {
                    Cv2.CvtColor(matImage, outImage, ColorConversionCodes.RGB2GRAY);
                    Cv2.AdaptiveThreshold(outImage,
                                            matImage,
                                            255,
                                            AdaptiveThresholdTypes.GaussianC,
                                            ThresholdTypes.Binary,
                                            modelStep.block,
                                            modelStep.gama);
                    result = codeDetector.Decode(matImage.ToBitmap());
                    if (result == null || result.Text.Length != Convert.ToInt32(modelStep.Spect))
                    {
                        try
                        {
                            if (result == null || result.Text.Length != Convert.ToInt32(modelStep.Spect))
                            {
                                for (int j = 0; j <= 10; j++)
                                {
                                    for (int i = 5; i < 25; i++)
                                    {
                                        Cv2.AdaptiveThreshold(
                                                                outImage,
                                                                matImage,
                                                                255,
                                                                AdaptiveThresholdTypes.GaussianC,
                                                                ThresholdTypes.Binary,
                                                                2 * i + 1,
                                                                j
                                                            );
                                        result = codeDetector.Decode(matImage.ToBitmap());
                                        if (result != null && result.Text.Length == Convert.ToInt32(modelStep.Spect))
                                        {
                                            modelStep.block = 2 * i + 1;
                                            modelStep.gama = j;
                                            break;
                                        }
                                    }
                                    if (result != null && result.Text.Length == Convert.ToInt32(modelStep.Spect))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message + e.ToString());
                        }
                    }
                }
            }
            if (result != null && result.Text.Length == Convert.ToInt32(modelStep.Spect))
            {
                modelStep.Result = Global.ICOP_tester_OK;
                QRcode = result.Text;
            }
            else
            {
                modelStep.Result = Global.ICOP_tester_NG;
            }
            return QRcode;
        }
        public Bitmap CODT(ModelStep modelStep, Bitmap bitmap)
        {
            Console.Write(modelStep.Name + "_" + modelStep.PBA.ToString() + "_" + modelStep.Func.ToString() + ".png :");
            var returnBitmap = new Bitmap(10, 10);
            for (int i = 0; i < modelStep.imageSources.Count; i++)
            {
                if (modelStep.imageSources[i].bitmap != null && bitmap != null)
                {
                    Console.Write(i + " ");
                    returnBitmap = modelStep.imageSources[i].bitmap;
                    Mat matSource = modelStep.imageSources[i].bitmap.ToMat();
                    Mat matArea = bitmap.ToMat();
                    Mat scoreMat = new Mat();

                    try
                    {
                        Cv2.MatchTemplate(matArea, matSource, scoreMat, TemplateMatchModes.CCoeffNormed);
                        Cv2.MinMaxLoc(scoreMat, out _, out double maxScore);
                        modelStep.Value = maxScore.ToString("f2");
                        if (Convert.ToDouble(modelStep.Value) >= Convert.ToDouble(modelStep.Spect))
                        {
                            modelStep.Result = Global.ICOP_tester_OK;
                            break;
                        }
                        else
                        {
                            modelStep.Result = Global.ICOP_tester_NG;
                        }
                    }
                    catch (Exception err)
                    {
                        modelStep.Result = Global.ICOP_tester_NG;
                        modelStep.Value = err.Message;
                    }

                    modelStep.sourceImage = matSource.ToBitmap();
                    modelStep.templateImage = matArea.ToBitmap();

                }
            }
            Console.WriteLine();
            return returnBitmap;
        }

    }
}
