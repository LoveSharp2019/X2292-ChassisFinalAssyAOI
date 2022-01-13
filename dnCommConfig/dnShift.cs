using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace dnCommConfig
{
    //[TypeConverterAttribute(typeof(PropertyDisplayConverterr<ComConfig>))]
    public class dnShift
    {
        System.Threading.Timer _timer;
        private string configName = Application.StartupPath + "\\Shift.ini";

        private const string startTime = "Start";

        private const string endTime = "End";

        private List<ShiftIitem> _shifts = new List<ShiftIitem>();

        private TimeSpan _interval;

        public delegate void ShiftChangingHandler();

        /// <summary>
        /// 班次切换前发生
        /// </summary>
        public event ShiftChangingHandler ShiftChanging;

        public delegate void ShiftChangedEventHandler(ShiftIitem _currentShift);
        /// <summary>
        /// 班次切换后发生
        /// </summary>
        public event ShiftChangedEventHandler ShiftChanged;
        /// <summary>
        /// 当前班次
        /// </summary>
        public ShiftIitem CurrentShift
        {
            private set;
            get;
        }
        /// <summary>
        /// 所有班次
        /// </summary>
        public List<ShiftIitem> Shifts
        {
            get
            {
                return this._shifts;
            }
        }

        /// <summary>
        /// 当前班次名称
        /// </summary>
        public string Name
        {
            get
            {
                string _name = string.Empty;

                if (CurrentShift != null)
                {
                    _name = CurrentShift.ShiftName;
                }
                return _name;
            }
        }
        /// <summary>
        /// 当前班次的时间间隔
        /// </summary>
        public TimeSpan ShiftInterval
        {
            get
            {
                return this._interval;
            }
        }
        /// <summary>
        /// 当前班次的日期
        /// </summary>
        public string ShiftDay
        {
            set;
            get;
        }
        /// <summary>
        /// 当前班次的月份
        /// </summary>
        public string ShiftMonth
        {
            set;
            get;
        }
        /// <summary>
        /// 当前班次的年份
        /// </summary>
        public string ShiftYear
        {
            set;
            get;
        }

        /// <summary>
        /// 实例化新轮班
        /// </summary>
        /// <param name="shiftCount">轮班循环个数</param>
        public dnShift(int shiftCount)
        {
            _shifts.Clear();

            _interval = new TimeSpan(24 / shiftCount, 0, 0);

            TimeSpan start = TimeSpan.Parse("07:30:00");

            TimeSpan end = new TimeSpan(7 + 24 / shiftCount, 30, 0);

            for (int i = 0; i < shiftCount; i++)
            {
                TimeSpan interval = new TimeSpan(24 / shiftCount * i, 0, 0);

                TimeSpan _start = start.Add(interval);

                TimeSpan _end = end.Add(interval);

                ShiftIitem s = new ShiftIitem() { ShiftName = Convert.ToChar(i + 65).ToString(), TimeShiftStart = new TimeSpan(_start.Hours, _start.Minutes, _start.Seconds), TimeShiftEnd = new TimeSpan(_end.Hours, _end.Minutes, _end.Seconds) };

                _shifts.Add(s);
            }
            this.Ini();
            _timer = new System.Threading.Timer(new TimerCallback(timer_tick), null, 0, 1000);
        }

        private void timer_tick(object obj)
        {
            DateTime dt = DateTime.Now;
            TimeSpan _now = dt.TimeOfDay;
            try
            {
                foreach (ShiftIitem si in _shifts)
                {
                    bool changEnable = false;
                    if (si.TimeShiftEnd > si.TimeShiftStart)
                    {
                        if (_now > si.TimeShiftStart && _now < si.TimeShiftEnd)
                        {
                            changEnable = true;
                        }
                    }
                    else if (_now < si.TimeShiftEnd || _now > si.TimeShiftStart)
                    {
                        changEnable = true;
                        if (_now < si.TimeShiftEnd)
                        {
                            dt = dt.AddDays(-1);
                        }

                    }
                    this.ShiftYear = dt.Year.ToString();
                    this.ShiftDay = dt.Day.ToString();
                    this.ShiftMonth = dt.Month.ToString();

                    if (changEnable)
                    {
                        if (this.ShiftChanging != null)
                        {
                            this.ShiftChanging();
                        }
                        this.CurrentShift = si;
                        if (this.ShiftChanged != null)
                        {
                            this.ShiftChanged(si);
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        public void Refresh()
        {
            timer_tick(null);
        }

        /// <summary>
        /// 保存班次信息
        /// </summary>
        public void Save()
        {
            if (File.Exists(configName))
            {
                File.Delete(configName);
            }

            foreach (ShiftIitem si in this._shifts)
            {
                ConfigOperator.SaveConfig(si.ShiftName, startTime, si.TimeShiftStart.ToString(), configName);
                ConfigOperator.SaveConfig(si.ShiftName, endTime, si.TimeShiftEnd.ToString(), configName);
            }
        }
        /// <summary>
        /// 初始化班次信息
        /// </summary>
        public void Ini()
        {
            if(!File.Exists(configName))
            {
                return;
            }

            List<string> _allShifts = ConfigOperator.ReadSections(configName);
            if (_shifts.Count < 1)
            {
                return;
            }

            if (_allShifts.Count != this._shifts.Count)
            {
                return;
            }

            this._shifts = new List<ShiftIitem>();
            this._shifts.Clear();
            foreach (string name in _allShifts)
            {
                ShiftIitem item = new ShiftIitem();
                item.ShiftName = name;
                string _start = ConfigOperator.GetConfig(name, startTime, configName);
                item.TimeShiftStart = TimeSpan.Parse(_start);
                string _end = ConfigOperator.GetConfig(name, endTime, configName);
                item.TimeShiftEnd = TimeSpan.Parse(_end);
                _shifts.Add(item);
            }
        }

        public string GetDisplayString()
        {
            if (this == null)
            {
                return "未设置自增列信息";
            }
            return String.Format("班次名称:{0}", this.Name);
        }
    }

    public class ShiftIitem : IDisplay
    {
      

        /// <summary>
        /// 班次开始时间
        /// </summary>
        public TimeSpan TimeShiftStart { set; get; }
        /// <summary>
        /// 班次结束时间
        /// </summary>
        public TimeSpan TimeShiftEnd { set; get; }
        /// <summary>
        /// 班次名称
        /// </summary>
        public string ShiftName { set; get; }

        

        public string GetDisplayString()
        {
            if (this == null)
            {
                return "未设置自增列信息";
            }
            return String.Format("开始时间;{0},结束时间:{1}", TimeShiftStart.ToString(), TimeShiftEnd.ToString());
        }
    }

    public class ConfigOperator
    {
        [DllImport("kernel32", CallingConvention = CallingConvention.StdCall)]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filepath);

        [DllImport("kernel32", CallingConvention = CallingConvention.StdCall)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filepath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);
        [DllImport("kernel32")]
        private static extern uint GetPrivateProfileString(

            string lpAppName, // points to section name

            string lpKeyName, // points to key name

            string lpDefault, // points to default string

            byte[] lpReturnedString, // points to destination buffer

            uint nSize, // size of destination buffer

            string lpFileName  // points to initialization filename

        );
        /// <summary>
        /// 读取指定参数值的默认返回值
        /// </summary>
        public const string DefaultValue = "0";

        /// <summary>
        /// 保存指定的参数值
        /// </summary>
        /// <param name="section">section名称</param>
        /// <param name="key">键名称</param>
        /// <param name="value">值</param>
        /// <param name="filepath">ini文件名称</param>
        public static void SaveConfig(string section, string key, string value, string filepath)
        {
            WritePrivateProfileString(section, key, value, filepath);
        }
        /// <summary>
        /// 获取指定的参数值
        /// </summary>
        /// <param name="section">section名称</param>
        /// <param name="key">键名称</param>
        /// <param name="filepath">ini文件名称</param>
        /// <returns></returns>
        public static string GetConfig(string section, string key, string filepath)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, DefaultValue, temp, 255, filepath);
            return temp.ToString();
        }
        /// <summary>
        /// 获取指定ini文件中指定section下的所有Key值
        /// </summary>
        /// <param name="section">指定的section名称</param>
        /// <param name="fileName">ini文件名称</param>
        /// <returns>Key值集合</returns>
        public static List<string> GetStringofSection(string section, string fileName)
        {
            List<string> results = new List<string>();
            byte[] buffer = new byte[16384];
            int bufLen = GetPrivateProfileString(section, null, null, buffer, buffer.GetUpperBound(0), fileName);
            if (bufLen > 0)
            {
                int start = 0;
                for (int i = 0; i < bufLen; i++)
                {
                    if ((buffer[i] == 0) && ((i - start) > 0))
                    {
                        string s = Encoding.GetEncoding(0).GetString(buffer, start, i - start);
                        results.Add(s);
                        start = i + 1;
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// 获取指定ini文件中所有的Section名称
        /// </summary>
        /// <param name="iniFilename">指定的ini文件名称</param>
        /// <returns>所有section名称集合</returns>
        public static List<string> ReadSections(string iniFilename)
        {

            List<string> result = new List<string>();

            byte[] buf = new byte[65536];

            uint len = GetPrivateProfileString(null, null, null, buf, (uint)buf.Length, iniFilename);

            int j = 0;

            for (int i = 0; i < len; i++)
            {
                if (buf[i] == 0)
                {

                    result.Add(Encoding.Default.GetString(buf, j, i - j));

                    j = i + 1;

                }
            }

            return result;

        }
    }


    public class PropertyDisplayConverterr<T> : ExpandableObjectConverter where T : IDisplay
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            if (destinationType == typeof(T))
                return true;
            return base.CanConvertTo(context, destinationType);
        }
        // This is a special type converter which will be associated with the T class.  
        // It converts an T object to string representation for use in a property grid.  
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            if (destinationType == typeof(System.String) && value is T)
            {
                return ((IDisplay)value).GetDisplayString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    
    public interface IDisplay
    {
        /// <summary>  
        /// 得到显示字符串  
        /// </summary>  
        /// <returns></returns>  
        string GetDisplayString();
    }

    /// <summary>  
    /// 实体属性处理  
    /// </summary>  
    public class PropertyHandle
    {
        #region 反射控制只读、可见属性
        //SetPropertyVisibility(obj,   "名称 ",   true);   
        //obj指的就是你的SelectObject，   “名称”是你SelectObject的一个属性   
        //当然，调用这两个方法后，重新SelectObject一下，就可以了。  
        /// <summary>  
        /// 通过反射控制属性是否只读  
        /// </summary>  
        /// <param name="obj">实体类变量</param>  
        /// <param name="propertyName">是否只读的属性名称</param>  
        /// <param name="readOnly">是否只读</param>  
        public static void SetPropertyReadOnly(object obj, string propertyName, bool readOnly)
        {
            Type type = typeof(ReadOnlyAttribute);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
            AttributeCollection attrs = props[propertyName].Attributes;
            FieldInfo fld = type.GetField("isReadOnly", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance);
            fld.SetValue(attrs[type], readOnly);
        }

        /// <summary>  
        /// 通过反射控制属性是否可见  
        /// </summary>  
        /// <param name="obj">实体类变量</param>  
        /// <param name="propertyName">是否可见的属性名称</param>  
        /// <param name="visible">属性是否可见</param>  
        public static void SetPropertyVisibility(object obj, string propertyName, bool visible)
        {
            Type type = typeof(BrowsableAttribute);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
            AttributeCollection attrs = props[propertyName].Attributes;
            FieldInfo fld = type.GetField("browsable", BindingFlags.Instance | BindingFlags.NonPublic);
            fld.SetValue(attrs[type], visible);
        }
        #endregion
    }


}
