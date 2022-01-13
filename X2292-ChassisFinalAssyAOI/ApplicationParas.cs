using dnCommConfig;
using dnDesktopAlerts;
using dnPW;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X2292_ChassisFinalAssyAOI
{
  public   class ApplicationParas
    {
        private dnUsers _users = new dnUsers();
        private dnDesktopAlerts.hDesktopAlerts _deskAlarms = new hDesktopAlerts();

      

        /// <summary>
        /// 用户
        /// </summary>
        public dnUsers Users
        {
            get
            {
                return this._users;
            }
        }
        /// <summary>
        /// 桌面报警
        /// </summary>
        public hDesktopAlerts DeskAlarms
        {
            get
            {
                return _deskAlarms;
            }
        }

        public Counter Line1Counter { set; get; }
        public PieChart.PieItem[] Line1PieSource { set; get; }

        public MeasureItems mFAIstation { get; set; }

        DataOperator mDataOperator = new DataOperator();
        DataOperator.DataSaveHelper mDsh = new DataOperator.DataSaveHelper();

        public  ComConfig mComConfig = new ComConfig(2);        //加载系统配置中的参数设置

      


        public void SaveFAIData()
        {
            mDsh.Shift = mComConfig.Shift;
            mDsh.Path = mComConfig.DataPath;
            mDsh.Data = mFAIstation;
            mDataOperator.SaveData(mDsh);
        }

        public ParaSetting mParaSetting = new ParaSetting();
        public ApplicationParas()
        {
            mParaSetting.Ini();

            mFAIstation = new MeasureItems();
            mFAIstation.Station = "2292";
            mFAIstation.IniFaisFromFileCsv();  
            Line1Counter = new Counter("Line1");          

            Line1PieSource = new PieChart.PieItem[3];
            Line1PieSource[0] = new PieChart.PieItem() { Name = "OK", Value = 0, PieColor = System.Drawing.Color.Green };
            Line1PieSource[1] = new PieChart.PieItem() { Name = "NG", Value = 0, PieColor = System.Drawing.Color.Red };
            Line1PieSource[2] = new PieChart.PieItem() { Name = "Err", Value = 0, PieColor = System.Drawing.Color.Blue };
        }

    }
}
