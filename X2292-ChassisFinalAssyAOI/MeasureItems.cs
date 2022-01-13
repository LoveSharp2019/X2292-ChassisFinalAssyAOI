using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.IO;

namespace X2292_ChassisFinalAssyAOI
{
   
    public class MeasureItems
    {
        private Dictionary<string, MeasureItem> _fais = new Dictionary<string, MeasureItem>();
        private string _station = "FAIs";
        private List<string[]> FAIlist;
     
        /// <summary> 
        /// 产品二维码
        /// </summary>
        public string SN
        {
            set;
            get;
        }
      
        /// <summary>
        /// 初始化一个测量集合
        /// </summary>
        /// <param name="stationName"></param>
        public MeasureItems(string stationName = "FAIs")
        {
            this._station = stationName;
            SN = string.Empty;
        }
        /// <summary>
        /// 测量站名
        /// </summary>
        public string Station
        {
            get
            {
                return this._station;
            }
            set { _station = value; }
        }

        /// <summary>
        /// 测量工站配置文件路径
        /// </summary>
        private string IniFileName
        {
            get
            {
                return string.Format("{0}/{1}.ini", System.Windows.Forms.Application.StartupPath+"//Settings", _station);
            }
        }

        /// <summary>
        /// 测量工站配置文件路径Csv
        /// </summary>
        private string IniFileNameCsv
        {
            get
            {
                return string.Format("{0}/{1}.csv", System.Windows.Forms.Application.StartupPath + "//Settings", _station);
            }
        }

        /// <summary>
        /// 所有的测量项字典
        /// </summary>
        public Dictionary<string, MeasureItem> FAIs
        {
            get
            {
                return this._fais;
            }
        }
        /// <summary>
        /// 所有测量的名称
        /// </summary>
        public  List<string> Names
        {
            get
            {
                return this._fais.Keys.ToList<string>();
            }
        }
        /// <summary>
        /// 所有测量值明细集合
        /// </summary>
        public List<MeasureItem> Values
        {
            get
            {
                return this._fais.Values.ToList<MeasureItem>();
            }
        }
        /// <summary>
        /// 所有测量项的总结果
        /// </summary>
        public bool Result
        {
            get
            {
                bool result = true;              
                foreach (MeasureItem item in this._fais.Values)
                {
                    if (!item.Result)
                    {
                        result = false;
                        break;                            
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 返回NG测量项的名字
        /// </summary>
        public List<string> Result_NG
        {
            get
            {
                List<string> ngName = new List<string>();
                
                foreach (MeasureItem item in this._fais.Values)
                {
                    if (!item.Result)
                    {
                        ngName.Add(item.ItemName);                        
                        continue;
                    }
                }               
                return ngName;
            }
        }

        /// <summary>
        /// 添加一个测量项
        /// </summary>
        /// <param name="item">测量项MeasureItem</param>
        public void AddItem(MeasureItem item)
        {
            this._fais.Add(item.ItemName, item);
        }
        /// <summary>
        /// 删除一个测量项
        /// </summary>
        /// <param name="name">测量项名称</param>
        /// <returns>删除结果</returns>
        public bool DeleteItem(string name)
        {
            return this._fais.Remove(name);
        }
        /// <summary>
        /// 清除所有测量项
        /// </summary>
        public void ClearItem()
        {
            this._fais = new Dictionary<string, MeasureItem>();
            this._fais.Clear();
        }
        /// <summary>
        /// 从本地文件初始化所有测量项
        /// </summary>
        /// <returns>初始化结果</returns>
        //public bool IniFaisFromFile()
        //{
        //    bool result = true;
           
        //    List<string> tempNames = ConfigOperator.ReadSections(this.IniFileName);
        //    this._fais = new Dictionary<string, MeasureItem>();
        //    this._fais.Clear();
        //    foreach (string name in tempNames)
        //    {
        //        MeasureItem item = new MeasureItem(name);
        //        string temp = item.IniItem(this.IniFileName);
        //        if (temp == string.Empty)
        //        {
        //            this.AddItem(item);
        //        }
        //        else
        //        {
        //            result = false;
        //        }
        //    }
        //    return result;
        //}

        public bool IniFaisFromFileCsv()
        {
            ReadCsv();
            bool result = true;
            this._fais = new Dictionary<string, MeasureItem>();
            this._fais.Clear();
            try
            {
                for (int i = 0; i < FAIlist.Count; i++)
                {
                    
                    MeasureItem item = new MeasureItem(FAIlist[i][0], Convert.ToDouble(FAIlist[i][2]), Convert.ToDouble(FAIlist[i][1]), Convert.ToDouble(FAIlist[i][3]), FAIlist[i][4],FAIlist[i][5]);
                    this._fais.Add(item.ItemName, item);
                }
            }
            catch 
            {
                result = false;
            }           
           
            return result;
        }

        private void ReadCsv()
        {
            FAIlist = new List<string[]>();
            try
            {
                string strLine;
                int i = 0;
                using (StreamReader sr = new StreamReader(this.IniFileNameCsv,Encoding.Default))
                {                    
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        i++;
                        if (i!=1)
                        {
                            string[] str = strLine.Split(',');
                            FAIlist.Add(str);
                        }                       
                    }
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }

           
        }
        /// <summary>
        /// 保存所有测量项
        /// </summary>
        /// <returns>保存结果</returns>
        public bool SaveAllFais()
        {
            bool result = true;
            if (System.IO.File.Exists(this.IniFileName))
            {
                System.IO.File.Delete(this.IniFileName);
            }
            foreach (MeasureItem item in this.Values)
            {
               string temp = item.SaveItem(this.IniFileName);
               if (temp != string.Empty)
               {
                   result = false;
               }
            }
            return result;
        }

        public string GetDisplayString()
        {
            if (this == null)
            {
                return "未设置信息";
            }

            return string.Format("Station:{0};Count:{1}", this.Station, this.FAIs.Count);
        }

        public override string ToString()
        {
            string result = string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (MeasureItem mi in this.Values)
            {
                sb.Append(string.Format("{0},", mi.FinnalValue.ToString("F3")));
            }
            result = sb.ToString().TrimEnd(',');
            return result;

        }

        public string SLString()
        {
            if (this != null)
            {

                StringBuilder sb = new StringBuilder();
                foreach (MeasureItem item in this._fais.Values)
                {
                    sb.Append(string.Format("{0},", item.NormalValue));

                }
                return sb.ToString().TrimEnd(',');
            }
            else
            {

                return string.Empty;
            }
        }

        public string USLString()
        {
            if (this != null)
            {

                StringBuilder sb = new StringBuilder();
                foreach (MeasureItem item in this._fais.Values)
                {
                    sb.Append(string.Format("{0},", item.Max));

                }
                return sb.ToString().TrimEnd(',');
            }
            else
            {

                return string.Empty;
            }
        }

        public string LSLString()
        {
            if (this != null)
            {

                StringBuilder sb = new StringBuilder();
                foreach (MeasureItem item in this._fais.Values)
                {
                    sb.Append(string.Format("{0},", item.Min));

                }
                return sb.ToString().TrimEnd(',');
            }
            else
            {

                return string.Empty;
            }
        }
        public string NamesString()
        {
            if (this != null)
            {
                List<string> tempV = this.Names;
                StringBuilder sb = new StringBuilder();
                foreach (string mi in tempV)
                {
                    sb.Append(string.Format("{0},", mi));

                }
                return sb.ToString().TrimEnd(',');
            }
            else
            {
                return string.Empty;
            }
        }                                           

        private List<string> SPCHNames = new List<string> { "FAI_8_1", "FAI_8_2", "FAI_8_3", "FAI_8_4", "FAI_8_5", "FAI_8_6", "FAI_8_7", "FAI_8_8", "FAI_8_9", "FAI_8_10" };


        /// <summary>
        /// SPCH 10个点
        /// </summary>
        public bool[] SPCH
        {
            get
            {
                bool[] mSPCH = new bool[10];
                foreach (MeasureItem item in this._fais.Values)
                {
                    if (SPCHNames.Contains(item.ItemName))
                    {
                        int indexof = SPCHNames.IndexOf(item.ItemName);
                        mSPCH[indexof] = item.Result;
                    }
                }
                return mSPCH;
            }

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
}
