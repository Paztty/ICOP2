using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Video.DirectShow;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace ICOP_V2.Class
{
    public class Camera
    {
        public bool camInitted = false;
        public int index = -1;
        public VideoCapture cam;
        public Mat image = new Mat();
        public Mat imagePreview = new Mat();
        public Bitmap nullImage;
        private FilterInfoCollection CaptureDevice;
        public PictureBox ICAMbox = new PictureBox();
        public PictureBox SettingBox = new PictureBox();
        public Camera() { }
        public Camera(string cameraID) 
        {
            Exception exception = new Exception("Not find any camera like this");
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            for (int i = 0; i < CaptureDevice.Count; i++)
            {
                if (cameraID == CaptureDevice[i].MonikerString)
                {
                    this.index = i;
                    cam = new VideoCapture(i);
                    cam.AutoFocus = false;
                    camInitted = true;
                }
            }
            if (index == -1)
                throw exception;
            

        }
        public Camera(int index)
        {
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (index <= CaptureDevice.Count)
            {
                cam = new VideoCapture(index);
                cam.AutoFocus = false;
                this.index = index;
                camInitted = true;
            }
            else
                Console.WriteLine("Out of cameras device");
        }

        public string GetCameraID(int index)
        {
            
            string result = "";
            if (camInitted)
            {
                if (index < CaptureDevice.Count)
                {
                    result = CaptureDevice[index].MonikerString;
                }
            }
            return result;
        }

        public void GetImage()
        {
            if (camInitted)
            {
                if (this.cam.IsOpened())
                {
                    if (cam.Read(image))
                    {
                        if (!image.Empty())
                        {
                            ICAMbox.Invoke(new MethodInvoker(delegate
                            {
                                if (ICAMbox.Image != null) ICAMbox.Image.Dispose();
                                ICAMbox.Image = this.image.ToBitmap();
                            }));
                        }
                    }
                    else
                    {
                        camInitted = false;
                    }
                }
            }
        }

        public void reconnectCam()
        {
            if (!camInitted)
            {
                cam.Release();
                cam = new VideoCapture(this.index);
                if (cam.Read(image))
                    camInitted = true;
            }
        }
        public void GetImagePreview()
        {
            if (camInitted)
            {
                if (this.cam.IsOpened())
                {
                    if (cam.Read(imagePreview))
                    {
                        if (!imagePreview.Empty())
                        {
                            if (SettingBox.Image != null) SettingBox.Image.Dispose();
                            SettingBox.Image = this.imagePreview.ToBitmap();
                        }
                    }
                }
            }
        }
        public void DisponseCamera()
        {
            if (this.cam != null)
            this.cam.Dispose();
        }
           
    }
}
