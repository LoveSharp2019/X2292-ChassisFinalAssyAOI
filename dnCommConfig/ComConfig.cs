using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace dnCommConfig
{
    //[TypeConverterAttribute(typeof(PropertyDisplayConverterr<ComConfig>))]
    public class ComConfig
    {
        private dnShift _shift;

        private string _iamgePath_2D = "E:\\2DIamges";

        private string _iamgePath_3D = "E:\\3DIamges";

        private string _dataPath = "E:\\Data";

        private int _imageMaxMonth = 3;

        private string configName = Application.StartupPath + "\\Config.ini";

        private const string sectionName = "CommConfig";

        public delegate void baoguangdelegateHnadler();
        public event baoguangdelegateHnadler baoguang;           //料号名称切换

        public ComConfig(int shiftCount)
        {
            _shift = new dnShift(shiftCount);
            Ini();
            //this.shiftCount = shiftCount;
        }

        public int _真空吸延时 { set; get; }

        public int _曝光时间 { set; get; }

        /// <summary>
        /// 是否保存所有图片
        /// </summary>
        public bool Enable_Image { set; get; }

        /// <summary>
        /// 是否保存不合格产品图片
        /// </summary>
        public bool Enable_NGImage { set; get; }

        /// <summary>
        /// 是否保存不合格二维码图片
        /// </summary>
        public bool Enable_NGCode { set; get; }

        /// <summary>
        /// 是否保存合格产品图片
        /// </summary>
        public bool Enable_OKImage { set; get; }

        /// <summary>
        /// NG料是否蜂鸣
        /// </summary>
        public bool Enable_Hummer { set; get; }

        /// <summary>
        /// NG料是否自动流出
        /// </summary>
        public bool Enable_NGAuto { set; get; }

        /// <summary>
        /// NG料是否手动取出
        /// </summary>
        public bool Enable_NGHand { set; get; }

        /// <summary>
        /// Mes是否上传
        /// </summary>
        public bool Enable_Mes { set; get; }
 
        /// <summary>
        /// 合格产品的图片保存路径_2D
        /// </summary>
        public string OKImagePath_2D
        {
            get
            {
                return string.Format("{0}\\合格\\", _iamgePath_2D);
            }
        }

        /// <summary>
        /// 不合格产品的图片保存路径_2D
        /// </summary>
        public string NGImagePath_2D
        {
            get
            {
                return string.Format("{0}\\不合格\\", _iamgePath_2D);
            }
        }

        /// <summary>
        /// 合格产品的图片保存路径_3D
        /// </summary>
        public string OKImagePath_3D
        {
            get
            {
                return string.Format("{0}\\合格\\", _iamgePath_3D);
            }
        }

        /// <summary>
        /// 不合格产品的图片保存路径_3D
        /// </summary>
        public string NGImagePath_3D
        {
            get
            {
                return string.Format("{0}\\不合格\\", _iamgePath_3D);
            }
        }

        /// <summary>
        /// 2D图片保存的总路径
        /// </summary>
        public string ImagePath_2D
        {
            set
            {
                this._iamgePath_2D = value;
            }
            get
            {
                return this._iamgePath_2D;
            }
        }
        /// <summary>
        /// 3D图片保存的总路径
        /// </summary>
        public string ImagePath_3D
        {
            set
            {
                this._iamgePath_3D = value;
            }
            get
            {
                return this._iamgePath_3D;
            }
        }
        /// <summary>
        /// 数据保存的路径
        /// </summary>
        public string DataPath
        {
            set
            {
                this._dataPath = value;
            }
            get
            {
                return this._dataPath;
            }
        }
        /// <summary>
        /// 保留最近N个月的所有图片
        /// </summary>
        public int MaxMonthReserved
        {
            set
            {
                this._imageMaxMonth = value;
            }
            get
            {
                return this._imageMaxMonth;
            }
        }

        /// <summary>
        ///轮班制度
        /// </summary>
        public dnShift Shift
        {
            set
            {
                this._shift = value;
            }
            get
            {
                return this._shift;
            }
        }
        /// <summary>
        /// 获取通用参数设置界面
        /// </summary>
        public frmComConfig ComConfigForm
        {
            get
            {
                frmComConfig form = new frmComConfig(this);

                form.TopLevel = false;

                form.FormBorderStyle = FormBorderStyle.None;

                form.Dock = DockStyle.Fill;

                return form;
            }
        }
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <returns>返回界面确认信息</returns>
        public DialogResult Set()
        {
            return new frmComConfig(this).ShowDialog();
        }

        public void ShiftRefresh()
        {
            Shift.Refresh();
        }

        public string savebaoguang()
        {
            string result = "";
            ConfigOperator.SaveConfig(sectionName, "曝光时间", this._曝光时间.ToString(), configName);
            if (baoguang != null)
            {
                baoguang();
            }
            return result;
        }

        /// <summary>
        /// 保存设置到本地文件
        /// </summary>
        /// <returns>返回错误信息，如果为null则保存成功</returns>
        public string Save()
        {
            string result = "";
            try
            {
                ConfigOperator.SaveConfig(sectionName, "ImageEnable", this.Enable_Image.ToString(), configName);

                ConfigOperator.SaveConfig(sectionName, "NGImageEnable", this.Enable_NGImage.ToString(), configName);

                ConfigOperator.SaveConfig(sectionName, "OKImageEnable", this.Enable_OKImage.ToString(), configName);

                ConfigOperator.SaveConfig(sectionName, "CodeImageEnable", this.Enable_NGCode.ToString(), configName);

                ConfigOperator.SaveConfig(sectionName, "ImagePath_2D", this.ImagePath_2D, configName);

                ConfigOperator.SaveConfig(sectionName, "ImagePath_3D", this.ImagePath_3D, configName);

                ConfigOperator.SaveConfig(sectionName, "DataPath", this.DataPath.ToString(), configName);

                ConfigOperator.SaveConfig(sectionName, "MaxMonth", this.MaxMonthReserved.ToString(), configName);

                ConfigOperator.SaveConfig(sectionName, "Hummer", this.Enable_Hummer.ToString(), configName);

                ConfigOperator.SaveConfig(sectionName, "NGAuto", this.Enable_NGAuto.ToString(), configName);

                ConfigOperator.SaveConfig(sectionName, "NGHand", this.Enable_NGHand.ToString(), configName);

                ConfigOperator.SaveConfig(sectionName, "Mes", this.Enable_Mes.ToString(), configName);

                ConfigOperator.SaveConfig(sectionName, "真空吸延时", this._真空吸延时.ToString(), configName);

                this.Shift.Save();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 从本地文件初始化参数（存储设置）
        /// </summary>
        /// <returns>返回错误信息，如果为null则初始化成功</returns>
        private string Ini()
        {
            string result = "";
            try
            {
                string str_Enable = ConfigOperator.GetConfig(sectionName, "ImageEnable", configName);

                this.Enable_Image = Boolean.Parse(str_Enable);

                string str_ngEnable = ConfigOperator.GetConfig(sectionName, "NGImageEnable", configName);

                this.Enable_NGImage = Boolean.Parse(str_ngEnable);

                string str_codeEnable = ConfigOperator.GetConfig(sectionName, "CodeImageEnable", configName);

                this.Enable_NGCode = Boolean.Parse(str_codeEnable);

                string str_okEnable = ConfigOperator.GetConfig(sectionName, "OKImageEnable", configName);

                this.Enable_OKImage = Boolean.Parse(str_okEnable);

                string Hummer = ConfigOperator.GetConfig(sectionName, "Hummer", configName);

                this.Enable_Hummer = Boolean.Parse(Hummer);

                string Mes = ConfigOperator.GetConfig(sectionName, "Mes", configName);

                this.Enable_Mes = Boolean.Parse(Mes);

                string NGAuto = ConfigOperator.GetConfig(sectionName, "NGAuto", configName);

                this.Enable_NGAuto = Boolean.Parse(NGAuto);

                string NGHand = ConfigOperator.GetConfig(sectionName, "NGHand", configName);

                this.Enable_NGHand = Boolean.Parse(NGHand);

                string time= ConfigOperator.GetConfig(sectionName, "真空吸延时", configName);

                this._真空吸延时 = Convert.ToInt32(time);

                string time1 = ConfigOperator.GetConfig(sectionName, "曝光时间", configName);

                this._曝光时间 = Convert.ToInt32(time1);

                this.ImagePath_2D = ConfigOperator.GetConfig(sectionName, "ImagePath_2D", configName);

                this.ImagePath_3D = ConfigOperator.GetConfig(sectionName, "ImagePath_3D", configName);

                this.DataPath = ConfigOperator.GetConfig(sectionName, "DataPath", configName);
                                
                string str_maxMonth = ConfigOperator.GetConfig(sectionName, "MaxMonth", configName);

                int i_maxMonth = Int32.Parse(str_maxMonth);

                if (i_maxMonth < 0)
                {
                    i_maxMonth = 3;
                }

                this.MaxMonthReserved = i_maxMonth;

                this.Shift.Ini();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
    } 
}
