using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X2292_ChassisFinalAssyAOI
{
    [TypeConverter(typeof(BaseClassConvert<ParaSetting>))]
    public class ParaSetting : CLSConfig
    {
        //private string _imagePath = "D:\\Images\\";
        //private string _logPath = "D:\\Log\\";

        ////[DescriptionAttribute("Line1启动料号")]
        ////[CategoryAttribute("软件开启默认料号")]
        ////[EditorAttribute(typeof(PropertyGridFileItem), typeof(System.Drawing.Design.UITypeEditor))]
        ////public string Line1StartName { set; get; }

        //[DescriptionAttribute("保存所有图片")]
        //[DefaultValue(false)]
        //[CategoryAttribute("图片保存设置")]
        //[TypeConverter(typeof(MyBoolenConverter))]
        //public bool 保存所有图片 { set; get; }

        //[DescriptionAttribute("只保存不良图片")]
        //[DefaultValue(false)]
        //[CategoryAttribute("图片保存设置")]
        //[TypeConverter(typeof(MyBoolenConverter))]
        //public bool 只保存不良图片 { set; get; }

        //[DescriptionAttribute("保存最新的前N个月的图片")]
        //[CategoryAttribute("图片保存设置")]
        //public int 图片保留时间 { set; get; }

        //[DescriptionAttribute("图片保存路径")]
        //[CategoryAttribute("图片保存设置")]
        //[EditorAttribute(typeof(PropertyGridFileItem), typeof(System.Drawing.Design.UITypeEditor))]
        //public string 图片路径 { set { this._imagePath = value; } get { return this._imagePath; } }

        //[DescriptionAttribute("Log文件保存路径")]
        //[EditorAttribute(typeof(PropertyGridFileItem), typeof(System.Drawing.Design.UITypeEditor))]
        //[CategoryAttribute("Log保存设置")]
        //public string Log路径 { set { this._logPath = value; } get { return this._logPath; } }

        //[DescriptionAttribute("3#反面CCD相机")]
        //[CategoryAttribute("3#CCD1")]
        //[ReadOnly(true)]
        //[Browsable(true)]
        //public string 反面CCD1工站 { internal set; get; }

        //[DescriptionAttribute("3#1工站侧面相机1")]
        //[CategoryAttribute("3#CCD2-1")]
        //[ReadOnly(true)]
        //[Browsable(true)]
        //public string 侧面相机11工站 { internal set; get; }

        //[DescriptionAttribute("3#1工站侧面相机2")]
        //[CategoryAttribute("3#CCD2-2")]
        //[ReadOnly(true)]
        //[Browsable(true)]
        //public string 侧面相机21工站 { internal set; get; }

        //[DescriptionAttribute("3#4工站量测相机")]
        //[CategoryAttribute("3#CCD4")]
        //[ReadOnly(true)]
        //[Browsable(true)]
        //public string 量测相机4工站 { internal set; get; }




        //[DescriptionAttribute("Line1光源IP")]
        //[CategoryAttribute("光源IP")]
        //public string Line1IP { internal set; get; }

        //[DescriptionAttribute("Line2光源IP")]
        //[CategoryAttribute("光源IP")]
        //public string Line2IP { internal set; get; }



        [DescriptionAttribute("空跑模式")]
        [DefaultValue(false)]
        [CategoryAttribute("运行模式")]
        [TypeConverter(typeof(MyBoolenConverter))]
        public bool Dry_Run { set; get; }

        [DescriptionAttribute("GRR模式")]
        [DefaultValue(false)]
        [CategoryAttribute("运行模式")]
        [TypeConverter(typeof(MyBoolenConverter))]
        public bool GRR_Run { set; get; }

        [DescriptionAttribute("生产模式")]
        [DefaultValue(false)]
        [CategoryAttribute("运行模式")]
        [TypeConverter(typeof(MyBoolenConverter))]
        public bool Product_Run { set; get; }


        [DescriptionAttribute("门禁屏蔽")]
        [DefaultValue(false)]
        [CategoryAttribute("屏蔽功能")]
        [TypeConverter(typeof(MyBoolenConverter))]
        public bool Disable_SaftyDoor { set; get; }


        [DescriptionAttribute("上料机器人")]
        [DefaultValue(false)]
        [CategoryAttribute("机器人屏蔽")]
        [TypeConverter(typeof(MyBoolenConverter))]
        public bool Disable_LoadRobot { set; get; }


        [DescriptionAttribute("下料机器人")]
        [DefaultValue(false)]
        [CategoryAttribute("机器人屏蔽")]
        [TypeConverter(typeof(MyBoolenConverter))]
        public bool Disable_UpLoadRobot { set; get; }

        [DescriptionAttribute("左工位")]
        [DefaultValue(false)]
        [CategoryAttribute("气缸感应器屏蔽")]
        [TypeConverter(typeof(MyBoolenConverter))]
        public bool Disable_LeftCylinderSensor { set; get; }

        [DescriptionAttribute("右工位")]
        [DefaultValue(false)]
        [CategoryAttribute("气缸感应器屏蔽")]
        [TypeConverter(typeof(MyBoolenConverter))]
        public bool Disable_RightCylinderSensor { set; get; }

        [DescriptionAttribute("延时时间")]
        [DefaultValue(false)]
        [CategoryAttribute("气缸感应器屏蔽")]       
        public int Disable_Time_CylinderSensor { set; get; } = 5;

    }
}
