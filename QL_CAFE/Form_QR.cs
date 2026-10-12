using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace QL_CAFE
{
    public partial class Form_QR : Form
    {
        
        public Form_QR(string noiDung)
        {
            InitializeComponent();
            lblThongTin.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            this.StartPosition = FormStartPosition.CenterScreen;
            lblThongTin.Text = noiDung;
            picQR.Image = TaoQR(noiDung);

            var btnConfirm = new Button { Text = "Đã thanh toán", DialogResult = DialogResult.OK, Width = 120, Height = 36 };
            var btnCancel = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, Width = 80, Height = 36 };
            btnCancel.Location = new Point(ClientSize.Width - btnCancel.Width - 16, ClientSize.Height - btnCancel.Height - 12);
            btnConfirm.Location = new Point(btnCancel.Left - btnConfirm.Width - 8, btnCancel.Top);
            btnConfirm.Anchor = btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Controls.Add(btnConfirm);
            Controls.Add(btnCancel);
            AcceptButton = btnConfirm;
            CancelButton = btnCancel;
        }
        Image TaoQR(string text)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 300
                }
            };

            return writer.Write(text);
        }

    }
}

