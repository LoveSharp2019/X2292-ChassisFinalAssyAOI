using System.Runtime.InteropServices;

namespace gts
{
    public class mc_cfg
	{
		
			public const short RES_LIMIT = 8;
			public const short RES_ALARM = 8;
			public const short RES_HOME = 8;
			public const short RES_GPI = 16;
			public const short RES_ARRIVE = 8;
			public const short RES_MPG = 7;
			
			public const short RES_ENABLE = 8;
			public const short RES_CLEAR = 8;
			public const short RES_GPO = 16;
			
			public const short RES_DAC = 12;
			public const short RES_STEP = 8;
			public const short RES_PULSE = 8;
			public const short RES_ENCODER = 11;
			public const short RES_LASER = 2;  
			
			public const short AXIS_MAX = 8;
			public const short PROFILE_MAX = 8;
			public const short CONTROL_MAX = 8;
			
			public const short PRF_MAP_MAX = 2;
			public const short ENC_MAP_MAX = 2;
			
			public const short STEP_DIR = 0;
			public const short STEP_PULSE = 1;
			
			public const short CRD_BUF_DATA_NULL = 0;
			
			public struct TDiConfig
			{
                public short active;
                public short reverse;
                public short filterTime;
			}

			public struct TCountConfig
			{
                public short active;
                public short reverse;
                public short filterType;

                public short captureSource;
                public short captureHomeSense;
                public short captureIndexSense;
			}

			public struct TDoConfig
			{
                public short active;
                public short axis;
                public short axisItem;
                public short reverse;
			}

			public struct TStepConfig
			{
                public short active;
                public short axis;
                public short mode;
                public short parameter;
                public short reverse;
			}

			public struct TDacConfig
			{
                public short active;
                public short control;
                public short reverse;
                public short bias;
                public short limit;
			}

			public struct TControlConfig
			{
                public short active;
                public short axis;
                public short encoder1;
                public short encoder2;
                public int errorLimit;
                public short filterType1;
                public short filterType2;
                public short filterType3;
                public short encoderSmooth;
                public short controlSmooth;
			}

			public struct TProfileConfig
			{
                public short active;
                public double decSmoothStop;
                public double decAbruptStop;
			}

			public struct TAxisConfig
			{
                public short active;
                public short alarmType;
                public short alarmIndex;
                public short limitPositiveType;
                public short limitPositiveIndex;
                public short limitNegativeType;
                public short limitNegativeIndex;
                public short smoothStopType;
                public short smoothStopIndex;
                public short abruptStopType;
                public short abruptStopIndex;
                public int prfMap;
                public int encMap;
                public short prfMapAlpha1;
                public short prfMapAlpha2;
                public short prfMapBeta1;
                public short prfMapBeta2;
                public short encMapAlpha1;
                public short encMapAlpha2;
                public short encMapBeta1;
                public short encMapBeta2;
			}

			public struct TMcConfig
			{
                public TProfileConfig profile1;
                public TProfileConfig profile2;
                public TProfileConfig profile3;
                public TProfileConfig profile4;
                public TProfileConfig profile5;
                public TProfileConfig profile6;
                public TProfileConfig profile7;
                public TProfileConfig profile8;
                public TAxisConfig axis1;
                public TAxisConfig axis2;
                public TAxisConfig axis3;
                public TAxisConfig axis4;
                public TAxisConfig axis5;
                public TAxisConfig axis6;
                public TAxisConfig axis7;
                public TAxisConfig axis8;
                public TControlConfig control1;
                public TControlConfig control2;
                public TControlConfig control3;
                public TControlConfig control4;
                public TControlConfig control5;
                public TControlConfig control6;
                public TControlConfig control7;
                public TControlConfig control8;
                public TDacConfig dac1;
                public TDacConfig dac2;
                public TDacConfig dac3;
                public TDacConfig dac4;
                public TDacConfig dac5;
                public TDacConfig dac6;
                public TDacConfig dac7;
                public TDacConfig dac8;
                public TDacConfig dac9;
                public TDacConfig dac10;
                public TDacConfig dac11;
                public TDacConfig dac12;
                public TStepConfig step1;
                public TStepConfig step2;
                public TStepConfig step3;
                public TStepConfig step4;
                public TStepConfig step5;
                public TStepConfig step6;
                public TStepConfig step7;
                public TStepConfig step8;
                public TCountConfig encoder1;
                public TCountConfig encoder2;
                public TCountConfig encoder3;
                public TCountConfig encoder4;
                public TCountConfig encoder5;
                public TCountConfig encoder6;
                public TCountConfig encoder7;
                public TCountConfig encoder8;
                public TCountConfig encoder9;
                public TCountConfig encoder10;
                public TCountConfig encoder11;
                public TCountConfig pulse1;
                public TCountConfig pulse2;
                public TCountConfig pulse3;
                public TCountConfig pulse4;
                public TCountConfig pulse5;
                public TCountConfig pulse6;
                public TCountConfig pulse7;
                public TCountConfig pulse8;
                public TDoConfig enable1;
                public TDoConfig enable2;
                public TDoConfig enable3;
                public TDoConfig enable4;
                public TDoConfig enable5;
                public TDoConfig enable6;
                public TDoConfig enable7;
                public TDoConfig enable8;
                public TDoConfig clear1;
                public TDoConfig clear2;
                public TDoConfig clear3;
                public TDoConfig clear4;
                public TDoConfig clear5;
                public TDoConfig clear6;
                public TDoConfig clear7;
                public TDoConfig clear8;
                public TDoConfig gpo1;
                public TDoConfig gpo2;
                public TDoConfig gpo3;
                public TDoConfig gpo4;
                public TDoConfig gpo5;
                public TDoConfig gpo6;
                public TDoConfig gpo7;
                public TDoConfig gpo8;
                public TDoConfig gpo9;
                public TDoConfig gpo10;
                public TDoConfig gpo11;
                public TDoConfig gpo12;
                public TDoConfig gpo13;
                public TDoConfig gpo14;
                public TDoConfig gpo15;
                public TDoConfig gpo16;
                public TDiConfig limitPositive1;
                public TDiConfig limitPositive2;
                public TDiConfig limitPositive3;
                public TDiConfig limitPositive4;
                public TDiConfig limitPositive5;
                public TDiConfig limitPositive6;
                public TDiConfig limitPositive7;
                public TDiConfig limitPositive8;
                public TDiConfig limitNegative1;
                public TDiConfig limitNegative2;
                public TDiConfig limitNegative3;
                public TDiConfig limitNegative4;
                public TDiConfig limitNegative5;
                public TDiConfig limitNegative6;
                public TDiConfig limitNegative7;
                public TDiConfig limitNegative8;
                public TDiConfig alarm1;
                public TDiConfig alarm2;
                public TDiConfig alarm3;
                public TDiConfig alarm4;
                public TDiConfig alarm5;
                public TDiConfig alarm6;
                public TDiConfig alarm7;
                public TDiConfig alarm8;
                public TDiConfig home1;
                public TDiConfig home2;
                public TDiConfig home3;
                public TDiConfig home4;
                public TDiConfig home5;
                public TDiConfig home6;
                public TDiConfig home7;
                public TDiConfig home8;
                public TDiConfig gpi1;
                public TDiConfig gpi2;
                public TDiConfig gpi3;
                public TDiConfig gpi4;
                public TDiConfig gpi5;
                public TDiConfig gpi6;
                public TDiConfig gpi7;
                public TDiConfig gpi8;
                public TDiConfig arrive1;
                public TDiConfig arrive2;
                public TDiConfig arrive3;
                public TDiConfig arrive4;
                public TDiConfig arrive5;
                public TDiConfig arrive6;
                public TDiConfig arrive7;
                public TDiConfig arrive8;
                public TDiConfig mpg1;
                public TDiConfig mpg2;
                public TDiConfig mpg3;
                public TDiConfig mpg4;
                public TDiConfig mpg5;
                public TDiConfig mpg6;
                public TDiConfig mpg7;
			}

			[DllImport("gts.dll")]
			public static extern short GT_SetDiConfig(short cardNum,short diType,short diIndex,ref TDiConfig pDi);
			[DllImport("gts.dll")]
			public static extern short GT_GetDiConfig(short cardNum,short diType,short diIndex,out TDiConfig pDi);
			[DllImport("gts.dll")]
			public static extern short GT_SetDoConfig(short cardNum,short doType,short doIndex,ref TDoConfig pDo);
			[DllImport("gts.dll")]
			public static extern short GT_GetDoConfig(short cardNum,short doType,short doIndex,out TDoConfig pDo);
			[DllImport("gts.dll")]
			public static extern short GT_SetStepConfig(short cardNum,short step,ref TStepConfig pStep);
			[DllImport("gts.dll")]
			public static extern short GT_GetStepConfig(short cardNum,short step,out TStepConfig pStep);
			[DllImport("gts.dll")]
			public static extern short GT_SetDacConfig(short cardNum,short dac,ref TDacConfig pDac);
			[DllImport("gts.dll")]
			public static extern short GT_GetDacConfig(short cardNum,short dac,out TDacConfig pDac);
			[DllImport("gts.dll")]
			public static extern short GT_SetCountConfig(short cardNum,short countType,short countIndex,ref TCountConfig pCount);
			[DllImport("gts.dll")]
			public static extern short GT_GetCountConfig(short cardNum,short countType,short countIndex,out TCountConfig pCount);
			[DllImport("gts.dll")]
			public static extern short GT_SetControlConfig(short cardNum,short control,ref TControlConfig pControl);
			[DllImport("gts.dll")]
			public static extern short GT_GetControlConfig(short cardNum,short control,out TControlConfig pControl);
			[DllImport("gts.dll")]
			public static extern short GT_SetProfileConfig(short cardNum,short profile,ref TProfileConfig pProfile);
			[DllImport("gts.dll")]
			public static extern short GT_GetProfileConfig(short cardNum,short profile,out TProfileConfig pProfile);
			[DllImport("gts.dll")]
			public static extern short GT_SetAxisConfig(short cardNum,short axis,ref TAxisConfig pAxis);
			[DllImport("gts.dll")]
			public static extern short GT_GetAxisConfig(short cardNum,short axis,out TAxisConfig pAxis);
			[DllImport("gts.dll")]
			public static extern short GT_GetConfigTable(short cardNum,short type,out short pCount);
			[DllImport("gts.dll")]
			public static extern short GT_GetConfigTableAll(short cardNum);
			[DllImport("gts.dll")]
			public static extern short GT_SetMcConfig(short cardNum,ref TMcConfig pMc);
			[DllImport("gts.dll")]
			public static extern short GT_GetMcConfig(short cardNum,out TMcConfig pMc);
			[DllImport("gts.dll")]
			public static extern short GT_SetMcConfigToFile(short cardNum,ref TMcConfig pMc,ref char pFile);
			[DllImport("gts.dll")]
			public static extern short GT_GetMcConfigFromFile(short cardNum,out TMcConfig pMc,out char pFile);
			[DllImport("gts.dll")]
			public static extern short GT_SaveConfig(short cardNum,out char pFile);
			[DllImport("gts.dll")]
			public static extern short GT_GetInterruptTime(short cardNum,out double pServoRunTime,out double pProfileRunTime);
			[DllImport("gts.dll")]
			public static extern short GT_GetInterruptTimeMax(short cardNum,out double pServoRunTimeMax,out double pProfileRunTimeMax);
			
			//////////////////////////////////////////////////////////////////////////
			//New Watch
			//////////////////////////////////////////////////////////////////////////
			public const short  WATCH_MODE_STATIC = 0;
            public const short WATCH_MODE_LOOP = 1;
            public const short WATCH_MODE_DYNAMIC = 2;

            public const short WATCH_MODE_STATIC_BACKGROUND = 10;
            public const short WATCH_MODE_LOOP_BACKGROUND = 11;
            public const short WATCH_MODE_DYNAMIC_BACKGROUND = 12;

            public const short WATCH_EVENT_RUN = 1;
            public const short WATCH_EVENT_START = 10;
            public const short WATCH_EVENT_STOP = 20;
            public const short WATCH_EVENT_OFF = 30;

            public const short WATCH_CONDITION_EQ = 1;
            public const short WATCH_CONDITION_NE = 2;
            public const short WATCH_CONDITION_GE = 3;
            public const short WATCH_CONDITION_LE = 4;

            public const short WATCH_CONDITION_CHANGE_TO = 11;
            public const short WATCH_CONDITION_CHANGE = 12;
            public const short WATCH_CONDITION_UP = 13;
            public const short WATCH_CONDITION_DOWN = 14;

            public const short WATCH_CONDITION_REMAIN_AT = 21;
            public const short WATCH_CONDITION_REMAIN = 22;

            public const long WATCH_VAR_CLOCK = 1200;
            public const long WATCH_VAR_PRF_LOOP = 1201;

            public const long WATCH_VAR_COMMAND_CODE = 1220;
            public const long WATCH_VAR_COMMAND_DATA = 1221;
            public const long WATCH_VAR_COMMAND_COUNT = 1222;
            public const long WATCH_VAR_COMMAND_READ_FLAG = 1223;

            public const long WATCH_VAR_PRF_POS = 6000;
            public const long WATCH_VAR_PRF_VEL = 6001;
            public const long WATCH_VAR_PRF_ACC = 6002;

            public const long WATCH_VAR_PRF_RUN = 6200;

            public const long WATCH_VAR_CRD_PRF_POS = 8000;
            public const long WATCH_VAR_CRD_PRF_VEL = 8001;
            public const long WATCH_VAR_CRD_PRF_ACC = 8002;

            public const long WATCH_VAR_CRD_RUN = 8200;

            public const long WATCH_VAR_CRD_SEGMENT_NUMBER = 8202;
            public const long WATCH_VAR_CRD_SEGMENT_NUMBER_USER = 8203;
            public const long WATCH_VAR_CRD_COMMAND_RECEIVE = 8204;
            public const long WATCH_VAR_CRD_COMMAND_EXECUTE = 8205;
            
            public const long WATCH_VAR_CRD_FOLLOW_SLAVE_POS = 8600;
            public const long WATCH_VAR_CRD_FOLLOW_SLAVE_VEL = 8601;
            
            public const long WATCH_VAR_CRD_FOLLOW_STAGE = 8610;

            public const long WATCH_VAR_SCAN_PRF_POS = 18000;
            public const long WATCH_VAR_SCAN_PRF_VEL = 18001;
            public const long WATCH_VAR_SCAN_PRF_ACC = 18002;

            public const long WATCH_VAR_SCAN_PRF_POS_X = 18010;
            public const long WATCH_VAR_SCAN_PRF_POS_Y = 18020;

            public const long WATCH_VAR_SCAN_RUN = 18200;

            public const long WATCH_VAR_SCAN_SEGMENT_NUMBER = 18202;


            public const long WATCH_VAR_LASER_HSIO = 18600;
            public const long WATCH_VAR_LASER_POWER = 18601;

            public const long WATCH_VAR_AXIS_PRF_POS = 20000;
            public const long WATCH_VAR_AXIS_PRF_VEL = 20001;
            public const long WATCH_VAR_AXIS_PRF_ACC = 20002;
            public const long WATCH_VAR_AXIS_ENC_POS = 20003;
            
						public const long WATCH_VAR_AXIS_PRF_VEL_FILTER = 20011;

            public const long WATCH_VAR_ENC_POS = 30000;

            public const long WATCH_VAR_ENC_VEL = 30001;

            public const long WATCH_VAR_GPI = 31000;

            public const long WATCH_VAR_GPO = 32000;

            public const long WATCH_VAR_AI = 33000;

            public const long WATCH_VAR_AO = 34000;

            public const long WATCH_VAR_AUTO_FOCUS_OUT = 34006;

            public const long WATCH_VAR_TRIGGER_EXECUTE = 38000;
            public const long WATCH_VAR_TRIGGER_STATUS = 38001;
            public const long WATCH_VAR_TRIGGER_POSITION = 38002;
            public const long WATCH_VAR_TRIGGER_COUNT = 38010;

            public const long WATCH_VAR_POS_LOOP_ERROR = 40000;

            public const long WATCH_VAR_CONTROL_REF_VEL = 41000;

            public const long WATCH_VAR_WATCH_TIME = 52001;
			
						public const long WATCH_VAR_INT32 = 52020;
						public const long WATCH_VAR_INT64 = 52021;
						public const long WATCH_VAR_FLOAT = 52022;
						public const long WATCH_VAR_DOUBLE = 52023;
						public const long WATCH_VAR_BOOL = 52024;
			
			
			public struct  TWatchVar
			{
                public ushort type;
                public ushort index;
                public ushort id;
			}
			
			public struct  TWatchEvent
			{
                public ushort type;
                public ushort loop;
                public ushort watchCount;
                public TWatchVar var;
                public ushort condition;
                public double value;
			}
			
			[DllImport("gts.dll")]
			public static extern short GT_ClearWatch(short cardNum,short mode);
			[DllImport("gts.dll")]
			public static extern short GT_AddWatchVar(short cardNum,ref TWatchVar pVar);
			[DllImport("gts.dll")]
			public static extern short GT_AddWatchEvent(short cardNum,ref TWatchEvent pEvent);
			[DllImport("gts.dll")]
			public static extern short GT_WatchOn(short cardNum,short interval,short mode,ushort count);
			[DllImport("gts.dll")]
			public static extern short GT_WatchOff(short cardNum);
			[DllImport("gts.dll")]
			public static extern short GT_PrintWatch(short cardNum,out char pFileName,int start,uint printCount);
			[DllImport("gts.dll")]
			public static extern short GT_GetMcVar(short cardNum,out TWatchVar pVar,out double pValue);
			[DllImport("gts.dll")]
			public static extern short GT_SetWatchGroup(short cardNum,short group);
			[DllImport("gts.dll")]
			public static extern short GT_GetWatchGroup(short cardNum,out short pGroup);
			[DllImport("gts.dll")]
			public static extern short GT_LoadWatchConfig(short cardNum,ref  char pFile);
			[DllImport("gts.dll")]
			public static extern short GT_SaveWatchConfig(short cardNum,short group,out char pFile);
			[DllImport("gts.dll")]
			public static extern short GT_ReadWatch(short cardNum,short varIndex,out double pBuffer,uint bufferSize,out uint pReadCount);
			
			public struct TWatchInfo
			{
                public short enable;						// 采集使能
                public short run;							// 采集状态
                public uint time;					// 采集次数
                public uint head;					// 头指针
                public uint threshold;			// 最多容纳采集次数

                public short interval;						// 采集间隔
                public short mode;							// 采集模式
                public ushort countBeforeEvent;	// 事件触发之前的采集数量
                public ushort countAfterEvent;		// 事件触发以后的采集数量
                public ushort varCount;			// 采集变量数量
                public ushort eventCount;			// 采集事件数量
			}
			
			public struct TWatchVarInfo
			{
                public uint pAddress;
                public ushort length;
                public short fraction;
                public ushort format;
                public ushort hex;
                public ushort sign;
                public ushort bit;
			}
			
			public struct TWatchFormat 
			{
                public short width;
                public short precision;
                public char seperator;
                public short hex;
			}
			
			[DllImport("gts.dll")]
			public static extern short  GT_GetWatchInfo(short cardNum,out TWatchInfo pInfo);
			[DllImport("gts.dll")]
			public static extern short  GT_GetWatchVar(short cardNum,short index,out TWatchVar pVar,out TWatchVarInfo pVarInfo);
			[DllImport("gts.dll")]
			public static extern short  GT_GetWatchEvent(short cardNum,short index,out TWatchEvent pEvent);
			[DllImport("gts.dll")]
			public static extern short  GT_SetWatchFormat(short cardNum,ref TWatchFormat pFormat);
			[DllImport("gts.dll")]
			public static extern short  GT_GetWatchFormat(short cardNum,out TWatchFormat pFormat);

			//Event and Task
			public const long TASK_SET_DO_BIT = 0x1101;
			public const long TASK_CRD_START = 0x4004;
			public const long TASK_CRD_STOP = 0x4005;
			public const long TASK_CRD_OVERRIDE = 0x4006;
			
			public struct TTaskSetDoBit 
			{
                public short doType;
                public short doIndex;
                public short doValue;
                public short mode;
                public int parameter1;
                public int parameter2;
                public int parameter3;
                public int parameter4;
                public int parameter5;
                public int parameter6;
                public int parameter7;
                public int parameter8;
			}
			
			public struct TTaskCrdStart 
			{
                public short mask;
                public short option;
			}
			
			public struct TTaskCrdStop 
			{
                public short mask;
                public short option;
			}
			
			public struct TTaskCrdOverride 
			{
                public short crd;
                public double synVelOverride;
			}
			
			public struct TEvent 
			{
                public int loop;
                public TWatchVar var;
                public ushort condition;
                public double value;
			}
			[DllImport("gts.dll")]
			public static extern short GT_ClearEvent(short cardNum);
			[DllImport("gts.dll")]
			public static extern short GT_ClearTask(short cardNum);
			[DllImport("gts.dll")]
			public static extern short GT_ClearEventTaskLink(short cardNum);   
			[DllImport("gts.dll")]
			public static extern short GT_AddEvent(short cardNum,ref TEvent pEvent,ref short pEventIndex);  
			[DllImport("gts.dll")]
			public static extern short GT_AddTask(short cardNum,short taskType,System.IntPtr pTaskData,ref short pTaskIndex);
			[DllImport("gts.dll")]
			public static extern short GT_AddEventTaskLink(short cardNum,short eventIndex,short taskIndex,ref short pLinkIndex);
			[DllImport("gts.dll")]
			public static extern short GT_GetEventCount(short cardNum,out short pCount);
			[DllImport("gts.dll")]
			public static extern short GT_GetEvent(short cardNum,short eventIndex,out TEvent pEvent);
			[DllImport("gts.dll")]
			public static extern short GT_GetEventLoop(short cardNum,short eventIndex,out int pCount); 
			[DllImport("gts.dll")]
			public static extern short GT_GetTaskCount(short cardNum,out short pCount);
			[DllImport("gts.dll")]
			public static extern short GT_GetTask(short cardNum,short taskIndex,out short pTaskType,System.IntPtr pTaskData); 
			[DllImport("gts.dll")]
			public static extern short GT_GetEventTaskLinkCount(short cardNum,out short pCount);
			[DllImport("gts.dll")]
			public static extern short GT_GetEventTaskLink(short cardNum,short linkIndex,out short pEventIndex,out short pTaskIndex); 
			[DllImport("gts.dll")]
			public static extern short GT_EventOn(short cardNum,short eventIndex,short count); 
			[DllImport("gts.dll")]
			public static extern short GT_EventOff(short cardNum,short eventIndex,short count); 
			[DllImport("gts.dll")]
			public static extern short GT_BufEventOn(short cardNum,short crd,short eventIndex,short count,short fifo); 
			[DllImport("gts.dll")]
			public static extern short GT_BufEventOff(short cardNum,short crd,short eventIndex,short count,short fifo); 
			
			public const long VAR_CALCULATE_NONE = 0;
			public const long VAR_CALCULATE_OR = 1;
			public const long VAR_CALCULATE_AND = 3;
			public const long VAR_CALCULATE_NOT = 5;
			
			public const long VAR_CALCULATE_ADD = 11;
			public const long VAR_CALCULATE_SUB = 12;
			public const long VAR_CALCULATE_MUL = 13;
			public const long VAR_CALCULATE_DIV = 14;

			public struct TWatchCondition 
			{
                public TWatchVar var;
                public ushort condition;
                public double value;
			}
			
			public struct TVarCalculate 
			{
                public ushort operation;
                public ushort varType;
                public ushort result;
                public ushort lhs;
                public ushort rhs;
			}
			
			[DllImport("gts.dll")]
			public static extern short GT_ClearVar(short cardNum);
			[DllImport("gts.dll")]
			public static extern short GT_SetVarBoolCondition(short cardNum,short varIndex,ref TWatchCondition pWatchCondition); 
			[DllImport("gts.dll")]
			public static extern short GT_GetVarBoolCondition(short cardNum,short varIndex,out TWatchCondition pWatchCondition); 
			[DllImport("gts.dll")]
			public static extern short GT_AddVarCalculate(short cardNum,ref TVarCalculate pVarCalculate,ref short pIndex); 
			[DllImport("gts.dll")]
			public static extern short GT_GetVarCalculateCount(short cardNum,out short pCount); 
			[DllImport("gts.dll")]
			public static extern short GT_GetVarCalculate(short cardNum,short index,out TVarCalculate pVarCalculate); 
	}
}