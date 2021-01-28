using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace ICOP
{
    public class IO_Port
    {
        public enum StatusCommand
        { 
            ModeNG = 48,
            ModeOut = 49,
            ModeIn = 50,
            ModePass = 52,
            ModeUp = 53,
        }
        

        public SerialPort IO_COM_Port = new SerialPort();

        public IO_Port() { }
        public void Set_IO_Port( string SerialPortName)
        {
            this.IO_COM_Port.PortName = SerialPortName;
            this.IO_COM_Port.BaudRate = 9600;
            if (!IO_COM_Port.IsOpen)
            {
                this.IO_COM_Port.Open();
            }
        }

        public void senMessenger(StatusCommand command)
        {
            if (IO_COM_Port.IsOpen)
            {
                IO_COM_Port.WriteLine(command.ToString());
            }
            else
            {
                Global.ICOP_messenger("Serial port is not connected.");
            }
        }
    }
}
