using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FocalSpec.Model.Settings;

namespace X2292_ChassisFinalAssyAOI
{
  public  class Recipe:SingleInstance<Recipe>
    {

        /// <summary>
        /// 改变配方事件
        /// </summary>
        public event Action<string> ChangeRecipe;

        /// <summary>
        /// 配方参数
        /// </summary>
        public Recipe_Para _Recipe_Para { get; set; } = new Recipe_Para();

        public Ini _AppIni;

        public string CurrentRecipe = string.Empty;
        private string strRecipePath = string.Empty;

        public Recipe()
        {
            strRecipePath = System.IO.Directory.GetCurrentDirectory() + "\\Recipe\\";
            if (!System.IO.Directory.Exists(strRecipePath))
            {
                System.IO.Directory.CreateDirectory(strRecipePath);

            }

            _AppIni = new Ini();
            _AppIni.FileName = "./Recipe.ini";
            _AppIni.onCannotRead += _AppIni_onCannotRead;
           

            _AppIni.GetValue("PRO", "CurrentPRO", out CurrentRecipe);
        }

        private void _AppIni_onCannotRead(Ini ini, string section, string key)
        {
           DialogResult Result =   MessageBox.Show($"不存在段名字：{section}，键名字{key}的键值。是否要创建默认程序","提示：",MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
           if(Result == DialogResult.OK)
            {
                _AppIni.SetValue("PRO", "CurrentPRO", "Default.dtg");

                SaveRecipe("Default.dtg");
            }
           
        }

        /// <summary>
        /// 加载配方
        /// </summary>
        public void LoadRecipe(string strRecipe)
        {
            if (System.IO.File.Exists(strRecipePath + strRecipe))
            {
                string json = System.IO.File.ReadAllText(strRecipePath + strRecipe);
                _Recipe_Para = Newtonsoft.Json.JsonConvert.DeserializeObject<Recipe_Para>(json);
                ChangeRecipe?.Invoke(strRecipe);
                _AppIni.SetValue("PRO", "CurrentPRO", strRecipe);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("当前文件不存在");
            }

        }

        /// <summary>
        /// 保存配方
        /// </summary>
        /// <param name="strRecipe"></param>
        public void SaveRecipe(string strRecipe)
        {
            using (System.IO.StreamWriter file = System.IO.File.CreateText(strRecipePath + strRecipe))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(file, _Recipe_Para);
            }
        }

    }


    /// <summary>
    /// 配方里面所有参数
    /// </summary>
    public class Recipe_Para
    {
        /// <summary>
        /// 左激光检测站
        /// </summary>
        public List<TablePara_LaserLeft_Item> DT_LeftLaser { get; set; } = new List<TablePara_LaserLeft_Item>();

        /// <summary>
        /// 右激光检测站
        /// </summary>
        public List<TablePara_LaserRight_Item> DT_RightLaser { get; set; } = new List<TablePara_LaserRight_Item>();

        /// <summary>
        /// 左翻转站
        /// </summary>
        public List<TablePara_RolledLeft_Item> DT_LeftRolled { get; set; } = new List<TablePara_RolledLeft_Item>();

        /// <summary>
        /// 右翻转站
        /// </summary>
        public List<TablePara_RolledRight_Item> DT_RightRolled { get; set; } = new List<TablePara_RolledRight_Item>();


        /// <summary>
        /// OK-Load
        /// </summary>
        public List<TablePara_Z_Item> DT_OK_Load { get; set; } = new List<TablePara_Z_Item>();

        /// <summary>
        /// OK-UpLoad
        /// </summary>
        public List<TablePara_Z_Item> DT_OK_UpLoad { get; set; } = new List<TablePara_Z_Item>();


        /// <summary>
        /// NG-Load
        /// </summary>
        public List<TablePara_Z_Item> DT_NG_Load { get; set; } = new List<TablePara_Z_Item>();

        /// <summary>
        /// NG-UpLoad
        /// </summary>
        public List<TablePara_Z_Item> DT_NG_UpLoad { get; set; } = new List<TablePara_Z_Item>();

        /// <summary>
        ///  ToDo添加其他配方参数
        /// </summary>

        public Recipe_Para()
        {           
        }

    }

  
    /// <summary>
    /// 激光左检测站
    /// </summary>
    public class TablePara_LaserLeft_Item
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 点位功能描述
        /// </summary>
        public string Description { get; set; } = "NA";

        /// <summary>
        /// 速度
        /// </summary>
        public double Vel { get; set; }

        /// <summary>
        /// 加/减速
        /// </summary>
        public double Acc { get; set; }

        /// <summary>
        /// Laser-X
        /// </summary>
        public double XPos { get; set; }

        /// <summary>
        /// Laser-Y
        /// </summary>
        public double YPos { get; set; }

        /// <summary>
        /// 左工位平台
        /// </summary>
        public double Y1Pos { get; set; }
    }

    /// <summary>
    /// 激光右检测站
    /// </summary>
    public class TablePara_LaserRight_Item
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 点位功能描述
        /// </summary>
        public string Description { get; set; } = "NA";

        /// <summary>
        /// 速度
        /// </summary>
        public double Vel { get; set; }

        /// <summary>
        /// 加/减速
        /// </summary>
        public double Acc { get; set; }

        /// <summary>
        /// Laser-X
        /// </summary>
        public double XPos { get; set; }

        /// <summary>
        /// Laser-Y
        /// </summary>
        public double YPos { get; set; }

        /// <summary>
        /// 右工位平台
        /// </summary>
        public double Y2Pos { get; set; }
    }

    /// <summary>
    /// 翻转左站
    /// </summary>
    public class TablePara_RolledLeft_Item
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 点位功能描述
        /// </summary>
        public string Description { get; set; } = "NA";

        /// <summary>
        /// 速度
        /// </summary>
        public double Vel { get; set; }

        /// <summary>
        /// 加/减速
        /// </summary>
        public double Acc { get; set; }

        /// <summary>
        /// 翻转机构-X
        /// </summary>
        public double XPos { get; set; }

        /// <summary>
        /// 翻转机构-Y
        /// </summary>
        public double ZPos { get; set; }

        /// <summary>
        /// 翻转机构-R
        /// </summary>
        public double RPos { get; set; }
    }

    /// <summary>
    /// 翻转右站
    /// </summary>
    public class TablePara_RolledRight_Item
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 点位功能描述
        /// </summary>
        public string Description { get; set; } = "NA";

        /// <summary>
        /// 速度
        /// </summary>
        public double Vel { get; set; }

        /// <summary>
        /// 加/减速
        /// </summary>
        public double Acc { get; set; }

        /// <summary>
        /// 翻转机构-X
        /// </summary>
        public double XPos { get; set; }

        /// <summary>
        /// 翻转机构-Y
        /// </summary>
        public double ZPos { get; set; }

        /// <summary>
        /// 翻转机构-R
        /// </summary>
        public double RPos { get; set; }
    }

    /// <summary>
    /// -Z
    /// </summary>
    public class TablePara_Z_Item
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 点位功能描述
        /// </summary>
        public string Description { get; set; } = "NA";

        /// <summary>
        /// 速度
        /// </summary>
        public double Vel { get; set; }

        /// <summary>
        /// 加/减速
        /// </summary>
        public double Acc { get; set; }

        /// <summary>
        /// Z
        /// </summary>
        public double ZPos { get; set; }

    
    }

}
