using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Windows.Forms;
using ZXing;
using Size = System.Drawing.Size;

namespace ICOP
{
    public class ModelProgram
    {
        public enum IcopFunction
        {
            [Description("Số phần trăm tương ứng giữa hình ảnh được tạo bởi thao tác này và hình ảnh thực tế.\\n" +
                " Hình ảnh thực càng giống ảnh gốc, số này càng có giá trị cao")]
            CODT = 0,
            [EnumMember(Value = "CRDT")]
            SDDT = 1,
            [EnumMember(Value = "QRDT")]
            QRDT = 2,
        }

        public string AccCreater { get; set; }
        public string ACCPass { get; set; }
        public string ModelName { get; set; } = "";
        public string PCB_Code { get; set; } = "";
        public string MotherFolder { get; set; } = "";
        public string ModelImagePathCam0 { get; set; } = "";
        public string ModelImagePathCam1 { get; set; } = "";
        public string ModelImagePathCam2 { get; set; } = "";
        public string ModelImagePathCam3 { get; set; } = "";
        public string ModelImageResult { get; set; } = "";
        public System.Drawing.Image ModelImageCam0;
        public System.Drawing.Image ModelImageCam1;
        public System.Drawing.Image ModelImageCam2;
        public System.Drawing.Image ModelImageCam3;

        public int PBA_Count { get; set; } = 1;
        public string result { get; set; } = "TESTTING";
        public bool loaded = false;
        public class ModelStep
        {
            public string Name { get; set; } = "CON1";
            public string Position { get; set; } = Global.Positions.ICam0;
            public int PBA { get; set; } = 0;
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
            public void addToReportDataView(int No, DataGridView reportView)
            {
                reportView.Rows.Add(No, this.Name, this.Func.ToString(), this.Spect, this.Value, this.Result, this.Skip);
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

                if (this.PBA > 0)
                {
                    try
                    {
                        PBA.SelectedIndex = this.PBA - 1;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("PBA: " + this.PBA + "  PBA count: " + PBA.Items.Count);
                    }
                    
                }
                    

                Spect.Text = this.Spect;
                Skip.Checked = this.Skip;
            }
            public void applyFromControlView(TextBox Name, Label Position, ComboBox PBA, ComboBox Func, TextBox Spect, CheckBox Skip)
            {
                if (this.Name.Contains("QR"))
                {
                    this.Spect = Spect.Text;
                    this.Skip = Skip.Checked;
                    this.PBA = PBA.SelectedIndex + 1;
                }
                else
                {
                    this.Name = Name.Text;
                    this.Position = Position.Text;
                    this.Func = (IcopFunction)Func.SelectedIndex;
                    this.PBA = PBA.SelectedIndex + 1;
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
                    try
                    {
                        if (File.Exists(imageSources[i].ImageName))
                        {
                            using (var fs = new System.IO.FileStream(imageSources[i].ImageName, System.IO.FileMode.Open))
                            {
                                var bmp = new Bitmap(fs);
                                imageSources[i].bitmap = (Bitmap)bmp.Clone();
                            }
                        }
                        else
                        {
                            Global.addToLog("not found " + imageSources[i].ImageName + "\n");
                        }
                    }
                    catch { }

                }
            }

        }

        public List<ModelStep> modelSteps { get; set; } = new List<ModelStep>(100);
        public ModelProgram()
        {
            modelSteps.Add(new ModelStep
            {
                Name = "QR",
                Spect = "23",
                Position = Global.Positions.ICam0,
                Func = IcopFunction.QRDT,
                PBA = 1
            });
        }
        public ModelProgram(string Report)
        {
        }

        public ModelProgram(string accCreater, string modelName, string PCB_code, int PBA_counter, int QR_length)
        {
            Console.WriteLine(modelName);
            this.AccCreater = accCreater;
            this.ModelName = modelName;
            this.PCB_Code = PCB_code;
            this.PBA_Count = PBA_counter;
            for (int i = 0; i < PBA_counter; i++)
            {
                modelSteps.Add(new ModelStep
                {
                    Name = "QR" + (i + 1),
                    Spect = QR_length.ToString(),
                    Position = Global.Positions.ICam0,
                    Func = IcopFunction.QRDT,
                    PBA = i + 1,
                    Skip = false
                });
            }
            Console.WriteLine(this.ModelName);
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
            this.MotherFolder = Global.ICOP_model_path + this.PCB_Code + @"\" + this.ModelName + @"\";
            if (Directory.Exists(MotherFolder + this.ModelName + @"_Image\"))
            {
                Directory.Move(MotherFolder + this.ModelName + @"_Image\", MotherFolder + this.ModelName + @"_ImageOld\");
                Directory.Delete(MotherFolder + this.ModelName + @"_ImageOld\", true);
                Directory.CreateDirectory(MotherFolder + this.ModelName + @"_Image\");
            }
            else
            {
                Directory.CreateDirectory(MotherFolder + this.ModelName + @"_Image\");
            }
            if (ModelImageCam0 != null)
            {
                using (Bitmap bmp = (Bitmap)ModelImageCam0.Clone())
                {
                    string fileNameSave = MotherFolder + this.ModelName + @"_Image\" + ModelName + "_cam0" + ".png";
                    Console.WriteLine(fileNameSave);
                    bmp.Save(fileNameSave);
                    ModelImagePathCam0 = fileNameSave;
                }
            }
            if (ModelImageCam1 != null)
            {
                using (Bitmap bmp = (Bitmap)ModelImageCam1.Clone())
                {
                    string fileNameSave = MotherFolder + this.ModelName + @"_Image\" + ModelName + "_cam1" + ".png";
                    Console.WriteLine(fileNameSave);
                    bmp.Save(fileNameSave);
                    ModelImagePathCam1 = fileNameSave;
                }
            }
            if (ModelImageCam2 != null)
            {
                using (Bitmap bmp = (Bitmap)ModelImageCam2.Clone())
                {
                    string fileNameSave = MotherFolder + this.ModelName + @"_Image\" + ModelName + "_cam2" + ".png";
                    Console.WriteLine(fileNameSave);
                    bmp.Save(fileNameSave);
                    ModelImagePathCam2 = fileNameSave;
                }
            }
            if (ModelImageCam3 != null)
            {
                using (Bitmap bmp = (Bitmap)ModelImageCam3.Clone())
                {
                    string fileNameSave = MotherFolder + this.ModelName + @"_Image\" + ModelName + "_cam3" + ".png";
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
                            string fileNameSave = MotherFolder + this.ModelName + @"_Image\" + modelSteps[i].Name + "_" + 0 + "_" + modelSteps[i].PBA.ToString() + "_" + modelSteps[i].Func.ToString() + ".png";
                            Console.WriteLine(fileNameSave);
                            bmp.Save(fileNameSave, ImageFormat.Png);
                            modelSteps[i].imageSources[0].ImageName = fileNameSave;
                        }
                        for (int imageCounter = modelSteps[i].imageSources.Count - 1; imageCounter >= 0; imageCounter--)
                        {
                            if (modelSteps[i].imageSources[imageCounter].bitmap.Size != modelSteps[i].imageSources[0].bitmap.Size)
                            {
                                if (File.Exists(modelSteps[i].imageSources[imageCounter].ImageName))
                                {
                                    File.Delete(modelSteps[i].imageSources[imageCounter].ImageName);
                                    Console.WriteLine("Delete: " + modelSteps[i].imageSources[imageCounter].ImageName);
                                }
                                modelSteps[i].imageSources.RemoveAt(imageCounter);
                            }
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
            File.WriteAllText(MotherFolder + this.ModelName + ".imdl", ModelJson);
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
            File.WriteAllText(MotherFolder + this.ModelName + ".imdl", ModelJson);
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
            File.WriteAllText(MotherFolder + this.ModelName + ".imdl", ModelJson);
        }

        #region Process Funtions



        public void QRDT(ModelStep modelStep, Bitmap bitmap)
        {
            Mat matImage = new Mat();
            Mat outImage = matImage;
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
                modelStep.Value = result.Text;
            }
            else
            {
                modelStep.Result = Global.ICOP_tester_NG;
                modelStep.Value = "QR_not_detect";
            }
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
                    Bitmap bitmapSource = resizeImage(modelStep.imageSources[i].bitmap, new Size(100, 100));
                    Bitmap bitmapArea = resizeImage(bitmap, new Size(100, 100));
                    Mat matSource = bitmapSource.ToMat();
                    Mat matArea = bitmapArea.ToMat();
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
                        modelStep.sourceImage = matSource.ToBitmap();
                        modelStep.templateImage = matArea.ToBitmap();
                    }
                    catch (Exception err)
                    {
                        modelStep.Result = Global.ICOP_tester_NG;
                        modelStep.Value = err.Message;
                    }


                }
            }
            Console.WriteLine();
            return returnBitmap;
        }
        #endregion

        #region Report
        public void saveResult(DateTime dateTime, Image ICam0, Image ICam1, Image ICam2, Image ICam3)
        {
            Global.Paint paint_NG = new Global.Paint
            {
                brush = new SolidBrush(Color.Red),
                Font = new Font("Microsoft YaHei UI", 20, FontStyle.Bold),
                pen = new Pen(Color.Red, 3)
            };
            Global.Paint paint_OK = new Global.Paint
            {
                brush = new SolidBrush(Color.Green),
                Font = new Font("Microsoft YaHei UI", 20, FontStyle.Bold),
                pen = new Pen(Color.Green, 3)
            };
            Global.Paint paint = paint_NG;

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
                    if (modelSteps[i].Result == Global.ICOP_tester_OK)
                    {
                        paint = paint_OK;
                    }
                    else
                    {
                        paint = paint_NG;
                    }
                    switch (modelSteps[i].Position)
                    {
                        case Global.Positions.ICam0:
                            rectangleF[0] = modelSteps[i].ForDrawResult(ICam0);
                            rectangleF[0].X = rectangleF[0].X;
                            rectangleF[0].Y = rectangleF[0].Y;
                            g.DrawRectangles(paint.pen, rectangleF);
                            g.DrawString
                                        (
                                        modelSteps[i].Name,
                                        paint.Font,
                                        paint.brush,
                                        rectangleF[0].X,
                                        rectangleF[0].Y - (paint.Font.Height)
                                        );
                            break;
                        case Global.Positions.ICam1:
                            rectangleF[0] = modelSteps[i].ForDrawResult(ICam1);
                            rectangleF[0].X = rectangleF[0].X + ICam0.Width;
                            rectangleF[0].Y = rectangleF[0].Y;
                            g.DrawRectangles(paint.pen, rectangleF);
                            g.DrawString
                                        (
                                        modelSteps[i].Name,
                                        paint.Font,
                                        paint.brush,
                                        rectangleF[0].X,
                                        rectangleF[0].Y - (paint.Font.Height)
                                        );
                            break;
                        case Global.Positions.ICam2:
                            rectangleF[0] = modelSteps[i].ForDrawResult(ICam2);
                            rectangleF[0].X = rectangleF[0].X;
                            rectangleF[0].Y = rectangleF[0].Y + ICam0.Height;
                            g.DrawRectangles(paint.pen, rectangleF);
                            g.DrawString
                                        (
                                        modelSteps[i].Name,
                                        paint.Font,
                                        paint.brush,
                                        rectangleF[0].X,
                                        rectangleF[0].Y - (paint.Font.Height)
                                        );
                            break;
                        case Global.Positions.ICam3:
                            rectangleF[0] = modelSteps[i].ForDrawResult(ICam3);
                            rectangleF[0].X = rectangleF[0].X + ICam0.Width;
                            rectangleF[0].Y = rectangleF[0].Y + ICam0.Height;
                            g.DrawRectangles(paint.pen, rectangleF);
                            g.DrawString
                                        (
                                        modelSteps[i].Name,
                                        paint.Font,
                                        paint.brush,
                                        rectangleF[0].X,
                                        rectangleF[0].Y - (paint.Font.Height)
                                        );
                            break;
                    }
                }
            }
            //resultBitmap = resizeImage(resultBitmap, new Size(1920, 1080));
            this.ModelImageResult = Global.ICOP_history_path_image + this.ModelName + "_" + dateTime.ToString("HHmmss") + "_" + this.result + ".png";
            resultBitmap.Save(ModelImageResult);
        }

        private Bitmap resizeImage(Image imgToResize, Size size)
        {
            return (new Bitmap(imgToResize, size));
        }

        public void SaveMESReport(DateTime dateTime, int PCB_count)
        {
            for (int i = 0; i < this.PBA_Count; i++)
            {
                if (modelSteps[i].Result == Global.ICOP_tester_OK)
                {
                    string PCB_report_name = modelSteps[i].Value;
                    if (modelSteps[i].Result == Global.ICOP_tester_NG)
                        PCB_report_name += "_" + (i + 1);
                    string reportContent = (PCB_count + i) + "/" + (i + 1) + "/" + dateTime.ToString() + "/" + Global.MachineLine + "/";
                    string PCB_result = "Y" + "/" + "OK";
                    for (int stepCount = 0; stepCount < modelSteps.Count; stepCount++)
                    {
                        if (modelSteps[stepCount].Result == Global.ICOP_tester_NG && modelSteps[stepCount].PBA == i)
                        {
                            PCB_result = "N" + "/" + "NG";
                            break;
                        }
                    }
                    File.WriteAllText(Global.ICOP_MES + PCB_report_name + ".txt", reportContent + PCB_result);
                }
            }
        }
        public void SaveTextReport(DateTime dateTime, User.Account OP)
        {
            for (int i = 0; i < this.PBA_Count; i++)
            {
                string PCB_report_name = dateTime.ToString("HHmmss") + "_" + modelSteps[i].Value + "_" + (i + 1) + "_" + this.result;

                if (modelSteps[i].Result == Global.ICOP_tester_NG)
                {
                    PCB_report_name = dateTime.ToString("HHmmss") + "_" + "No-QR" + "_" + (i + 1) + "_" + this.result;
                }

                ModelProgram modelReport = new ModelProgram("Report")
                {
                    AccCreater = this.AccCreater,
                    ACCPass = "Machine",
                    ModelName = this.ModelName,
                    PCB_Code = this.PCB_Code,
                    MotherFolder = this.MotherFolder,
                    ModelImagePathCam0 = this.ModelImagePathCam0,
                    ModelImagePathCam1 = this.ModelImagePathCam1,
                    ModelImagePathCam2 = this.ModelImagePathCam2,
                    ModelImagePathCam3 = this.ModelImagePathCam3,
                    ModelImageResult = this.ModelImageResult,
                    PBA_Count = this.PBA_Count,
                    result = this.result,
                };

                if (this.result == Global.ICOP_tester_OP_PASS)
                {
                    modelReport.ACCPass = OP.userName;
                }

                for (int stepCount = 0; stepCount < this.modelSteps.Count; stepCount++)
                {
                    if (modelSteps[stepCount].PBA == i + 1)
                    {
                        modelReport.modelSteps.Add(this.modelSteps[stepCount]);
                    }
                }
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string ModelJson = JsonSerializer.Serialize(modelReport, options);
                File.WriteAllText(Global.ICOP_history_path + PCB_report_name + ".ihf", ModelJson);
            }
        }
        #endregion

    }
}
