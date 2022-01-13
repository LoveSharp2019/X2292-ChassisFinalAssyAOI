namespace dnPW
{
    partial class ChangePW
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangePW));
            this.label1 = new System.Windows.Forms.Label();
            this.tbAdminPW = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbOldPW = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbNewPW = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbNewPWagain = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(70, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "管理员密码";
            // 
            // tbAdminPW
            // 
            this.tbAdminPW.Location = new System.Drawing.Point(146, 13);
            this.tbAdminPW.Name = "tbAdminPW";
            this.tbAdminPW.PasswordChar = '*';
            this.tbAdminPW.Size = new System.Drawing.Size(269, 21);
            this.tbAdminPW.TabIndex = 1;
            this.tbAdminPW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(70, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "账户旧密码";
            // 
            // tbOldPW
            // 
            this.tbOldPW.Location = new System.Drawing.Point(146, 79);
            this.tbOldPW.Name = "tbOldPW";
            this.tbOldPW.PasswordChar = '*';
            this.tbOldPW.Size = new System.Drawing.Size(269, 21);
            this.tbOldPW.TabIndex = 1;
            this.tbOldPW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(96, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "新密码";
            // 
            // tbNewPW
            // 
            this.tbNewPW.Location = new System.Drawing.Point(146, 112);
            this.tbNewPW.Name = "tbNewPW";
            this.tbNewPW.PasswordChar = '*';
            this.tbNewPW.Size = new System.Drawing.Size(269, 21);
            this.tbNewPW.TabIndex = 1;
            this.tbNewPW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnOK
            // 
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(50, 203);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(147, 29);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确认";
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
            this.btnCancel.Location = new System.Drawing.Point(288, 203);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(147, 29);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.MouseEnter += new System.EventHandler(this.MouseIn);
            this.btnCancel.MouseLeave += new System.EventHandler(this.MouseOut);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "要修改的账户名称";
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(146, 46);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.ReadOnly = true;
            this.tbUserName.Size = new System.Drawing.Size(269, 21);
            this.tbUserName.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(70, 149);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "新密码确认";
            // 
            // tbNewPWagain
            // 
            this.tbNewPWagain.Location = new System.Drawing.Point(146, 145);
            this.tbNewPWagain.Name = "tbNewPWagain";
            this.tbNewPWagain.PasswordChar = '*';
            this.tbNewPWagain.Size = new System.Drawing.Size(269, 21);
            this.tbNewPWagain.TabIndex = 1;
            this.tbNewPWagain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ChangePW
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(488, 244);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbNewPWagain);
            this.Controls.Add(this.tbNewPW);
            this.Controls.Add(this.tbUserName);
            this.Controls.Add(this.tbOldPW);
            this.Controls.Add(this.tbAdminPW);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChangePW";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "修改密码";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbAdminPW;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbOldPW;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbNewPW;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbNewPWagain;
    }
}