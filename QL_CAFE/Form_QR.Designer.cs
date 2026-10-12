namespace QL_CAFE
{
    partial class Form_QR
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
            this.lblThongTin = new System.Windows.Forms.Label();
            this.picQR = new Guna.UI2.WinForms.Guna2PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picQR)).BeginInit();
            this.SuspendLayout();
            // 
            // lblThongTin
            // 
            this.lblThongTin.AutoSize = true;
            this.lblThongTin.Location = new System.Drawing.Point(162, 443);
            this.lblThongTin.Name = "lblThongTin";
            this.lblThongTin.Size = new System.Drawing.Size(51, 20);
            this.lblThongTin.TabIndex = 1;
            this.lblThongTin.Text = "label1";
            // 
            // picQR
            // 
            this.picQR.ImageRotate = 0F;
            this.picQR.Location = new System.Drawing.Point(166, 33);
            this.picQR.Name = "picQR";
            this.picQR.Size = new System.Drawing.Size(436, 407);
            this.picQR.TabIndex = 0;
            this.picQR.TabStop = false;
            // 
            // Form_QR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 581);
            this.Controls.Add(this.lblThongTin);
            this.Controls.Add(this.picQR);
            this.Name = "Form_QR";
            this.Text = "Form_QR";
            ((System.ComponentModel.ISupportInitialize)(this.picQR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblThongTin;
        private Guna.UI2.WinForms.Guna2PictureBox picQR;
    }
}