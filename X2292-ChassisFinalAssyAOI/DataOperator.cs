using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
//using dnCommConfig;
//using dnMeasureItems;
using System.Runtime.InteropServices;
using dnCommConfig;

namespace X2292_ChassisFinalAssyAOI
{
    public class DataOperator
    {
        private string stationName = null;

        public string StationName
        {
            get
            {
                return stationName;
            }

            set
            {
                stationName = value;
            }
        }

        public void SaveData(object dataSaveHelper)
        {
            Thread t = new Thread(new ParameterizedThreadStart(SavingData));
            t.Start(dataSaveHelper);
        }

        public void SaveDataEachST(object dataSaveHelper)
        {
            Thread t = new Thread(new ParameterizedThreadStart(SavingDataEachST));
            t.Start(dataSaveHelper);
        }

        private void SavingData(object dataSave)
        {
            try
            {
                DataSaveHelper dataHelper = (DataSaveHelper)dataSave;

                string recipeName = dataHelper.recipeName;

                string fileName = string.Format("{0}\\{1}_Data.csv", dataHelper.FolderName, recipeName);
                if (!File.Exists(fileName))
                {
                    using (StreamWriter sw = new StreamWriter(fileName, false, Encoding.GetEncoding("gb2312")))
                    {
                        sw.WriteLine(dataHelper.HeaderText);
                        sw.WriteLine(dataHelper.SLHeaderText);
                        sw.WriteLine(dataHelper.USLHeaderText);
                        sw.WriteLine(dataHelper.LSLHeaderText);
                        sw.Flush();
                        sw.Close();
                    }
                }
                using (StreamWriter sw = new StreamWriter(fileName, true, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(dataHelper.Text);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            { }
        }

        private void SavingDataEachST(object dataSave)
        {
            try
            {
                DataSaveHelper dataHelper = (DataSaveHelper)dataSave;
                string recipeName = dataHelper.recipeName;
                string strPath = string.Format("D:\\Data\\{0}\\{1}\\", "单站数据", recipeName);
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                string fileName = strPath + dataHelper.STName1 + ".csv";
                if (!File.Exists(fileName))
                {
                    using (StreamWriter sw = new StreamWriter(fileName, false, Encoding.GetEncoding("gb2312")))
                    {
                        sw.WriteLine(dataHelper.HeaderTextEachST);
                        sw.WriteLine(dataHelper.FaiSL1);
                        sw.WriteLine(dataHelper.FaiUSL1);
                        sw.WriteLine(dataHelper.FaiLSL1);
                        sw.Flush();
                        sw.Close();
                    }
                }
                using (StreamWriter sw = new StreamWriter(fileName, true, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(dataHelper.TextEachST);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            { }
        }

        public class DataSaveHelper
        {
            public string Code { set; get; }
            public dnShift Shift { set; get; }
            // public bool ProductResult { set; get; }
            // public string LineName { set; get; }
            public MeasureItems Data { set; get; }
            private string STName = null;
            private string FaiNames = null;
            private string FaiValues = null;
            private string FaiUSL = null;
            private string FaiSL = null;
            private string FaiLSL = null;
            private string _Path = "D:\\Data";
            public string STName1
            {
                get { return STName; }
                set { STName = value; }
            }

            public string FaiNames1
            {
                get { return FaiNames; }
                set { FaiNames = value; }
            }

            public string FaiValues1
            {
                get { return FaiValues; }
                set { FaiValues = value; }
            }
            public string FaiUSL1
            {
                get { return FaiUSL; }
                set { FaiUSL = value; }
            }

            public string FaiSL1
            {
                get { return FaiSL; }
                set { FaiSL = value; }
            }

            public string FaiLSL1
            {
                get { return FaiLSL; }
                set { FaiLSL = value; }
            }

            public string Path
            {
                set { _Path = value; }
                get { return _Path; }
            }

            public string recipeName
            {
                get
                {    
                    return DateTime.Now.ToString("yyyy-MM-dd");
                }
  
            }
            private string Result
            {
                get
                {
                    if (Data.Result)
                    {
                        return "OK";
                    }
                    else
                    {
                        return "NG";
                    }
                }
            }

            private string FResult
            {
                get
                {
                    if (Data.Result)
                    {
                        return "OK";
                    }
                    else
                    {
                        return "NG";
                    }
                }
            }

            private string _Machine_LineName;
            public string Machine_LineName
            {
                get
                {
                    return _Machine_LineName;
                }
                set
                {
                    this._Machine_LineName = value;
                }
            }



            public string HeaderText
            {
                get
                {
                    return string.Format("日期,班次,二维码,结果,{0}", Data.NamesString());
                }
            }

            public string HeaderTextEachST
            {
                get
                {
                    return string.Format("日期,班次,二维码,结果,{0}", FaiNames1);
                }
            }

            public string SLHeaderText
            {
                get
                {
                    return string.Format(" , , ,SL,{0}", Data.SLString());
                }
            }

            public string USLHeaderText
            {
                get
                {
                    return string.Format(" , , ,USL,{0}", Data.USLString());
                }
            }

            public string LSLHeaderText
            {
                get
                {
                    return string.Format(" , , ,LSL,{0}", Data.LSLString());
                }
            }

            public string Text
            {
                get
                {
                    return string.Format("{0},{1},{2},{3},{4}", DateTime.Now.ToString(), Shift.Name, Data.SN.Trim(), Result, Data.ToString());
                }
            }

            public string TextEachST
            {
                get
                {
                    return string.Format("{0},{1},{2},{3},{4}", DateTime.Now.ToString(), Shift.Name, Data.SN.Trim(), Result, FaiValues1);
                }
            }

            public string FolderName
            {
                get
                {
                    string fileName = string.Empty;

                    fileName = string.Format("{0}\\{1}年\\{2}月\\{3}日\\{4}班\\", _Path, Shift.ShiftYear, Shift.ShiftMonth, Shift.ShiftDay, Shift.Name);

                    string tempPath = System.IO.Path.GetDirectoryName(fileName);

                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }
                    return fileName;
                }
            }


        }
    }

    //public class ConfigOperator
    //{
    //    [DllImport("kernel32", CallingConvention = CallingConvention.StdCall)]
    //    private static extern long WritePrivateProfileString(string section, string key, string value, string filepath);

    //    [DllImport("kernel32", CallingConvention = CallingConvention.StdCall)]
    //    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filepath);
    //    [DllImport("kernel32")]
    //    private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);
    //    [DllImport("kernel32")]
    //    private static extern uint GetPrivateProfileString(

    //        string lpAppName, // points to section name

    //        string lpKeyName, // points to key name

    //        string lpDefault, // points to default string

    //        byte[] lpReturnedString, // points to destination buffer

    //        uint nSize, // size of destination buffer

    //        string lpFileName  // points to initialization filename

    //    );
    //    /// <summary>
    //    /// 读取指定参数值的默认返回值
    //    /// </summary>
    //    public const string DefaultValue = "0";

    //    /// <summary>
    //    /// 保存指定的参数值
    //    /// </summary>
    //    /// <param name="section">section名称</param>
    //    /// <param name="key">键名称</param>
    //    /// <param name="value">值</param>
    //    /// <param name="filepath">ini文件名称</param>
    //    public static void SaveConfig(string section, string key, string value, string filepath)
    //    {
    //        WritePrivateProfileString(section, key, value, filepath);
    //    }
    //    /// <summary>
    //    /// 获取指定的参数值
    //    /// </summary>
    //    /// <param name="section">section名称</param>
    //    /// <param name="key">键名称</param>
    //    /// <param name="filepath">ini文件名称</param>
    //    /// <returns></returns>
    //    public static string GetConfig(string section, string key, string filepath)
    //    {
    //        StringBuilder temp = new StringBuilder(255);
    //        int i = GetPrivateProfileString(section, key, DefaultValue, temp, 255, filepath);
    //        return temp.ToString();
    //    }
    //    /// <summary>
    //    /// 获取指定ini文件中指定section下的所有Key值
    //    /// </summary>
    //    /// <param name="section">指定的section名称</param>
    //    /// <param name="fileName">ini文件名称</param>
    //    /// <returns>Key值集合</returns>
    //    public static List<string> GetStringofSection(string section, string fileName)
    //    {
    //        List<string> results = new List<string>();
    //        byte[] buffer = new byte[16384];
    //        int bufLen = GetPrivateProfileString(section, null, null, buffer, buffer.GetUpperBound(0), fileName);
    //        if (bufLen > 0)
    //        {
    //            int start = 0;
    //            for (int i = 0; i < bufLen; i++)
    //            {
    //                if ((buffer[i] == 0) && ((i - start) > 0))
    //                {
    //                    string s = Encoding.GetEncoding(0).GetString(buffer, start, i - start);
    //                    results.Add(s);
    //                    start = i + 1;
    //                }
    //            }
    //        }
    //        return results;
    //    }

    //    /// <summary>
    //    /// 获取指定ini文件中所有的Section名称
    //    /// </summary>
    //    /// <param name="iniFilename">指定的ini文件名称</param>
    //    /// <returns>所有section名称集合</returns>
    //    public static List<string> ReadSections(string iniFilename)
    //    {

    //        List<string> result = new List<string>();

    //        byte[] buf = new byte[65536];

    //        uint len = GetPrivateProfileString(null, null, null, buf, (uint)buf.Length, iniFilename);

    //        int j = 0;

    //        for (int i = 0; i < len; i++)
    //        {
    //            if (buf[i] == 0)
    //            {

    //                result.Add(Encoding.Default.GetString(buf, j, i - j));

    //                j = i + 1;

    //            }
    //        }

    //        return result;

    //    }
    //}
}
