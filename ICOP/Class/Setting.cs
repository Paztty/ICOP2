using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace ICOP_V2.Class
{
    public class Setting
    {
        public const string Path = @"D:\DEV_PJT\ICOPv2\setting.icop";

        public class cameraSetting
        {
            public string cameraID { get; set; } = "";
            public int cameraIndex { get; set; } = 0;
            public double cameraFocus { get; set; } = 0;
            public double cameraExposure { get; set; } = 0;
            public double cameraZoom { get; set; } = 0;
            public int cameraWidth { get; set; } = 1600;
            public int cameraHeight { get; set; } = 900;
            public cameraSetting() { }
            public void getFromCamera(Camera camera)
            {
                this.cameraIndex = camera.index;
                this.cameraID = camera.GetCameraID(camera.index);
                this.cameraFocus = camera.cam.Focus;
                this.cameraExposure = camera.cam.Exposure;
                this.cameraZoom = camera.cam.Zoom;
                this.cameraWidth = camera.cam.FrameWidth;
                this.cameraHeight = camera.cam.FrameHeight;
            }
            public void setToCamera(Camera camera)
            {
                camera.cam.Set(OpenCvSharp.VideoCaptureProperties.Focus, this.cameraFocus);
                camera.cam.Set(OpenCvSharp.VideoCaptureProperties.Exposure, this.cameraExposure);
                camera.cam.Set(OpenCvSharp.VideoCaptureProperties.Zoom, this.cameraZoom);
                camera.cam.Set(OpenCvSharp.VideoCaptureProperties.FrameWidth, cameraWidth);
                camera.cam.Set(OpenCvSharp.VideoCaptureProperties.FrameHeight, cameraHeight);
            }
        }

        public List<cameraSetting> camerasSetting { get; set; } = new List<cameraSetting>(4);
        public Setting()
        {
            camerasSetting.Add(new cameraSetting());
            camerasSetting.Add(new cameraSetting());
            camerasSetting.Add(new cameraSetting());
            camerasSetting.Add(new cameraSetting());
        }

        public bool Save()
        {
            bool saveresult = true;
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                if (!Directory.Exists(Path.Replace("setting.icop", ""))) Directory.CreateDirectory(Path.Replace("setting.icop", ""));
                string settingJson = JsonSerializer.Serialize(this, options);
                File.WriteAllText(Path, settingJson);
            }
            catch (Exception err)
            {
                saveresult = false;
                Console.WriteLine("Save error:" + err.ToString());
            }
            return saveresult;
        }

        public bool load(Setting setting)
        {
            bool loadResult = true;
            try
            {
                string settingJson = File.ReadAllText(Path);
                setting = JsonSerializer.Deserialize<Setting>(settingJson);
            }
            catch (Exception)
            {
                loadResult = false;
            }
            return loadResult;
        }
        public string showDebug()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string settingJson = JsonSerializer.Serialize(this, options);
            return settingJson;
        }
    }
}
