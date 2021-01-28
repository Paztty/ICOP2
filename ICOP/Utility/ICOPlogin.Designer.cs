namespace ICOP
{
    partial class IcopLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IcopLogin));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btClose = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btCancle = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.pnChangePassword = new System.Windows.Forms.Panel();
            this.tbRePassNew = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbPasswordNew = new System.Windows.Forms.TextBox();
            this.tbPasswordOld = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbNotification = new System.Windows.Forms.Label();
            this.lbChangePass = new System.Windows.Forms.Label();
            this.cbbAcc = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.pnChangePassword.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(52)))), ((int)(((byte)(93)))));
            this.panel1.Controls.Add(this.btClose);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(477, 24);
            this.panel1.TabIndex = 0;
            // 
            // btClose
            // 
            this.btClose.AutoSize = true;
            this.btClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btClose.FlatAppearance.BorderSize = 0;
            this.btClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(11)))), ((int)(((byte)(23)))));
            this.btClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(17)))), ((int)(((byte)(35)))));
            this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btClose.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.btClose.ForeColor = System.Drawing.Color.Red;
            this.btClose.Location = new System.Drawing.Point(447, 0);
            this.btClose.Margin = new System.Windows.Forms.Padding(0);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(30, 24);
            this.btClose.TabIndex = 6;
            this.btClose.Text = "O";
            this.btClose.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::ICOP.Properties.Resources.LogoHeader;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(109, 24);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(477, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "LOGIN";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btCancle
            // 
            this.btCancle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(52)))), ((int)(((byte)(93)))));
            this.btCancle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btCancle.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(52)))), ((int)(((byte)(93)))));
            this.btCancle.FlatAppearance.BorderSize = 2;
            this.btCancle.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(53)))), ((int)(((byte)(78)))));
            this.btCancle.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(17)))), ((int)(((byte)(26)))));
            this.btCancle.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(34)))), ((int)(((byte)(62)))));
            this.btCancle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCancle.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCancle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btCancle.Location = new System.Drawing.Point(200, 180);
            this.btCancle.Name = "btCancle";
            this.btCancle.Size = new System.Drawing.Size(136, 43);
            this.btCancle.TabIndex = 5;
            this.btCancle.Text = "Cancle";
            this.btCancle.UseVisualStyleBackColor = false;
            this.btCancle.Click += new System.EventHandler(this.btCancle_Click);
            // 
            // btOK
            // 
            this.btOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btOK.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(52)))), ((int)(((byte)(93)))));
            this.btOK.FlatAppearance.BorderSize = 2;
            this.btOK.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(53)))), ((int)(((byte)(78)))));
            this.btOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(119)))), ((int)(((byte)(147)))));
            this.btOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(79)))), ((int)(((byte)(98)))));
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btOK.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btOK.Location = new System.Drawing.Point(341, 180);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(136, 43);
            this.btOK.TabIndex = 4;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(52)))), ((int)(((byte)(93)))));
            this.label3.Location = new System.Drawing.Point(8, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 22);
            this.label3.TabIndex = 7;
            this.label3.Text = "User name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(52)))), ((int)(((byte)(93)))));
            this.label2.Location = new System.Drawing.Point(8, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 22);
            this.label2.TabIndex = 8;
            this.label2.Text = "Password";
            // 
            // tbPassword
            // 
            this.tbPassword.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(52)))), ((int)(((byte)(93)))));
            this.tbPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tbPassword.Location = new System.Drawing.Point(185, 52);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(276, 27);
            this.tbPassword.TabIndex = 2;
            // 
            // pnChangePassword
            // 
            this.pnChangePassword.BackColor = System.Drawing.SystemColors.Control;
            this.pnChangePassword.Controls.Add(this.tbRePassNew);
            this.pnChangePassword.Controls.Add(this.label6);
            this.pnChangePassword.Controls.Add(this.tbPasswordNew);
            this.pnChangePassword.Controls.Add(this.tbPasswordOld);
            this.pnChangePassword.Controls.Add(this.label4);
            this.pnChangePassword.Controls.Add(this.label5);
            this.pnChangePassword.Location = new System.Drawing.Point(4, 30);
            this.pnChangePassword.Name = "pnChangePassword";
            this.pnChangePassword.Size = new System.Drawing.Size(477, 150);
            this.pnChangePassword.TabIndex = 11;
            // 
            // tbRePassNew
            // 
            this.tbRePassNew.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbRePassNew.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tbRePassNew.Location = new System.Drawing.Point(185, 83);
            this.tbRePassNew.Name = "tbRePassNew";
            this.tbRePassNew.PasswordChar = '*';
            this.tbRePassNew.Size = new System.Drawing.Size(280, 21);
            this.tbRePassNew.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(52)))), ((int)(((byte)(93)))));
            this.label6.Location = new System.Drawing.Point(12, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(167, 22);
            this.label6.TabIndex = 15;
            this.label6.Text = "Enter the password";
            // 
            // tbPasswordNew
            // 
            this.tbPasswordNew.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPasswordNew.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tbPasswordNew.Location = new System.Drawing.Point(185, 47);
            this.tbPasswordNew.Name = "tbPasswordNew";
            this.tbPasswordNew.PasswordChar = '*';
            this.tbPasswordNew.Size = new System.Drawing.Size(280, 21);
            this.tbPasswordNew.TabIndex = 2;
            // 
            // tbPasswordOld
            // 
            this.tbPasswordOld.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPasswordOld.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tbPasswordOld.Location = new System.Drawing.Point(185, 9);
            this.tbPasswordOld.Name = "tbPasswordOld";
            this.tbPasswordOld.PasswordChar = '*';
            this.tbPasswordOld.Size = new System.Drawing.Size(280, 21);
            this.tbPasswordOld.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(52)))), ((int)(((byte)(93)))));
            this.label4.Location = new System.Drawing.Point(12, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 22);
            this.label4.TabIndex = 12;
            this.label4.Text = "New password";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(52)))), ((int)(((byte)(93)))));
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 22);
            this.label5.TabIndex = 11;
            this.label5.Text = "Password";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbChangePass);
            this.panel2.Controls.Add(this.cbbAcc);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.tbPassword);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(4, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(473, 130);
            this.panel2.TabIndex = 12;
            // 
            // lbNotification
            // 
            this.lbNotification.AutoSize = true;
            this.lbNotification.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNotification.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lbNotification.Location = new System.Drawing.Point(1, 183);
            this.lbNotification.Name = "lbNotification";
            this.lbNotification.Size = new System.Drawing.Size(81, 17);
            this.lbNotification.TabIndex = 13;
            this.lbNotification.Text = "Notifications";
            // 
            // lbChangePass
            // 
            this.lbChangePass.AutoSize = true;
            this.lbChangePass.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbChangePass.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbChangePass.Location = new System.Drawing.Point(345, 87);
            this.lbChangePass.Name = "lbChangePass";
            this.lbChangePass.Size = new System.Drawing.Size(116, 17);
            this.lbChangePass.TabIndex = 3;
            this.lbChangePass.Text = "Change Password";
            this.lbChangePass.Click += new System.EventHandler(this.lbChangePass_Click);
            // 
            // cbbAcc
            // 
            this.cbbAcc.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cbbAcc.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbbAcc.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbAcc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(52)))), ((int)(((byte)(93)))));
            this.cbbAcc.FormattingEnabled = true;
            this.cbbAcc.Location = new System.Drawing.Point(185, 16);
            this.cbbAcc.Name = "cbbAcc";
            this.cbbAcc.Size = new System.Drawing.Size(276, 27);
            this.cbbAcc.Sorted = true;
            this.cbbAcc.TabIndex = 1;
            this.cbbAcc.SelectedIndexChanged += new System.EventHandler(this.cbbAcc_SelectedIndexChanged);
            // 
            // IcopLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 224);
            this.ControlBox = false;
            this.Controls.Add(this.lbNotification);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btCancle);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnChangePassword);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(1960, 1066);
            this.Name = "IcopLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.pnChangePassword.ResumeLayout(false);
            this.pnChangePassword.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Button btCancle;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Panel pnChangePassword;
        private System.Windows.Forms.TextBox tbRePassNew;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbPasswordNew;
        private System.Windows.Forms.TextBox tbPasswordOld;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cbbAcc;
        private System.Windows.Forms.Label lbChangePass;
        private System.Windows.Forms.Label lbNotification;
    }
}

