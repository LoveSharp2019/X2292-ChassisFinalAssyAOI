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
    class SensorInformation
    {
        public SensorInformation()
        {

        }

        /// <summary>
        /// 获取传感器内部Job
        /// </summary>
        /// <param name="_sensor"></param>
        /// <returns></returns>
        public List<string> Get_SeneorJob(GoSensor sensor)
        {
            List<string> sensorJobName = new List<string>();
            for (int i = 0; i < sensor.FileCount; i++)
            {
                sensorJobName.Add(sensor.GetFileName(i));
            }
            return sensorJobName;
        }
        public string Get_SensorDefaultJob(GoSensor sensor)
        {
            string sensorDefaultJob = sensor.DefaultJob;
            return sensorDefaultJob;
        }

        /// <summary>
        /// 获取传感器状态
        /// </summary>
        /// <param name="_sensor"></param>
        /// <returns></returns>
        public string Get_SensorStatus(GoSensor sensor)
        {
            GoState sensorStatus;
            sensorStatus = sensor.State;
            return sensorStatus.ToString();
        }

        /// <summary>
        /// 获取传感器版本号
        /// </summary>
        /// <param name="_sensor"></param>
        /// <returns></returns>
        public string Get_SensorVersions(GoSensor sensor)
        {
            GoSensorInfo goSensorInfo = new GoSensorInfo();
            KVersion sensorVersion = sensor.FirmwareVersion;
            return sensorVersion.ToString();
        }

        /// <summary>
        /// 获取传感器扫描模式
        /// </summary>
        /// <param name="_sensor"></param>
        /// <returns></returns>
        public string Get_SensorScanMode(GoSensor sensor)
        {
            GoMode sensorScanMode;
            sensorScanMode = sensor.ScanMode;
            return sensorScanMode.ToString();
        }
        /// <summary>
        /// 获取传感器序列号
        /// </summary>
        /// <param name="_sensor"></param>
        /// <returns></returns>
        public string Get_SensorSerialNumber(GoSensor sensor)
        {
            UInt32 sensorSerialNumber;
            sensorSerialNumber = sensor.Id;
            return sensorSerialNumber.ToString();
        }

        /// <summary>
        /// 获取传感器型号
        /// </summary>
        /// <param name="_sensor"></param>
        /// <returns></returns>
        public string Get_SensorType(GoSensor sensor)
        {
            string goMode = null;
            try
            {
                goMode = sensor.Model;
                return goMode;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
