using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace X2292_ChassisFinalAssyAOI
{
    /// <summary>
    /// 激光检测站
    /// </summary>
    public class Laser_Station: SingleInstance<Laser_Station>
    {
        private System.Threading.Timer TimerAxisStatus;//轴状态扫描

        /// <summary>
        /// 激光X轴
        /// </summary>
        public Axis Axis_X   =   new Axis  { Axis_Index = 4, PulseToMM = 0.001 };

        /// <summary>
        /// 激光Y轴
        /// </summary>
        public Axis Axis_Y   =   new Axis  { Axis_Index = 1, PulseToMM = 0.001 };

        /// <summary>
        /// 左工位-Y1轴
        /// </summary>
        public Axis Axis_Y1 =   new Axis  { Axis_Index = 3, PulseToMM = 0.001 };

        /// <summary>
        /// 右工位-Y2轴
        /// </summary>
        public Axis Axis_Y2 =   new Axis  { Axis_Index = 2, PulseToMM = 0.001 };
        public void Ini()
        {
            TimerAxisStatus = new System.Threading.Timer(new System.Threading.TimerCallback(AxisStatusScan), null, 0, 500);
        }
        private void AxisStatusScan(object value)
        {
            bool _IsAlarm, _IsLimitPositive, _IsLimitNegative;
            double Pos;
            //-Axis_X状态扫描
            frm_Main.mCard1.CheckAxisStatus(Axis_X.Axis_Index, out _IsAlarm, out _IsLimitPositive, out _IsLimitNegative);
            frm_Main.mCard1.GetPrfPos(Axis_X.Axis_Index, Axis_X.PulseToMM, out Pos);
            Axis_X.IsAlarm = _IsAlarm;
            Axis_X.IsLimitPositive = _IsLimitPositive;
            Axis_X.IsLimitNegative = _IsLimitNegative;
            Axis_X.Pos = Pos;

            //-Axis_Y状态扫描
            frm_Main.mCard1.CheckAxisStatus(Axis_Y.Axis_Index, out _IsAlarm, out _IsLimitPositive, out _IsLimitNegative);
            frm_Main.mCard1.GetPrfPos(Axis_Y.Axis_Index, Axis_Y.PulseToMM, out Pos);
            Axis_Y.IsAlarm = _IsAlarm;
            Axis_Y.IsLimitPositive = _IsLimitPositive;
            Axis_Y.IsLimitNegative = _IsLimitNegative;
            Axis_Y.Pos = Pos;

            //-Axis_Y1状态扫描
            frm_Main.mCard1.CheckAxisStatus(Axis_Y1.Axis_Index, out _IsAlarm, out _IsLimitPositive, out _IsLimitNegative);
            frm_Main.mCard1.GetPrfPos(Axis_Y1.Axis_Index, Axis_Y1.PulseToMM, out Pos);
            Axis_Y1.IsAlarm = _IsAlarm;
            Axis_Y1.IsLimitPositive = _IsLimitPositive;
            Axis_Y1.IsLimitNegative = _IsLimitNegative;
            Axis_Y1.Pos = Pos;

            //-Axis_Y2状态扫描
            frm_Main.mCard1.CheckAxisStatus(Axis_Y2.Axis_Index, out _IsAlarm, out _IsLimitPositive, out _IsLimitNegative);
            frm_Main.mCard1.GetPrfPos(Axis_Y2.Axis_Index, Axis_Y2.PulseToMM, out Pos);
            Axis_Y2.IsAlarm = _IsAlarm;
            Axis_Y2.IsLimitPositive = _IsLimitPositive;
            Axis_Y2.IsLimitNegative = _IsLimitNegative;
            Axis_Y2.Pos = Pos;

        }

        public bool HomeXY()
        {
            bool isHomeFinishedX = false, isHomeFinishedY = false;
            frm_Main.mCard1.Home(Axis_X.Axis_Index, HomeMoveDirection.Negative, HomeMode.LimitHome, Axis_X. PulseToMM, 200, 50, 0);
            frm_Main.mCard1.Home(Axis_Y.Axis_Index, HomeMoveDirection.Negative, HomeMode.LimitHome, Axis_Y.PulseToMM, 200, 50, 0);
            frm_Main.mCard1.IsHomeFinished( Axis_X.Axis_Index, out isHomeFinishedX);
            frm_Main.mCard1.IsHomeFinished(Axis_Y.Axis_Index, out isHomeFinishedY);
            return isHomeFinishedX && isHomeFinishedY;
        }

        public bool HomeY1()
        {
            bool  isHomeFinishedY1 = false;
            frm_Main.mCard1.Home(Axis_Y1.Axis_Index, HomeMoveDirection.Negative, HomeMode.LimitHome, Axis_Y1.PulseToMM, 200, 50, 0);
            frm_Main.mCard1.IsHomeFinished(Axis_Y1.Axis_Index, out isHomeFinishedY1);
            return  isHomeFinishedY1 ;
        }
        public bool HomeY2()
        {
            bool isHomeFinishedY2 = false;
            frm_Main.mCard1.Home(Axis_Y2.Axis_Index, HomeMoveDirection.Negative, HomeMode.LimitHome, Axis_Y2.PulseToMM, 200, 50, 0);
            frm_Main.mCard1.IsHomeFinished(Axis_Y2.Axis_Index, out isHomeFinishedY2);
            return isHomeFinishedY2;
        }

        public bool Home(Axis mAxis)
        {
            bool isHomeFinished = false;
            frm_Main.mCard1.Home(mAxis.Axis_Index, HomeMoveDirection.Negative, HomeMode.LimitHome, mAxis.PulseToMM, 200, 10, 0);
            frm_Main.mCard1.IsHomeFinished(mAxis.Axis_Index, out isHomeFinished);

            return isHomeFinished;
        }

        public void JogStart(Axis mAxis, double Speed, bool isPositive)
        {
            frm_Main.mCard1.Jog(mAxis.Axis_Index, mAxis.PulseToMM, Speed, isPositive);
        }

        public void JogStop(Axis mAxis)
        {
            frm_Main.mCard1.AxisStop(mAxis.Axis_Index);
        }

        public void MoveTo(Axis mAxis , double Speed, double Pos)
        {
            bool bMotionDone = false;
            frm_Main.mCard1.AbsoluteMove(mAxis.Axis_Index, mAxis.PulseToMM, 1, Speed, Pos);
            frm_Main.mCard1.MotionDone(mAxis.Axis_Index, out bMotionDone);
        }
       
        public bool LeftStationMoveTo(int index,bool isOnlyXYWork,bool isOnlyYWork,bool isXYYWork)
        {
            bool bMotionDone = false;
            double speed =  Recipe.Instance._Recipe_Para.DT_LeftLaser.ToList()[index].Vel;
            double acc =      Recipe.Instance._Recipe_Para.DT_LeftLaser.ToList()[index].Acc;
            double posX =   Recipe.Instance._Recipe_Para.DT_LeftLaser.ToList()[index].XPos;
            double posY =   Recipe.Instance._Recipe_Para.DT_LeftLaser.ToList()[index].YPos;
            double posY1 = Recipe.Instance._Recipe_Para.DT_LeftLaser.ToList()[index].Y1Pos;
           
            if(isOnlyXYWork)
            {
                frm_Main.mCard1.AbsoluteMove(Axis_X.Axis_Index, Axis_X.PulseToMM, acc, speed, posX);
                frm_Main.mCard1.AbsoluteMove(Axis_Y.Axis_Index, Axis_Y.PulseToMM, acc, speed, posY);               
                frm_Main.mCard1.MotionDone(Axis_X.Axis_Index, out bMotionDone);
                frm_Main.mCard1.MotionDone(Axis_Y.Axis_Index, out bMotionDone);
            }
            else if(isOnlyYWork)
            {
                frm_Main.mCard1.AbsoluteMove(Axis_Y1.Axis_Index, Axis_Y1.PulseToMM, acc, speed, posY1);
                frm_Main.mCard1.MotionDone(Axis_Y1.Axis_Index, out bMotionDone);
            }
            else if (isXYYWork)
            {
                frm_Main.mCard1.AbsoluteMove(Axis_X.Axis_Index, Axis_X.PulseToMM, acc, speed, posX);
                frm_Main.mCard1.AbsoluteMove(Axis_Y.Axis_Index, Axis_Y.PulseToMM, acc, speed, posY);
                frm_Main.mCard1.AbsoluteMove(Axis_Y1.Axis_Index, Axis_Y1.PulseToMM, acc, speed, posY1);
                frm_Main.mCard1.MotionDone(Axis_X.Axis_Index, out bMotionDone);
                frm_Main.mCard1.MotionDone(Axis_Y.Axis_Index, out bMotionDone);
                frm_Main.mCard1.MotionDone(Axis_Y1.Axis_Index, out bMotionDone);
            }
            else
            {

            }
            return Axis_X.IsAlarm || Axis_Y.IsAlarm || Axis_Y1.IsAlarm;
        }

        public bool RightStationMoveTo(int index , bool isOnlyXYWork, bool isOnlyYWork, bool isXYYWork)
        {
            bool bMotionDone = false;
            double speed =  Recipe.Instance._Recipe_Para.DT_RightLaser.ToList()[index].Vel;
            double acc =      Recipe.Instance._Recipe_Para.DT_RightLaser.ToList()[index].Acc;
            double posX =   Recipe.Instance._Recipe_Para.DT_RightLaser.ToList()[index].XPos;
            double posY =   Recipe.Instance._Recipe_Para.DT_RightLaser.ToList()[index].YPos;
            double posY2 = Recipe.Instance._Recipe_Para.DT_RightLaser.ToList()[index].Y2Pos;

            if(isOnlyXYWork)
            {
                frm_Main.mCard1.AbsoluteMove(Axis_X.Axis_Index, Axis_X.PulseToMM, acc, speed, posX);
                frm_Main.mCard1.AbsoluteMove(Axis_Y.Axis_Index, Axis_Y.PulseToMM, acc, speed, posY); 
                frm_Main.mCard1.MotionDone(Axis_X.Axis_Index, out bMotionDone);
                frm_Main.mCard1.MotionDone(Axis_Y.Axis_Index, out bMotionDone);
            }
            else if(isOnlyYWork)
            {
                frm_Main.mCard1.AbsoluteMove(Axis_Y2.Axis_Index, Axis_Y2.PulseToMM, acc, speed, posY2);
                frm_Main.mCard1.MotionDone(Axis_Y2.Axis_Index, out bMotionDone);
            }
            else if(isXYYWork)
            {
                frm_Main.mCard1.AbsoluteMove(Axis_X.Axis_Index, Axis_X.PulseToMM, acc, speed, posX);
                frm_Main.mCard1.AbsoluteMove(Axis_Y.Axis_Index, Axis_Y.PulseToMM, acc, speed, posY);
                frm_Main.mCard1.AbsoluteMove(Axis_Y2.Axis_Index, Axis_Y2.PulseToMM, acc, speed, posY2);

                frm_Main.mCard1.MotionDone(Axis_X.Axis_Index, out bMotionDone);
                frm_Main.mCard1.MotionDone(Axis_Y.Axis_Index, out bMotionDone);
                frm_Main.mCard1.MotionDone(Axis_Y2.Axis_Index, out bMotionDone);
            }
            else
            {

            }

            return Axis_X.IsAlarm || Axis_Y.IsAlarm || Axis_Y2.IsAlarm;
        }


        public bool  RightStatonFixtureRBCylinder(bool IsWork)
        {
            bool result = false;
            if(IsWork)//左前侧推工作
            {
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 19-1, false);//载具2右侧推气缸回
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 20-1, true);//载具2右侧推气缸出
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 23-1, false);//载具2后侧推气缸回
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 24-1, true);//载具2后侧推气缸出
            Air:
                DateTime now = DateTime.Now;
                while (true)
                {
                    Application.DoEvents();
                    Thread.Sleep(20);
                    if (MotionControl_GTS.Input(frm_Main.InputBitInfoS, 20-1) == 1 || MotionControl_GTS.Input(frm_Main.InputBitInfoS, 24-1) == 1)
                    {
                        result = true;
                        break;
                    }
                    if (!frm_Main.MyPara.mParaSetting.Disable_RightCylinderSensor)
                    {
                        if ((DateTime.Now - now).Seconds > frm_Main.MyPara.mComConfig._真空吸延时)
                        {
                            TaskManage.GetSingleton().RaiseDisplayLog("载具2右后侧推-出超时", Color.Red);
                            DialogResult res = MessageBox.Show("载具2右后侧推-出超时", "超时错误...", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);

                            if (res == DialogResult.Abort)
                            {
                                Task.Factory.StartNew(() => TaskManage.GetSingleton().StopWork());
                                break;
                            }
                            else if (res == DialogResult.Retry)
                            {
                                goto Air;
                            }
                            else if (res == DialogResult.Ignore)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(frm_Main.MyPara.mParaSetting.Disable_Time_CylinderSensor);
                    }
                     
                }
            }
            else
            {
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 19 - 1, true);//载具2右侧推气缸回
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 20 - 1, false);//载具2右侧推气缸出
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 23 - 1, true);//载具2后侧推气缸回
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 24 - 1, false);//载具2后侧推气缸出
               
            }          
            return result;
        }

        public bool RightStatonFixtureLFCylinder(bool IsWork)
        {
            bool result = false;
            if (IsWork)
            {
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 17-1, false);//载具1左侧推气缸回
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 18-1, true);//载具1左侧推气缸出          
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 21-1, false);//载具1前侧推气缸回
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 22-1, true);//载具1前侧推气缸出
            Air:
                DateTime now = DateTime.Now;
                while (true)
                {
                    Application.DoEvents();
                    Thread.Sleep(20);
                    if (MotionControl_GTS.Input(frm_Main.InputBitInfoS, 18-1) == 1|| MotionControl_GTS.Input(frm_Main.InputBitInfoS, 22-1) == 1)
                    {
                        result = true;
                        break;
                    }
                    if (!frm_Main.MyPara.mParaSetting.Disable_RightCylinderSensor)
                    {
                        if ((DateTime.Now - now).Seconds > frm_Main.MyPara.mComConfig._真空吸延时)
                        {
                            TaskManage.GetSingleton().RaiseDisplayLog("载具1左前侧推-出超时", Color.Red);
                            DialogResult res = MessageBox.Show("载具1左前侧推-出超时", "超时错误...", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);

                            if (res == DialogResult.Abort)
                            {
                                Task.Factory.StartNew(() => TaskManage.GetSingleton().StopWork());
                                break;
                            }
                            else if (res == DialogResult.Retry)
                            {
                                goto Air;
                            }
                            else if (res == DialogResult.Ignore)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(frm_Main.MyPara.mParaSetting.Disable_Time_CylinderSensor);
                    }
                      
                }
            }
            else
            {
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 17-1, true);//载具1左侧推气缸回
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 18-1, false);//载具1左侧推气缸出          
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 21-1, true);//载具1前侧推气缸回
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 22-1, false);//载具1前侧推气缸出

            }
            return result;
        }      


        public bool LeftStatonFixtureRBCylinder(bool IsWork)
        {
            bool result = false;
            if (IsWork)//左前侧推工作
            {
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 27-1, false);//载具2右侧推气缸回
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 28-1, true);//载具2右侧推气缸出
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 31-1, false);//载具2后侧推气缸回
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 32-1, true);//载具2后侧推气缸出
            Air:
                DateTime now = DateTime.Now;
                while (true)
                {
                    Application.DoEvents();
                    Thread.Sleep(20);
                    if (MotionControl_GTS.Input(frm_Main.InputBitInfoS, 28-1) == 1 || MotionControl_GTS.Input(frm_Main.InputBitInfoS, 32 - 1) == 1)
                    {
                        result = true;
                        break;
                    }
                    if (!frm_Main.MyPara.mParaSetting.Disable_LeftCylinderSensor)
                    {
                        if ((DateTime.Now - now).Seconds > frm_Main.MyPara.mComConfig._真空吸延时)
                        {
                            TaskManage.GetSingleton().RaiseDisplayLog("载具2右后侧推-出超时", Color.Red);
                            DialogResult res = MessageBox.Show("载具2右后侧推-出超时", "超时错误...", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);

                            if (res == DialogResult.Abort)
                            {
                                Task.Factory.StartNew(() => TaskManage.GetSingleton().StopWork());
                                break;
                            }
                            else if (res == DialogResult.Retry)
                            {
                                goto Air;
                            }
                            else if (res == DialogResult.Ignore)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(frm_Main.MyPara.mParaSetting.Disable_Time_CylinderSensor);
                    }
                       
                }
            }
            else
            {
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 27 - 1, true);//载具2右侧推气缸回
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 28 - 1, false);//载具2右侧推气缸出
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 31 - 1, true);//载具2后侧推气缸回
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 32 - 1, false);//载具2后侧推气缸出

            }
            return result;

        }

        public bool LeftStatonFixtureLFCylinder(bool IsWork)
        {
            bool result = false;
            if (IsWork)//左前侧推工作
            {
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 25-1, false);//载具2左侧推气缸回
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 26-1, true);//载具2左侧推气缸出          
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 29-1, false);//载具2前侧推气缸回
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 30-1, true);//载具2前侧推气缸出
            Air:                
                DateTime now = DateTime.Now;
                while (true)
                {
                    Application.DoEvents();
                    Thread.Sleep(20);
                    if (MotionControl_GTS.Input(frm_Main.InputBitInfoS, 26-1) == 1|| MotionControl_GTS.Input(frm_Main.InputBitInfoS, 30-1) == 1)
                    {
                        result = true;
                        break;
                    }
                    if(!frm_Main.MyPara.mParaSetting.Disable_LeftCylinderSensor)
                    {
                        if ((DateTime.Now - now).Seconds > frm_Main.MyPara.mComConfig._真空吸延时)
                        {

                            TaskManage.GetSingleton().RaiseDisplayLog("载具2左前侧推-出超时", Color.Red);
                            DialogResult res = MessageBox.Show("载具2左前侧推-出超时", "超时错误...", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);

                            if (res == DialogResult.Abort)
                            {
                                Task.Factory.StartNew(() => TaskManage.GetSingleton().StopWork());
                                break;
                            }
                            else if (res == DialogResult.Retry)
                            {
                                goto Air;
                            }
                            else if (res == DialogResult.Ignore)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(frm_Main.MyPara.mParaSetting.Disable_Time_CylinderSensor);
                    }

                }
            }
            else
            {
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 25 - 1, true);//载具2左侧推气缸回
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 26 - 1, false);//载具2左侧推气缸出          
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 29 - 1, true);//载具2前侧推气缸回
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 30 - 1, false);//载具2前侧推气缸出

            }
            return result;

        }

       
        public bool RightStationVacummOn(bool IsON)
        {
            bool result = false;
            if (IsON)
            {
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 33-1, true);//载具1吸真空1
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 35-1, true);//载具1吸真空2
            Air:
                DateTime now = DateTime.Now;
                while (true)
                {
                    Application.DoEvents();
                    Thread.Sleep(20);
                    if (MotionControl_GTS.Input(frm_Main.InputBitInfoS, 33-1) == 1 || MotionControl_GTS.Input(frm_Main.InputBitInfoS, 35 - 1) == 1)
                    {
                        result = true;
                        break;
                    }
                    if (!frm_Main.MyPara.mParaSetting.Disable_LeftCylinderSensor)
                    {
                        if ((DateTime.Now - now).Seconds > frm_Main.MyPara.mComConfig._真空吸延时)
                        {
                            TaskManage.GetSingleton().RaiseDisplayLog("右工位载具1吸真空超时", Color.Red);
                            DialogResult res = MessageBox.Show("右工位载具1吸真空超时", "超时错误...", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);

                            if (res == DialogResult.Abort)
                            {
                                Task.Factory.StartNew(() => TaskManage.GetSingleton().StopWork());
                                break;
                            }
                            else if (res == DialogResult.Retry)
                            {
                                goto Air;
                            }
                            else if (res == DialogResult.Ignore)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(frm_Main.MyPara.mParaSetting.Disable_Time_CylinderSensor);
                    }
                        
                }
                return result;
            }
            else
            {
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 33 - 1, false);//载具1吸真空1
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 35 - 1, false);//载具1吸真空2
                RightStationBreakVacummON(true);
                System.Threading.Thread.Sleep(500);
                RightStationBreakVacummON(false);
                return true;
            }
          
           

        }
        private void RightStationBreakVacummON(bool isON)
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 34-1, isON);//载具1破真空1
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 36-1, isON);//载具1破真空2
        }

        public bool LeftStationVacummOn(bool IsON)
        {
            bool result = false;
            if(IsON)
            {
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 37-1, true);//载具2吸真空1
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 39-1, true);//载具2吸真空2
            Air:
                DateTime now = DateTime.Now;
                while (true)
                {
                    Application.DoEvents();
                    Thread.Sleep(20);
                    if (MotionControl_GTS.Input(frm_Main.InputBitInfoS, 37-1) == 1 || MotionControl_GTS.Input(frm_Main.InputBitInfoS, 39 - 1) == 1)
                    {
                        result = true;
                        break;
                    }
                    if ((DateTime.Now - now).Seconds > frm_Main.MyPara.mComConfig._真空吸延时)
                    {
                        TaskManage.GetSingleton().RaiseDisplayLog("左工位载具2吸真空超时", Color.Red);
                        DialogResult res = MessageBox.Show("左工位载具2吸真空超时", "超时错误...", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);

                        if (res == DialogResult.Abort)
                        {
                            Task.Factory.StartNew(() => TaskManage.GetSingleton().StopWork());
                            break;
                        }
                        else if (res == DialogResult.Retry)
                        {
                            goto Air;
                        }
                        else if (res == DialogResult.Ignore)
                        {
                            break;
                        }
                    }
                }
                return result;
            }
            else
            {
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 37 - 1, false);//载具2吸真空1
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 39 - 1, false);//载具2吸真空2
                LeftStationBreakVacummON(true);
                System.Threading.Thread.Sleep(500);
                LeftStationBreakVacummON(false);
                return true;
            }
          
        }
        private void LeftStationBreakVacummON(bool IsON)
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 38-1, IsON);//载具2破真空1
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 40-1, IsON);//载具2破真空2
        }

        public bool RightStationMaterialSensor()
        {
          int res =  MotionControl_GTS.Input(frm_Main.InputBitInfoS, 41-1);//载具1物料感应
            if (res == 1)
                return true;
            else
                return false;
        }
        public bool LeftStationMaterialSensor()
        {
            int res = MotionControl_GTS.Input(frm_Main.InputBitInfoS, 42-1);//载具2物料感应
            if (res == 1)
                return true;
            else
                return false;
        }


    }

    /// <summary>
    /// 翻转站
    /// </summary>
    public class Rolled_Station : SingleInstance<Rolled_Station>
    {
        private System.Threading.Timer TimerAxisStatus;//轴状态扫描

        /// <summary>
        /// 翻转-X轴
        /// </summary>
        public Axis Axis_X = new Axis { Axis_Index = 5, PulseToMM = 0.001 };

        /// <summary>
        /// 翻转-Y轴
        /// </summary>
        public Axis Axis_Z = new Axis { Axis_Index = 6, PulseToMM = 0.001 };

        /// <summary>
        /// 翻转-R轴
        /// </summary>
        public Axis Axis_R = new Axis { Axis_Index = 7, PulseToMM = 0.001 };
    
        public void Ini()
        {
            TimerAxisStatus = new System.Threading.Timer(new System.Threading.TimerCallback(AxisStatusScan), null, 0, 500);
        }
        private void AxisStatusScan(object value)
        {
            bool _IsAlarm, _IsLimitPositive, _IsLimitNegative;
            double Pos;
            //-Axis_X状态扫描
            frm_Main.mCard1.CheckAxisStatus(Axis_X.Axis_Index, out _IsAlarm, out _IsLimitPositive, out _IsLimitNegative);
            frm_Main.mCard1.GetPrfPos(Axis_X.Axis_Index, Axis_X.PulseToMM, out Pos);
            Axis_X.IsAlarm = _IsAlarm;
            Axis_X.IsLimitPositive = _IsLimitPositive;
            Axis_X.IsLimitNegative = _IsLimitNegative;
            Axis_X.Pos = Pos;

            //-Axis_Y状态扫描
            frm_Main.mCard1.CheckAxisStatus(Axis_Z.Axis_Index, out _IsAlarm, out _IsLimitPositive, out _IsLimitNegative);
            frm_Main.mCard1.GetPrfPos(Axis_Z.Axis_Index, Axis_Z.PulseToMM, out Pos);
            Axis_Z.IsAlarm = _IsAlarm;
            Axis_Z.IsLimitPositive = _IsLimitPositive;
            Axis_Z.IsLimitNegative = _IsLimitNegative;
            Axis_Z.Pos = Pos;

            //-Axis_R状态扫描
            frm_Main.mCard1.CheckAxisStatus(Axis_R.Axis_Index, out _IsAlarm, out _IsLimitPositive, out _IsLimitNegative);
            frm_Main.mCard1.GetPrfPos(Axis_R.Axis_Index, Axis_R.PulseToMM, out Pos);
            Axis_R.IsAlarm = _IsAlarm;
            Axis_R.IsLimitPositive = _IsLimitPositive;
            Axis_R.IsLimitNegative = _IsLimitNegative;
            Axis_R.Pos = Pos;

        }

        public bool Home()
        {
            bool isHomeFinishedX = false, isHomeFinishedY = false, isHomeFinishedR = false;
            frm_Main.mCard1.Home(Axis_Z.Axis_Index, HomeMoveDirection.Negative, HomeMode.LimitHome, Axis_Z.PulseToMM, 50, 10, 0);
            frm_Main.mCard1.IsHomeFinished(Axis_Z.Axis_Index, out isHomeFinishedY);

            double posZ = Recipe.Instance._Recipe_Para.DT_RightRolled.ToList()[0].ZPos;
            bool bMotionDone;
            frm_Main.mCard1.AbsoluteMove(Axis_Z.Axis_Index, Axis_Z.PulseToMM, 1, 50, posZ);
            frm_Main.mCard1.MotionDone(Axis_Z.Axis_Index, out bMotionDone);

            frm_Main.mCard1.Home(Axis_X.Axis_Index, HomeMoveDirection.Negative, HomeMode.LimitHome, Axis_X.PulseToMM, 50, 10, 0);          
            frm_Main.mCard1.Home(Axis_R.Axis_Index, HomeMoveDirection.Negative, HomeMode.Home, Axis_R.PulseToMM, 50, 10, 0);

            frm_Main.mCard1.IsHomeFinished(Axis_X.Axis_Index, out isHomeFinishedX);           
            frm_Main.mCard1.IsHomeFinished(Axis_R.Axis_Index, out isHomeFinishedR);
      

            return isHomeFinishedX && isHomeFinishedY && isHomeFinishedR;
        }
        public bool Home(Axis mAxis)
        {
            bool isHomeFinished = false;
            frm_Main.mCard1.Home(mAxis.Axis_Index, HomeMoveDirection.Negative, HomeMode.LimitHome, mAxis.PulseToMM, 100, 10, 0);
            frm_Main.mCard1.IsHomeFinished(mAxis.Axis_Index, out isHomeFinished);

            return isHomeFinished;
        }

        public bool HomeR(Axis mAxis)
        {
            bool isHomeFinished = false;
            frm_Main.mCard1.Home(mAxis.Axis_Index, HomeMoveDirection.Negative, HomeMode.Home, mAxis.PulseToMM, 100, 10, 0);
            frm_Main.mCard1.IsHomeFinished(mAxis.Axis_Index, out isHomeFinished);

            return isHomeFinished;
        }

        public void JogStart(Axis mAxis, double Speed, bool isPositive)
        {
            frm_Main.mCard1.Jog(mAxis.Axis_Index, mAxis.PulseToMM, Speed, isPositive);
        }

        public void JogStop(Axis mAxis)
        {
            frm_Main.mCard1.AxisStop(mAxis.Axis_Index);
        }

        public void MoveTo(Axis mAxis, double Speed, double Pos)
        {
            bool bMotionDone = false;
            frm_Main.mCard1.AbsoluteMove(mAxis.Axis_Index, mAxis.PulseToMM, 1, Speed, Pos);
            frm_Main.mCard1.MotionDone(mAxis.Axis_Index, out bMotionDone);
        }

        public bool LeftStationMoveTo(int index)
        {
            bool bMotionDone = false;
            double speed =  Recipe.Instance._Recipe_Para.DT_LeftRolled.ToList()[index].Vel;
            double acc =      Recipe.Instance._Recipe_Para.DT_LeftRolled.ToList()[index].Acc;
            double posX =   Recipe.Instance._Recipe_Para.DT_LeftRolled.ToList()[index].XPos;
            double posZ =   Recipe.Instance._Recipe_Para.DT_LeftRolled.ToList()[index].ZPos;
            double posR =   Recipe.Instance._Recipe_Para.DT_LeftRolled.ToList()[index].RPos;

            frm_Main.mCard1.AbsoluteMove(Axis_X.Axis_Index, Axis_X.PulseToMM, acc, speed, posX);
            frm_Main.mCard1.AbsoluteMove(Axis_Z.Axis_Index, Axis_Z.PulseToMM, acc, speed, posZ);
            frm_Main.mCard1.AbsoluteMove(Axis_R.Axis_Index, Axis_R.PulseToMM, acc, speed, posR);

            frm_Main.mCard1.MotionDone(Axis_X.Axis_Index, out bMotionDone);
            frm_Main.mCard1.MotionDone(Axis_Z.Axis_Index, out bMotionDone);
            frm_Main.mCard1.MotionDone(Axis_R.Axis_Index, out bMotionDone);
            return Axis_X.IsAlarm || Axis_Z.IsAlarm || Axis_R.IsAlarm;
        }

        public bool RightStationMoveTo(int index)
        {
            bool bMotionDone = false;
            double speed = Recipe.Instance._Recipe_Para.DT_RightRolled.ToList()[index].Vel;
            double acc = Recipe.Instance._Recipe_Para.DT_RightRolled.ToList()[index].Acc;
            double posX = Recipe.Instance._Recipe_Para.DT_RightRolled.ToList()[index].XPos;
            double posZ = Recipe.Instance._Recipe_Para.DT_RightRolled.ToList()[index].ZPos;
            double posR = Recipe.Instance._Recipe_Para.DT_RightRolled.ToList()[index].RPos;

            frm_Main.mCard1.AbsoluteMove(Axis_X.Axis_Index, Axis_X.PulseToMM, acc, speed, posX);
            frm_Main.mCard1.AbsoluteMove(Axis_Z.Axis_Index, Axis_Z.PulseToMM, acc, speed, posZ);
            frm_Main.mCard1.AbsoluteMove(Axis_R.Axis_Index, Axis_R.PulseToMM, acc, speed, posR);

            frm_Main.mCard1.MotionDone(Axis_X.Axis_Index, out bMotionDone);
            frm_Main.mCard1.MotionDone(Axis_Z.Axis_Index, out bMotionDone);
            frm_Main.mCard1.MotionDone(Axis_R.Axis_Index, out bMotionDone);

            return Axis_X.IsAlarm || Axis_Z.IsAlarm || Axis_R.IsAlarm;
        }

        public bool Vacumm( bool isON )
        {
            bool result = false;
            if(isON)
            {
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 43-1, true);//翻转吸真空
            Air:
                DateTime now = DateTime.Now;
                while (true)
                {
                    Application.DoEvents();
                    Thread.Sleep(20);
                    if (MotionControl_GTS.Input(frm_Main.InputBitInfoS, 43-1) == 1)
                    {
                        result = true;
                        break;
                    }
                    if ((DateTime.Now - now).Seconds > frm_Main.MyPara.mComConfig._真空吸延时)
                    {
                        TaskManage.GetSingleton().RaiseDisplayLog("翻转站-吸真空超时", Color.Red);
                        DialogResult res = MessageBox.Show("翻转站-吸真空超时", "超时错误...", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);

                        if (res == DialogResult.Abort)
                        {
                            Task.Factory.StartNew(() => TaskManage.GetSingleton().StopWork());
                            break;
                        }
                        else if (res == DialogResult.Retry)
                        {
                            goto Air;
                        }
                        else if (res == DialogResult.Ignore)
                        {
                            break;
                        }
                    }
                }
                return result;
            }
            else
            {
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 43-1, false);//翻转吸真空
                BreakVacumm(true);
                System.Threading.Thread.Sleep(200);
                BreakVacumm(false);
                return true;
            }
          
        }
        public bool IsVacummOK()
        {
          int res =   MotionControl_GTS.Input(frm_Main.InputBitInfoS, 43-1);//翻转吸真空检测
            if (res == 1)
                return true;
            else
                return false;
        }

        private void BreakVacumm(bool isOn)
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 44 - 1, isOn);//翻转破真空
        }

    }

    public class LoadRobot : SingleInstance<LoadRobot>
    {
        /// <summary>
        /// 上料机器人运行
        /// </summary>
        /// <returns></returns>
        public bool GetRobotIsRun()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 49-1) == 1;
        }

        public bool GetRobotIsStop()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 50 - 1) == 1;
        }

        public bool GetRobotIsPause()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 51 - 1) == 1;
        }

        /// <summary>
        /// 上料机器人报警
        /// </summary>
        /// <returns></returns>
        public bool GetRobotIsAlarm()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 52-1) == 1;
        }

        /// <summary>
        /// 上料机器人准备
        /// </summary>
        /// <returns></returns>
        public bool GetRobotIsReady() 
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 53-1) == 1;
        }

        /// <summary>
        /// 上料机器人初始化是否OK
        /// </summary>
        /// <returns></returns>
        public bool GetRobotIsIniOK()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 54-1) == 1;
        }

        /// <summary>
        /// 上料载台1放料到位
        /// </summary>
        /// <returns></returns>
        public bool GetFixture1PutPosOk()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 57-1) == 1;
        }

        /// <summary>
        /// 上料载台1放料完成
        /// </summary>
        /// <returns></returns>
        public bool GetFixture1PutDone()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 58-1) == 1;
        }

        /// <summary>
        /// 上料载台2放料到位
        /// </summary>
        /// <returns></returns>
        public bool GetFixture2PutPosOk()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 59-1) == 1;
        }

        /// <summary>
        /// 上料载台2放料完成
        /// </summary>
        /// <returns></returns>
        public bool GetFixture2PutDone()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 60-1) == 1;
        }

        /// <summary>
        /// 上料机器人开始
        /// </summary>
        public void SetRobotStart(bool isOn) 
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 49-1, isOn);
        }

        /// <summary>
        /// 上料机器人暂停
        /// </summary>
        public void SetRobotPause(bool isOn)
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 50-1, isOn);
        }

        /// <summary>
        /// 上料机器人停止
        /// </summary>
        public void SetRobotStop(bool isOn)
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 51-1, isOn);
        }

        /// <summary>
        /// 上料机器人复位
        /// </summary>
        public void SetRobotReset(bool isOn)
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 52-1, isOn);
        }

        /// <summary>
        /// 上料载台1可放料
        /// </summary>
        public void SetLoadFixture1CanPut(bool isOn)
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 57-1, isOn);
        }

        /// <summary>
        /// 上料载台1放料OK 
        /// </summary>
        public void SetLoadFixture1PutOK(bool isOn)
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 58-1, isOn);
        }

        /// <summary>
        /// 上料载台2可放料
        /// </summary>
        public void SetLoadFixture2CanPut(bool isOn)
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 59-1, isOn);
        }

        /// <summary>
        /// 上料载台2放料OK 
        /// </summary>
        public void SetLoadFixture2PutOK(bool isOn)
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 60-1, isOn);
        }

    }

    public class UpLoadRobot : SingleInstance<UpLoadRobot>
    {
        /// <summary>
        /// 下料机器人是否运行
        /// </summary>
        /// <returns></returns>
        public bool GetRobotIsRun()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 65-1) == 1;
        }

        public bool GetRobotIsStop()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 66 - 1) == 1;
        }

        public bool GetRobotIsPause()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 67 - 1) == 1;
        }

        /// <summary>
        /// 下料机器人是否报警
        /// </summary>
        /// <returns></returns>
        public bool GetRobotIsAlarm()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 68-1) == 1;
        }

        /// <summary>
        /// 下料机器人是否准备
        /// </summary>
        /// <returns></returns>
        public bool GetRobotIsReady()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 69-1) == 1;
        }

        /// <summary>
        /// 下料机器人是否初始化OK
        /// </summary>
        /// <returns></returns>
        public bool GetRobotIsIniOK()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 70-1) == 1;
        }

        /// <summary>
        /// 下料载台1取料到位
        /// </summary>
        /// <returns></returns>
        public bool GetFixture1PickPosOK()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 73-1) == 1;
        }

        /// <summary>
        /// 下料载台1取料完成
        /// </summary>
        /// <returns></returns>
        public bool GetFixture1PickDone()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 74-1) == 1;
        }

        /// <summary>
        /// 下料载台2取料到位
        /// </summary>
        /// <returns></returns>
        public bool GetFixture2PickPosOK()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 77-1) == 1;
        }

        /// <summary>
        /// 下料载台2取料完成
        /// </summary>
        /// <returns></returns>
        public bool GetFixture2PickDone()
        {
            return MotionControl_GTS.Input(frm_Main.InputBitInfoS, 78-1) == 1;
        }

        /// <summary>
        /// 下料机器人开始
        /// </summary>
        public void SetRobotStart(bool isON)
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 65-1, isON);
        }

        /// <summary>
        /// 下料机器人暂停
        /// </summary>
        public void SetRobotPause(bool isON)
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 66-1, isON);
        }

        /// <summary>
        /// 下料机器人停止
        /// </summary>
        public void SetRobotStop(bool isON)
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 67-1, isON);
        }

        /// <summary>
        /// 下料机器人复位
        /// </summary>
        public void SetRobotReset(bool isON)
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 68-1, isON);
        }

        /// <summary>
        /// 设置治具1结果-右工位
        /// </summary>
        /// <param name="isOK"></param>
        public void SetFixture1Result(bool isOK)
        {
            if(isOK)
            {
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 73-1, true);
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 74-1, false);

            }
            else
            {
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 73 - 1, false);
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 74 - 1, true);

            }
        }

        /// <summary>
        /// 治具1可以取料
        /// </summary>
        public void SetFxiture1CanPick(bool isON)
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 75-1, isON);

        }

        /// <summary>
        /// 治具1可以取料可以取走
        /// </summary>
        public void SetFxiture1PickCanGo(bool isON)
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 76-1, isON);
        }

        /// <summary>
        /// 设置治具2结果
        /// </summary>
        /// <param name="isOK"></param>
        public void SetFixture2Result(bool isOK)
        {
            if (isOK)
            {
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 77-1, true);
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 78-1, false);
            }
            else
            {
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 77-1, false);
                MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 78-1, true);

            }
        }

        /// <summary>
        /// 治具2可以取料
        /// </summary>
        public void SetFxiture2CanPick(bool isON)
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 79-1, isON);
        }

        /// <summary>
        /// 治具2可以取料可以取走
        /// </summary>
        public void SetFxiture2PickCanGo(bool isON)
        {
            MotionControl_GTS.Output(frm_Main.OutputBitInfoS, 80-1, isON);

        }

    }



    /// <summary>
    /// 轴机构元素：轴号，脉冲转毫米单位，位置，报警，正极限，负极限
    /// </summary>
    public struct Axis
    {
        public short Axis_Index { get; set; }
        public double PulseToMM { get; set; }
        public double Pos { get; set; }

        public bool IsAlarm { get; set; }

        public bool IsLimitNegative { get; set; }

        public bool IsLimitPositive { get; set; }

    }
}
