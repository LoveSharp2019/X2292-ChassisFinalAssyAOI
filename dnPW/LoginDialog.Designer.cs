namespace dnPW
{
    partial class LoginDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginDialog));
            this.cobUsers = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPW = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnChangePw = new System.Windows.Forms.Button();
            this.cbAutoLogout = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cobUsers
            // 
            this.cobUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobUsers.FormattingEnabled = true;
            this.cobUsers.Location = new System.Drawing.Point(165, 38);
            this.cobUsers.Name = "cobUsers";
            this.cobUsers.Size = new System.Drawing.Size(140, 20);
            this.cobUsers.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(78, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "用户名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(91, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "密码";
            // 
            // tbPW
            // 
            this.tbPW.Location = new System.Drawing.Point(165, 87);
            this.tbPW.Name = "tbPW";
            this.tbPW.PasswordChar = '*';
            this.tbPW.Size = new System.Drawing.Size(140, 21);
            this.tbPW.TabIndex = 2;
            this.tbPW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnOK
            // 
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(47, 150);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.btnOK.MouseEnter += new System.EventHandler(this.MouseIn);
            this.btnOK.MouseLeave += new System.EventHandler(this.MouseOut);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(285, 150);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.MouseEnter += new System.EventHandler(this.MouseIn);
            this.btnCancel.MouseLeave += new System.EventHandler(this.MouseOut);
            // 
            // btnChangePw
            // 
            this.btnChangePw.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnChangePw.FlatAppearance.BorderSize = 0;
            this.btnChangePw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangePw.Location = new System.Drawing.Point(345, 195);
            this.btnChangePw.Name = "btnChangePw";
            this.btnChangePw.Size = new System.Drawing.Size(75, 23);
            this.btnChangePw.TabIndex = 3;
            this.btnChangePw.Text = "修改密码";
            this.btnChangePw.UseVisualStyleBackColor = true;
            this.btnChangePw.Click += new System.EventHandler(this.btnChangePw_Click);
            this.btnChangePw.MouseEnter += new System.EventHandler(this.MouseIn);
            this.btnChangePw.MouseLeave += new System.EventHandler(this.MouseOut);
            // 
            // cbAutoLogout
            // 
            this.cbAutoLogout.AutoSize = true;
            this.cbAutoLogout.Location = new System.Drawing.Point(7, 200);
            this.cbAutoLogout.Name = "cbAutoLogout";
            this.cbAutoLogout.Size = new System.Drawing.Size(115, 16);
            this.cbAutoLogout.TabIndex = 4;
            this.cbAutoLogout.Text = "自动退出管理员";
            this.cbAutoLogout.UseVisualStyleBackColor = true;
            // 
            // LoginDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(421, 219);
            this.Controls.Add(this.cbAutoLogout);
            this.Controls.Add(this.btnChangePw);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbPW);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cobUsers);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "LoginDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cobUsers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPW;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnChangePw;
        private System.Windows.Forms.CheckBox cbAutoLogout;
    }
}