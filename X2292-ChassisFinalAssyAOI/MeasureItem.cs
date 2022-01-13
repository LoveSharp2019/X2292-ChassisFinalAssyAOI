using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace X2292_ChassisFinalAssyAOI
{

    public class MeasureItem
    {

        private string _station = "FAIs";
        private double _value = -99.99;
        
        private int iCaveIndex = -1;
        private double[] _compensationValueKArray = { 1, 1 };//FAI多穴位系数K
        private double[] _compensationValueBArray = { 0, 0};//FAI多穴位系数B

/// <summary>
/// 实例化新测量项
/// </summary>
/// <param name="name">名称</param>
public MeasureItem(string name, string stationName = "FAIS")
        {
            this.ItemName = name;
            _station = stationName;
        }
        /// <summary>
        /// 实例化新测量项
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="b">理论值</param>
        public MeasureItem(string name, double min, double max, double normal,string k,string b, string stationName = "FAIS")
        {
            this.ItemName = name;
            this.Min = min;
            this.Max = max;
            this.NormalValue = normal;
            double[] Kk = Array.ConvertAll(k.Split(',').ToArray(),s=>double.Parse(s));  
            double[] bb = Array.ConvertAll(b.Split(',').ToArray(), s => double.Parse(s));
            this.CompensationValueKArray = Kk;
            this.CompensationValueBArray = bb;
            _station = stationName;
        }

        /// <summary>
        /// ini文件名称
        /// </summary>
        public string IniFileName
        {
            get
            {
                return string.Format("{0}/{1}.ini", System.Windows.Forms.Application.StartupPath + "//Settings", _station);
            }
        }

        /// <summary>
        /// 测量项名称
        /// </summary>
        public string ItemName { set; get; }
        /// <summary>
        /// 测量值
        /// </summary>
        public double Value
        {
            set
            {
                this._value = value;
            }
            get
            {

                return this._value;
            }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public double Max { set; get; }
        /// <summary>
        /// 最小值
        /// </summary>
        public double Min { set; get; }
        /// <summary>
        /// 理论值
        /// </summary>
        public double NormalValue { set; get; }

        ///// <summary>
        ///// 补偿值_B
        ///// </summary>
        //public double CompensationValueB { set; get; }
        ///// <summary>
        ///// 补偿值_K
        ///// </summary>
        //public double CompensationValueK
        //{
        //    set
        //    {
        //        _compensationValueK = value;
        //    }
        //    get
        //    {
        //        return this._compensationValueK;
        //    }
        //}

        /// <summary>
        /// 单工站Cave索引
        /// </summary>
        public int ICaveIndex
        {
            get { return iCaveIndex; }
            set { iCaveIndex = value; }
        }

        /// <summary>
        /// 单工站12穴位K系数
        /// </summary>
        public double[] CompensationValueKArray
        {
            get { return _compensationValueKArray; }
            set { _compensationValueKArray = value; }
        }

        /// <summary>
        /// 单工站12穴位B常量
        /// </summary>
        public double[] CompensationValueBArray
        {
            get { return _compensationValueBArray; }
            set { _compensationValueBArray = value; }
        }

        ///// <summary>
        ///// 加入补偿后的最终值value=k*value+b
        ///// </summary>
        //public double FinnalValue
        //{
        //    get
        //    {
        //        return CompensationValueK * Value + CompensationValueB;
        //    }
        //}

        /// <summary>
        /// 加入单穴补偿后的最终值value=k*value+b
        /// </summary>
        public double FinnalValue
        {
            get
            {
                return CompensationValueKArray[ICaveIndex] * Value + CompensationValueBArray[ICaveIndex];
                //return Math.Abs();
            }
        }

        private List<string> finnalValues = new List<string>();


        public void AddValue(double val)
        {
            this._value = val;

            if (!(FinnalValue >= Min && FinnalValue <= Max))
                result = false;
            finnalValues.Add(FinnalValue.ToString());
        }

        public void Reset()
        {
            result = true;
            finnalValues.Clear();
        }
        public string allFinnalValues
        {
            get
            {
                return string.Join(",", finnalValues.ToArray());
            }
        }


        private bool result = true;
        /// <summary>
        /// 单项结果
        /// </summary>
        public bool Result
        {
            get
            {
                return FinnalValue >= Min && FinnalValue <= Max;
                //return result;
            }
        }
        /// <summary>
        /// 设置测量项的最小值和最大值
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public void SetMinMax(double min, double max)
        {
            this.Min = min;
            this.Max = max;
        }
        ///// <summary>
        ///// 设置测量项补偿值的k,b值
        ///// </summary>
        ///// <param name="k"></param>
        ///// <param name="b"></param>
        //public void SetCompensationValue(double k, double b)
        //{
        //    this.CompensationValueK = k;
        //    this.CompensationValueB = b;
        //}

        /// <summary>
        /// 设置单工站12穴位测量项补偿值的k,b值
        /// </summary>
        /// <param name="k"></param>
        /// <param name="b"></param>
        //public void SetCompensationValue(double kArray, double bArray)
        //{
        //    this.CompensationValueKArray = kArray;
        //    this.CompensationValueBArray = bArray;
        //}


        /// <summary>
        /// 保存测量项的最值和补偿值（k,b)
        /// </summary>
        /// <returns>返回保存时的错误结果，为空时表示保存成功</returns>
        //public string SaveItem()
        //{
        //    return SaveItem(this.IniFileName);
        //}

        /// <summary>
        /// 保存测量项的最值和补偿值（k,b)
        /// </summary>
        /// <param name="iniName">文件名</param>
        /// <returns>返回保存时的错误结果，为空时表示保存成功</returns>
        public string SaveItem(string iniName)
        {
            string result = string.Empty;

            try
            {
                //ConfigOperator.SaveConfig(this.ItemName, "Min", this.Min.ToString("F3"), iniName);
                //ConfigOperator.SaveConfig(this.ItemName, "Max", this.Max.ToString("F3"), iniName);
                //ConfigOperator.SaveConfig(this.ItemName, "Normal", this.NormalValue.ToString("F3"), iniName);
                //ConfigOperator.SaveConfig(this.ItemName, "K", this.CompensationValueKArray.ToString("F3"), iniName);
                //ConfigOperator.SaveConfig(this.ItemName, "B", this.CompensationValueBArray.ToString("F3"), iniName);
                //单工站12穴位INI保存
                //if ((CompensationValueKArray.Length != 12) && (CompensationValueBArray.Length != 12))
                //{
                //    System.Windows.Forms.MessageBox.Show(iniName + "缺少补偿系数K和B，请重新确认补偿系数个数并设置！");
                //}
                //else
                //{
                //    ConfigOperator.SaveConfig(this.ItemName, "K", String.Join(",", CompensationValueKArray), iniName);
                //    ConfigOperator.SaveConfig(this.ItemName, "B", String.Join(",", CompensationValueBArray), iniName);
                //}
                //ConfigOperator.SaveConfig(this.ItemName, "B", this.CompensationValueB.ToString("F3"), iniName);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 从本地文件中初始化检测项
        /// </summary>
        /// <returns>返回初始化时的错误结果，为空时表示初始化成功</returns>
        //public string IniItem()
        //{
        //    return this.IniItem(this.IniFileName);
        //}


        /// <summary>
        /// 从本地文件初始化检测项
        /// </summary>
        /// <param name="iniNmae">文件名</param>
        /// <returns>返回初始化时的错误结果，为空时表示初始化成功</returns>
        //public string IniItem(string iniNmae)
        //{
        //    string result = string.Empty;

        //    try
        //    {
        //        string tempMin = ConfigOperator.GetConfig(this.ItemName, "Min", iniNmae);
        //        string tempMax = ConfigOperator.GetConfig(this.ItemName, "Max", iniNmae);
        //        string tempNormal = ConfigOperator.GetConfig(this.ItemName, "Normal", iniNmae);
        //        //读取单工站12穴位K，B系数
        //        string tempK = ConfigOperator.GetConfig(this.ItemName, "K", iniNmae);
        //        string tempB = ConfigOperator.GetConfig(this.ItemName, "B", iniNmae);
        //        string[] tempKArray = tempK.Split(',').ToArray();
        //        string[] tempBArray = tempB.Split(',').ToArray();
        //        if ((tempKArray.Length != 12) && (tempBArray.Length != 12))
        //        {
        //            System.Windows.Forms.MessageBox.Show(iniNmae + "缺少补偿系数K和B，请重新确认补偿系数个数！");
        //        }
        //        else
        //        {
        //            double dKArray = 1;
        //            double dBArray = 0;

        //            this.CompensationValueKArray = dKArray;
        //            this.CompensationValueBArray = dBArray;
        //        }
        //        //this.CompensationValueK = Convert.ToDouble(tempK);
        //        //this.CompensationValueB = Convert.ToDouble(tempB);
        //        this.NormalValue = Convert.ToDouble(tempNormal);
        //        this.Min = Convert.ToDouble(tempMin);
        //        this.Max = Convert.ToDouble(tempMax);
        //    }
        //    catch (Exception ex)
        //    {
        //        result = ex.Message;
        //    }
        //    return result;
        //}

        public string GetDisplayString()
        {
            if (this == null)
            {
                return "未设置信息";
            }

            return string.Format("Name:{0};Min:{1};Normal:{2};Max:{3};K:{4};B:{5}", this.ItemName, this.Min, this.NormalValue, this.Max, String.Join(",",this.CompensationValueKArray), String.Join(",",this.CompensationValueBArray));
        }

    }
}
