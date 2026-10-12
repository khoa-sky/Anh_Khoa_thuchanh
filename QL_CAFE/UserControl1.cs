using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.QrCode.Internal;
using static System.Windows.Forms.LinkLabel;

namespace QL_CAFE
{
    public partial class UserControl1 : UserControl
    {
        PictureBox picBarcode;
        public event Action OnClickCard;
        public int MaMon { get; set; }
        public string TenMon { get; set; }
        public string Barcode { get; set; }
        public UserControl1()
        {
            InitializeComponent();         
            but_tick.Visible = false;

            picBarcode = new PictureBox();
            picBarcode.Size = new Size(250, 80);
            picBarcode.Visible = false;
            picBarcode.BackColor = Color.White;

            // đặt vị trí dưới card
            picBarcode.Location = new Point(5, this.Height - 45);

            this.Controls.Add(picBarcode);
            picBarcode.BringToFront();

            RegisterClick(this);


        }
        Image TaoBarcode(string code)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 100,
                    Margin = 10
                }
            };

            return writer.Write(code);
        }
        // ===== BẮT CLICK TOÀN BỘ CONTROL =====
        void RegisterClick(Control parent)
        {
            parent.Click += (s, e) =>
            {
                OnClickCard?.Invoke();
            };

            // ⭐ hover hiện barcode
            parent.MouseEnter += (s, e) =>
            {
                picBarcode.Visible = true;
            };

            parent.MouseLeave += (s, e) =>
            {
                picBarcode.Visible = false;
            };

            foreach (Control c in parent.Controls)
            {
                RegisterClick(c);
            }
        }
        
        void HienTick()
        {
            but_tick.Visible = true;
        }

        

        private void linepanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linepanel_Click(object sender, EventArgs e)
        {
           
        }

        private void UserControl1_Click(object sender, EventArgs e)
        {
           

        }

        private void but_tick_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        void ToggleTick()
        {
            but_tick.Visible = !but_tick.Visible;
        }





        public void SetData(int maMon, string ten, int gia, string hinhPath, string barcode)
        {
            MaMon = maMon;
            TenMon = ten;
            Barcode = barcode;
            label1.Text = ten;
            lab_gia.Text = gia.ToString("N0") + " VND";
            SetCardImage(hinhPath);

            var oldBarcode = picBarcode.Image;
            picBarcode.Image = null;
            oldBarcode?.Dispose();
            if (!string.IsNullOrWhiteSpace(barcode)) picBarcode.Image = TaoBarcode(barcode);
        }

        private void SetCardImage(string path)
        {
            var oldImage = guna2PictureBox1.Image;
            guna2PictureBox1.Image = null;
            oldImage?.Dispose();
            if (string.IsNullOrWhiteSpace(path)) return;

            string fullPath = Path.IsPathRooted(path)
                ? path
                : Path.Combine(Application.StartupPath, path);
            if (!File.Exists(fullPath)) return;
            using (var image = Image.FromFile(fullPath))
                guna2PictureBox1.Image = new Bitmap(image);
        }

        public void Chon()
        {
            but_tick.Visible = true;
        }

        public void BoChon()
        {
            but_tick.Visible = false;
        }
        string RemoveUnicode(string text)
        {
            text = text.Replace("đ", "d").Replace("Đ", "D");

            var normalized = text.Normalize(System.Text.NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (char c in normalized)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c)
                    != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            return builder.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }
    } 
  }

