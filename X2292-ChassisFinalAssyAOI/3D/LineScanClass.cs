using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lmi3d.GoSdk;
using Lmi3d.Zen;
using Lmi3d.Zen.Io;
using Lmi3d.GoSdk.Messages;
using System.Runtime.InteropServices;
using HalconDotNet;
using System.IO;
using System.Threading;

using System.Drawing.Imaging;
//using FT;

namespace ReceiveAsync
{
    public class LineScanClass
    {
        public static int ProfileGenImageLength = 0;   //设定生成点云时的profile数量
        public static List<short[]> MM = new List<short[]>();
       public static HObject image;
        public static string mm = "";

        public static string strCloudPointNumber = "7900";
        public static object objDatasoure = null;
        public static string strSenserDefalutJob = null;
        public static string strSenserStatus = null;
        public static string strSenserType = null;
        public static string strSenserVersion = null;
        public static string strSenserSerialNumber = null;
        public static string strSenserSpeed = null;
        public static string strSenserTriggerMode = null;
        public static string strSenserExpose = null;
        public static int timerInterval1 = 100;
        public static int timerInterval2 = 25500;

        public static string[] hImageData = null;
        public static bool bImageEnd = false;
        private static GoSystem system;
        private static GoSensor sensor;
        System.Windows.Forms.Timer timer1;
        Thread th = null;
        public LineScanClass()
        {


        }

        public struct ProfilePoint
        {
            public double x;
            public double z;
            byte intensity;
        }

        public struct SurfacePoints
        {
            public double x;
            public double y;
            public double z;
        }




        /// <summary>
        /// 3D初始化，连接
        /// </summary>
        /// <param name="strSN"></param>
        public static bool IniLineScan(string strSN, ref string StrError)
        {
            bool init = false;
            try
            {
                StrError = IniLMISystem(strSN);
                if (StrError != "")
                    init = false;
                else
                    init = true;
            }
            catch (Exception ex)
            {
                init = false;
            }
            return init;
        }

        public static void startWork()
        {
            WriteLog2("上次扫描数量:" + MM.Count);
            MM.Clear();
            Thread.Sleep(10);

            system.Start();
        }

        public static void stopWork()
        {
            Thread.Sleep(30);
            system.Stop();
        }

        public static void disConnect()
        {
            if (sensor != null)
                sensor.Disconnect();
        }


        /// <summary>
        /// 切换job
        /// </summary>
        /// <param name="strJobNme"></param>
        public void IDSwitcher(string strJobNme)
        {
            try
            {
                sensor.DefaultJob = strJobNme;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="str"></param>
        public static void WriteLog(string str)
        {
            string Time = DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second + "." + DateTime.Now.Millisecond;
            FileInfo fi = new FileInfo(Application.StartupPath + "\\3DLog\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
            if (!Directory.Exists(fi.DirectoryName))
            {
                Directory.CreateDirectory(fi.DirectoryName);
            }

            using (StreamWriter sw = new StreamWriter(fi.FullName, true))
            {
                sw.WriteLine(Time + str);
                sw.WriteLine("---------------------------------------------------------");
                sw.WriteLine(" ");
                sw.Close();
            }

        }

        public static void WriteLog2(string str)
        {
            string Time = DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second + "." + DateTime.Now.Millisecond;
            FileInfo fi = new FileInfo(Application.StartupPath + "\\3DLog\\" + DateTime.Now.ToString("yyyyMMdd") + "_1.txt");
            if (!Directory.Exists(fi.DirectoryName))
            {
                Directory.CreateDirectory(fi.DirectoryName);
            }

            using (StreamWriter sw = new StreamWriter(fi.FullName, true))
            {
                sw.WriteLine(Time + str);
                sw.WriteLine("---------------------------------------------------------");
                sw.WriteLine(" ");
                sw.Close();
            }

        }

        public static HObject GetData()
        {
            ProfileGenImageLength = Convert.ToInt32(strCloudPointNumber);
            return GenTiffImage(MM);
        }



        /// <summary>
        /// 3D相机初始化
        /// </summary>
        /// <param name="strIP"></param>
        private static string IniLMISystem(string strIP)
        {
            string Message = "";

            try
            {
                SensorInformation sensorInformation = new SensorInformation();
                SensorOperation sensorOperation = new SensorOperation();

                KApiLib.Construct();
                GoSdkLib.Construct();
                system = new GoSystem();

                KIpAddress ipAddress = KIpAddress.Parse(strIP);
                sensor = system.FindSensorByIpAddress(ipAddress);
                sensor.Connect();
                GoSetup setup = sensor.Setup;
                system.EnableData(true);
                system.SetDataHandler(onData);
                //system.Stop();

                objDatasoure = sensorInformation.Get_SeneorJob(sensor);//显示传感器内部Job
                strSenserDefalutJob = sensorInformation.Get_SensorDefaultJob(sensor);
                strSenserStatus = sensorInformation.Get_SensorStatus(sensor);//显示传感器状态
                strSenserType = sensorInformation.Get_SensorType(sensor);//显示传感器型号
                strSenserVersion = sensorInformation.Get_SensorVersions(sensor);//显示传感器版本号
                strSenserSerialNumber = sensorInformation.Get_SensorSerialNumber(sensor);//显示传感器序列号
                strSenserSpeed = sensorOperation.Get_MaxFrameRate(sensor, setup).ToString() + " HZ";
                strSenserTriggerMode = sensorOperation.Get_TriggerSource(sensor, setup);
                strSenserExpose = sensorOperation.Get_Exposure(sensor, setup).ToString() + " ms";

            }
            catch (KException ex)
            {
                Message = ex.Status.ToString();
                //System.Windows.Forms.MessageBox.Show("Error:" + ex.Status);
            }
            return Message;
        }

        private static void onData(KObject data)
        {
            GoDataSet dataSet = (GoDataSet)data;
            for (UInt32 i = 0; i < dataSet.Count; i++)
            {
                GoDataMsg dataObj = (GoDataMsg)dataSet.Get(i);
                switch (dataObj.MessageType)
                {
                    case GoDataMessageType.UniformProfile://轮廓线
                        {
                            GoUniformProfileMsg profileMsg = (GoUniformProfileMsg)dataObj;
                            for (UInt32 k = 0; k < profileMsg.Count; ++k)
                            {
                                int profilePointCount = profileMsg.Width;
                                short[] points = new short[profilePointCount];
                                ProfilePoint[] profileBuffer = new ProfilePoint[profilePointCount];
                                IntPtr pointsPtr = profileMsg.Data;
                                Marshal.Copy(pointsPtr, points, 0, points.Length);
                                MM.Add(points);    //profile增加到list列表中
                            }
                        }
                        break;
                    case GoDataMessageType.UniformSurface://点云数据
                        {
                            GoUniformSurfaceMsg goSurfaceMsg = (GoUniformSurfaceMsg)dataObj; // 定义变量gosurfacemsg,类型gosurfacemsg
                            long length = goSurfaceMsg.Length;    //surface长度
                            long width = goSurfaceMsg.Width;      //surface宽度
                            long bufferSize = width * length;
                            double XResolution = goSurfaceMsg.XResolution / 1000000.0;  //surface 数据X方向分辨率为nm,转为mm
                            double YResolution = goSurfaceMsg.YResolution / 1000000.0;  //surface 数据Y方向分辨率为nm,转为mm
                            double ZResolution = goSurfaceMsg.ZResolution / 1000000.0;  //surface 数据Z方向分辨率为nm,转为mm
                            double XOffset = goSurfaceMsg.XOffset / 1000.0;             //接收到surface数据X方向补偿单位um，转mm
                            double YOffset = goSurfaceMsg.YOffset / 1000.0;             //接收到surface数据Y方向补偿单位um，转mm
                            double ZOffset = goSurfaceMsg.ZOffset / 1000.0;             //接收到surface数据Z方向补偿单位um，转mm

                            IntPtr bufferPointer = goSurfaceMsg.Data;
                            short[] ranges = new short[bufferSize];
                            Marshal.Copy(bufferPointer, ranges, 0, ranges.Length);

                            #region 点云数据保存
                            //int rowIdx, colIdx;
                            //SurfacePoints[] surfacePointCloud = new SurfacePoints[bufferSize];

                            //FileStream fs = new FileStream("C:\\Soft\\TestTxt.txt", FileMode.Append, FileAccess.Write);
                            //StreamWriter sr = new StreamWriter(fs);
                            //for (rowIdx = 0; rowIdx < length; rowIdx++)//row is in Y direction
                            //{
                            //    for (colIdx = 0; colIdx < width; colIdx++)//col is in X direction
                            //    {
                            //        surfacePointCloud[rowIdx * width + colIdx].x = colIdx * XResolution + XOffset;//客户需要的点云数据X值
                            //        surfacePointCloud[rowIdx * width + colIdx].y = rowIdx * YResolution + YOffset;//客户需要的点云数据Y值
                            //        surfacePointCloud[rowIdx * width + colIdx].z = ranges[rowIdx * width + colIdx] * ZResolution + ZOffset;//客户需要的点云数据Z值
                            //        sr.WriteLine(surfacePointCloud[rowIdx * width + colIdx].x + "," + surfacePointCloud[rowIdx * width + colIdx].y + "," + surfacePointCloud[rowIdx * width + colIdx].z);//开始写入值
                            //    }
                            //}
                            //sr.Close();
                            //fs.Close();
                            #endregion

                            #region bitmap图像转换
                            //ColorMaps colorMaps;

                            //ushort[] ZValues = new ushort[ranges.Length];
                            //for (int k = 0; k < ranges.Length; k++)
                            //{
                            //    ZValues[k] = (ushort)(ranges[k] - short.MinValue);
                            //}
                            //colorMaps = new ColorMaps();
                            //ushort minValue = 0, maxValue = 0;
                            //colorMaps.FindMinMaxForColor(ZValues, (UInt32)ranges.Length, 0, ref minValue, ref maxValue);
                            //Color[] colors = new Color[ranges.Length];
                            //colorMaps.ToColors(ZValues, minValue, maxValue, 0, ref colors, (UInt32)ranges.Length);

                            //int Width = (int)width;
                            //int Height = (int)length;

                            //Bitmap bitmap = new Bitmap(Width, Height);
                            //BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                            //IntPtr intPtr = bitmapData.Scan0;
                            //byte[] Pixlemaps = new byte[bitmapData.Stride * bitmapData.Height];
                            //int offset = bitmapData.Stride - bitmapData.Width * 3;
                            //unsafe
                            //{
                            //    byte* pp = (byte*)(void*)bitmapData.Scan0;
                            //    for (int k = 0; k < bitmapData.Height; k++)
                            //    {
                            //        for (int m = 0; m < bitmapData.Width; m++)
                            //        {
                            //            pp[0] = (byte)(colors[k * bitmapData.Width + m].R);
                            //            pp[1] = (byte)(colors[k * bitmapData.Width + m].G);
                            //            pp[2] = (byte)(colors[k * bitmapData.Width + m].B);
                            //            pp += 3;
                            //        }
                            //        pp += bitmapData.Stride - bitmapData.Width * 3;
                            //    }
                            //}
                            //bitmap.UnlockBits(bitmapData);
                            //Bitmap aBitmap = bitmap.Clone(new RectangleF(0, 0, bitmap.Width, bitmap.Height), PixelFormat.Format24bppRgb);
                            ////this.pictureBox1.Image = aBitmap;
                            #endregion

                            #region HALCON image1

                            //HOperatorSet.GenImage1Extern(out image, "tiff", width, length, bufferPointer, IntPtr.Zero);
                            #endregion

                            #region HALCON image2

                            //HOperatorSet.GenImageConst(out image, "real", width, length);
                            //int rowIdx, colIdx;
                            //for (rowIdx = 0; rowIdx < length; rowIdx++)//row is in Y direction
                            //{
                            //    for (colIdx = 0; colIdx < width; colIdx++)//col is in X direction
                            //    {
                            //        HOperatorSet.SetGrayval(image, rowIdx, colIdx, ranges[rowIdx * width + colIdx] * ZResolution + ZOffset);
                            //    }
                            //}

                            #endregion

                        }
                        break;
                }
            }

            // Refer to example ReceiveRange, ReceiveProfile, ReceiveMeasurement and ReceiveWholePart on how to receive data
        }

        private static HObject GenTiffImage(List<short[]> Original)
        {
            HObject Image = new HObject();
            try
            {
                HTuple t1, t2, t3, t4, t5, t6;


                HObject domain;
                HTuple RowS, cols;
                HOperatorSet.CountSeconds(out t1);
                int row = Original[0].Length;
                int col = Original.Count;

                HOperatorSet.GenImageConst(out Image, "real", row, col);
                HOperatorSet.GetDomain(Image, out domain);
                HOperatorSet.GetRegionPoints(domain, out RowS, out cols);

                double[] zz = new double[col * row];

                for (int i = 0; i < col; i++)     //行数据
                {
                    int ProfileIndex = 0;
                    foreach (var points in Original[i])     //列数据
                    {
                        if (points != -32768)
                        {
                            zz[i * row + ProfileIndex] = points / 5000.0;   //数据换算
                        }
                        else
                        {
                            zz[i * row + ProfileIndex] = -10;   //无效位置赋值
                        }
                        ProfileIndex++;
                    }
                }

                HOperatorSet.SetGrayval(Image, RowS, cols, zz);

            }
            catch (Exception ex)
            {
                WriteLog("出现异常情况,LMI数据量:" + Original.Count + ex.Message + ex.StackTrace);
            }

            WriteLog("  ");
            return Image;
        }


    }
}
