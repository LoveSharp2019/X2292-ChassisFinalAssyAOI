using dnCommConfig;
using dnPW;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace X2292_ChassisFinalAssyAOI
{
    public partial class frm_Main : Form
    {
        public int MtabControl_M_SelectedIndex = 0;
        public static ApplicationParas MyPara = new ApplicationParas();
        public static List<OutputBitInfo> OutputBitInfoS = new List<OutputBitInfo>();
        public static List<InputBitInfo> InputBitInfoS = new List<InputBitInfo>();
        public static NetWorkHelper.TCP.ITcpClient CodeReaderClient = new NetWorkHelper.TCP.ITcpClient();//开启读码客户端
       // public NetWorkHelper.TCP.ITcpServer CCD1_Server = new NetWorkHelper.TCP.ITcpServer();//开启CCD1TCP服务器
        public static MotionControl_GTS mCard1;
        public static  HslCommunication.LogNet.ILogNet logNet;

        private System.Threading.Timer TimerAxisStatus;//轴状态扫描
        string dgvName = string.Empty;

        public static event Action TrigIsStart;
        public static event Action TrigIsStop;
        public static event Action TrigIsReset;      
        public static event Action TrigIsEStop;
        public static event Action TrigSafeDoor;

        public static string SN = string.Empty;

        public bool IsCurStart { get; private set; }
        public bool IsCurStop { get; private set; }
        public bool IsCurReset { get; private set; }      
        public bool IsCurEStop { get; private set; }
        public bool IsCurSafeDoor { get; private set; }


        public frm_Main()
        {
            InitializeComponent();
            MyPara.Users.UserLogInChanged += Users_UserLogInChanged;
            MyPara.Line1Counter.CounterRefreshed += Line1Counter_CounterRefreshed;
            
            IniCard();
            Initial_Log();
            IniRecipe();
            IniNetWork();
            Laser_Station.Instance.Ini();
            Rolled_Station.Instance.Ini();

            //3D初始化
            string LMI_Result;
            bool Res = LMIControl.Instance.InitLMI("192.168.1.10", out LMI_Result);
            if (Res)
            {
                CtrDisplay.ShowText(sts_LMI, "LMI连接成功", Color.White, Color.Green);
            }  
            else
            {
                CtrDisplay.ShowText(sts_LMI, "LMI连接失败", Color.White, Color.Red);
            }         

            Res = LMIControl.Instance.Process3D_Ini();
            if (!Res) { MessageBox.Show("3D算法初始化失败"); }
        }

        private void Line1Counter_CounterRefreshed(object sender, Counter.CounterEventArgs e)
        {
            MyPara.Line1PieSource[0].Value = (int)e.Pass;

            MyPara.Line1PieSource[1].Value = (int)e.Fail0;

            MyPara.Line1PieSource[2].Value = (int)e.Err0;

            dnPieChart1.SetDataSource(MyPara.Line1PieSource);
        }

        private void Users_UserLogInChanged(object sender, dnUsers.UserLogChangedEventArgs e)
        {
          
        }

        private void frm_Main_Load(object sender, EventArgs e)
        {
            Ini_IOConfig();
            TimerAxisStatus = new System.Threading.Timer(new System.Threading.TimerCallback(AxisStatusScan), null, 0, 500);          
            timer1.Enabled = true;
            TaskManage.GetSingleton().DatasEvent += Frm_Main_DatasEvent;
            TaskManage.GetSingleton().DisplayLog += Frm_Main_DisplayLog1; ;
            TaskManage.GetSingleton().InitModels();
            MyPara.Line1Counter.IniCounter();
          

          
        }

        private void Frm_Main_DisplayLog1(string arg1, Color arg2)
        {
            CtrDisplay.AddTextToRichTextBox(rtb_Mesage, arg1, arg2, true);
        }

        private void Frm_Main_DatasEvent(MeasureItems obj)
        {
            CtrDisplay.AddRowOnDgv(dgv_data, new object[] { "1", "2","1", "2" , "1", "2" });
        }

        public void Ini_IOConfig()
        {
            string[] Lines = File.ReadAllLines("Inputs.csv", Encoding.GetEncoding("gb2312"));

            string[] Lines_Out = File.ReadAllLines("Outputs.csv", Encoding.GetEncoding("gb2312"));

            dgvOutput.AutoGenerateColumns = false;
            for (int i = 1; i < Lines_Out.Length; i++)
            {
                string[] str = Lines_Out[i].Split(',');
                OutputBitInfoS.Add(new OutputBitInfo() { Index = short.Parse(str[0]), CardNo = short.Parse(str[1]), BitNum = short.Parse(str[2]), BitPort = short.Parse(str[3]), Item = str[4] });
            }
            dgvOutput.DataSource = OutputBitInfoS;

            dgvInput.AutoGenerateColumns = false;
            for (int i = 1; i < Lines.Length; i++)
            {
                string[] str = Lines[i].Split(',');
                InputBitInfoS.Add(new InputBitInfo() { Index = short.Parse(str[0]), CardNo = short.Parse(str[1]), BitNum = short.Parse(str[2]), BitPort = short.Parse(str[3]), Item = str[4] });
            }
            dgvInput.DataSource = InputBitInfoS;
        }


        private void AxisStatusScan(object value)
        {
            switch (MtabControl_M_SelectedIndex)
            {
                case 0:
                    {
                        if (cb_X.Checked)
                        {
                            CtrDisplay.ShowText(Txt_InsepctionStation_Pos, Laser_Station.Instance.Axis_X.Pos.ToString("f3"));
                            Pic_InsepctionStation_Alarm.BackColor = Laser_Station.Instance.Axis_X.IsAlarm ? Color.Red : Color.Green;
                            Pic_InsepctionStation_Negative.BackColor = Laser_Station.Instance.Axis_X.IsLimitNegative ? Color.Red : Color.Green;
                            Pic_InsepctionStation_Positive.BackColor = Laser_Station.Instance.Axis_X.IsLimitPositive ? Color.Red : Color.Green;
                        }
                        else if (cb_Y.Checked)
                        {
                            CtrDisplay.ShowText(Txt_InsepctionStation_Pos, Laser_Station.Instance.Axis_Y.Pos.ToString("f3"));
                            Pic_InsepctionStation_Alarm.BackColor = Laser_Station.Instance.Axis_Y.IsAlarm ? Color.Red : Color.Green;
                            Pic_InsepctionStation_Negative.BackColor = Laser_Station.Instance.Axis_Y.IsLimitNegative ? Color.Red : Color.Green;
                            Pic_InsepctionStation_Positive.BackColor = Laser_Station.Instance.Axis_Y.IsLimitPositive ? Color.Red : Color.Green;
                        }
                        else if (cb_Y1.Checked)
                        {
                            CtrDisplay.ShowText(Txt_InsepctionStation_Pos, Laser_Station.Instance.Axis_Y1.Pos.ToString("f3"));
                            Pic_InsepctionStation_Alarm.BackColor = Laser_Station.Instance.Axis_Y1.IsAlarm ? Color.Red : Color.Green;
                            Pic_InsepctionStation_Negative.BackColor = Laser_Station.Instance.Axis_Y1.IsLimitNegative ? Color.Red : Color.Green;
                            Pic_InsepctionStation_Positive.BackColor = Laser_Station.Instance.Axis_Y1.IsLimitPositive ? Color.Red : Color.Green;
                        }
                        else if (cb_Y2.Checked)
                        {
                            CtrDisplay.ShowText(Txt_InsepctionStation_Pos, Laser_Station.Instance.Axis_Y2.Pos.ToString("f3"));
                            Pic_InsepctionStation_Alarm.BackColor = Laser_Station.Instance.Axis_Y2.IsAlarm ? Color.Red : Color.Green;
                            Pic_InsepctionStation_Negative.BackColor = Laser_Station.Instance.Axis_Y2.IsLimitNegative ? Color.Red : Color.Green;
                            Pic_InsepctionStation_Positive.BackColor = Laser_Station.Instance.Axis_Y2.IsLimitPositive ? Color.Red : Color.Green;
                        }
                        else { }
                    }
                    break;
                case 1:
                    {
                         if (cb_Rolled_X.Checked)
                        {
                            CtrDisplay.ShowText(Txt_RolledStation_Pos, Rolled_Station.Instance.Axis_X.Pos.ToString("f3"));
                            Pic_RolledStation_Alarm.BackColor = Rolled_Station.Instance.Axis_X.IsAlarm ? Color.Red : Color.Green;
                            Pic_RolledStation_Negative.BackColor = Rolled_Station.Instance.Axis_X.IsLimitNegative ? Color.Red : Color.Green;
                            Pic_RolledStation_Positive.BackColor = Rolled_Station.Instance.Axis_X.IsLimitPositive ? Color.Red : Color.Green;
                        }
                        else if (cb_Rolled_Y.Checked)
                        {
                            CtrDisplay.ShowText(Txt_RolledStation_Pos, Rolled_Station.Instance.Axis_Z.Pos.ToString("f3"));
                            Pic_RolledStation_Alarm.BackColor = Rolled_Station.Instance.Axis_Z.IsAlarm ? Color.Red : Color.Green;
                            Pic_RolledStation_Negative.BackColor = Rolled_Station.Instance.Axis_Z.IsLimitNegative ? Color.Red : Color.Green;
                            Pic_RolledStation_Positive.BackColor = Rolled_Station.Instance.Axis_Z.IsLimitPositive ? Color.Red : Color.Green;
                        }
                        else if (cb_Rolled_R.Checked)
                        {
                            CtrDisplay.ShowText(Txt_RolledStation_Pos, Rolled_Station.Instance.Axis_R.Pos.ToString("f3"));
                            Pic_RolledStation_Alarm.BackColor = Rolled_Station.Instance.Axis_R.IsAlarm ? Color.Red : Color.Green;
                            Pic_RolledStation_Negative.BackColor = Rolled_Station.Instance.Axis_R.IsLimitNegative ? Color.Red : Color.Green;
                            Pic_RolledStation_Positive.BackColor = Rolled_Station.Instance.Axis_R.IsLimitPositive ? Color.Red : Color.Green;
                        }
                        else { }
                    }
                    break;              
                default:
                    break;
            }

        }

        private void Instance_ChangeRecipe(string obj)
        {
            CtrDisplay.ShowText(sts_Recipe, string.Format("当前配方：{0}", obj), Color.Transparent, Color.YellowGreen);  
        }

        private void IniNetWork()
        {
            //if (CCD1_Server.IsStartListening)
            //    CCD1_Server.Stop();
            //CCD1_Server.ServerIp = "192.168.1.100";
            //CCD1_Server.ServerPort = 5000;
            //CCD1_Server.CheckTime = 2000;
            //CCD1_Server.OnRecevice += CCD1_Server_OnRecevice; ;
            //CCD1_Server.OnStateInfo += CCD1_Server_OnStateInfo; ;
            //CCD1_Server.OnOnlineClient += CCD1_Server_OnOnlineClient;
            //CCD1_Server.OnOfflineClient += CCD1_Server_OnOfflineClient;
            //CCD1_Server.Start();

            if (CodeReaderClient.IsStart)
            {
                CodeReaderClient.IsReconnection = false;
                CodeReaderClient.StopConnect();
            }
            CodeReaderClient.ServerIp = "192.168.100.100";
            CodeReaderClient.ServerPort = 9004;
            CodeReaderClient.ReConnectionTime = 2000;
            CodeReaderClient.IsReconnection = true;
            CodeReaderClient.OnRecevice += CodeReaderClient_OnRecevice;
            CodeReaderClient.OnStateInfo += CodeReaderClient_OnStateInfo;
            CodeReaderClient.StartConnect();
        }

        #region ------TCP 通信函数-------------
        NetWorkHelper.IModels.IClient ClientCCD1;
        private void CCD1_Server_OnOnlineClient(object sender, NetWorkHelper.ICommond.TcpServerClientEventArgs e)
        {
            ClientCCD1 = e.IClient;
        }
        private void CCD1_Server_OnOfflineClient(object sender, NetWorkHelper.ICommond.TcpServerClientEventArgs e)
        {

        }

        private void CCD1_Server_OnStateInfo(object sender, NetWorkHelper.ICommond.TcpServerStateEventArgs e)
        {
            string a = System.Enum.GetName(typeof(NetWorkHelper.SocketState), e.State);
            string s = e.Msg;
          //  CtrDisplay.ShowText(sts_CCD1, string.Format("CCD1状态：{0}", a));
        }

        private void CCD1_Server_OnRecevice(object sender, NetWorkHelper.ICommond.TcpServerReceviceaEventArgs e)
        {
            string s = System.Text.Encoding.UTF8.GetString(e.Data).Trim();         
        }

       

        private void CodeReaderClient_OnStateInfo(object sender, NetWorkHelper.ICommond.TcpClientStateEventArgs e)
        {
            string s = e.StateInfo;
            //string a = System.Enum.GetName(typeof(NetWorkHelper.SocketState), e.State);           
            CtrDisplay.ShowText(sts_Robot, string.Format("读码状态：{0}", s));
        }
        private void CodeReaderClient_OnRecevice(object sender, NetWorkHelper.ICommond.TcpClientReceviceEventArgs e)
        {
           SN = System.Text.Encoding.UTF8.GetString(e.Data);
            CtrDisplay.ShowText(sts_SN, "SN:" + SN);
           // RobotClient.SendData(System.Text.Encoding.UTF8.GetBytes("+"));
        }

        #endregion
        private void IniCard()
        {
            mCard1 = new MotionControl_GTS(0, 7);          
            mCard1.CardInit("GTS800_1.cfg");         
            mCard1.ExtMdlInit();//卡1扩展8个IO模块
        }

        private void IniRecipe()
        {
            Recipe.Instance.ChangeRecipe += Instance_ChangeRecipe;
            string s = Recipe.Instance.CurrentRecipe;
            Recipe.Instance.LoadRecipe(s);

            if(Recipe.Instance._Recipe_Para.DT_LeftLaser.Count>0)           
                dgv_LeftLaser.DataSource = Recipe.Instance._Recipe_Para.DT_LeftLaser;        
            else         
                dgv_LeftLaser.DataSource = null;

            if (Recipe.Instance._Recipe_Para.DT_RightLaser.Count > 0)
                dgv_RightLaser.DataSource = Recipe.Instance._Recipe_Para.DT_RightLaser;
            else
                dgv_RightLaser.DataSource = null;

            if (Recipe.Instance._Recipe_Para.DT_LeftRolled.Count > 0)
                dgv_LeftRolled.DataSource = Recipe.Instance._Recipe_Para.DT_LeftRolled;
            else
                dgv_LeftRolled.DataSource = null;

            if (Recipe.Instance._Recipe_Para.DT_RightRolled.Count > 0)
                dgv_RightRolled.DataSource = Recipe.Instance._Recipe_Para.DT_RightRolled;
            else
                dgv_RightRolled.DataSource = null;
        }



        private void contextMenuLaser_Opening(object sender, CancelEventArgs e)
        {
             dgvName = (sender as ContextMenuStrip).SourceControl.Name;         
        }

        private void 添加坐标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (dgvName)
            {
                case "dgv_LeftLaser":
                    {                       
                        int _ID = Recipe.Instance._Recipe_Para.DT_LeftLaser.Count + 1;
                        double Xpos = Laser_Station.Instance.Axis_X.Pos;
                        double Ypos = Laser_Station.Instance.Axis_Y.Pos;
                        double Y1pos = Laser_Station.Instance.Axis_Y1.Pos;
                        dgv_LeftLaser.DataSource = null;
                        Recipe.Instance._Recipe_Para.DT_LeftLaser.Add(new TablePara_LaserLeft_Item() { ID = _ID, Acc = 1, Vel = 50, XPos = Xpos, YPos = Ypos, Y1Pos = Y1pos });
                        dgv_LeftLaser.DataSource = Recipe.Instance._Recipe_Para.DT_LeftLaser;
                    }
                    break;
                case "dgv_RightLaser":
                    {
                        int _ID = Recipe.Instance._Recipe_Para.DT_RightLaser.Count + 1;
                        double Xpos = Laser_Station.Instance.Axis_X.Pos;
                        double Ypos = Laser_Station.Instance.Axis_Y.Pos;
                        double Y2pos = Laser_Station.Instance.Axis_Y2.Pos;
                        dgv_RightLaser.DataSource = null;
                        Recipe.Instance._Recipe_Para.DT_RightLaser.Add( new TablePara_LaserRight_Item() { ID = _ID, Acc = 1, Vel = 50, XPos = Xpos, YPos = Ypos, Y2Pos = Y2pos });                      
                        dgv_RightLaser.DataSource = Recipe.Instance._Recipe_Para.DT_RightLaser;
                     
                    }
                    break;

                case "dgv_LeftRolled":
                    {
                        int _ID = Recipe.Instance._Recipe_Para.DT_LeftRolled.Count + 1;
                        double Xpos = Rolled_Station.Instance.Axis_X.Pos;
                        double Zpos = Rolled_Station.Instance.Axis_Z.Pos;
                        double Rpos = Rolled_Station.Instance.Axis_R.Pos;
                        dgv_LeftRolled.DataSource = null;
                        Recipe.Instance._Recipe_Para.DT_LeftRolled.Add(new TablePara_RolledLeft_Item() { ID = _ID, Acc = 1, Vel = 50, XPos = Xpos, ZPos = Zpos, RPos = Rpos });   
                        dgv_LeftRolled.DataSource = Recipe.Instance._Recipe_Para.DT_LeftRolled;
                    }
                    break;

                case "dgv_RightRolled":
                    {
                        int _ID = Recipe.Instance._Recipe_Para.DT_RightRolled.Count + 1;
                        double Xpos = Rolled_Station.Instance.Axis_X.Pos;
                        double Zpos = Rolled_Station.Instance.Axis_Z.Pos;
                        double Rpos = Rolled_Station.Instance.Axis_R.Pos;
                        dgv_RightRolled.DataSource = null;
                        Recipe.Instance._Recipe_Para.DT_RightRolled.Add(new TablePara_RolledRight_Item() { ID = _ID, Acc = 1, Vel = 50, XPos = Xpos, ZPos = Zpos, RPos = Rpos });                      
                        dgv_RightRolled.DataSource = Recipe.Instance._Recipe_Para.DT_RightRolled;
                    }
                    break;
                default:
                    break;
            }
        }
        private void 插入ToolStripMenuItem_Click(object sender, EventArgs e)
        {          
            switch (dgvName)
            {
                case "dgv_LeftLaser":
                    {
                        int index = dgv_LeftLaser.CurrentCell.RowIndex;
                       
                        int _ID = Recipe.Instance._Recipe_Para.DT_LeftLaser.Count +1;
                        double Xpos = Laser_Station.Instance.Axis_X.Pos;
                        double Ypos = Laser_Station.Instance.Axis_Y.Pos;
                        double Y1pos = Laser_Station.Instance.Axis_Y1.Pos;                      
                        dgv_LeftLaser.DataSource = null;

                        Recipe.Instance._Recipe_Para.DT_LeftLaser.Insert(index +1, new TablePara_LaserLeft_Item() { ID = _ID, Acc = 1, Vel = 50, XPos = Xpos, YPos = Ypos, Y1Pos = Y1pos });

                        for (int i = 0; i < Recipe.Instance._Recipe_Para.DT_LeftLaser.Count; i++)
                        {
                            Recipe.Instance._Recipe_Para.DT_LeftLaser[i].ID = i + 1;
                        }                        
                        dgv_LeftLaser.DataSource = Recipe.Instance._Recipe_Para.DT_LeftLaser;
                        dgv_LeftLaser.Rows[index + 1].Selected = true;
                    }
                    break;
                case "dgv_RightLaser":
                    {
                        int index = dgv_RightLaser.CurrentCell.RowIndex;

                        int _ID = Recipe.Instance._Recipe_Para.DT_RightLaser.Count + 1;
                        double Xpos = Laser_Station.Instance.Axis_X.Pos;
                        double Ypos = Laser_Station.Instance.Axis_Y.Pos;
                        double Y2pos = Laser_Station.Instance.Axis_Y2.Pos;
                        dgv_RightLaser.DataSource = null;

                        Recipe.Instance._Recipe_Para.DT_RightLaser.Insert(index + 1, new TablePara_LaserRight_Item() { ID = _ID, Acc = 1, Vel = 50, XPos = Xpos, YPos = Ypos, Y2Pos = Y2pos });
                        for (int i = 0; i < Recipe.Instance._Recipe_Para.DT_RightLaser.Count; i++)
                        {
                            Recipe.Instance._Recipe_Para.DT_RightLaser[i].ID = i + 1;
                        }
                        dgv_RightLaser.DataSource = Recipe.Instance._Recipe_Para.DT_RightLaser;
                        dgv_RightLaser.Rows[index + 1].Selected = true;
                    }
                    break;

                case "dgv_LeftRolled":
                    {
                        int index = dgv_LeftRolled.CurrentCell.RowIndex;
                        int _ID = Recipe.Instance._Recipe_Para.DT_LeftRolled.Count + 1;
                        double Xpos = Rolled_Station.Instance.Axis_X.Pos;
                        double Zpos = Rolled_Station.Instance.Axis_Z.Pos;
                        double Rpos = Rolled_Station.Instance.Axis_R.Pos;
                        dgv_LeftRolled.DataSource = null;

                        Recipe.Instance._Recipe_Para.DT_LeftRolled.Insert(index + 1, new  TablePara_RolledLeft_Item() { ID = _ID, Acc = 1, Vel = 50, XPos = Xpos, ZPos = Zpos, RPos = Rpos });
                        for (int i = 0; i < Recipe.Instance._Recipe_Para.DT_LeftRolled.Count; i++)
                        {
                            Recipe.Instance._Recipe_Para.DT_LeftRolled[i].ID = i + 1;
                        }

                        dgv_LeftRolled.DataSource = Recipe.Instance._Recipe_Para.DT_LeftRolled;
                        dgv_LeftRolled.Rows[index + 1].Selected = true;

                    }
                    break;

                case "dgv_RightRolled":
                    {
                        int index = dgv_RightRolled.CurrentCell.RowIndex;
                        int _ID = Recipe.Instance._Recipe_Para.DT_RightRolled.Count + 1;
                        double Xpos = Rolled_Station.Instance.Axis_X.Pos;
                        double Zpos = Rolled_Station.Instance.Axis_Z.Pos;
                        double Rpos = Rolled_Station.Instance.Axis_R.Pos;
                        dgv_RightRolled.DataSource = null;

                        Recipe.Instance._Recipe_Para.DT_RightRolled.Insert(index + 1, new  TablePara_RolledRight_Item() { ID = _ID, Acc = 1, Vel = 50, XPos = Xpos, ZPos = Zpos, RPos = Rpos });
                        for (int i = 0; i < Recipe.Instance._Recipe_Para.DT_RightRolled.Count; i++)
                        {
                            Recipe.Instance._Recipe_Para.DT_RightRolled[i].ID = i + 1;
                        }

                        dgv_RightRolled.DataSource = Recipe.Instance._Recipe_Para.DT_RightRolled;
                        dgv_RightRolled.Rows[index + 1].Selected = true;

                    }
                    break;
                default:
                    break;
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (dgvName)
            {
                case "dgv_LeftLaser":
                    {
                        int index = dgv_LeftLaser.CurrentCell.RowIndex;
                        dgv_LeftLaser.DataSource = null;
                        Recipe.Instance._Recipe_Para.DT_LeftLaser.RemoveAt(index);

                        foreach (var item in Recipe.Instance._Recipe_Para.DT_LeftLaser)
                        {
                            if(item.ID>=index+1)
                            {
                                Recipe.Instance._Recipe_Para.DT_LeftLaser.ToList()[item.ID - 2].ID = item.ID - 1;
                            }
                        }
                        dgv_LeftLaser.DataSource = Recipe.Instance._Recipe_Para.DT_LeftLaser;
                     
                    }
                    break;
                case "dgv_RightLaser":
                    {
                        int index = dgv_RightLaser.CurrentCell.RowIndex;
                        dgv_RightLaser.DataSource = null;
                        Recipe.Instance._Recipe_Para.DT_RightLaser.RemoveAt(index);
                        foreach (var item in Recipe.Instance._Recipe_Para.DT_RightLaser)
                        {
                            if (item.ID >= index + 1)
                            {
                                Recipe.Instance._Recipe_Para.DT_RightLaser.ToList()[item.ID - 2].ID = item.ID - 1;
                            }
                        }
                        dgv_RightLaser.DataSource = Recipe.Instance._Recipe_Para.DT_RightLaser;
                    }
                    break;

                case "dgv_LeftRolled":
                    {
                        int index = dgv_LeftRolled.CurrentCell.RowIndex;
                        dgv_LeftRolled.DataSource = null;
                        Recipe.Instance._Recipe_Para.DT_LeftRolled.RemoveAt(index);
                        foreach (var item in Recipe.Instance._Recipe_Para.DT_LeftRolled)
                        {
                            if (item.ID >= index + 1)
                            {
                                Recipe.Instance._Recipe_Para.DT_LeftRolled.ToList()[item.ID - 2].ID = item.ID - 1;
                            }
                        }
                        dgv_LeftRolled.DataSource = Recipe.Instance._Recipe_Para.DT_LeftRolled;
                    }
                    break;

                case "dgv_RightRolled":
                    {
                        int index = dgv_RightRolled.CurrentCell.RowIndex;
                        dgv_RightRolled.DataSource = null;
                        Recipe.Instance._Recipe_Para.DT_RightRolled.RemoveAt(index);
                        foreach (var item in Recipe.Instance._Recipe_Para.DT_RightRolled)
                        {
                            if (item.ID >= index + 1)
                            {
                                Recipe.Instance._Recipe_Para.DT_RightRolled.ToList()[item.ID - 2].ID = item.ID - 1;
                            }
                        }
                        dgv_RightRolled.DataSource = Recipe.Instance._Recipe_Para.DT_RightRolled;
                    }
                    break;
                default:
                    break;
            }
        }

        private void 修改坐标ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            switch (dgvName)
            {
                case "dgv_LeftLaser":
                    {
                        int index = dgv_LeftLaser.CurrentCell.RowIndex;
                        dgv_LeftLaser.DataSource = null;
                        Recipe.Instance._Recipe_Para.DT_LeftLaser.ToList()[index].XPos = Laser_Station.Instance.Axis_X.Pos;
                        Recipe.Instance._Recipe_Para.DT_LeftLaser.ToList()[index].YPos = Laser_Station.Instance.Axis_Y.Pos;
                        Recipe.Instance._Recipe_Para.DT_LeftLaser.ToList()[index].Y1Pos = Laser_Station.Instance.Axis_Y1.Pos;
                        dgv_LeftLaser.DataSource = Recipe.Instance._Recipe_Para.DT_LeftLaser;
                    }
                    break;
                case "dgv_RightLaser":
                    {
                        int index = dgv_RightLaser.CurrentCell.RowIndex;
                        dgv_RightLaser.DataSource = null;
                        Recipe.Instance._Recipe_Para.DT_RightLaser.ToList()[index].XPos = Laser_Station.Instance.Axis_X.Pos;
                        Recipe.Instance._Recipe_Para.DT_RightLaser.ToList()[index].YPos = Laser_Station.Instance.Axis_Y.Pos;
                        Recipe.Instance._Recipe_Para.DT_RightLaser.ToList()[index].Y2Pos = Laser_Station.Instance.Axis_Y2.Pos;
                        dgv_RightLaser.DataSource = Recipe.Instance._Recipe_Para.DT_RightLaser;
                    }
                    break;

                case "dgv_LeftRolled":
                    {
                        int index = dgv_LeftRolled.CurrentCell.RowIndex;
                        dgv_LeftRolled.DataSource = null;
                        Recipe.Instance._Recipe_Para.DT_LeftRolled.ToList()[index].XPos = Rolled_Station.Instance.Axis_X.Pos;
                        Recipe.Instance._Recipe_Para.DT_LeftRolled.ToList()[index].ZPos = Rolled_Station.Instance.Axis_Z.Pos;
                        Recipe.Instance._Recipe_Para.DT_LeftRolled.ToList()[index].RPos = Rolled_Station.Instance.Axis_R.Pos;
                        dgv_LeftRolled.DataSource = Recipe.Instance._Recipe_Para.DT_LeftRolled;
                    }
                    break;

                case "dgv_RightRolled":
                    {
                        int index = dgv_RightRolled.CurrentCell.RowIndex;
                        dgv_RightRolled.DataSource = null;
                        Recipe.Instance._Recipe_Para.DT_RightRolled.ToList()[index].XPos = Rolled_Station.Instance.Axis_X.Pos;
                        Recipe.Instance._Recipe_Para.DT_RightRolled.ToList()[index].ZPos = Rolled_Station.Instance.Axis_Z.Pos;
                        Recipe.Instance._Recipe_Para.DT_RightRolled.ToList()[index].RPos = Rolled_Station.Instance.Axis_R.Pos;
                        dgv_RightRolled.DataSource = Recipe.Instance._Recipe_Para.DT_RightRolled;
                    }
                    break;
                default:
                    break;
            }
        }

        private void 运动到此位置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (dgvName)
            {
                case "dgv_LeftLaser":
                    {                       
                        Task.Run(() =>
                        {
                            Laser_Station.Instance.LeftStationMoveTo(dgv_LeftLaser.CurrentCell.RowIndex,false,false,true);
                        });
                    }
                    break;
                case "dgv_RightLaser":
                    {
                        Task.Run(() =>
                        {
                            Laser_Station.Instance.RightStationMoveTo(dgv_RightLaser.CurrentCell.RowIndex, false, false, true);
                        });
                    }
                    break;

                case "dgv_LeftRolled":
                    {
                        Task.Run(() =>
                        {
                            Rolled_Station.Instance.LeftStationMoveTo(dgv_LeftRolled.CurrentCell.RowIndex);
                        });
                    }
                    break;

                case "dgv_RightRolled":
                    {
                        Task.Run(() =>
                        {
                            Rolled_Station.Instance.RightStationMoveTo(dgv_RightRolled.CurrentCell.RowIndex);
                        });
                    }
                    break;
                default:
                    break;
            }
        }

        private void 保存配方ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog() { InitialDirectory = Application.StartupPath + "\\Recipe", RestoreDirectory = true, Filter = "程序文件(*.dtg)|*.dtg" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileNameExt = sfd.FileName.Substring(sfd.FileName.LastIndexOf("\\") + 1);
                Recipe.Instance.SaveRecipe(fileNameExt);
            }
        }

        private void 加载配方ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog() { InitialDirectory = Application.StartupPath + "\\Recipe", RestoreDirectory = true, Filter = "程序文件(*.dtg)|*.dtg" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fileNameExt = ofd.FileName.Substring(ofd.FileName.LastIndexOf("\\") + 1);
                Recipe.Instance.LoadRecipe(fileNameExt);
            }
            dgv_LeftLaser.DataSource = null;
            dgv_RightLaser.DataSource = null;
            dgv_LeftLaser.DataSource = Recipe.Instance._Recipe_Para.DT_LeftLaser;
            dgv_RightLaser.DataSource = Recipe.Instance._Recipe_Para.DT_RightLaser;
          
        }

      

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskManage.GetSingleton().RaiseDisplayLog("载具2左前侧推-出超时", Color.Red);
            //TaskManage.GetSingleton().StartWork();
        }

        private void test1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TaskManage.GetSingleton().StopWork();
        }

        private void dgvOutput_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex != -1)
            {
                if (dgvOutput.Rows[e.RowIndex].Cells[0].EditedFormattedValue.ToString() == "True")
                {
                    if(OutputBitInfoS[e.RowIndex].BitPort == -1)
                    {
                        short sRtn = 0;
                        sRtn += gts.mc.GT_SetDoBit((short)OutputBitInfoS[e.RowIndex].CardNo, gts.mc.MC_GPO, (short)OutputBitInfoS[e.RowIndex].BitNum, 0);                      
                    }
                    else
                    {
                        short sRtn = 0;
                        ushort pStatus;                      
                        sRtn = gts.mc.GT_GetStsExtMdl((short)OutputBitInfoS[e.RowIndex].CardNo, (short)OutputBitInfoS[e.RowIndex].BitPort, 0, out pStatus);
                        if (pStatus != 0)
                        {
                            System.Windows.Forms.MessageBox.Show("扩展IO通信异常", "出错：", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);                          
                        }
                        sRtn = gts.mc.GT_SetExtIoBit((short)OutputBitInfoS[e.RowIndex].CardNo, (short)OutputBitInfoS[e.RowIndex].BitPort, (short)OutputBitInfoS[e.RowIndex].BitNum, 0);                     
                    }
                    OutputBitInfoS[e.RowIndex].Status = IOState.ON;
                    dgvOutput.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.Red;
                }
                else
                {
                    if (OutputBitInfoS[e.RowIndex].BitPort == -1)
                    {
                        short sRtn = 0;
                        sRtn += gts.mc.GT_SetDoBit((short)OutputBitInfoS[e.RowIndex].CardNo, gts.mc.MC_GPO, (short)OutputBitInfoS[e.RowIndex].BitNum, 1);
                    }
                    else
                    {
                        short sRtn = 0;
                        ushort pStatus;
                        sRtn = gts.mc.GT_GetStsExtMdl((short)OutputBitInfoS[e.RowIndex].CardNo, (short)OutputBitInfoS[e.RowIndex].BitPort, 0, out pStatus);
                        if (pStatus != 0)
                        {
                            System.Windows.Forms.MessageBox.Show("扩展IO通信异常", "出错：", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        }
                        sRtn = gts.mc.GT_SetExtIoBit((short)OutputBitInfoS[e.RowIndex].CardNo, (short)OutputBitInfoS[e.RowIndex].BitPort, (short)OutputBitInfoS[e.RowIndex].BitNum, 1);
                    }
                    OutputBitInfoS[e.RowIndex].Status = IOState.OFF;
                    dgvOutput.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.White;
                }
            }
            dgvOutput.Invalidate();
        }

        private void dgvOutput_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //DataGridView dgv = (DataGridView)sender;
            //if (dgv.Columns[e.ColumnIndex].HeaderText == "Status")
            //{
            //    if (e.RowIndex >= 0 && e.RowIndex <= dgv.RowCount)
            //    {
            //        if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "ON")
            //        {
            //            e.CellStyle.BackColor = Color.Red;
            //            if (dgv.Tag.ToString() == "Output")
            //            {
            //                DataGridViewCheckBoxCell checkBox = dgv.Rows[e.RowIndex].Cells[0] as DataGridViewCheckBoxCell;
            //                checkBox.Value = 1;
            //            }
            //        }
            //        else
            //        {
            //            e.CellStyle.BackColor = Color.White;
            //            if (dgv.Tag.ToString() == "Output")
            //            {
            //                DataGridViewCheckBoxCell checkBox = dgv.Rows[e.RowIndex].Cells[0] as DataGridViewCheckBoxCell;
            //                checkBox.Value = 0;
            //            }
            //        }
            //    }
            //}
            //e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
            //e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
        }

        private void ScanRisingIO()
        {
            //扫描启动上升沿
            if (IsCurStart == false && MotionControl_GTS.Input(frm_Main.InputBitInfoS, 0) == 1)
            {
                TrigIsStart?.BeginInvoke(null, null);
            }
            IsCurStart = MotionControl_GTS.Input(frm_Main.InputBitInfoS, 0) == 1;

            //扫描停止上升沿
            if (IsCurStop == false && MotionControl_GTS.Input(frm_Main.InputBitInfoS, 1) == 1)
            {
                TrigIsStop?.BeginInvoke(null, null);
            }
            IsCurStop = MotionControl_GTS.Input(frm_Main.InputBitInfoS, 1) == 1;

            //扫描复位上升沿
            if (IsCurReset == false && MotionControl_GTS.Input(frm_Main.InputBitInfoS, 2) == 1)
            {
                TrigIsReset?.BeginInvoke(null, null);
            }
            IsCurReset = MotionControl_GTS.Input(frm_Main.InputBitInfoS, 2) == 1;

            //扫描急停上升沿
            if (IsCurEStop == false && MotionControl_GTS.Input(frm_Main.InputBitInfoS, 3) == 1)
            {
                TrigIsEStop?.BeginInvoke(null, null);
            }
            IsCurEStop = MotionControl_GTS.Input(frm_Main.InputBitInfoS, 3) == 1;

            //扫描安全门上升沿
            if (IsCurSafeDoor == false && MotionControl_GTS.Input(frm_Main.InputBitInfoS, 4) == 1)
            {
                TrigSafeDoor?.BeginInvoke(null, null);
            }
            IsCurSafeDoor = MotionControl_GTS.Input(frm_Main.InputBitInfoS, 2) == 1;
        }
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            //扫描上升沿IO
            ScanRisingIO();

            //IO扫描状态
            if (MtabControl_M_SelectedIndex ==2)
            {
                foreach (var item in InputBitInfoS)
                {
                    if (item.BitPort == -1)
                    {
                        short sRtn;
                        int iValue;
                        sRtn = gts.mc.GT_GetDi(item.CardNo, gts.mc.MC_GPI, out iValue);
                        if ((1 << (item.BitNum) & iValue) == 0)
                        {
                            item.Status = IOState.ON;
                            dgvInput.Rows[item.Index - 1].Cells[1].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            item.Status = IOState.OFF;
                            dgvInput.Rows[item.Index - 1].Cells[1].Style.BackColor = Color.White;
                        }
                    }
                    else
                    {
                        ushort iValue;
                        gts.mc.GT_GetExtIoBit(item.CardNo, item.BitPort, item.BitNum, out iValue);
                        if (iValue == 0)
                        {
                            item.Status = IOState.ON;
                            dgvInput.Rows[item.Index - 1].Cells[1].Style.BackColor = Color.Red;
                        }                           
                        else
                        {
                            item.Status = IOState.OFF;
                            dgvInput.Rows[item.Index - 1].Cells[1].Style.BackColor = Color.White;
                        }
                    }
                }
            }
           
        }

        private void 存储设置toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            frmComConfig ComConfig = new frmComConfig(MyPara.mComConfig);
            ComConfig.ShowDialog();
        }

        private void 用户登录toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MyPara.Users.LoginDialog();
        }



        private void btn_InsepctionStation_Left_Click(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            if(bt.Text.Contains('+'))
            {
                if (rb_InsepctionStation_Step.Checked)
                {
                    switch (MtabControl_M_SelectedIndex)
                    {
                        case 0:
                            {
                                if (cb_X.Checked)//检测站
                                {
                                    Laser_Station.Instance.MoveTo(Laser_Station.Instance.Axis_X, (double)InsepctionStation_Speed.Value, Laser_Station.Instance.Axis_X.Pos + (double)InsepctionStation_Step.Value);
                                }
                                else if (cb_Y.Checked)
                                {
                                    Laser_Station.Instance.MoveTo(Laser_Station.Instance.Axis_Y, (double)InsepctionStation_Speed.Value, Laser_Station.Instance.Axis_Y.Pos + (double)InsepctionStation_Step.Value);
                                }
                                else if (cb_Y1.Checked)
                                {
                                    Laser_Station.Instance.MoveTo(Laser_Station.Instance.Axis_Y1, (double)InsepctionStation_Speed.Value, Laser_Station.Instance.Axis_Y1.Pos + (double)InsepctionStation_Step.Value);
                                }
                                else if (cb_Y2.Checked)
                                {
                                    Laser_Station.Instance.MoveTo(Laser_Station.Instance.Axis_Y2, (double)InsepctionStation_Speed.Value, Laser_Station.Instance.Axis_Y2.Pos + (double)InsepctionStation_Step.Value);
                                }
                                else { }
                            }
                            break;
                        case 1:
                            {
                                if (cb_Rolled_X.Checked)//翻转站
                                {
                                    Rolled_Station.Instance.MoveTo(Rolled_Station.Instance.Axis_X, (double)RolledStation_Speed.Value, Rolled_Station.Instance.Axis_X.Pos + (double)RolledStation_Step.Value);
                                }
                                else if (cb_Rolled_Y.Checked)
                                {
                                    Rolled_Station.Instance.MoveTo(Rolled_Station.Instance.Axis_Z, (double)RolledStation_Speed.Value, Rolled_Station.Instance.Axis_Z.Pos + (double)RolledStation_Step.Value);
                                }
                                else if (cb_Rolled_R.Checked)
                                {
                                    Rolled_Station.Instance.MoveTo(Rolled_Station.Instance.Axis_R, (double)RolledStation_Speed.Value, Rolled_Station.Instance.Axis_R.Pos + (double)RolledStation_Step.Value);
                                }
                                else { }
                            }
                            break;                     
                        default:
                            break;
                    }
                }
            }
            else
            {
                if (rb_InsepctionStation_Step.Checked)
                {
                    switch (MtabControl_M_SelectedIndex)
                    {
                        case 0:
                            {
                                if (cb_X.Checked)
                                {
                                    Laser_Station.Instance.MoveTo(Laser_Station.Instance.Axis_X, (double)InsepctionStation_Speed.Value, Laser_Station.Instance.Axis_X.Pos - (double)InsepctionStation_Step.Value);
                                }
                                else if (cb_Y.Checked)
                                {
                                    Laser_Station.Instance.MoveTo(Laser_Station.Instance.Axis_Y, (double)InsepctionStation_Speed.Value, Laser_Station.Instance.Axis_Y.Pos - (double)InsepctionStation_Step.Value);
                                }
                                else if (cb_Y1.Checked)
                                {
                                    Laser_Station.Instance.MoveTo(Laser_Station.Instance.Axis_Y1, (double)InsepctionStation_Speed.Value, Laser_Station.Instance.Axis_Y1.Pos - (double)InsepctionStation_Step.Value);
                                }
                                else if (cb_Y2.Checked)
                                {
                                    Laser_Station.Instance.MoveTo(Laser_Station.Instance.Axis_Y2, (double)InsepctionStation_Speed.Value, Laser_Station.Instance.Axis_Y2.Pos - (double)InsepctionStation_Step.Value);
                                }
                                else { }
                            }
                            break;
                        case 1:
                            {
                                if (cb_Rolled_X.Checked)//翻转站
                                {
                                    Rolled_Station.Instance.MoveTo(Rolled_Station.Instance.Axis_X, (double)RolledStation_Speed.Value, Rolled_Station.Instance.Axis_X.Pos - (double)RolledStation_Step.Value);
                                }
                                else if (cb_Rolled_Y.Checked)
                                {
                                    Rolled_Station.Instance.MoveTo(Rolled_Station.Instance.Axis_Z, (double)RolledStation_Speed.Value, Rolled_Station.Instance.Axis_Z.Pos - (double)RolledStation_Step.Value);
                                }
                                else if (cb_Rolled_R.Checked)
                                {
                                    Rolled_Station.Instance.MoveTo(Rolled_Station.Instance.Axis_R, (double)RolledStation_Speed.Value, Rolled_Station.Instance.Axis_R.Pos - (double)RolledStation_Step.Value);
                                }
                                else { }
                            }
                            break;                   
                        default:
                            break;
                    }              

                }
            }

          
        }

        private void btn_InsepctionStation_Left_MouseDown(object sender, MouseEventArgs e)
        {
            Button bt = sender as Button;
            if (bt.Text.Contains('+'))
            {
                if (rb_InsepctionStation_Jog.Checked)
                {
                    switch (MtabControl_M_SelectedIndex)
                    {
                        case 0:
                            {
                                if (cb_X.Checked)
                                {
                                    Laser_Station.Instance.JogStart(Laser_Station.Instance.Axis_X, (double)InsepctionStation_Speed.Value, true);
                                }
                                else if (cb_Y.Checked)
                                {
                                    Laser_Station.Instance.JogStart(Laser_Station.Instance.Axis_Y, (double)InsepctionStation_Speed.Value, true);
                                }
                                else if (cb_Y1.Checked)
                                {
                                    Laser_Station.Instance.JogStart(Laser_Station.Instance.Axis_Y1, (double)InsepctionStation_Speed.Value, true);
                                }
                                else if (cb_Y2.Checked)
                                {
                                    Laser_Station.Instance.JogStart(Laser_Station.Instance.Axis_Y2, (double)InsepctionStation_Speed.Value, true);
                                }
                                else { }
                            }
                            break;
                        case 1:
                            {
                                if (cb_Rolled_X.Checked)//翻转站
                                {
                                    Rolled_Station.Instance.JogStart(Rolled_Station.Instance.Axis_X, (double)RolledStation_Speed.Value, true);
                                }
                                else if (cb_Rolled_Y.Checked)
                                {
                                    Rolled_Station.Instance.JogStart(Rolled_Station.Instance.Axis_Z, (double)RolledStation_Speed.Value, true);
                                }
                                else if (cb_Rolled_R.Checked)
                                {
                                    Rolled_Station.Instance.JogStart(Rolled_Station.Instance.Axis_R, (double)RolledStation_Speed.Value, true);
                                }
                                else { }
                            }
                            break;                
                        default:
                            break;
                    }
                   
                   
                   
                }
            }
            else
            {
                if (rb_InsepctionStation_Jog.Checked)
                {
                    switch (MtabControl_M_SelectedIndex)
                    {
                        case 0:
                            {
                                if (cb_X.Checked)
                                {
                                    Laser_Station.Instance.JogStart(Laser_Station.Instance.Axis_X, (double)InsepctionStation_Speed.Value, false);
                                }
                                else if (cb_Y.Checked)
                                {
                                    Laser_Station.Instance.JogStart(Laser_Station.Instance.Axis_Y, (double)InsepctionStation_Speed.Value, false);
                                }
                                else if (cb_Y1.Checked)
                                {
                                    Laser_Station.Instance.JogStart(Laser_Station.Instance.Axis_Y1, (double)InsepctionStation_Speed.Value, false);
                                }
                                else if (cb_Y2.Checked)
                                {
                                    Laser_Station.Instance.JogStart(Laser_Station.Instance.Axis_Y2, (double)InsepctionStation_Speed.Value, false);
                                }
                                else { }
                            }
                            break;
                        case 1:
                            {
                                if (cb_Rolled_X.Checked)//翻转站
                                {
                                    Rolled_Station.Instance.JogStart(Rolled_Station.Instance.Axis_X, (double)RolledStation_Speed.Value, false);
                                }
                                else if (cb_Rolled_Y.Checked)
                                {
                                    Rolled_Station.Instance.JogStart(Rolled_Station.Instance.Axis_Z, (double)RolledStation_Speed.Value, false);
                                }
                                else if (cb_Rolled_R.Checked)
                                {
                                    Rolled_Station.Instance.JogStart(Rolled_Station.Instance.Axis_R, (double)RolledStation_Speed.Value, false);
                                }
                                else { }
                            }
                            break;                   
                        default:
                            break;
                    }
                  
                }
            }
             
        }

        private void btn_InsepctionStation_Left_MouseUp(object sender, MouseEventArgs e)
        {           
            if (rb_InsepctionStation_Jog.Checked)
            {
                switch (MtabControl_M_SelectedIndex)
                {
                    case 0:
                        {
                            if (cb_X.Checked)
                            {
                                Laser_Station.Instance.JogStop(Laser_Station.Instance.Axis_X);
                            }
                            else if (cb_Y.Checked)
                            {
                                Laser_Station.Instance.JogStop(Laser_Station.Instance.Axis_Y);
                            }
                            else if (cb_Y1.Checked)
                            {
                                Laser_Station.Instance.JogStop(Laser_Station.Instance.Axis_Y1);
                            }
                            else if (cb_Y2.Checked)
                            {
                                Laser_Station.Instance.JogStop(Laser_Station.Instance.Axis_Y2);
                            }
                            else { }
                        }
                        break;
                    case 1:
                        {
                             if (cb_Rolled_X.Checked)//翻转站
                            {
                                Rolled_Station.Instance.JogStop(Rolled_Station.Instance.Axis_X);
                            }
                            else if (cb_Rolled_Y.Checked)
                            {
                                Rolled_Station.Instance.JogStop(Rolled_Station.Instance.Axis_Z);
                            }
                            else if (cb_Rolled_R.Checked)
                            {
                                Rolled_Station.Instance.JogStop(Rolled_Station.Instance.Axis_R);
                            }
                            else { }
                        }
                        break;               
                    default:
                        break;
                }
              
               
              
            }
        }

        private void btn_InsepctionStationHome_Click(object sender, EventArgs e)
        {
            switch (MtabControl_M_SelectedIndex)
            {
                case 0:
                    {
                        if (cb_X.Checked)
                        {
                            Task.Factory.StartNew(() => {
                                Laser_Station.Instance.Home(Laser_Station.Instance.Axis_X);
                            });
                        }
                        else if (cb_Y.Checked)
                        {
                            Task.Factory.StartNew(() => {
                                Laser_Station.Instance.Home(Laser_Station.Instance.Axis_Y);
                            });
                        }
                        else if (cb_Y1.Checked)
                        {
                            Task.Factory.StartNew(() => {
                                Laser_Station.Instance.Home(Laser_Station.Instance.Axis_Y1);
                            });
                        }
                        else if (cb_Y2.Checked)
                        {
                            Task.Factory.StartNew(() => {
                                Laser_Station.Instance.Home(Laser_Station.Instance.Axis_Y2);
                            });
                        }
                        else { }
                    }
                    break;
                case 1:
                    {
                         if (cb_Rolled_X.Checked)//翻转站
                        {
                            Task.Factory.StartNew(() =>
                            {
                                Rolled_Station.Instance.Home(Rolled_Station.Instance.Axis_X);
                            });
                        }
                        else if (cb_Rolled_Y.Checked)
                        {
                            Task.Factory.StartNew(() =>
                            {
                                Rolled_Station.Instance.Home(Rolled_Station.Instance.Axis_Z);
                            });
                        }
                        else if (cb_Rolled_R.Checked)
                        {
                            Task.Factory.StartNew(() =>
                            {
                                Rolled_Station.Instance.HomeR(Rolled_Station.Instance.Axis_R);
                            });
                        }
                        else { }
                    }
                    break;           
                default:
                    break;
            }
           

        }

        private void Initial_Log()
        {
            logNet = new HslCommunication.LogNet.LogNetDateTime(Application.StartupPath + "\\Logs", HslCommunication.LogNet.GenerateMode.ByEveryDay);
            logNet.SetMessageDegree(HslCommunication.LogNet.HslMessageDegree.INFO);
            logNet.WriteInfo("程序初始化....");
        }

        private void 参数设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_ParaSetting _ParaSetting = new frm_ParaSetting(MyPara);
            _ParaSetting.ShowDialog();
        }

        private void frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            mCard1.CardClose();
            LMIControl.Instance.DisconnectLMI();
            TaskManage.GetSingleton().StopWork();           
        }

        private void tabControl_M_SelectedIndexChanged(object sender, EventArgs e)
        {
            MtabControl_M_SelectedIndex = tabControl_M.SelectedIndex;            
        }

        private void btn_InsepctionStation_ClearAlarm_Click(object sender, EventArgs e)
        {
            MotionControl_GTS.ClearAlarm();
        }

        private void btn_RolledStation_ClearAlarm_Click(object sender, EventArgs e)
        {
            MotionControl_GTS.ClearAlarm();
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            TaskManage.GetSingleton().ResetProcessAll();
        }

        private void btn_LeftStart_Click(object sender, EventArgs e)
        {
            TaskManage.GetSingleton().LeftStart = true;
        }

        private void btn_RightStart_Click(object sender, EventArgs e)
        {
            TaskManage.GetSingleton().RightStart = true;
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            TaskManage.GetSingleton().StopWork();
            CtrDisplay.AddTextToRichTextBox(rtb_Mesage, "设备停止", Color.Red, true);
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            TaskManage.GetSingleton().StartWork();
            CtrDisplay.AddTextToRichTextBox(rtb_Mesage, "设备运行开始", Color.Green, true);
        }

        private void dTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(()=> {
                Laser_Station.Instance.LeftStationMoveTo(3, false, false, true);
                LMIControl.Instance.StartLMI();
                Laser_Station.Instance.LeftStationMoveTo(4, false, false, true);
                HalconDotNet.HObject obj =  LMIControl.Instance.StopLMI();
            });
        }

        private void 读码测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {           
            CodeReaderClient.SendData(System.Text.Encoding.UTF8.GetBytes("LON"+"\r\n"));
        }

        private void 读码测试关ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeReaderClient.SendData(System.Text.Encoding.UTF8.GetBytes("LOFF" + "\r\n"));
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                Laser_Station.Instance.RightStatonFixtureRBCylinder(true);
            else
                Laser_Station.Instance.RightStatonFixtureRBCylinder(false);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                Laser_Station.Instance.RightStatonFixtureLFCylinder(true);
            else
                Laser_Station.Instance.RightStatonFixtureLFCylinder(false);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
                Laser_Station.Instance.RightStationVacummOn(true);
            else
                Laser_Station.Instance.RightStationVacummOn(false);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
                Laser_Station.Instance.LeftStatonFixtureRBCylinder(true);
            else
                Laser_Station.Instance.LeftStatonFixtureRBCylinder(false);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
                Laser_Station.Instance.LeftStatonFixtureLFCylinder(true);
            else
                Laser_Station.Instance.LeftStatonFixtureLFCylinder(false);
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
                Laser_Station.Instance.LeftStationVacummOn(true);
            else
                Laser_Station.Instance.LeftStationVacummOn(false);
        }
    }
}
