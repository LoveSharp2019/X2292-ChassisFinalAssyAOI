using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace X2292_ChassisFinalAssyAOI
{
    public partial class frm_ParaSetting : Form
    {
        public frm_ParaSetting(ApplicationParas para)
        {
            InitializeComponent();
            MyPara = para;

            propertyGrid1.SelectedObject = para.mParaSetting;
        }
        private ApplicationParas MyPara { set; get; }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyPara.mParaSetting.Save();
            MessageBox.Show("参数保存成功", "信息...", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
