using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using dnProductRecipes;
using System.IO;


namespace dnCommConfig
{   
    public partial class frmComConfig : Form
    {

        ComConfig mConfig;
        public frmComConfig(ComConfig _config)
        {
            InitializeComponent();
            mConfig = _config;
           
            combShift.Items.Clear();
            foreach (ShiftIitem si in _config.Shift.Shifts)
            {
                combShift.Items.Add(si.ShiftName);
            }
            this.numMaxMonth.Value = _config.MaxMonthReserved;
        }
        private void frmComConfig_Load(object sender, EventArgs e)
        {
            tbImagePath.Text = mConfig.ImagePath_2D;
            textBox1.Text = mConfig.ImagePath_3D;
            tbDataPath.Text = mConfig.DataPath;
            combShift.SelectedIndex = 0;
            cbImageEnable.Checked = mConfig.Enable_Image;
            cbNGImageEnable.Checked = mConfig.Enable_NGImage;
            cbNGCode.Checked = mConfig.Enable_NGCode;
            tB_Time.Text = mConfig._真空吸延时.ToString();
            tbbaoguang.Text= mConfig._曝光时间.ToString();
            cbNGHummer.Checked = mConfig.Enable_Hummer;
            cbNGHand.Checked = mConfig.Enable_NGHand;
            cbNGAuto.Checked = mConfig.Enable_NGAuto;
            cbMES.Checked = mConfig.Enable_Mes;
        }

        private void btnImagePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            dialog.Description = "选择保存2D图片的路径";

            dialog.ShowNewFolderButton = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {

                this.tbImagePath.Text = dialog.SelectedPath;

            }
        }

        private void btnDathPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            dialog.Description = "选择保存数据的路径";

            dialog.ShowNewFolderButton = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.tbDataPath.Text = dialog.SelectedPath;
            }
        }

        private void combShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combShift.SelectedIndex != -1)
            {
                this.tbStartTime.Text = mConfig.Shift.Shifts[combShift.SelectedIndex].TimeShiftStart.ToString();
                this.tbEndTime.Text = mConfig.Shift.Shifts[combShift.SelectedIndex].TimeShiftEnd.ToString();
            }
        }

        private void btnShiftSet_Click(object sender, EventArgs e)
        {
            try
            {
             //   mConfig.Shift.Name=combShift
                mConfig.Shift.Shifts[combShift.SelectedIndex].TimeShiftStart = TimeSpan.Parse(tbStartTime.Text);
                mConfig.Shift.Shifts[combShift.SelectedIndex].TimeShiftEnd = TimeSpan.Parse(tbEndTime.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                mConfig.Enable_Image = this.cbImageEnable.Checked;
                mConfig.Enable_NGImage = this.cbNGImageEnable.Checked;
                mConfig.Enable_NGCode = this.cbNGCode.Checked;
                mConfig.Enable_Hummer = this.cbNGHummer.Checked;
                mConfig.Enable_Mes = this.cbMES.Checked;
                mConfig.Enable_NGAuto = this.cbNGAuto.Checked;
                mConfig.Enable_NGHand = this.cbNGHand.Checked;
                mConfig.ImagePath_2D = this.tbImagePath.Text;
                mConfig.ImagePath_3D = this.textBox1.Text;
                mConfig.DataPath = this.tbDataPath.Text;
                mConfig.MaxMonthReserved = Convert.ToInt32(this.numMaxMonth.Value);
                mConfig._真空吸延时 = Convert.ToInt32(tB_Time.Text);
                mConfig.Save();
                MessageBox.Show("参数保存成功！", "信息...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            dialog.Description = "选择保存3D图片的路径";

            dialog.ShowNewFolderButton = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {

                this.textBox1.Text = dialog.SelectedPath;

            }
        }

        private void cbNGAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (cbNGAuto.Checked)
            {
                cbNGHand.Checked = false;
            }
            else
            {
                cbNGHand.Checked = true;
            }
        }

        private void cbNGHand_CheckedChanged(object sender, EventArgs e)
        {
            if (cbNGHand.Checked)
            {
                cbNGAuto.Checked = false;
            }
            else
            {
                cbNGAuto.Checked = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mConfig._曝光时间 = Convert.ToInt32(tbbaoguang.Text);
            mConfig.savebaoguang();
        }
    }
}
