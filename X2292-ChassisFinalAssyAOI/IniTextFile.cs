using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace X2292_ChassisFinalAssyAOI
{
    public class IniFile
    {
        private static object obj1 = new object();
        private static object obj2 = new object();
        private static object obj3 = new object();
        private static object obj4 = new object();

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
            lock (obj1)
            {
                WritePrivateProfileString(section, key, value, filepath);
            }
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
            lock (obj2)
            {
                StringBuilder temp = new StringBuilder(255);
                int i = GetPrivateProfileString(section, key, DefaultValue, temp, 255, filepath);
                return temp.ToString();
            }
        }

        /// <summary>
        /// 获取指定ini文件中指定section下的所有Key值
        /// </summary>
        /// <param name="section">指定的section名称</param>
        /// <param name="fileName">ini文件名称</param>
        /// <returns>Key值集合</returns>
        public static List<string> GetStringofSection(string section, string fileName)
        {
            lock (obj3)
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
        }

        /// <summary>
        /// 获取指定ini文件中所有的Section名称
        /// </summary>
        /// <param name="iniFilename">指定的ini文件名称</param>
        /// <returns>所有section名称集合</returns>
        public static List<string> ReadSections(string iniFilename)
        {
            lock (obj4)
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
}
