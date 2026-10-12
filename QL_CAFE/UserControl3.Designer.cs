namespace QL_CAFE
{
    partial class UserControl3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl3));
            this.guna2ShadowPanel1 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.lab_ten = new System.Windows.Forms.Label();
            this.linepanel = new Guna.UI.WinForms.GunaLinePanel();
            this.but_tick = new Guna.UI2.WinForms.Guna2GradientButton();
            this.guna2ShadowPanel1.SuspendLayout();
            this.linepanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2ShadowPanel1
            // 
            this.guna2ShadowPanel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel1.Controls.Add(this.lab_ten);
            this.guna2ShadowPanel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.guna2ShadowPanel1.Location = new System.Drawing.Point(3, 3);
            this.guna2ShadowPanel1.Name = "guna2ShadowPanel1";
            this.guna2ShadowPanel1.ShadowColor = System.Drawing.Color.Black;
            this.guna2ShadowPanel1.ShadowDepth = 10;
            this.guna2ShadowPanel1.Size = new System.Drawing.Size(112, 38);
            this.guna2ShadowPanel1.TabIndex = 0;
            // 
            // lab_ten
            // 
            this.lab_ten.AutoSize = true;
            this.lab_ten.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_ten.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_ten.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lab_ten.Location = new System.Drawing.Point(0, 0);
            this.lab_ten.Name = "lab_ten";
            this.lab_ten.Size = new System.Drawing.Size(73, 28);
            this.lab_ten.TabIndex = 0;
            this.lab_ten.Text = "BÀN 1";
            this.lab_ten.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linepanel
            // 
            this.linepanel.BackColor = System.Drawing.Color.Transparent;
            this.linepanel.Controls.Add(this.guna2ShadowPanel1);
            this.linepanel.Controls.Add(this.but_tick);
            this.linepanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linepanel.LineBottom = 3;
            this.linepanel.LineColor = System.Drawing.Color.Tomato;
            this.linepanel.LineLeft = 3;
            this.linepanel.LineRight = 3;
            this.linepanel.LineStyle = System.Windows.Forms.BorderStyle.None;
            this.linepanel.LineTop = 3;
            this.linepanel.Location = new System.Drawing.Point(0, 0);
            this.linepanel.Name = "linepanel";
            this.linepanel.Size = new System.Drawing.Size(270, 150);
            this.linepanel.TabIndex = 1;
            this.linepanel.Click += new System.EventHandler(this.linepanel_Click);
            this.linepanel.Paint += new System.Windows.Forms.PaintEventHandler(this.linepanel_Paint);
            // 
            // but_tick
            // 
            this.but_tick.BorderRadius = 10;
            this.but_tick.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.but_tick.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.but_tick.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.but_tick.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.but_tick.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.but_tick.FillColor = System.Drawing.Color.Red;
            this.but_tick.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.but_tick.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.but_tick.ForeColor = System.Drawing.Color.White;
            this.but_tick.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.but_tick.Image = ((System.Drawing.Image)(resources.GetObject("but_tick.Image")));
            this.but_tick.ImageSize = new System.Drawing.Size(30, 30);
            this.but_tick.Location = new System.Drawing.Point(212, 96);
            this.but_tick.Name = "but_tick";
            this.but_tick.Size = new System.Drawing.Size(55, 54);
            this.but_tick.TabIndex = 3;
            // 
            // UserControl3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Controls.Add(this.linepanel);
            this.DoubleBuffered = true;
            this.Name = "UserControl3";
            this.Size = new System.Drawing.Size(270, 150);
            this.Load += new System.EventHandler(this.UserControl3_Load);
            this.Click += new System.EventHandler(this.UserControl3_Click);
            this.guna2ShadowPanel1.ResumeLayout(false);
            this.guna2ShadowPanel1.PerformLayout();
            this.linepanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel1;
        private System.Windows.Forms.Label lab_ten;
        private Guna.UI.WinForms.GunaLinePanel linepanel;
        private Guna.UI2.WinForms.Guna2GradientButton but_tick;
    }
}
