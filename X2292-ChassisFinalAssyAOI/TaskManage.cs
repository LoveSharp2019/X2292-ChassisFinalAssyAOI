using HalconDotNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace X2292_ChassisFinalAssyAOI
{
    public enum EN_RunRet
    {
        IOOK,
        IOErr,
        MotionOk,
        MotionErr,
        TaskOk,
        TaskErr,
    }
    public enum EN_MachineStatus
    {
        IsReseting,
        Reseted,
        Run,
        Stop,
        Err
    }
    public enum EN_RunStep
    {
        Idle,
        Err,
        Ini,
        LoadPos,
        StartWork,
        LoadRobotWork,
        CylinderWorkOn,
        Barcode,
        LaserPos,
        UpLoadPos,
        CylinderWorkOff,
        UpLoadRobotWork
    }
    public   class TaskManage
    {
        // private static ManualResetEvent manualResetEvent = new ManualResetEvent(false);//AutoResetEvent略去
        //  private static CancellationTokenSource cancellationToken = new CancellationTokenSource();//cancellationToken.Cancel();
       // private static ConcurrentBag<int> list = new ConcurrentBag<int>();
        private EN_RunStep RunLeftStep = EN_RunStep.Ini;
        private EN_RunStep RunRightStep = EN_RunStep.Ini;
        private EN_MachineStatus MachineStatus = EN_MachineStatus.Stop;
        private Thread m_pRunLeftThread = null;
        private Thread m_pRunRightThread = null;
        private Thread m_3DProcess = null;
        public bool LeftStart = false, RightStart = false;
        private static TaskManage m_pThis = null;
        public bool AutoRun { get; set; } = false;//自动运行标记
        public volatile bool isLaserWorking = false;
        public volatile bool isRolledWorking  = false;

        public event Action<MeasureItems> DatasEvent;

        public event Action<String,Color> DisplayLog;

        public bool BtnStart = false;

        public ConcurrentQueue<Infor3D> CQueue_3DImage = new ConcurrentQueue<Infor3D>();

        public static TaskManage GetSingleton()
        {
            if (m_pThis == null)
                m_pThis = new TaskManage();
            return m_pThis;
        }

        public void InitModels()
        {
            frm_Main.TrigIsStart += Frm_Main_TrigIsStart; ;
            frm_Main.TrigIsStop += Frm_Main_TrigIsStop;
            frm_Main.TrigIsReset += Frm_Main_TrigIsReset;
            frm_Main.TrigSafeDoor += Frm_Main_TrigSafeDoor;
            frm_Main.TrigIsEStop += Frm_Main_TrigIsEStop;
        }

        private void Frm_Main_TrigIsEStop()
        {
           
        }

        private void Frm_Main_TrigSafeDoor()
        {
            
        }

        private void Frm_Main_TrigIsReset()
        {
           // ResetProcessAll();
        }

        private void Frm_Main_TrigIsStop()
        {
          //  StopWork();
        }

        private void Frm_Main_TrigIsStart()
        {
           // StartWork();
        }

        public void StartWork()
        {
            m_pRunLeftThread = new Thread(MasterLeftRun);
            m_pRunLeftThread.Priority = ThreadPriority.AboveNormal;
            m_pRunLeftThread.Start();

            m_pRunRightThread = new Thread(MasterRightRun);
            m_pRunRightThread.Priority = ThreadPriority.AboveNormal;
            m_pRunRightThread.Start();

            m_3DProcess = new Thread(LMIProcess);
            m_3DProcess.Priority = ThreadPriority.AboveNormal;
            m_3DProcess.Start();

        }
        public void StopWork()
        {
            m_pRunLeftThread?.Abort();
            m_pRunLeftThread = null;

            m_pRunRightThread?.Abort();
            m_pRunRightThread = null;

            m_3DProcess?.Abort();
            m_3DProcess = null;
        }
        public void RaiseDataResultShow(MeasureItems mItems)
        {
            DatasEvent?.BeginInvoke(mItems, null, null);
        }
        public void RaiseDisplayLog(string strContent,Color color)
        {
            DisplayLog?.BeginInvoke(strContent, color, null, null);
        }
        /// <summary>
        /// 左工位流程
        /// </summary>
        private void MasterLeftRun()
        {           
            while (true)
            {
                System.Threading.Thread.Sleep(50);
                switch (RunLeftStep)
                {
                    case EN_RunStep.Err:
                        break;
                    case EN_RunStep.Idle:
                        {
                          
                            RunLeftStep = EN_RunStep.Ini;                      
                        }
                        break;
                    case EN_RunStep.Ini:
                        {
                            //程序生命周期只运行一次
                            RunLeftStep = EN_RunStep.StartWork;
                        }
                      break;
                    case EN_RunStep.StartWork:
                        {
                            
                           
                            Laser_Station.Instance.LeftStationMoveTo(1, false, true, false);//上料位置
                            Laser_Station.Instance.LeftStatonFixtureLFCylinder(false);
                            Laser_Station.Instance.LeftStatonFixtureRBCylinder(false);
                            Laser_Station.Instance.LeftStationVacummOn(false);

                            RunLeftStep = EN_RunStep.LoadPos;
                        }
                        break;
                    case EN_RunStep.LoadPos:
                        {
                           
                            if (frm_Main.MyPara.mParaSetting.Disable_LoadRobot)
                            {
                                if(LeftStart)
                                {
                                    RunLeftStep = EN_RunStep.CylinderWorkOn;
                                    LeftStart = false;
                                }
                            }
                            else
                            {
                                RunLeftStep = EN_RunStep.LoadRobotWork;
                            }
                        }
                        break;
                    case EN_RunStep.LoadRobotWork:
                        {
                            LoadRobot.Instance.SetLoadFixture2CanPut(true);
                            while(true)
                            {
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(200);
                                if (LoadRobot.Instance.GetFixture2PutPosOk())
                                    break;
                            }
                            LoadRobot.Instance.SetLoadFixture2CanPut(false);
                            // Laser_Station.Instance.LeftStationVacummOn(true);
                            System.Threading.Thread.Sleep(500);

                            LoadRobot.Instance.SetLoadFixture2PutOK(true);
                            while (true)
                            {
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(200);
                                if (LoadRobot.Instance.GetFixture2PutDone())
                                    break;
                            }
                            LoadRobot.Instance.SetLoadFixture2PutOK(false);
                            RunLeftStep = EN_RunStep.CylinderWorkOn;
                        }
                        break;
                    case EN_RunStep.CylinderWorkOn:
                        {
                            Laser_Station.Instance.LeftStatonFixtureRBCylinder(true);                         
                            System.Threading.Thread.Sleep(2000);
                            Laser_Station.Instance.LeftStatonFixtureLFCylinder(true);
                            Laser_Station.Instance.LeftStationVacummOn(true);
                            RunLeftStep = EN_RunStep.Barcode;
                        }
                        break;
                    case EN_RunStep.Barcode:
                        {
                            Laser_Station.Instance.LeftStationMoveTo(4, false, true, false);//左平台读码位置
                            RunLeftStep = EN_RunStep.LaserPos;
                        }
                        break;
                    case EN_RunStep.LaserPos:
                        {
                            if(!isLaserWorking)
                            {
                                isLaserWorking = true;

                               
                                Laser_Station.Instance.LeftStationMoveTo(4, false, false, true);//读码位置
                                frm_Main.CodeReaderClient.SendData(System.Text.Encoding.UTF8.GetBytes("LON" + "\r\n"));

                                DateTime now = DateTime.Now;
                                while(string.IsNullOrEmpty (frm_Main.SN))
                                {
                                    Application.DoEvents();
                                    System.Threading.Thread.Sleep(50);
                                    if((DateTime.Now - now).Seconds >2)
                                    {
                                        MessageBox.Show("读码超时");
                                        frm_Main.MyPara.mFAIstation.SN = DateTime.Now.ToString("DDHHmmssffff");
                                        break;
                                    }
                                }

                                frm_Main.MyPara.mFAIstation.SN = frm_Main.SN;
                                frm_Main.SN = string.Empty;

                                Laser_Station.Instance.LeftStationMoveTo(6, false, false, true);//激光工作开始位置
                                LMIControl.Instance.StartLMI();//激光开始扫描
                                Laser_Station.Instance.LeftStationMoveTo(7, false, false, true);//激光工作结束位置
                                HalconDotNet.HObject obj = LMIControl.Instance.StopLMI();//激光收集图片
                                HObject objRate;
                                LMIControl.Instance.TransImage(obj, out objRate, 1);
                                CQueue_3DImage.Enqueue(new Infor3D() { Object = objRate, IndexImg = "1", StationName = "L" });//添加到图片队列给算法处理


                                Laser_Station.Instance.LeftStationMoveTo(8, false, false, true);//激光工作开始位置
                                LMIControl.Instance.StartLMI();//激光开始扫描
                                Laser_Station.Instance.LeftStationMoveTo(9, false, false, true);//激光工作结束位置
                                obj = LMIControl.Instance.StopLMI();//激光收集图片                             
                                LMIControl.Instance.TransImage(obj, out objRate, 2);
                                CQueue_3DImage.Enqueue(new Infor3D() { Object = objRate, IndexImg = "2", StationName = "L" });//添加到图片队列给算法处理

                                Laser_Station.Instance.LeftStationMoveTo(10, false, false, true);//激光工作开始位置
                                LMIControl.Instance.StartLMI();//激光开始扫描
                                Laser_Station.Instance.LeftStationMoveTo(11, false, false, true);//激光工作结束位置
                                obj = LMIControl.Instance.StopLMI();//激光收集图片                             
                                LMIControl.Instance.TransImage(obj, out objRate, 3);
                                CQueue_3DImage.Enqueue(new Infor3D() { Object = objRate, IndexImg = "3", StationName = "L" });//添加到图片队列给算法处理

                                Laser_Station.Instance.LeftStationMoveTo(12, false, false, true);//激光工作开始位置
                                LMIControl.Instance.StartLMI();//激光开始扫描
                                Laser_Station.Instance.LeftStationMoveTo(13, false, false, true);//激光工作结束位置
                                obj = LMIControl.Instance.StopLMI();//激光收集图片                             
                                LMIControl.Instance.TransImage(obj, out objRate, 4);
                                CQueue_3DImage.Enqueue(new Infor3D() { Object = objRate, IndexImg = "4", StationName = "L" });//添加到图片队列给算法处理

                                Laser_Station.Instance.LeftStationMoveTo(14, false, false, true);//激光工作开始位置
                                LMIControl.Instance.StartLMI();//激光开始扫描
                                Laser_Station.Instance.LeftStationMoveTo(15, false, false, true);//激光工作结束位置
                                obj = LMIControl.Instance.StopLMI();//激光收集图片                             
                                LMIControl.Instance.TransImage(obj, out objRate, 5);
                                CQueue_3DImage.Enqueue(new Infor3D() { Object = objRate, IndexImg = "5", StationName = "L" });//添加到图片队列给算法处理

                                Laser_Station.Instance.LeftStationMoveTo(16, false, false, true);//激光工作开始位置
                                LMIControl.Instance.StartLMI();//激光开始扫描
                                Laser_Station.Instance.LeftStationMoveTo(17, false, false, true);//激光工作结束位置
                                obj = LMIControl.Instance.StopLMI();//激光收集图片                             
                                LMIControl.Instance.TransImage(obj, out objRate, 6);
                                CQueue_3DImage.Enqueue(new Infor3D() { Object = objRate, IndexImg = "6", StationName = "L" });//添加到图片队列给算法处理

                                Laser_Station.Instance.LeftStationMoveTo(18, false, false, true);//激光工作开始位置
                                LMIControl.Instance.StartLMI();//激光开始扫描
                                Laser_Station.Instance.LeftStationMoveTo(19, false, false, true);//激光工作结束位置
                                obj = LMIControl.Instance.StopLMI();//激光收集图片                             
                                LMIControl.Instance.TransImage(obj, out objRate, 7);
                                CQueue_3DImage.Enqueue(new Infor3D() { Object = objRate, IndexImg = "7", StationName = "L" });//添加到图片队列给算法处理

                                Laser_Station.Instance.LeftStationMoveTo(5, false, false, true);//激光安全位置

                                isLaserWorking = false;
                                if (frm_Main.MyPara.mParaSetting.Disable_UpLoadRobot || frm_Main.MyPara.mParaSetting.GRR_Run)
                                {
                                    RunLeftStep = EN_RunStep.Ini;
                                }
                                else
                                {
                                    RunLeftStep = EN_RunStep.UpLoadPos;
                                   
                                }
                                   
                            }
                        }
                        break;
                    case EN_RunStep.UpLoadPos:
                        {
                            Laser_Station.Instance.LeftStationMoveTo(2, false, true, false);//左平台移到翻转位置   
                                
                            if (!isRolledWorking)
                            {
                                isRolledWorking = true;
                                                   
                                Rolled_Station.Instance.LeftStationMoveTo(1);//翻转机构运动到取料等待位置
                                Rolled_Station.Instance.LeftStationMoveTo(2);//翻转机构运动到取料位置
                                Laser_Station.Instance.LeftStationVacummOn(false);//平台真空关
                                System.Threading.Thread.Sleep(200);
                                Rolled_Station.Instance.Vacumm(true);//翻转机构吸真空
                               
                                Laser_Station.Instance.LeftStatonFixtureLFCylinder(false);
                                Laser_Station.Instance.LeftStatonFixtureRBCylinder(false);
                               
                                Rolled_Station.Instance.LeftStationMoveTo(3);//翻转机构运动到取料等待位置
                                Rolled_Station.Instance.LeftStationMoveTo(4);//翻转机构运动到翻转等待位置
                                Rolled_Station.Instance.LeftStationMoveTo(5);//翻转机构运动到翻转180位置

                              
                                RunLeftStep = EN_RunStep.UpLoadRobotWork;
                               
                            }
                          
                        }
                        break;
                    case EN_RunStep.UpLoadRobotWork:
                        {
                            UpLoadRobot.Instance.SetFixture2Result(true);
                            UpLoadRobot.Instance.SetFxiture2CanPick(true);
                            while(true)
                            {
                                System.Threading.Thread.Sleep(50);
                                Application.DoEvents();
                                if(UpLoadRobot.Instance.GetFixture2PickPosOK())
                                {
                                    break;
                                }
                            }
                            UpLoadRobot.Instance.SetFxiture2CanPick(false);
                            Rolled_Station.Instance.Vacumm(false);

                            UpLoadRobot.Instance.SetFxiture2PickCanGo(true);
                            while (true)
                            {
                                System.Threading.Thread.Sleep(50);
                                Application.DoEvents();
                                if (UpLoadRobot.Instance.GetFixture2PickDone())
                                {
                                    break;
                                }
                            }
                            UpLoadRobot.Instance.SetFxiture2PickCanGo(false);

                            Rolled_Station.Instance.LeftStationMoveTo(0);//翻转机构运动到待机位置
                            isRolledWorking = false;

                            RunLeftStep = EN_RunStep.StartWork;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 右工位流程
        /// </summary>
        private void MasterRightRun()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(50);
                switch (RunRightStep)
                {
                    case EN_RunStep.Err:
                        break;
                    case EN_RunStep.Idle:
                        {

                            RunRightStep = EN_RunStep.Ini;
                        }
                        break;
                    case EN_RunStep.Ini:
                        {
                            //程序生命周期只运行一次
                            RunRightStep = EN_RunStep.StartWork;
                        }
                        break;
                    case EN_RunStep.StartWork:
                        {


                            Laser_Station.Instance.RightStationMoveTo(1, false, true, false);//上料位置
                            Laser_Station.Instance.RightStatonFixtureLFCylinder(false);
                            Laser_Station.Instance.RightStatonFixtureRBCylinder(false);
                            Laser_Station.Instance.RightStationVacummOn(false);

                            RunRightStep = EN_RunStep.LoadPos;
                        }
                        break;
                    case EN_RunStep.LoadPos:
                        {

                            if (frm_Main.MyPara.mParaSetting.Disable_LoadRobot)
                            {
                                if (RightStart)
                                {
                                    RunRightStep = EN_RunStep.CylinderWorkOn;
                                    RightStart = false;
                                }
                            }
                            else
                            {
                                RunRightStep = EN_RunStep.LoadRobotWork;
                            }
                        }
                        break;
                    case EN_RunStep.LoadRobotWork:
                        {
                            LoadRobot.Instance.SetLoadFixture1CanPut(true);
                            while (true)
                            {
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(200);
                                if (LoadRobot.Instance.GetFixture1PutPosOk())
                                    break;
                            }
                            LoadRobot.Instance.SetLoadFixture1CanPut(false);
                            // Laser_Station.Instance.LeftStationVacummOn(true);
                            System.Threading.Thread.Sleep(500);

                            LoadRobot.Instance.SetLoadFixture1PutOK(true);
                            while (true)
                            {
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(200);
                                if (LoadRobot.Instance.GetFixture1PutDone())
                                    break;
                            }
                            LoadRobot.Instance.SetLoadFixture1PutOK(false);
                            RunRightStep = EN_RunStep.CylinderWorkOn;
                        }
                        break;
                    case EN_RunStep.CylinderWorkOn:
                        {
                            Laser_Station.Instance.RightStatonFixtureRBCylinder(true);
                            System.Threading.Thread.Sleep(2000);
                            Laser_Station.Instance.RightStatonFixtureLFCylinder(true);
                            Laser_Station.Instance.RightStationVacummOn(true);
                            RunRightStep = EN_RunStep.Barcode;
                        }
                        break;
                    case EN_RunStep.Barcode:
                        {
                            Laser_Station.Instance.RightStationMoveTo(4, false, true, false);//右平台到读码位置
                            RunRightStep = EN_RunStep.LaserPos;
                        }
                        break;
                    case EN_RunStep.LaserPos:
                        {
                            if (!isLaserWorking)
                            {
                                isLaserWorking = true;

                              
                                Laser_Station.Instance.RightStationMoveTo(4, false, false, true);//读码位置

                                Laser_Station.Instance.RightStationMoveTo(6, false, false, true);//激光工作开始位置
                                LMIControl.Instance.StartLMI();//激光开始扫描
                                Laser_Station.Instance.RightStationMoveTo(7, false, false, true);//激光工作结束位置
                                HalconDotNet.HObject obj = LMIControl.Instance.StopLMI();//激光收集图片
                                HObject objRate;
                                LMIControl.Instance.TransImage(obj, out objRate, 1);
                                CQueue_3DImage.Enqueue(new Infor3D() { Object = objRate, IndexImg = "1", StationName ="R" });//添加到图片队列给算法处理


                                Laser_Station.Instance.RightStationMoveTo(8, false, false, true);//激光工作开始位置
                                LMIControl.Instance.StartLMI();//激光开始扫描
                                Laser_Station.Instance.RightStationMoveTo(9, false, false, true);//激光工作结束位置
                                obj = LMIControl.Instance.StopLMI();//激光收集图片                             
                                LMIControl.Instance.TransImage(obj, out objRate, 2);
                                CQueue_3DImage.Enqueue(new Infor3D() { Object = objRate, IndexImg = "2", StationName = "R" });//添加到图片队列给算法处理

                                Laser_Station.Instance.RightStationMoveTo(10, false, false, true);//激光工作开始位置
                                LMIControl.Instance.StartLMI();//激光开始扫描
                                Laser_Station.Instance.RightStationMoveTo(11, false, false, true);//激光工作结束位置
                                obj = LMIControl.Instance.StopLMI();//激光收集图片                             
                                LMIControl.Instance.TransImage(obj, out objRate, 3);
                                CQueue_3DImage.Enqueue(new Infor3D() { Object = objRate, IndexImg = "3", StationName = "R" });//添加到图片队列给算法处理

                                Laser_Station.Instance.RightStationMoveTo(12, false, false, true);//激光工作开始位置
                                LMIControl.Instance.StartLMI();//激光开始扫描
                                Laser_Station.Instance.RightStationMoveTo(13, false, false, true);//激光工作结束位置
                                obj = LMIControl.Instance.StopLMI();//激光收集图片                             
                                LMIControl.Instance.TransImage(obj, out objRate, 4);
                                CQueue_3DImage.Enqueue(new Infor3D() { Object = objRate, IndexImg = "4", StationName = "R" });//添加到图片队列给算法处理

                                Laser_Station.Instance.RightStationMoveTo(14, false, false, true);//激光工作开始位置
                                LMIControl.Instance.StartLMI();//激光开始扫描
                                Laser_Station.Instance.RightStationMoveTo(15, false, false, true);//激光工作结束位置
                                obj = LMIControl.Instance.StopLMI();//激光收集图片                             
                                LMIControl.Instance.TransImage(obj, out objRate, 5);
                                CQueue_3DImage.Enqueue(new Infor3D() { Object = objRate, IndexImg = "5", StationName = "R" });//添加到图片队列给算法处理

                                Laser_Station.Instance.RightStationMoveTo(16, false, false, true);//激光工作开始位置
                                LMIControl.Instance.StartLMI();//激光开始扫描
                                Laser_Station.Instance.RightStationMoveTo(17, false, false, true);//激光工作结束位置
                                obj = LMIControl.Instance.StopLMI();//激光收集图片                             
                                LMIControl.Instance.TransImage(obj, out objRate, 6);
                                CQueue_3DImage.Enqueue(new Infor3D() { Object = objRate, IndexImg = "6", StationName = "R" });//添加到图片队列给算法处理

                                Laser_Station.Instance.RightStationMoveTo(18, false, false, true);//激光工作开始位置
                                LMIControl.Instance.StartLMI();//激光开始扫描
                                Laser_Station.Instance.RightStationMoveTo(19, false, false, true);//激光工作结束位置
                                obj = LMIControl.Instance.StopLMI();//激光收集图片                             
                                LMIControl.Instance.TransImage(obj, out objRate, 7);
                                CQueue_3DImage.Enqueue(new Infor3D() { Object = objRate, IndexImg = "7", StationName = "R" });//添加到图片队列给算法处理

                                Laser_Station.Instance.RightStationMoveTo(5, false, false, true);//激光安全位置

                                isLaserWorking = false;
                                if (frm_Main.MyPara.mParaSetting.Disable_UpLoadRobot || frm_Main.MyPara.mParaSetting.GRR_Run)
                                {
                                    RunRightStep = EN_RunStep.Ini;
                                }
                                else
                                {                                  
                                    RunRightStep = EN_RunStep.UpLoadPos;
                                }

                            }
                        }
                        break;
                    case EN_RunStep.UpLoadPos:
                        {
                            Laser_Station.Instance.RightStationMoveTo(2, false, true, false);//右平台移动到翻转位置     
                            if (!isRolledWorking)
                            {
                                isRolledWorking = true;
                                                     
                                Rolled_Station.Instance.RightStationMoveTo(1);//翻转机构运动到取料等待位置
                                Rolled_Station.Instance.RightStationMoveTo(2);//翻转机构运动到取料位置
                                Laser_Station.Instance.RightStationVacummOn(false);//平台真空关
                                System.Threading.Thread.Sleep(200);
                                Rolled_Station.Instance.Vacumm(true);//翻转机构吸真空

                                Laser_Station.Instance.RightStatonFixtureLFCylinder(false);
                                Laser_Station.Instance.RightStatonFixtureRBCylinder(false);

                                Rolled_Station.Instance.RightStationMoveTo(3);//翻转机构运动到取料等待位置
                                Rolled_Station.Instance.RightStationMoveTo(4);//翻转机构运动到翻转等待位置
                                Rolled_Station.Instance.RightStationMoveTo(5);//翻转机构运动到翻转180位置


                                RunRightStep = EN_RunStep.UpLoadRobotWork;

                            }

                        }
                        break;
                    case EN_RunStep.UpLoadRobotWork:
                        {
                            UpLoadRobot.Instance.SetFixture1Result(false);
                            UpLoadRobot.Instance.SetFxiture1CanPick(true);
                            while (true)
                            {
                                System.Threading.Thread.Sleep(50);
                                Application.DoEvents();
                                if (UpLoadRobot.Instance.GetFixture1PickPosOK())
                                {
                                    break;
                                }
                            }
                            UpLoadRobot.Instance.SetFxiture1CanPick(false);
                            Rolled_Station.Instance.Vacumm(false);

                            UpLoadRobot.Instance.SetFxiture1PickCanGo(true);
                            while (true)
                            {
                                System.Threading.Thread.Sleep(50);
                                Application.DoEvents();
                                if (UpLoadRobot.Instance.GetFixture1PickDone())
                                {
                                    break;
                                }
                            }
                            UpLoadRobot.Instance.SetFxiture1PickCanGo(false);

                            Rolled_Station.Instance.RightStationMoveTo(0);//翻转机构运动到待机位置
                            isRolledWorking = false;

                            RunRightStep = EN_RunStep.StartWork;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// LMI处理算法
        /// </summary>
        private void LMIProcess()
        {
            while (true)
            {
                Thread.Sleep(20);
                if(CQueue_3DImage.Count >0)
                {
                    Infor3D temp3D;
                    bool res =   CQueue_3DImage.TryDequeue(out temp3D);

                    string file = DateTime.Now.ToString("DDHHmmssffff");
                    LMIControl.Instance.SaveImage(temp3D.Object, file);

                    if(res)
                    {
                        switch (temp3D.IndexImg)
                        {
                            case "1":
                               
                                //TODO算法运行获取结果 //BS-9,BT-113,BF-9
                                HObject RegX, RegY, hCrossBS, hCrossBT, hCrossBF;
                                HTuple hValueBS, hValueBT, hValueBF;
                                LMIControl.Instance.SetCordinate(temp3D.Object, out RegX, out RegY);
                                LMIControl.Instance.SPC(temp3D.Object, out hValueBS, out hCrossBS, out hValueBT, out hCrossBT, out hValueBF, out hCrossBF);
                                LMIControl.Instance.Flatness(temp3D.Object, 1);
                                for (int i=0;i< hValueBS.Length;i++)
                                {
                                    string str = "SPC_BS_" + (i + 1).ToString();
                                    frm_Main.MyPara.mFAIstation.FAIs[str].Value = Math.Round(hValueBS[i].D, 4);
                                }
                                for (int i = 0; i < hValueBT.Length; i++)
                                {
                                    string str = "SPC_BT_" + (i + 1).ToString();
                                    frm_Main.MyPara.mFAIstation.FAIs[str].Value = Math.Round(hValueBT[i].D, 4);
                                }
                                for (int i = 0; i < hValueBF.Length; i++)
                                {
                                    string str = "SPC_BT_" + (i + 1).ToString();
                                    frm_Main.MyPara.mFAIstation.FAIs[str].Value = Math.Round(hValueBF[i].D, 4);
                                }
                                break;

                            case "2":
                                LMIControl.Instance.Flatness(temp3D.Object, 2);
                                break;
                            case "3":
                                LMIControl.Instance.Flatness(temp3D.Object, 3);
                                break;
                            case "4":
                                LMIControl.Instance.Flatness(temp3D.Object, 4);
                                break;
                            case "5":
                                LMIControl.Instance.Flatness(temp3D.Object, 5);
                                break;
                            case "6":
                                LMIControl.Instance.Flatness(temp3D.Object, 6);
                                break;
                            case "7":
                                LMIControl.Instance.hValuePoints = null;
                                LMIControl.Instance.hValueFlatness = null;

                                LMIControl.Instance.Flatness(temp3D.Object, 7);

                                frm_Main.MyPara.mFAIstation.FAIs["SPC_Flatness"].Value = Math.Round(LMIControl.Instance.hValueFlatness.D, 4);

                                for(int i=0;i< LMIControl.Instance.hValuePoints.Length;i++)
                                {

                                    string str = "SPC_Flatness_" + (i + 1).ToString();
                                    frm_Main.MyPara.mFAIstation.FAIs[str].Value = Math.Round(LMIControl.Instance.hValuePoints[i].D, 4);
                                }
                               
                                frm_Main.MyPara.SaveFAIData();
                                frm_Main.MyPara.Line1Counter.OKIncrease();
                                TaskManage.GetSingleton().RaiseDataResultShow(frm_Main.MyPara.mFAIstation);
                                break;

                            default:
                                break;
                        }
                    }

                }
           
            }
        }

     

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public EN_RunRet AutoRunLeftStationStart()
        {
            try
            {
                if (!Laser_Station.Instance.LeftStatonFixtureLFCylinder(true)) //左前气缸动作
                {
                    return EN_RunRet.IOErr;
                }
                System.Threading.Thread.Sleep(2000);
                if (!Laser_Station.Instance.LeftStatonFixtureRBCylinder(true))//右后气缸动作
                {
                    return EN_RunRet.IOErr;
                }

                if(!Laser_Station.Instance.LeftStationVacummOn(true))
                {
                    return EN_RunRet.IOErr;
                }

                while (isLaserWorking)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(50);
                }
                isLaserWorking = true;
              
                //if (Laser_Station.Instance.LeftStationMoveTo(2,true))//运动到Laser扫描点位
                //{
                //    return EN_RunRet.MotionErr;
                //}
                isLaserWorking = false;

                if (frm_Main.MyPara.mFAIstation.Result)
                {
                  //  CtrDisplay.AddRowOnDgv(dgv_data, new object[] { });
                  
                }
                else
                {
                    
                }

                if (Rolled_Station.Instance.LeftStationMoveTo(1))//运动到翻转点位
                {
                    return EN_RunRet.MotionErr;
                }
               if(!Rolled_Station.Instance.Vacumm(true)) //翻转站吸真空
                {
                    return EN_RunRet.IOErr;
                }

                if (Rolled_Station.Instance.LeftStationMoveTo(2))//运动到翻转点位
                {
                    return EN_RunRet.MotionErr;
                }

            }
            catch (Exception ex) 
            {
                frm_Main.logNet.WriteError(ex.ToString() + ex.StackTrace);
                return EN_RunRet.TaskErr;
            }
            return EN_RunRet.TaskOk;



        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public EN_RunRet AutoRunRightStationStart()
        {
            try
            {
                if (!Laser_Station.Instance.RightStatonFixtureLFCylinder(true)) //左前气缸动作
                {
                    return EN_RunRet.IOErr;
                }
                System.Threading.Thread.Sleep(2000);
                if (!Laser_Station.Instance.RightStatonFixtureRBCylinder(true))//右后气缸动作
                {
                    return EN_RunRet.IOErr;
                }

                if (!Laser_Station.Instance.RightStationVacummOn(true))
                {
                    return EN_RunRet.IOErr;
                }

                while (isLaserWorking) //等待到右工位工作条件
                {
                    System.Threading.Thread.Sleep(50);
                    Application.DoEvents();
                }

                isLaserWorking = true;
                //if (Laser_Station.Instance.RightStationMoveTo(2, true))//运动到Laser扫描点位
                //{
                //    return EN_RunRet.MotionErr;
                //}
                isLaserWorking = false;

                if (Rolled_Station.Instance.RightStationMoveTo(1))//运动到翻转点位
                {
                    return EN_RunRet.MotionErr;
                }
                if (!Rolled_Station.Instance.Vacumm(true)) //翻转站吸真空
                {
                    return EN_RunRet.IOErr;
                }

                if (Rolled_Station.Instance.RightStationMoveTo(2))//运动到翻转点位
                {
                    return EN_RunRet.MotionErr;
                }

            }
            catch (Exception ex)
            {
                frm_Main.logNet.WriteError(ex.ToString() + ex.StackTrace);
                return EN_RunRet.TaskErr;
            }
            return EN_RunRet.TaskOk;



        }
     
        /// <summary>
        /// 设备回原点工作
        /// </summary>
        /// <returns></returns>
        public bool ResetProcessAll()
        {
            if(MachineStatus != EN_MachineStatus.IsReseting)
            {
                if (Laser_Station.Instance.LeftStationMaterialSensor() || Laser_Station.Instance.RightStationMaterialSensor() || Rolled_Station.Instance.IsVacummOK())
                {
                    MessageBox.Show("载台治具上有物料，请清除后再复位", "出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   // return false;
                }

                MachineStatus = EN_MachineStatus.IsReseting;

                DisplayLog("设备正在复位中", Color.Blue);

                //不屏蔽上料机器人
                if (!frm_Main.MyPara.mParaSetting.Disable_LoadRobot)
                {
                    LoadRobot.Instance.SetLoadFixture1CanPut(false);
                    LoadRobot.Instance.SetLoadFixture1PutOK(false);
                    LoadRobot.Instance.SetLoadFixture2CanPut(false);
                    LoadRobot.Instance.SetLoadFixture2PutOK(false);
                    LoadRobot.Instance.SetRobotStart(false);
                    LoadRobot.Instance.SetRobotStop(false);
                    LoadRobot.Instance.SetRobotPause(false);
                    LoadRobot.Instance.SetRobotReset(false);

                    LoadRobot.Instance.SetRobotReset(true);
                    LoadRobot.Instance.SetRobotStop(true);
                    System.Threading.Thread.Sleep(1000);
                    LoadRobot.Instance.SetRobotReset(false);
                    LoadRobot.Instance.SetRobotStop(false);

                    LoadRobot.Instance.SetRobotStart(true);
                    System.Threading.Thread.Sleep(1000);
                    LoadRobot.Instance.SetRobotStart(false);

                    while(true)
                    {
                        System.Threading.Thread.Sleep(50);
                        Application.DoEvents();
                        if(LoadRobot.Instance.GetRobotIsIniOK())
                        {
                            break;
                        }
                    }
                }

                //不屏蔽下料机器人
                if (!frm_Main.MyPara.mParaSetting.Disable_UpLoadRobot)
                {
                    UpLoadRobot.Instance.SetFixture1Result(false);
                    UpLoadRobot.Instance.SetFixture2Result(false);
                    UpLoadRobot.Instance.SetFxiture1CanPick(false);
                    UpLoadRobot.Instance.SetFxiture1PickCanGo(false);
                    UpLoadRobot.Instance.SetFxiture2CanPick(false);
                    UpLoadRobot.Instance.SetFxiture2PickCanGo(false);
                    UpLoadRobot.Instance.SetRobotStart(false);
                    UpLoadRobot.Instance.SetRobotStop(false);
                    UpLoadRobot.Instance.SetRobotReset(false);
                    UpLoadRobot.Instance.SetRobotPause(false);

                    UpLoadRobot.Instance.SetRobotReset(true);
                    UpLoadRobot.Instance.SetRobotStop(true);
                    System.Threading.Thread.Sleep(1000);
                    UpLoadRobot.Instance.SetRobotReset(false);
                    UpLoadRobot.Instance.SetRobotStop(false);

                    UpLoadRobot.Instance.SetRobotStart(true);
                    System.Threading.Thread.Sleep(1000);
                    UpLoadRobot.Instance.SetRobotStart(false);

                    while (true)
                    {
                        System.Threading.Thread.Sleep(50);
                        Application.DoEvents();
                        if (UpLoadRobot.Instance.GetRobotIsIniOK())
                        {
                            break;
                        }
                    }
                }

                Rolled_Station.Instance.Home();

                Laser_Station.Instance.HomeXY();
                Laser_Station.Instance.HomeY1();
                Laser_Station.Instance.HomeY2();

                Laser_Station.Instance.LeftStationMoveTo(0, false, false, true); //XYY1待机位置
                Laser_Station.Instance.RightStationMoveTo(0, false, true, false);// Y2待机位置
                Rolled_Station.Instance.LeftStationMoveTo(0);//翻转机构运动到待机位置

                Laser_Station.Instance.LeftStatonFixtureLFCylinder(false);
                Laser_Station.Instance.LeftStatonFixtureRBCylinder(false);
                Laser_Station.Instance.RightStatonFixtureLFCylinder(false);
                Laser_Station.Instance.RightStatonFixtureRBCylinder(false);

                Laser_Station.Instance.LeftStationVacummOn(false);
                Laser_Station.Instance.RightStationVacummOn(false);
                Rolled_Station.Instance.Vacumm(false);

              

                DisplayLog("设备复位完成", Color.Blue);

                MachineStatus = EN_MachineStatus.Reseted;
                return true;
            }
            else
            {
                MessageBox.Show("设备正在回原点中","出错",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }

        }


    }

    public class Infor3D
    { 
        /// <summary>
        /// 3D图片
        /// </summary>
         public HalconDotNet.HObject Object { get; set; }

        /// <summary>
        /// 图片编号
        /// </summary>
         public string IndexImg { get; set; }

        /// <summary>
        /// 站名
        /// </summary>
        public string StationName { get; set; }
    }

}
