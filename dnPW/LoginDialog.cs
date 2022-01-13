using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dnPW
{
    public partial class LoginDialog : Form
    {
        public LoginDialog(dnUsers dnusers)
        {
            InitializeComponent();
            mUsers = dnusers;
            this.cbAutoLogout.Checked = mUsers.IsAutoLogout;
        }

        private dnUsers mUsers;

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(cobUsers.Text))
            { 
                mUsers.IsAutoLogout = cbAutoLogout.Checked;
                if (!mUsers.LogIn(cobUsers.SelectedIndex, tbPW.Text))
                {
                    //MessageBox.Show("当前用户密码输入有误，请重新输入！", "修改密码...");
                    tbPW.Focus();
                    tbPW.SelectAll();
                }
                else
                {

                    this.DialogResult = DialogResult.OK;
                }
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.cobUsers.DataSource = mUsers.UserNames;
            //if (mUsers.UserLevel == 2)
            //{
            //    btnChangePw.Visible = true;
            //}
            //else
            //{
            //    btnChangePw.Visible = false;
            //}
        }

        private void btnChangePw_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cobUsers.Text))
            {
                mUsers.SetPwDialog(cobUsers.Text);
            }
        }

        #region//鼠标移动事件
        private void MouseIn(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.ForeColor = Color.LawnGreen;
            button.BackColor = button.Parent.BackColor;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = Color.LawnGreen;
            button.Font = new Font(button.Font.Name, button.Font.Size, FontStyle.Bold);
        }

        private void MouseOut(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.ForeColor = SystemColors.ControlText;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font(button.Font.Name, button.Font.Size, FontStyle.Regular);
            button.BackColor = button.Parent.BackColor;
        }

        #endregion

       
    }
}
