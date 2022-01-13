using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace  X2292_ChassisFinalAssyAOI
{
    public  class MotionControl_GTS
    {
        public  bool _isInitOk = false;
        private bool isExtMdlInitOk = false;
        /// <summary>
        /// 轴数量
        /// </summary>
        public  short AxisNo { get; set; } = 8;

        /// <summary>
        /// 卡号
        /// </summary>
        public  short CardNo { get; set; } = 0;


        public MotionControl_GTS(short _CardNo,short _AxisNo)
        {
            CardNo = _CardNo;
            AxisNo = _AxisNo;
        }
        public  bool CardInit(string CardConfigName)
        {          
            short sRtn = 0;
            sRtn += gts.mc.GT_Open(CardNo, 0, 1);
            sRtn += gts.mc.GT_Reset(CardNo);
            // sRtn += gts.mc.GT_LoadConfig(CardNo, "GTS800_1.cfg");
            sRtn += gts.mc.GT_LoadConfig(CardNo, CardConfigName);

            gts.mc.GT_SetDo(CardNo, gts.mc.MC_CLEAR, 15);          
            System.Threading.Thread.Sleep(500);
            gts.mc.GT_SetDo(CardNo, gts.mc.MC_CLEAR, 0);

            sRtn += gts.mc.GT_ClrSts(CardNo, 1, AxisNo);       

            for (short iAxis = 1; iAxis <= AxisNo; iAxis++)
            {
                sRtn += gts.mc.GT_AxisOn(CardNo, iAxis);
            }
            if (sRtn == 0)
            {
                _isInitOk = true;
                return true;
            }
            else
            {
                MessageBox.Show($"{CardNo}:板卡初始化失败");
                _isInitOk = false;
                return false;
            }

        }

        public void ExtMdlInit()
        {
            short sRtn = 0;
            sRtn += gts.mc.GT_OpenExtMdl(CardNo, "gts.dll");  //拓展模块与gts卡函数都在gts.dll其中
            sRtn += gts.mc.GT_ResetExtMdl(CardNo);            //复位拓展模块
            sRtn += gts.mc.GT_LoadExtConfig(CardNo, "ExtMdl.cfg");//加载拓展模块配置文件
            if (sRtn != 0)
            {
                MessageBox.Show($"{CardNo}:板卡-扩展IO板块初始化失败");
            }
            isExtMdlInitOk = true;
        }

        /// <summary>
        /// IO输出操作---本地卡
        /// </summary>
        /// <param name="card">卡号</param>
        /// <param name="bit">位</param>
        /// <param name="bSwitch">开/关</param>
        /// <returns></returns>
        public  bool Output( short bit, bool bSwitch)
        {
            short sRtn = 0;
            sRtn += gts.mc.GT_SetDoBit(CardNo, gts.mc.MC_GPO, bit, (short)Convert.ToUInt16(!bSwitch));
            int pValue = 0;
            gts.mc.GT_GetDo(CardNo, gts.mc.MC_GPO, out pValue);
            return sRtn == 0 ? true : false;
        }


        /// <summary>
        /// IO输出操作---扩展卡
        /// </summary>
        /// <param name="card">卡号</param>
        /// <param name="mdl">扩展卡模块号</param>
        /// <param name="bit">位</param>
        /// <param name="bSwitch">开/关</param>
        /// <returns></returns>
        public  bool Output( short mdl, short bit, bool bSwitch)
        {
            short sRtn = 0;
            ushort pStatus;

            if (isExtMdlInitOk)
            {              
                sRtn = gts.mc.GT_GetStsExtMdl(CardNo, mdl, 0, out pStatus);
                if (pStatus != 0)
                {
                    MessageBox.Show("扩展IO通信异常", "出错：", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
                sRtn = gts.mc.GT_SetExtIoBit(CardNo, mdl, bit, (ushort)Convert.ToUInt16(!bSwitch));
                //    gts.mc.GT_GetExtDoValue()       
                return sRtn == 0 ? true : false;
            }
            else
            {
                MessageBox.Show("扩展IO板块初始化失败", "出错：", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return  false;
            }
          
        }

        /// <summary>
        /// IO输入操作---本地
        /// </summary>
        /// <param name="CardNo">卡号</param>
        /// <param name="bit">位</param>
        /// <param name="IOResult">IO信号</param>
        /// <returns></returns>
        public  bool Input( short bit, out bool IOResult)
        {
            short sRtn;
            int iValue;
            sRtn = gts.mc.GT_GetDi(CardNo, gts.mc.MC_GPI, out iValue);
            if ((1 << (bit) & iValue) == 0)
            {
                IOResult = true;
            }
            else
            {
                IOResult = false;
            }

            return sRtn == 0 ? true : false;
        }

        /// <summary>
        /// IO输入操作---扩展IO
        /// </summary>
        /// <param name="CardNo">卡号</param>
        /// <param name="mdl">模块号</param>
        /// <param name="bit">位</param>
        /// <param name="IOResult">IO信号</param>
        /// <returns></returns>
        public  bool Input(short mdl, short bit, out bool IOResult)
        {
            short sRtn;
            ushort iValue;

            if (isExtMdlInitOk)
            {
                //sRtn = gts.mc.GT_GetExtIoBit(CardNo, mdl, bit, out iValue);
                sRtn = gts.mc.GT_GetExtIoValue(CardNo, mdl, out iValue);
                if ((1 << (bit) & iValue) == 0)
                {
                    IOResult = true;
                }
                else
                {
                    IOResult = false;
                }

                return sRtn == 0 ? true : false;
            }
            else
            {
                IOResult = false;
                return false;
            }
                
        }

        public static int  Input(List<InputBitInfo> InputBitInfoS,int index )
        {
            int  result = -1;
            if(InputBitInfoS[index].BitPort ==-1)
            {
                short sRtn;
                int iValue;
                sRtn = gts.mc.GT_GetDi(InputBitInfoS[index].CardNo, gts.mc.MC_GPI, out iValue);
                if ((1 << (InputBitInfoS[index].BitNum) & iValue) == 0)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
            else
            {
                ushort iValue;
                gts.mc.GT_GetExtIoBit(InputBitInfoS[index].CardNo, InputBitInfoS[index].BitPort, InputBitInfoS[index].BitNum, out iValue);
                if (iValue == 0)
                    result = 1;
                else
                    result = 0;
            }

            return result;

        }

        public static void ClearAlarm()
        {
            gts.mc.GT_ClrSts(0, 1, 7);
        }

        public static void Output(List<OutputBitInfo> OutputBitInfoS,int index,bool IsTrue)
        {
            if (OutputBitInfoS[index].BitPort == -1)
            {
                short sRtn = 0;
                if(IsTrue)
                {
                    sRtn += gts.mc.GT_SetDoBit((short)OutputBitInfoS[index].CardNo, gts.mc.MC_GPO, (short)OutputBitInfoS[index].BitNum, 0);

                }
                else
                {
                    sRtn += gts.mc.GT_SetDoBit((short)OutputBitInfoS[index].CardNo, gts.mc.MC_GPO, (short)OutputBitInfoS[index].BitNum, 1);

                }
            }
            else
            {
                short sRtn = 0;
                ushort pStatus;
                sRtn = gts.mc.GT_GetStsExtMdl((short)OutputBitInfoS[index].CardNo, (short)OutputBitInfoS[index].BitPort, 0, out pStatus);
                if (pStatus != 0)
                {
                    System.Windows.Forms.MessageBox.Show("扩展IO通信异常", "出错：", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                if(IsTrue)
                {
                    sRtn = gts.mc.GT_SetExtIoBit((short)OutputBitInfoS[index].CardNo, (short)OutputBitInfoS[index].BitPort, (short)OutputBitInfoS[index].BitNum, 0);
                }
                else
                {
                    sRtn = gts.mc.GT_SetExtIoBit((short)OutputBitInfoS[index].CardNo, (short)OutputBitInfoS[index].BitPort, (short)OutputBitInfoS[index].BitNum, 1);
                }
            }
        }

        /// <summary>
        /// 绝对运动
        /// </summary>
        /// <param name="CardNo"></param>
        /// <param name="Axis_Index"></param>
        /// <param name="AccDec"></param>
        /// <param name="RunSpeed"></param>
        /// <param name="Position"></param>
        /// <returns></returns>
        public  bool AbsoluteMove(short Axis_Index,double PulseToMM,double AccDec, double RunSpeed, double Position)
        {
            short sRtn = 0;//返回值
            gts.mc.TTrapPrm trap;

            //设置为点位运动，模式切换需要停止轴运动。
            //若返回值为 1：若当前轴在规划运动，请调用GT_Stop停止运动再调用该指令。
            sRtn += gts.mc.GT_PrfTrap(CardNo, Axis_Index);

            //读取点位运动参数（不一定需要）。若返回值为 1：请检查当前轴是否为 Trap 模式
            // 若不是，请先调用 GT_PrfTrap 将当前轴设置为 Trap 模式。
            sRtn += gts.mc.GT_GetTrapPrm(CardNo, Axis_Index, out trap);
            trap.acc = AccDec;              //单位pulse/ms2
            trap.dec = AccDec;              //单位pulse/ms2
            trap.velStart = 0;           //起跳速度，默认为0。
            trap.smoothTime = 25;         //平滑时间，使加减速更为平滑。范围[0,50]单位ms。

            sRtn += gts.mc.GT_SetTrapPrm(CardNo, Axis_Index, ref trap);//设置点位运动参数。

            sRtn += gts.mc.GT_SetVel(CardNo, Axis_Index, RunSpeed / PulseToMM / 1000);        //设置目标速度
            sRtn += gts.mc.GT_SetPos(CardNo, Axis_Index, (int)(Position / PulseToMM));        //设置目标位置
            sRtn += gts.mc.GT_Update(CardNo, 1 << (Axis_Index - 1));   //更新轴运动

            return sRtn == 0 ? true : false;
        }

        /// <summary>
        /// 停止轴
        /// </summary>
        /// <returns></returns>
        public  bool AxisStop( short Axis_Index)
        {
            short sRtn = 0;
            sRtn = gts.mc.GT_Stop(CardNo, 1 << Axis_Index - 1, 0);
            return sRtn == 0 ? true : false;
        }

        /// <summary>
        /// 获取规划位置
        /// </summary>
        /// <param name="CardNo"></param>
        /// <param name="Axis_Index"></param>
        /// <param name="PulseToMM"></param>
        /// <param name="Postion"></param>
        /// <returns></returns>
        public  bool GetPrfPos( short Axis_Index ,double PulseToMM,  out double Postion)
        {
            double pPrf1;
            uint pClock;
            short sRtn = 0;
            Postion = 0;
            sRtn = gts.mc.GT_GetPrfPos(CardNo, Axis_Index, out pPrf1, 1, out pClock);
            Postion = Math.Round( pPrf1 * PulseToMM,4);
            return sRtn == 0 ? true : false;
        }

        /// <summary>
        /// 获取轴到位信号
        /// </summary>
        /// <param name="isMotionDone"></param>
        /// <returns></returns>
        public  bool MotionDone( short Axis_Index, out bool isMotionDone)
        {
            //   sRtn = gts.mc.GT_GetDi(cardNo, gts.mc.MC_ARRIVE, out pValue);           
            short sRtn = 0;
            int status;
            uint pClock;
            isMotionDone = false;

            do
            {
                System.Threading.Thread.Sleep(10);
                sRtn = gts.mc.GT_GetSts(CardNo, Axis_Index, out status, 1, out pClock);

            } while ((status & 0x0400) != 0);

            isMotionDone = true;
            return sRtn == 0 ? true : false;
        }

        /// <summary>
        /// 检测轴状态
        /// </summary>
        /// <returns></returns>
        public  bool CheckAxisStatus( short Axis_Index, out bool IsAlarm ,out bool IsLimitPositive,out bool IsLimitNegative)
        {
            short sRtn = 0;
            int status;
            uint pClock;
            //状态字查询
            sRtn += gts.mc.GT_GetSts(CardNo, Axis_Index, out status, 1, out pClock);

            if ((status & 0x0001) == 0)
                IsAlarm = false;
            else
                IsAlarm = false;

            if ((status & 0x0020) == 0)
                IsLimitPositive = false;
            else
                IsLimitPositive = true;

            if ((status & 0x0040) == 0)
                IsLimitNegative = false;
            else
                IsLimitNegative = true;

            return sRtn == 0 ? true : false;
        }

        /// <summary>
        /// 相对运动
        /// </summary>
        /// <param name="AccDec"></param>
        /// <param name="RelativeDistance"></param>
        /// <returns></returns>
        public  bool RelativeMove( short Axis_Index, double PulseToMM,double DebugSpeed,double AccDec, double RelativeDistance)
        {
            short sRtn = 0;     //返回值
            double prfPos; //规划脉冲
            uint pClock;    //时钟信号
            gts.mc.TTrapPrm trap;

            //设置为点位运动，模式切换需要停止轴运动。
            //若返回值为 1：若当前轴在规划运动，请调用GT_Stop停止运动再调用该指令。
            sRtn += gts.mc.GT_PrfTrap(CardNo, Axis_Index);

            //读取点位运动参数（不一定需要）。若返回值为 1：请检查当前轴是否为 Trap 模式
            // 若不是，请先调用 GT_PrfTrap 将当前轴设置为 Trap 模式。
            sRtn += gts.mc.GT_GetTrapPrm(CardNo, Axis_Index, out trap);

            trap.acc = AccDec;              //单位pulse/ms2
            trap.dec = AccDec;              //单位pulse/ms2
            trap.velStart = 0;           //起跳速度，默认为0。
            trap.smoothTime = 25;         //平滑时间，使加减速更为平滑。范围[0,50]单位ms。

            sRtn += gts.mc.GT_SetTrapPrm(CardNo, Axis_Index, ref trap);//设置点位运动参数。
            sRtn += gts.mc.GT_GetPrfPos(CardNo, Axis_Index, out prfPos, 1, out pClock);//读取规划位置
            sRtn += gts.mc.GT_SetVel(CardNo, Axis_Index, DebugSpeed / PulseToMM / 1000);        //设置目标速度
            sRtn += gts.mc.GT_SetPos(CardNo, Axis_Index, (int)(RelativeDistance / PulseToMM + prfPos));        //设置目标位置
            sRtn += gts.mc.GT_Update(CardNo, 1 << (Axis_Index - 1));   //更新轴运动

            return sRtn == 0 ? true : false;
        }

        /// <summary>
        /// Jog运动
        /// </summary>
        /// <param name="isPositive"></param>
        /// <returns></returns>
        public  bool Jog( short Axis_Index,double PulseToMM,double DebugSpeed,bool isPositive)
        {
            short sRtn = 0;
            gts.mc.TJogPrm pJog = new gts.mc.TJogPrm() { acc = 1, dec = 1, smooth = 0.5 };//平滑系数,取值范围[0, 1),平滑系数的数值越大，加减速过程越平稳。
            sRtn += gts.mc.GT_PrfJog(CardNo, Axis_Index);
            sRtn += gts.mc.GT_SetJogPrm(CardNo, Axis_Index, ref pJog);//设置jog运动参数
            if (isPositive)
            {
                double vel = DebugSpeed / PulseToMM / 1000;     //速度mm/s转化为pulse/ms
                sRtn += gts.mc.GT_SetVel(CardNo, Axis_Index, vel);//设置目标速度,velJd的符号决定JOG运动方向
            }
            else
            {
                double vel = DebugSpeed / PulseToMM / 1000;     //速度mm/s转化为pulse/ms
                sRtn += gts.mc.GT_SetVel(CardNo, Axis_Index, -vel);//设置目标速度,velJd的符号决定JOG运动方向
            }

            sRtn += gts.mc.GT_Update(CardNo, 1 << (Axis_Index - 1));//更新轴运动
            return sRtn == 0 ? true : false;
        }

        /// <summary>
        /// 轴回原点
        /// </summary>
        /// <param name="HomeHightSpeed"></param>
        /// <param name="HomeLowSpeed"></param>
        /// <param name="HomeOffset"></param>
        /// <returns></returns>
        public  bool Home( short Axis_Index, HomeMoveDirection AxisHomeMoveDirection, HomeMode AxisHomeMode, double PulseToMM, double HomeHightSpeed, double HomeLowSpeed, double HomeOffset)
        {
            short sRtn = 0;
            gts.mc.THomePrm tHomePrm = new gts.mc.THomePrm();


            sRtn += gts.mc.GT_Stop(CardNo, 1 << (Axis_Index - 1), 0);
            sRtn += gts.mc.GT_ClrSts(CardNo, Axis_Index, 1);
            sRtn += gts.mc.GT_ZeroPos(CardNo, Axis_Index, 1);
            sRtn += gts.mc.GT_GetHomePrm(CardNo, Axis_Index, out tHomePrm);

            switch (AxisHomeMoveDirection)
            {
                case HomeMoveDirection.Positive:
                    tHomePrm.moveDir = 1; //1:正向移动，-1负向移动 
                    break;
                case HomeMoveDirection.Negative:
                    tHomePrm.moveDir = -1; //1:正向移动，-1负向移动 
                    break;
                default:
                    break;
            }

            tHomePrm.edge = 0;//设置捕获沿： 设置捕获沿： 0-下降沿 1-上升沿 
            tHomePrm.velHigh = HomeHightSpeed / PulseToMM / 1000;
            tHomePrm.velLow = HomeLowSpeed / PulseToMM / 1000;
            tHomePrm.acc = 0.1;
            tHomePrm.dec = 0.1;
            tHomePrm.smoothTime = 10;
            tHomePrm.homeOffset = (int)(HomeOffset / PulseToMM); //偏移距离   
            tHomePrm.searchHomeDistance = 0;//搜搜距离          
            tHomePrm.escapeStep = 20000;//采用“限位回原点” 方式时，反向离开限位的脱离步长
                                        //pad2[1]表示在电机启动回零时是否检测机械处于限位或 原点位置，0 或其它值 - 不检测（默认），1 - 检测
            switch (AxisHomeMode)
            {
                case HomeMode.Limit:
                    tHomePrm.mode = gts.mc.HOME_MODE_LIMIT;
                    break;
                case HomeMode.Home:
                    tHomePrm.mode = gts.mc.HOME_MODE_HOME;
                    break;
                case HomeMode.LimitHome:
                    tHomePrm.mode = gts.mc.HOME_MODE_LIMIT_HOME;
                    break;
                case HomeMode.LimitHomeIndex:
                    tHomePrm.mode = gts.mc.HOME_MODE_LIMIT_HOME_INDEX;
                    break;
                default:
                    break;
            }

            sRtn += gts.mc.GT_GoHome(CardNo, Axis_Index, ref tHomePrm);//启动SmartHome回原点
            return sRtn == 0 ? true : false;

        }

        /// <summary>
        /// 判断原点是否完成
        /// </summary>
        /// <param name="CardNo"></param>
        /// <param name="Axis_Index"></param>
        /// <param name="isHomeFinished"></param>
        /// <returns></returns>
        public  bool IsHomeFinished( short Axis_Index, out bool isHomeFinished)
        {
            short sRtn = 0;
            isHomeFinished = false;
            gts.mc.THomeStatus tHomeStatus = new gts.mc.THomeStatus();
            do
            {
                sRtn = gts.mc.GT_ClrSts(CardNo, Axis_Index, 1);
                sRtn = gts.mc.GT_GetHomeStatus(CardNo, Axis_Index, out tHomeStatus);//获取回原点状态
            } while (tHomeStatus.run == 1); // 等待搜索原点停止
            System.Threading.Thread.Sleep(500);
            sRtn = gts.mc.GT_ClrSts(CardNo, Axis_Index, 1);
            sRtn = gts.mc.GT_ZeroPos(CardNo, Axis_Index, 1);
            isHomeFinished = true;
            return sRtn == 0 ? true : false;
        }

        public  bool CardClose()
        {
            short sRtn = 0;
            for (short iAxis = 1; iAxis <= AxisNo; iAxis++)
            {
                sRtn += gts.mc.GT_AxisOff(CardNo, iAxis);
            }
            sRtn += gts.mc.GT_CloseExtMdl(CardNo);
            sRtn += gts.mc.GT_Close(CardNo);          
            return sRtn == 0 ? true : false;
        }
    }

    public enum HomeMode { Limit = 1, Home = 2, LimitHome = 3, LimitHomeIndex = 4 }

    public enum HomeMoveDirection { Positive = 1, Negative = 2 }

}
