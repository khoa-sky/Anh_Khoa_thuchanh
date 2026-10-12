namespace QL_CAFE
{
    partial class UserControl1
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl1));
            this.lab_gia = new System.Windows.Forms.Label();
            this.linepanel = new Guna.UI.WinForms.GunaLinePanel();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.but_tick = new Guna.UI2.WinForms.Guna2GradientButton();
            this.guna2CirclePictureBox1 = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.linepanel.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lab_gia
            // 
            this.lab_gia.AutoSize = true;
            this.lab_gia.BackColor = System.Drawing.Color.Transparent;
            this.lab_gia.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_gia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lab_gia.Location = new System.Drawing.Point(145, 102);
            this.lab_gia.Name = "lab_gia";
            this.lab_gia.Size = new System.Drawing.Size(144, 32);
            this.lab_gia.TabIndex = 0;
            this.lab_gia.Text = "10.000VND";
            // 
            // linepanel
            // 
            this.linepanel.BackColor = System.Drawing.Color.Transparent;
            this.linepanel.Controls.Add(this.guna2Panel1);
            this.linepanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linepanel.LineBottom = 3;
            this.linepanel.LineColor = System.Drawing.Color.Tomato;
            this.linepanel.LineLeft = 3;
            this.linepanel.LineRight = 3;
            this.linepanel.LineStyle = System.Windows.Forms.BorderStyle.None;
            this.linepanel.LineTop = 3;
            this.linepanel.Location = new System.Drawing.Point(0, 0);
            this.linepanel.Margin = new System.Windows.Forms.Padding(10);
            this.linepanel.Name = "linepanel";
            this.linepanel.Size = new System.Drawing.Size(372, 152);
            this.linepanel.TabIndex = 3;
            this.linepanel.Click += new System.EventHandler(this.linepanel_Click);
            this.linepanel.Paint += new System.Windows.Forms.PaintEventHandler(this.linepanel_Paint);
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel1.Controls.Add(this.lab_gia);
            this.guna2Panel1.Controls.Add(this.label1);
            this.guna2Panel1.Controls.Add(this.but_tick);
            this.guna2Panel1.Controls.Add(this.guna2CirclePictureBox1);
            this.guna2Panel1.Controls.Add(this.guna2PictureBox1);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(372, 152);
            this.guna2Panel1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(176, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 28);
            this.label1.TabIndex = 3;
            this.label1.Text = "Cafe";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // but_tick
            // 
            this.but_tick.BorderRadius = 10;
            this.but_tick.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.but_tick.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.but_tick.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.but_tick.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.but_tick.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.but_tick.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.but_tick.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.but_tick.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.but_tick.ForeColor = System.Drawing.Color.White;
            this.but_tick.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.but_tick.Image = ((System.Drawing.Image)(resources.GetObject("but_tick.Image")));
            this.but_tick.ImageSize = new System.Drawing.Size(30, 30);
            this.but_tick.Location = new System.Drawing.Point(314, 3);
            this.but_tick.Name = "but_tick";
            this.but_tick.Size = new System.Drawing.Size(55, 54);
            this.but_tick.TabIndex = 2;
            this.but_tick.Click += new System.EventHandler(this.but_tick_Click);
            // 
            // guna2CirclePictureBox1
            // 
            this.guna2CirclePictureBox1.ImageRotate = 0F;
            this.guna2CirclePictureBox1.Location = new System.Drawing.Point(181, 64);
            this.guna2CirclePictureBox1.Name = "guna2CirclePictureBox1";
            this.guna2CirclePictureBox1.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2CirclePictureBox1.Size = new System.Drawing.Size(8, 8);
            this.guna2CirclePictureBox1.TabIndex = 1;
            this.guna2CirclePictureBox1.TabStop = false;
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.BorderRadius = 15;
            this.guna2PictureBox1.FillColor = System.Drawing.Color.Transparent;
            this.guna2PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("guna2PictureBox1.Image")));
            this.guna2PictureBox1.ImageRotate = 0F;
            this.guna2PictureBox1.Location = new System.Drawing.Point(17, 15);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.Size = new System.Drawing.Size(122, 119);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2PictureBox1.TabIndex = 0;
            this.guna2PictureBox1.TabStop = false;
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.linepanel);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(372, 152);
            this.Click += new System.EventHandler(this.UserControl1_Click);
            this.linepanel.ResumeLayout(false);
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lab_gia;
        private Guna.UI.WinForms.GunaLinePanel linepanel;
        private Guna.UI2.WinForms.Guna2GradientButton but_tick;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2CirclePictureBox guna2CirclePictureBox1;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private System.Windows.Forms.Label label1;
    }
}
