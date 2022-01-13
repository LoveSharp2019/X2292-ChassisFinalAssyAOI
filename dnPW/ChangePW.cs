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
    public partial class ChangePW : Form
    {
        public ChangePW(string str,dnUsers dnusers)
        {
            InitializeComponent();
            this.tbUserName.Text = str;
            mUsers = dnusers;
        }

       private dnUsers mUsers;

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (mUsers.AdminConfirm(tbAdminPW.Text))
            {
                if(mUsers.UserConfirm(tbUserName.Text,tbOldPW.Text))
                {
                    if (tbNewPW.Text == tbNewPWagain.Text)
                    {
                        mUsers.ChangePW(tbUserName.Text, tbNewPW.Text);
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("两次输入密码不一致，请重新输入！", "修改密码...");
                        tbNewPWagain.Focus();
                        tbNewPWagain.SelectAll();
                    }
                }
                else
                {
                    MessageBox.Show("当前用户密码输入有误，请重新输入！", "修改密码...");
                    tbOldPW.Focus();
                    tbOldPW.SelectAll();
                }
            }
            else
            {
                MessageBox.Show("管理员密码错误，请重新输入！", "修改密码...");
                tbAdminPW.Focus();
                tbAdminPW.SelectAll();
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
