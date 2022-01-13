using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lmi3d.GoSdk;
using Lmi3d.GoSdk.Messages;
using Lmi3d.GoSdk.Tools;
using Lmi3d.Zen;
using Lmi3d.Zen.Io;

namespace ReceiveAsync
{
    class SensorOperation
    {
        public SensorOperation()
        {

        }
        public double Get_MaxFrameRate(GoSensor sensor,GoSetup setup)
        {
            setup = sensor.Setup;
            double speed = setup.FrameRate;
            return speed;
        }
        public string Get_TriggerSource(GoSensor sensor, GoSetup setup)
        {
            setup = sensor.Setup;
            
            string triggerSource = setup.TriggerSource.ToString();
            return triggerSource;
        }
        public double Get_Exposure(GoSensor sensor, GoSetup setup)
        {
            setup = sensor.Setup;
            double exposure = setup.GetExposure(0);
            return exposure;
        }
    }
}
