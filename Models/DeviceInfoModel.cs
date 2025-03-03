using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BluetoothAssistant.Models
{
    public class DeviceInfoModel
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public int ComPort { get; set; } // Default or parsed COM port, may be -1 if not assigned
    }
}
