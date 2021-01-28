namespace ICOP
{
    partial class ModelWizar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelWizar));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPCBCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbModelName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btCancle = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.lbUserCreater = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.nUD_PBA_Counter = new System.Windows.Forms.NumericUpDown();
            this.nUD_QR_Length = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_PBA_Counter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_QR_Length)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(52)))), ((int)(((byte)(93)))));
            this.panel1.Controls.Add(this.btClose);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(315, 24);
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
            this.btClose.Location = new System.Drawing.Point(285, 0);
            this.btClose.Margin = new System.Windows.Forms.Padding(0);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(30, 24);
            this.btClose.TabIndex = 2;
            this.btClose.Text = "O";
            this.btClose.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(109, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "MODEL WIZARD";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(52)))), ((int)(((byte)(93)))));
            this.label2.Location = new System.Drawing.Point(153, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 22);
            this.label2.TabIndex = 7;
            this.label2.Text = "PCB Code";
            // 
            // tbPCBCode
            // 
            this.tbPCBCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbPCBCode.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPCBCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tbPCBCode.Location = new System.Drawing.Point(157, 65);
            this.tbPCBCode.Name = "tbPCBCode";
            this.tbPCBCode.Size = new System.Drawing.Size(141, 28);
            this.tbPCBCode.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(52)))), ((int)(((byte)(93)))));
            this.label3.Location = new System.Drawing.Point(152, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 22);
            this.label3.TabIndex = 9;
            this.label3.Text = "Model Name";
            // 
            // tbModelName
            // 
            this.tbModelName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbModelName.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbModelName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tbModelName.Location = new System.Drawing.Point(157, 139);
            this.tbModelName.Name = "tbModelName";
            this.tbModelName.Size = new System.Drawing.Size(141, 28);
            this.tbModelName.TabIndex = 8;
            this.tbModelName.TextChanged += new System.EventHandler(this.tbModelName_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(52)))), ((int)(((byte)(93)))));
            this.label4.Location = new System.Drawing.Point(153, 186);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 22);
            this.label4.TabIndex = 11;
            this.label4.Text = "PBA Counter";
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
            this.btCancle.Location = new System.Drawing.Point(24, 352);
            this.btCancle.Name = "btCancle";
            this.btCancle.Size = new System.Drawing.Size(136, 41);
            this.btCancle.TabIndex = 13;
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
            this.btOK.Location = new System.Drawing.Point(171, 352);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(136, 41);
            this.btOK.TabIndex = 12;
            this.btOK.Text = "Next";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // lbUserCreater
            // 
            this.lbUserCreater.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUserCreater.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(52)))), ((int)(((byte)(93)))));
            this.lbUserCreater.Location = new System.Drawing.Point(9, 165);
            this.lbUserCreater.Name = "lbUserCreater";
            this.lbUserCreater.Size = new System.Drawing.Size(147, 149);
            this.lbUserCreater.TabIndex = 14;
            this.lbUserCreater.Text = "User creat information";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::ICOP.Properties.Resources.LightLogo;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(12, 56);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(109, 106);
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // nUD_PBA_Counter
            // 
            this.nUD_PBA_Counter.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nUD_PBA_Counter.Location = new System.Drawing.Point(156, 211);
            this.nUD_PBA_Counter.Maximum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.nUD_PBA_Counter.Name = "nUD_PBA_Counter";
            this.nUD_PBA_Counter.Size = new System.Drawing.Size(141, 28);
            this.nUD_PBA_Counter.TabIndex = 16;
            this.nUD_PBA_Counter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nUD_QR_Length
            // 
            this.nUD_QR_Length.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nUD_QR_Length.Location = new System.Drawing.Point(156, 286);
            this.nUD_QR_Length.Name = "nUD_QR_Length";
            this.nUD_QR_Length.Size = new System.Drawing.Size(141, 28);
            this.nUD_QR_Length.TabIndex = 18;
            this.nUD_QR_Length.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nUD_QR_Length.Value = new decimal(new int[] {
            23,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(52)))), ((int)(((byte)(93)))));
            this.label5.Location = new System.Drawing.Point(153, 261);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 22);
            this.label5.TabIndex = 17;
            this.label5.Text = "QR Length";
            // 
            // ModelWizar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 401);
            this.ControlBox = false;
            this.Controls.Add(this.nUD_QR_Length);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nUD_PBA_Counter);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbUserCreater);
            this.Controls.Add(this.btCancle);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbModelName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbPCBCode);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(1960, 1066);
            this.Name = "ModelWizar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_PBA_Counter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_QR_Length)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPCBCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbModelName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btCancle;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Label lbUserCreater;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.NumericUpDown nUD_PBA_Counter;
        private System.Windows.Forms.NumericUpDown nUD_QR_Length;
        private System.Windows.Forms.Label label5;
    }
}

