using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Globalization;

namespace X2292_ChassisFinalAssyAOI
{
    public abstract class CLSConfig
    {

        protected string SectionName = string.Empty;

        /// <summary>
        /// 保存参数至Ini文件
        /// </summary>
        public void Save()
        {
            Type t = this.GetType();

            string sectionName = SectionName;

            if (string.IsNullOrEmpty(sectionName))
            {
                sectionName = t.Name;
            }

            PropertyInfo[] properties = t.GetProperties();

            foreach (PropertyInfo pi in properties)
            {
                if (pi.Name != "SectionName")
                {
                    IniOperator.SaveConfig(sectionName, pi.Name, pi.GetValue(this, null).ToString());
                }
            }
        }
        /// <summary>
        /// 保存参数至Ini文件
        /// </summary>
        /// <param name="iniFileName">ini文件路径和名称</param>
        public void Save(string iniFileName)
        {
            Type t = this.GetType();

            string sectionName = SectionName;

            if (string.IsNullOrEmpty(sectionName))
            {
                sectionName = t.Name;
            }

            PropertyInfo[] properties = t.GetProperties();

            foreach (PropertyInfo pi in properties)
            {
                if (pi.Name != "SectionName")
                {
                    object obj = pi.GetValue(this, null);

                    IniOperator.SaveConfig(sectionName, pi.Name, pi.GetValue(this, null).ToString(), iniFileName);
                }
            }
        }


        /// <summary>
        /// 从ini文件初始化参数
        /// </summary>
        public void Ini()
        {
            Type t = this.GetType();

            string sectionName = SectionName;

            if (string.IsNullOrEmpty(sectionName))
            {
                sectionName = t.Name;
            }

            PropertyInfo[] properties = t.GetProperties();

            foreach (PropertyInfo pi in properties)
            {
                try
                {
                    string value = IniOperator.GetConfig(sectionName, pi.Name);

                    if (value != IniOperator.DefaultValue && value != IniOperator.NoFileValue)
                    {
                        pi.SetValue(this, Convert.ChangeType(value, pi.PropertyType), null);
                    }

                }
                catch { }
            }
        }

        /// <summary>
        /// 从ini文件初始化参数
        /// </summary>
        /// <param name="iniFileName">ini文件路径和名称</param>
        public void Ini(string iniFileName)
        {
            Type t = this.GetType();

            string sectionName = SectionName;

            if (string.IsNullOrEmpty(sectionName))
            {
                sectionName = t.Name;
            }

            PropertyInfo[] properties = t.GetProperties();

            foreach (PropertyInfo pi in properties)
            {
                try
                {
                    string value = IniOperator.GetConfig(sectionName, pi.Name,iniFileName);

                    if (value != IniOperator.DefaultValue && value != IniOperator.NoFileValue)
                    {
                        pi.SetValue(this, Convert.ChangeType(value, pi.PropertyType), null);
                    }

                }
                catch { }
            }
        }

        public override string ToString()
        {
            Type t = this.GetType();

            PropertyInfo[] properties = t.GetProperties();

            StringBuilder sb = new StringBuilder();

            foreach (PropertyInfo pi in properties)
            {
                sb.Append(string.Format("{0},", pi.GetValue(this, null).ToString()));
            }

            return sb.ToString().TrimEnd(',');
        }
    }

    

    public  class LogOperator
    {
        public static object lockObj = new object();

        public static void WriteLog(params object[] content)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {

                lock (lockObj)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();

                        string fileName = string.Format("D:\\CognexLog\\{0}.csv", DateTime.Now.ToString("yyyy_MM_dd"));

                        string directory = System.IO.Path.GetDirectoryName(fileName);

                        if (!System.IO.Directory.Exists(directory))
                        {
                            System.IO.Directory.CreateDirectory(directory);
                        }

                        sb.Append(string.Format("{0},", DateTime.Now.ToString("HH_mm_ss")));

                        foreach (object obj in content)
                        {
                            sb.Append(obj.ToString());
                            sb.Append(",");
                        }

                        string contents = string.Format("{0}{1}", sb.ToString().TrimEnd(','), Environment.NewLine);

                        if (!System.IO.File.Exists(fileName))
                        {
                            System.IO.File.WriteAllText(fileName, string.Format("Time,Action,Item{0}", Environment.NewLine));
                        }

                        System.IO.File.AppendAllText(fileName, contents);
                    }
                    catch { }
                }
            });
        }
    }

    public class BaseClassConvert<T> : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            T result = default(T);
            if ((value as string) != null)
            {
                result = FromString(value as string);
            }
            return result;
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return value.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public T FromString(string s)
        {
            T result = default(T);

            if (string.IsNullOrEmpty(s))
            {
                return result;
            }
            string[] tempValues = s.Split(',');

            Type t = this.GetType();

            PropertyInfo[] properties = t.GetProperties();

            if (properties.Length != tempValues.Length)
            {
                return result;
            }

            for (int i = 0; i < tempValues.Length; i++)
            {
                properties[i].SetValue(this, Convert.ChangeType(tempValues[i], properties[i].PropertyType), null);
            }
            return result;
        }

    }

    public class PropertyGridFileItem : UITypeEditor
    {

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {

            return UITypeEditorEditStyle.Modal;

        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {

                // 可以打开任何特定的对话框

                FolderBrowserDialog dialog = new FolderBrowserDialog();

                //dialog.AddExtension = false;

                if (dialog.ShowDialog().Equals(DialogResult.OK))
                {

                    return dialog.SelectedPath;

                }

            }

            return value;

        }

    }

    public class MyDoubleConverter : DoubleConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return string.Format("{0:0.000}", value);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class MyBoolenConverter : BooleanConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                bool bValue = Convert.ToBoolean(value);
                return bValue ? "是" : "否";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string sValue=value as string;
            if (sValue != null)
            {
                if (sValue == "是")
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    public class IniOperator
    {
        /// <summary>
        /// 默认的ini文件名称
        /// </summary>
        public static string IniFileName
        {
            get
            {
                string _file = string.Format("{0}\\Config.ini", System.Windows.Forms.Application.StartupPath);
                return _file;
            }
        }

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
        public const string DefaultValue = "Null";

        /// <summary>
        /// 没有找到指定文件时的返回值
        /// </summary>
        public const string NoFileValue = "Null";


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
        /// 保存指定的参数值,默认的文件在程序文件夹下的Config.ini文件
        /// </summary>
        /// <param name="section">section名称</param>
        /// <param name="key">键名称</param>
        /// <param name="value">值</param>
        public static void SaveConfig(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, IniFileName);
        }


        /// <summary>
        /// 获取指定的参数值
        /// </summary>
        /// <param name="section">section名称</param>
        /// <param name="key">键名称</param>
        /// <param name="fileName">ini文件名称</param>
        /// <returns>读取结果</returns>
        public static string GetConfig(string section, string key, string fileName)
        {
            if (!System.IO.File.Exists(fileName))
            {
                return NoFileValue;
            }

            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, DefaultValue, temp, 255, fileName);
            if (i > 0)
            {
                return temp.ToString();
            }
            else
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// 获取指定的参数值,默认的文件在程序文件夹下的Config.ini文件
        /// </summary>
        /// <param name="section">section名称</param>
        /// <param name="key">键名称</param>
        /// <returns>读取结果</returns>
        public static string GetConfig(string section, string key)
        {
            if (!System.IO.File.Exists(IniFileName))
            {
                return NoFileValue;
            }

            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, DefaultValue, temp, 255, IniFileName);
            if (i > 0)
            {
                return temp.ToString();
            }
            else
            {
                return DefaultValue;
            }
        }


        /// <summary>
        /// 获取指定ini文件中指定section下的所有Key值
        /// </summary>
        /// <param name="section">指定的section名称</param>
        /// <param name="fileName">ini文件名称</param>
        /// <returns>Key值集合</returns>
        public static List<string> GetKeysofSection(string section, string fileName)
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
        /// 获取指定ini文件中指定section下的所有Key值,,默认的文件在程序文件夹下的Config.ini文件
        /// </summary>
        /// <param name="section">指定的section名称</param>
        /// <returns>Key值集合</returns>
        public static List<string> GetKeysofSection(string section)
        {
            List<string> results = new List<string>();
            byte[] buffer = new byte[16384];
            int bufLen = GetPrivateProfileString(section, null, null, buffer, buffer.GetUpperBound(0), IniFileName);
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
        public static List<string> GetSections(string iniFilename)
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

        /// <summary>
        /// 获取指定ini文件中所有的Section名称,默认的文件在程序文件夹下的Config.ini文件
        /// </summary>
        /// <returns>所有section名称集合</returns>
        public static List<string> GetSections()
        {

            List<string> result = new List<string>();

            byte[] buf = new byte[65536];

            uint len = GetPrivateProfileString(null, null, null, buf, (uint)buf.Length, IniFileName);

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

    public class CtrDisplay
    {

        public static string[] RtbStrings
        {
            get;
            private set;
        }
        /// <summary>
        /// RichTextBox最大追加内容的次数
        /// </summary>
        private const int RtbMaxAdd = 1000;
        /// <summary>
        /// RichTextBox当前追加的次数
        /// </summary>
        private static int RtbAddCount { set; get; }

        public static void button_MouseEnter(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Font = new Font("宋体", 9, FontStyle.Bold);
            btn.ForeColor = Color.Green;
            btn.FlatAppearance.BorderSize = 1;

        }

        public static void button_MouseLeave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Font = new Font("宋体", 9);
            btn.ForeColor = SystemColors.ControlText;
            btn.FlatAppearance.BorderSize = 0;
        }


        /// <summary>
        /// 在Form窗体上修改Text属性值
        /// </summary>
        /// <param name="form">窗体名称</param>
        /// <param name="text">需要显示的文本</param>
        public static void ShowText(Form form, string text)
        {
            if (form.InvokeRequired)
            {
                form.Invoke(new Action<Form, string>((frm, str) => ShowText(frm, str)), form, text);
                return;
            }
            form.Text = text;

        }
        /// <summary>
        /// 在StatusStrip控件的子控件上修改Text属性值
        /// </summary>
        /// <param name="statusControl">StatusStrip的控件名称</param>
        /// <param name="index">子控件的序列号</param>
        /// <param name="text">需要显示的文本</param>
        public static void ShowText(StatusStrip statusControl, int index, string text)
        {
            if (statusControl.InvokeRequired)
            {
                statusControl.Invoke(new Action<StatusStrip, int, string>((sts, i, str) => ShowText(sts, index, str)), statusControl, index, text);
                return;
            }
            if (statusControl.Items.Count > index)
            {
                statusControl.Items[index].Text = text;
            }

        }
        /// <summary>
        /// 在StatusStrip控件的子控件上修改Text，背景色和前景色，如果前景色或者背景色值为Color.Empty时，则不发生改变
        /// </summary>
        /// <param name="statusControl">StatusStrip的控件名称</param>
        /// <param name="index">子控件的序列号</param>
        /// <param name="text">需要显示的文本</param>
        /// <param name="backColor">背景颜色</param>
        /// <param name="foreColor">前景色</param>
        public static void ShowText(StatusStrip statusControl, int index, string text, Color backColor, Color foreColor)
        {
            if (statusControl.InvokeRequired)
            {
                statusControl.Invoke(new Action<StatusStrip, int, string, Color, Color>((sts, i, str, bclr, fclr) => ShowText(sts, index, str, bclr, fclr)), statusControl, index, text, backColor, foreColor);
                return;
            }
            if (statusControl.Items.Count > index)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    statusControl.Items[index].Text = text;
                }

                if (backColor != Color.Empty)
                {
                    statusControl.Items[index].BackColor = backColor;
                }
                if (foreColor != Color.Empty)
                {
                    statusControl.Items[index].ForeColor = foreColor;
                }
            }
        }
        /// <summary>
        /// 在ToolStripStatusLabel上修改Text属性值
        /// </summary>
        /// <param name="stslabel">ToolStripStatusLabel控件名称</param>
        /// <param name="text">需要显示的文本</param>
        public static void ShowText(ToolStripStatusLabel stslabel, string text)
        {
            if ((stslabel.Owner).InvokeRequired)
            {
                (stslabel.Owner).Invoke(new Action<ToolStripStatusLabel, string>((sts, str) => ShowText(sts, str)), stslabel, text);
                return;
            }
            if (!string.IsNullOrEmpty(text))
            {
                stslabel.Text = text;
            }
        }
        /// <summary>
        /// 在ToolStripStatusLabel上修改Text,背景色和前景色，如果前景色或者背景色值为Color.Empty时，则不发生改变
        /// </summary>
        /// <param name="stslabel">需要被改变的ToolStripStatusLabel控件名称</param>
        /// <param name="text">显示在控件上的字符串</param>
        /// <param name="backcolor">控件的背景色</param>
        /// <param name="foreColor">控件的前景色</param>
        public static void ShowText(ToolStripStatusLabel stslabel, string text, Color backcolor, Color foreColor)
        {
            if ((stslabel.Owner).InvokeRequired)
            {
                (stslabel.Owner).Invoke(new Action<ToolStripStatusLabel, string, Color, Color>((sts, str, bclr, fclr) => ShowText(sts, str, bclr, fclr)), stslabel, text, backcolor, foreColor);
                return;
            }
            if (!string.IsNullOrEmpty(text))
            {
                stslabel.Text = text;
            }
            if (backcolor != Color.Empty)
            {
                stslabel.BackColor = backcolor;
            }
            if (foreColor != Color.Empty)
            {
                stslabel.ForeColor = foreColor;
            }
        }

        /// <summary>
        /// 在指定Control类控件上显示Text属性值
        /// </summary>
        /// <param name="control">控件的名称</param>
        /// <param name="text">需要显示的文本</param>
        public static void ShowText(Control control, string text)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action<Control, string>((ctr, str) => ShowText(ctr, str)), control, text);
                return;
            }
            if (!string.IsNullOrEmpty(text))
            {
                control.Text = text;
            }
        }
        /// <summary>
        /// 在指定Control类控件上显示Text，背景色和前景色，如果前景色或者背景色值为Color.Empty时，则不发生改变
        /// </summary>
        /// <param name="control">需要被改变的Control类控件名称</param>
        /// <param name="text">显示在控件上的字符串</param>
        /// <param name="backColor">控件背景色</param>
        /// <param name="foreColor">控件前景色</param>
        public static void ShowText(Control control, string text, Color backColor, Color foreColor)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action<Control, string, Color, Color>((sts, str, bclr, fclr) => ShowText(sts, str, bclr, fclr)), control, text, backColor, foreColor);
                return;
            }
            if (!string.IsNullOrEmpty(text))
            {
                control.Text = text;
            }
            if (backColor != Color.Empty)
            {
                control.BackColor = backColor;
            }
            if (foreColor != Color.Empty)
            {
                control.ForeColor = foreColor;
            }
        }
        /// <summary>
        /// 在指定Control类控件上显示Value属性值
        /// </summary>
        /// <param name="dgv">需要被改变的Datagridview控件名称</param>
        /// <param name="row">被改变的单元格行索引</param>
        /// <param name="col">被改变的单元格列索引</param>
        /// <param name="text">需要显示在指定单元格的字符串</param>
        public static void ShowText(DataGridView dgv, int row, int col, string text)
        {
            if (dgv.Created && dgv.InvokeRequired)
            {
                dgv.Invoke(new Action<DataGridView, int, int, string>((dgview, rowindex, colindex, str) => ShowText(dgview, rowindex, colindex, str)), dgv, row, col, text);
                return;
            }
            if (dgv.RowCount > row && dgv.ColumnCount > col)
            {
                dgv.Rows[row].Cells[col].Value = text;
            }
        }

        /// <summary>
        /// 在指定的Datagridview控件中修改指定单元格的Text属性,并指定背景色和前景色，如果前景色或者背景色值为Color.Empty时，则不发生改变
        /// </summary>
        /// <param name="dgv">需要被改变的Datagridview控件名称</param>
        /// <param name="row">被改变的单元格行索引</param>
        /// <param name="col">被改变的单元格列索引</param>
        /// <param name="text">需要显示在指定单元格的字符串</param>
        /// <param name="backColor">单元格背景色</param>
        /// <param name="foreColor">单元格前景色</param>
        public static void ShowText(DataGridView dgv, int row, int col, string text, Color backColor, Color foreColor)
        {
            if (dgv.Created && dgv.InvokeRequired)
            {
                dgv.Invoke(new Action<DataGridView, int, int, string, Color, Color>((dgview, rowindex, colindex, str, bclr, fclr) => ShowText(dgview, rowindex, colindex, str, bclr, fclr)), dgv, row, col, text, backColor, foreColor);
                return;
            }
            if (dgv.RowCount > row && dgv.ColumnCount > col)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    dgv.Rows[row].Cells[col].Value = text;
                }

                if (backColor != Color.Empty)
                {
                    dgv.Rows[row].Cells[col].Style.BackColor = backColor;
                }
                if (foreColor != Color.Empty)
                {
                    dgv.Rows[row].Cells[col].Style.ForeColor = foreColor;
                }
            }

        }
        /// <summary>
        /// 在指定的Datagridview控件中修改指定CheckboxCell的值
        /// </summary>
        /// <param name="dgv">需要被改变的Datagridview控件名称</param>
        /// <param name="row">行序列</param>
        /// <param name="col">列序列</param>
        /// <param name="boolVal">需要写入的值</param>
        public static void ShowText(DataGridView dgv, int row, int col, bool boolVal)
        {
            if (dgv.Created && dgv.InvokeRequired)
            {
                dgv.Invoke(new Action<DataGridView, int, int, bool>((dgview, rowindex, colindex, val) => ShowText(dgview, rowindex, colindex, val)), dgv, row, col, boolVal);
                return;
            }
            if (dgv.RowCount > row && dgv.ColumnCount > col)
            {
                DataGridViewCheckBoxCell ccell = dgv.Rows[row].Cells[col] as DataGridViewCheckBoxCell;

                ccell.Value = boolVal;
            }

        }

        /// <summary>
        /// 在指定的Datagridview控件中新增一行数据
        /// </summary>
        /// <param name="dgv">需要增加一行的Datagridview控件名称</param>
        /// <param name="objs">需要填充到最新一行的数据数组</param>
        public static void AddRowOnDgv(DataGridView dgv, object[] objs)
        {
            if (dgv.InvokeRequired)
            {
                dgv.Invoke(new Action<DataGridView, object[]>((dgview, obj) => AddRowOnDgv(dgview, obj)), dgv, objs);
                return;
            }
            dgv.Rows.Add();
            for (int i = 0; i < objs.Length; i++)
            {
                if (i < dgv.ColumnCount)
                {
                    dgv.Rows[dgv.RowCount - 1].Cells[i].Value = objs[i].ToString();
                }
            }

            dgv.FirstDisplayedScrollingRowIndex = dgv.RowCount - 1;
        }
        /// <summary>
        /// 在指定的Datagridview控件中的指定位置插入一行
        /// </summary>
        /// <param name="dgv">需要增加一行的Datagridview控件名称</param>
        /// <param name="index">序号</param>
        public static void InsertRow(DataGridView dgv, int index)
        {
            if (dgv.InvokeRequired)
            {
                dgv.Invoke(new Action<DataGridView, int>((dgview, i) => InsertRow(dgview, i)), dgv, index);
                return;
            }
            dgv.Rows.Insert(index, new DataGridViewRow());
        }

        /// <summary>
        /// 在指定的Datagridview控件中新增一行数据,并指定背景色和前景色，如果前景色或者背景色值为Color.Empty时，则不发生改变
        /// </summary>
        /// <param name="dgv">需要增加一行的Datagridview控件名称</param>
        /// <param name="objs">需要填充到最新一行的数据数组</param>
        /// <param name="backcolor">添加行的背景色</param>
        /// <param name="forecolor">添加行的前景色</param>
        public static void AddRowOnDgv(DataGridView dgv, object[] objs, Color backcolor, Color forecolor)
        {
            if (dgv.InvokeRequired)
            {
                dgv.Invoke(new Action<DataGridView, object[], Color, Color>((dgview, obj, bclr, fclr) => AddRowOnDgv(dgview, obj, bclr, fclr)), dgv, objs, backcolor, forecolor);
                return;
            }
            dgv.Rows.Add();
            if (backcolor != Color.Empty)
            {
                dgv.Rows[dgv.RowCount - 1].DefaultCellStyle.BackColor = backcolor;
            }
            if (forecolor != Color.Empty)
            {
                dgv.Rows[dgv.RowCount - 1].DefaultCellStyle.ForeColor = forecolor;
            }
            for (int i = 0; i < objs.Length; i++)
            {
                if (i < dgv.ColumnCount)
                {
                    dgv.Rows[dgv.RowCount - 1].Cells[i].Value = objs[i].ToString();
                }
            }

            dgv.FirstDisplayedScrollingRowIndex = dgv.RowCount - 1;
        }

        /// <summary>
        /// 改变TabControl控件的选中页面
        /// </summary>
        /// <param name="tp">TabControl的控件名称</param>
        /// <param name="index">需要的页面索引值</param>
        public static void PageSelect(TabControl tp, int index)
        {
            if (tp.Created && tp.InvokeRequired)
            {
                tp.Invoke(new Action<TabControl, int>((tabp, i) => PageSelect(tabp, i)), tp, index);
                return;
            }
            tp.SelectedIndex = index;
        }

        /// <summary>
        /// 在RichTextBox控件中追加数据
        /// </summary>
        /// <param name="rtbox">RichTextBox控件名称</param>
        /// <param name="text">需要追加的字符串内容</param>
        /// <param name="addTime">是否在追加内容上增加当前时间</param>
        public static void AddTextToRichTextBox(RichTextBox rtbox, string text, bool addTime)
        {
            if (rtbox.InvokeRequired)
            {
                rtbox.Invoke(new Action<RichTextBox, string, bool>((rtb, str, addtime) => AddTextToRichTextBox(rtb, str, addtime)), rtbox, text, addTime);
                return;
            }

            if (addTime)
            {
                text = string.Format("【{0}】--{1}", DateTime.Now.ToString(), text);
            }

            rtbox.AppendText(text + "\r\n");

            rtbox.ScrollToCaret();
        }
        /// <summary>
        /// 在ListBox控件中追加数据
        /// </summary>
        /// <param name="rtbox">Listbox控件名称</param>
        /// <param name="text">需要追加的内容</param>
        /// <param name="addTime">是否增加时间显示</param>
        public static void AddListToListBox(ListBox rtbox, string text, Brush foreColor,bool addTime)
        {
            if (rtbox.InvokeRequired)
            {
                rtbox.Invoke(new Action<ListBox, string, Brush, bool>((rtb, str, color, addtime) => AddListToListBox(rtb, str, color, addtime)), rtbox, text, foreColor, addTime);
                return;
            }

            if (addTime)
            {
                text = string.Format("【{0}】--{1}", DateTime.Now.ToString(), text);
            }

             rtbox.Items.Add(new ListBoxItem(text,foreColor));          

            if (rtbox.Items.Count > 100)
            {
                int count = rtbox.Items.Count - 100;

                List<object> temp = new List<object>();

                for (int i = 0; i < count; i++)
                {
                    temp.Add(rtbox.Items[i]);
                }

                foreach (object obj in temp)
                {
                    rtbox.Items.Remove(obj);
                }
            }

            rtbox.SelectedIndex = rtbox.Items.Count - 1;
        }

        /// <summary>
        /// 在RichTextBox控件中追加数据，并指定当前内容显示的前景色，如果前景色值为Color.Empty时，则不发生改变
        /// </summary>
        /// <param name="rtbox">RichTextBox控件名称</param>
        /// <param name="text">需要追加的字符串内容</param>
        /// <param name="color">追加内容的前景色</param>
        /// <param name="addTime">是否在追加内容上增加当前时间</param>
        public static void AddTextToRichTextBox(RichTextBox rtbox, string text, Color color, bool addTime)
        {           
                if (rtbox.InvokeRequired)
                {
                    rtbox.Invoke(new Action<RichTextBox, string, Color, bool>((rtb, str, forecolor, addtime) => AddTextToRichTextBox(rtb, str, forecolor, addtime)), rtbox, text, color, addTime);
                    return;
                }

                if (addTime)
                {
                    text = string.Format("【{0}】--{1}", DateTime.Now.ToString(), text);
                }
                if (color != Color.Empty)
                {
                    rtbox.SelectionColor = color;
                }

                rtbox.AppendText(text + "\r\n");

                rtbox.ScrollToCaret();
           
         
        }
        /// <summary>
        /// 在RichTextBox控件中追加数据，并指定当前内容显示的字体，如果字体为Color.Empty时，则字体不发生改变
        /// </summary>
        /// <param name="rtbox">RichTextBox控件名称</param>
        /// <param name="text">需要追加的字符串内容</param>
        /// <param name="font">追加内容的字体</param>
        /// <param name="addTime">是否在追加内容上增加当前时间</param>
        public static void AddTextToRichTextBox(RichTextBox rtbox, string text, Font font, bool addTime)
        {

            if (rtbox.InvokeRequired)
            {
                rtbox.Invoke(new Action<RichTextBox, string, Font, bool>((rtb, str, fnt, addtime) => AddTextToRichTextBox(rtb, str, fnt, addtime)), rtbox, text, font, addTime);
                return;
            }

            if (addTime)
            {
                text = string.Format("【{0}】--{1}", DateTime.Now.ToString(), text);
            }
            if (font != null)
            {
                rtbox.SelectionFont = font;
            }

            rtbox.AppendText(text + "\r\n");

            rtbox.ScrollToCaret();

        }
        /// <summary>
        /// 在RichTextBox控件中追加数据，并指定当前内容显示的前景色，如果前景色值为Color.Empty时，则不发生改变,如果字体为null时，则字体不发生改变
        /// </summary>
        /// <param name="rtbox">RichTextBox控件名称</param>
        /// <param name="text">需要追加的字符串内容</param>
        /// <param name="color">追加内容的前景色</param>
        /// <param name="font">追加内容的字体</param>
        /// <param name="addTime">是否在追加内容上增加当前时间</param>
        public static void AddTextToRichTextBox(RichTextBox rtbox, string text, Color color, Font font, bool addTime)
        {
            if (rtbox.InvokeRequired)
            {
                rtbox.Invoke(new Action<RichTextBox, string, Color, Font, bool>((rtb, str, forecolor, fnt, addtime) => AddTextToRichTextBox(rtb, str, forecolor, fnt, addtime)), rtbox, text, color, font, addTime);
                return;
            }

            if (addTime)
            {
                text = string.Format("【{0}】--{1}", DateTime.Now.ToString(), text);
            }
            if (color != Color.Empty)
            {
                rtbox.SelectionColor = color;
            }

            if (font != null)
            {
                rtbox.SelectionFont = font;
            }

            rtbox.AppendText(text + "\r\n");

            rtbox.ScrollToCaret();
        }
        /// <summary>
        /// 清空指定RichTextBox控件的所有内容
        /// </summary>
        /// <param name="rtbox">RichTextBox的名称</param>
        public static void ClearRichTextBox(RichTextBox rtbox)
        {
            if (rtbox.InvokeRequired)
            {
                rtbox.Invoke(new Action<RichTextBox>((rtb) => ClearRichTextBox(rtb)), rtbox);
                return;
            }

            rtbox.ResetText();
        }
        /// <summary>
        /// 获取指定RichTextBox控件的所有内容
        /// </summary>
        /// <param name="rtbox">RichTextBox的名称</param>
        public static void GetAllText(RichTextBox rtbox)
        {
            if (rtbox.InvokeRequired)
            {
                rtbox.Invoke(new Action<RichTextBox>((rtb) => GetAllText(rtb)), rtbox);
                return;
            }
            RtbStrings = new string[] { };
            RtbStrings = rtbox.Lines;
        }

        /// <summary>
        /// 是否启用指定的控件
        /// </summary>
        /// <param name="btn">指定的控件的名称</param>
        /// <param name="enable">是否启用</param>
        public static void ControlEnable(Control btn, bool enable)
        {
            if (btn.InvokeRequired)
            {
                btn.Invoke(new Action<Button, bool>((b, e) => ControlEnable(b, e)), btn, enable);
                return;
            }
            btn.Enabled = enable;
        }


        /// <summary>
        /// 是否启用指定的控件
        /// </summary>
        /// <param name="btn">指定的控件的名称</param>
        /// <param name="enable">是否启用</param>
        public static void ControlEnable(ToolStripButton btn, bool enable)
        {
            ToolStrip ts = btn.GetCurrentParent();

            if (ts != null && ts.InvokeRequired)
            {
                ts.Invoke(new Action<ToolStripButton, bool>((b, e) => ControlEnable(b, e)), btn, enable);
                return;
            }
            btn.Enabled = enable;
        }


        public static void ControlEnable(ToolStripMenuItem btn, bool enable)
        {
            ToolStrip ts = btn.GetCurrentParent();

            if (ts != null && ts.InvokeRequired)
            {
                ts.Invoke(new Action<ToolStripMenuItem, bool>((b, e) => ControlEnable(b, e)), btn, enable);
                return;
            }
            btn.Enabled = enable;
        }
    }

    public class ListBoxItem
    {
        public string Text { set; get; }

        public Brush ForeColor { set; get; }

        public ListBoxItem(string text, Brush foreColor)
        {
            this.Text = text;
            this.ForeColor = foreColor;
        }
    }


    public class CycleTimeOperator
    {
        private StringBuilder _header = new StringBuilder();

        private StringBuilder _times = new StringBuilder();

        private System.Diagnostics.Stopwatch _stepwatch = new System.Diagnostics.Stopwatch();

        private System.Diagnostics.Stopwatch _totalwatch = new System.Diagnostics.Stopwatch();

        private string _name = "";

        private int count = 1;

        public CycleTimeOperator(string name)
        {
            this._name = name;

            _header.Append("Index");

            count = 1;

            _times.Append(count.ToString());

        }

        public void Reset()
        {
            _header = new StringBuilder();

            _times = new StringBuilder();

            _stepwatch.Stop();

            _totalwatch.Stop();

            _stepwatch = new System.Diagnostics.Stopwatch();

            _totalwatch = new System.Diagnostics.Stopwatch();

            _header.Append("Index");

            count = 1;

            _times.Append(count.ToString());
        }

        public void TotalStart()
        {
            this._totalwatch.Restart();
            _times.Append(count.ToString());
        }

        public void StepStart()
        {
            _stepwatch.Restart();
        }

        public void TotalStop()
        {
            _totalwatch.Stop();

            _header.Append(",Total");

            _times.Append(string.Format(",{0:F2}", _totalwatch.ElapsedMilliseconds / 1000.0));
        }

        public void StepStop(string stepName)
        {
            _stepwatch.Stop();

            _header.Append(string.Format(",{0}", stepName));

            _times.Append(string.Format(",{0:F2}", _stepwatch.ElapsedMilliseconds / 1000.0));
        }

        public void Write()
        {
            string header = string.Format("{0}{1}", _header.ToString(), Environment.NewLine);

            string times = string.Format("{0}{1}", _times.ToString(), Environment.NewLine);

            try
            {
                string fileName = string.Format("D:\\CycleTimeRecord\\{0}.csv", _name);

                string directory = System.IO.Path.GetDirectoryName(fileName);

                if (!System.IO.Directory.Exists(directory))
                {
                    System.IO.Directory.CreateDirectory(directory);
                }

                if (!System.IO.File.Exists(fileName))
                {
                    System.IO.File.WriteAllText(fileName, header, Encoding.UTF8);
                }

                System.IO.File.AppendAllText(fileName, times, Encoding.UTF8);
            }
            catch { }
            finally
            {
                _header.Clear();
                _times.Clear();
                count++;
            }
        }
    }

}

