using HalconDotNet;
using HEngineCall;
using ReceiveAsync;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace X2292_ChassisFinalAssyAOI
{
    public class LMIControl : SingleInstance<LMIControl>
    {
        ClassEngine _engine;
        HTuple hAxisX, hAxisY, hSinAngle;
        //平面度测量点的值
        HTuple hValue1, hValue2, hValue3, hValue4, hValue5, hValue6, hValue7;
        public HTuple hValuePoints, hValueFlatness;
        //初始化
        public bool InitLMI(string id, out string msg)
        {
            bool op;
            try
            {
                msg = "连接成功！";
                op = LineScanClass.IniLineScan(id, ref msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                op = false;
            }
            return op;
        }

        //关闭相机
        public void DisconnectLMI()
        {
            LineScanClass.disConnect();
        }

        //开始采集
        public void StartLMI()
        {
            LineScanClass.startWork();
        }

        //停止采集
        public HObject StopLMI()
        {
            LineScanClass.stopWork();

            HObject img = new HObject();
            img = LineScanClass.GetData();
            return img;
        }

        //显示图像
        public void DispImage(HObject image, HWindowControl _hWindow)
        {
            HTuple w, h;
            HOperatorSet.GetImageSize(image, out w, out h);
            HOperatorSet.ClearWindow(_hWindow.HalconWindow);
            HOperatorSet.SetPart(_hWindow.HalconWindow, 0, 0, h - 1, w - 1);
            HOperatorSet.DispObj(image, _hWindow.HalconWindow);
        }

        public bool Process3D_Ini()
        {
            bool op = false;
            string programFile = Application.StartupPath + "\\AOI测量.hdev";
            string procedurePath = "C:\\Program Files\\MVTec\\HALCON-19.11-Progress\\嘉彰AOI";
            string coordinateName = "SetCoordinate";
            string spcName = "MeasureSPC";
            string getFlatnessValuesName = "MeasureFlatnessValues";
            string flatnessName = "MeasureFlatness";

            _engine = new ClassEngine(procedurePath);
            op = _engine.InitEngine_Coordinate(programFile, coordinateName);          

            op = _engine.InitEngine_SPC(programFile, spcName);
            if (!op) return false;
          

            op = _engine.InitEngine_GetFlatnessValues(programFile, getFlatnessValuesName);
            if (!op) return false;

            op = _engine.InitEngine_Flatness(programFile, flatnessName);
            if (!op) return false;
            return true;
        }

        /// <summary>
        /// 1-建立坐标系
        /// </summary>
        /// <param name="_hImg"></param>
        /// <param name="RegX"></param>
        /// <param name="RegY"></param>
        public void SetCordinate(HObject _hImg, out HObject RegX, out HObject RegY)
        {
            bool op = _engine.ExecuteEngine_Coordinate(_hImg, out RegX, out RegY, out hAxisX, out hAxisY, out hSinAngle);
        }

        /// <summary>
        /// 测量SPC值
        /// </summary>
        /// <param name="_hImg"></param>
        /// <param name="hValueBS"></param>
        /// <param name="hCrossBS"></param>
        /// <param name="hValueBT"></param>
        /// <param name="hCrossBT"></param>
        /// <param name="hValueBF"></param>
        /// <param name="hCrossBF"></param>
        public void SPC(HObject _hImg, out HTuple hValueBS, out HObject hCrossBS, out HTuple hValueBT, out HObject hCrossBT, out HTuple hValueBF, out HObject hCrossBF)
        {
            bool op = _engine.ExecuteEngine_SPC(_hImg, hAxisX, hAxisY, hSinAngle, out hValueBS, out hCrossBS, out hValueBT, out hCrossBT, out hValueBF, out hCrossBF);

        }

        /// <summary>
        /// 测量平面度-结果hValuePoints, hValueFlatness
        /// </summary>
        /// <param name="_hImg"></param>
        /// <param name="IndexImage"></param>
        public void Flatness(HObject _hImg, int IndexImage)
        {
            if (IndexImage == 1)
            {
                HObject hCross1;
                bool op = _engine.ExecuteEngine_GetFlatnessValues(_hImg, hAxisX, hAxisY, hSinAngle, 1, out hCross1, out hValue1);
            }
            if (IndexImage == 2)
            {
                HObject hCross2;
                bool op = _engine.ExecuteEngine_GetFlatnessValues(_hImg, hAxisX, hAxisY, hSinAngle, 2, out hCross2, out hValue2);
            }
            if (IndexImage == 3)
            {
                HObject hCross3;
                bool op = _engine.ExecuteEngine_GetFlatnessValues(_hImg, hAxisX, hAxisY, hSinAngle, 3, out hCross3, out hValue3);
            }
            if (IndexImage == 4)
            {
                HObject hCross4;
                bool op = _engine.ExecuteEngine_GetFlatnessValues(_hImg, hAxisX, hAxisY, hSinAngle, 4, out hCross4, out hValue4);
            }
            if (IndexImage == 5)
            {
                HObject hCross5;
                bool op = _engine.ExecuteEngine_GetFlatnessValues(_hImg, hAxisX, hAxisY, hSinAngle, 5, out hCross5, out hValue5);
            }
            if (IndexImage == 6)
            {
                HObject hCross6;
                bool op = _engine.ExecuteEngine_GetFlatnessValues(_hImg, hAxisX, hAxisY, hSinAngle, 6, out hCross6, out hValue6);
            }
            if (IndexImage == 7)
            {
                HObject hCross7;
                bool op = _engine.ExecuteEngine_GetFlatnessValues(_hImg, hAxisX, hAxisY, hSinAngle, 7, out hCross7, out hValue7);

            }


            //3.2计算平面度数值
            if (IndexImage == 7)
            {

                bool op = _engine.ExecuteEngine_Flatness(hValue1, hValue2, hValue3, hValue4, hValue5, hValue6, hValue7, out hValuePoints, out hValueFlatness);
            }

        }
        /// <summary>
        /// 旋转图片
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <param name="ho_OutImage"></param>
        /// <param name="hv_IndexImage"></param>
        public void TransImage(HObject ho_Image, out HObject ho_OutImage, HTuple hv_IndexImage)
        {




            // Local iconic variables 

            HObject ho_ImageMirror = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_OutImage);
            HOperatorSet.GenEmptyObj(out ho_ImageMirror);


            if ((int)((new HTuple((new HTuple((new HTuple(hv_IndexImage.TupleEqual(1))).TupleOr(
                new HTuple(hv_IndexImage.TupleEqual(3))))).TupleOr(new HTuple(hv_IndexImage.TupleEqual(
                5))))).TupleOr(new HTuple(hv_IndexImage.TupleEqual(7)))) != 0)
            {
                ho_ImageMirror.Dispose();
                HOperatorSet.MirrorImage(ho_Image, out ho_ImageMirror, "column");
                ho_OutImage.Dispose();
                HOperatorSet.RotateImage(ho_ImageMirror, out ho_OutImage, 180, "constant");
            }
            else
            {
                ho_OutImage.Dispose();
                HOperatorSet.CopyImage(ho_Image, out ho_OutImage);
            }



            ho_ImageMirror.Dispose();


            return;


        }

        public void SaveImage(HObject image, string fileName)
        {
            try
            {
                string PathName = string.Format("{0}\\{1}年\\{2}月\\{3}日\\{4}班\\", frm_Main.MyPara.mComConfig.ImagePath_3D, frm_Main.MyPara.mComConfig.Shift.ShiftYear, frm_Main.MyPara.mComConfig.Shift.ShiftMonth, frm_Main.MyPara.mComConfig.Shift.ShiftDay, frm_Main.MyPara.mComConfig.Shift.Name);
                string tempPath = System.IO.Path.GetDirectoryName(PathName);
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }

                HOperatorSet.WriteImage(image, "tiff", 0, PathName+ fileName + ".tiff");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存图片异常。" + ex.Message);
            }
        }
    }



}
