using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows.Forms;

namespace QL_CAFE
{
    


    public partial class Form1 : Form
    {
        GIOI_THIEU fGioiThieu;
        string currentUser;
        string currentRole;
        QL_MON fQLMon;
        DO_UONG fDoUong;
        public DAC_BAN fBan;
        public QL_DON fDon;
        TRANG_CHU fTrangChu;      
        public Form1(string user, string role)
        {
            InitializeComponent();
            currentUser = user;
            currentRole = role;
        }


        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            

        }
        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            panel1.Visible = false;

            if (currentRole == "staff")
            {
                guna2Button6.Visible = false; 
                guna2Button7.Visible = false; 
                guna2Button8.Visible = false; 
            }

            
            if (currentRole == "admin")
                guna2Button5.Text = "Quản lý";
            else
                guna2Button5.Text = "Nhân viên";


        }
       

        private void guna2PictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // ===== KHUNG =====
            Rectangle rect = new Rectangle(40, 40, 500, 180);

            // nền mờ (glass)
            Color bgColor = Color.FromArgb(120, 0, 0, 0); // đen mờ

            using (SolidBrush bgBrush = new SolidBrush(bgColor))
            {
                g.FillRoundedRectangle(bgBrush, rect, 20);
            }

            // ===== VIỀN =====
            using (Pen pen = new Pen(Color.FromArgb(180, 255, 255, 255), 1))
            {
                g.DrawRoundedRectangle(pen, rect, 20);
            }

            // ===== TEXT =====
            string title = "Chào mừng đến với";
            string name = "Cafe KT ☕";
            string desc = "Quản lý quán của bạn một cách dễ dàng & chuyên nghiệp";

            Font fontTitle = new Font("Segoe UI", 16, FontStyle.Regular);
            Font fontName = new Font("Segoe UI", 26, FontStyle.Bold);
            Font fontDesc = new Font("Segoe UI", 10, FontStyle.Italic);

            int x = rect.X + 20;
            int y = rect.Y + 20;

            // shadow
            Brush shadow = new SolidBrush(Color.FromArgb(150, 0, 0, 0));

            // ----- TITLE -----
            g.DrawString(title, fontTitle, shadow, new Point(x + 2, y + 2));
            g.DrawString(title, fontTitle, Brushes.White, new Point(x, y));

            // ----- NAME -----
            y += 30;
            Brush nameColor = new SolidBrush(Color.FromArgb(255, 215, 0)); // vàng cafe
            g.DrawString(name, fontName, shadow, new Point(x + 2, y + 2));
            g.DrawString(name, fontName, nameColor, new Point(x, y));

            // ----- DESC -----
            y += 45;
            g.DrawString(desc, fontDesc, shadow, new Point(x + 2, y + 2));
            g.DrawString(desc, fontDesc, Brushes.LightGray, new Point(x, y));
            // ===== KHUNG GIỚI THIỆU DƯỚI =====
            Rectangle rect2 = new Rectangle(40, 250, 600, 200);

            // nền mờ
            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
            {
                g.FillRoundedRectangle(bgBrush, rect2, 20);
            }

            // viền
            using (Pen pen = new Pen(Color.FromArgb(150, 255, 255, 255), 1))
            {
                g.DrawRoundedRectangle(pen, rect2, 20);
            }
            // ===== LOAD ICON =====
            Image iconMoney = Image.FromFile(Application.StartupPath + "\\salary.png");
            Image iconTable = Image.FromFile(Application.StartupPath + "\\round-table.png");
            Image iconDrink = Image.FromFile(Application.StartupPath + "\\drink.png");
            Image iconPay = Image.FromFile(Application.StartupPath + "\\atm-card.png");

            // ===== TIÊU ĐỀ =====
            Font headerFont = new Font("Segoe UI", 14, FontStyle.Bold);
            Brush headerColor = new SolidBrush(Color.FromArgb(255, 215, 0)); // vàng cafe

            string header = "Tính năng nổi bật";

            g.DrawString(header, headerFont, headerColor, rect2.X + 20, rect2.Y + 10);

            // ===== FONT =====
            Font titleFont = new Font("Segoe UI", 11, FontStyle.Bold);
            Font descFont = new Font("Segoe UI", 9);

            int startX = rect2.X + 20;
            int startY = rect2.Y + 50;

            // ===== CỘT TRÁI =====
            g.DrawImage(iconMoney, new Rectangle(startX, startY, 30, 30));
            g.DrawString("Quản lý doanh thu", titleFont, Brushes.White, startX + 30, startY);
            g.DrawString("Theo dõi bán hàng mỗi ngày", descFont, Brushes.LightGray, startX + 30, startY + 20);

            g.DrawImage(iconDrink, new Rectangle(startX, startY + 70, 30, 30));
            g.DrawString("Order nhanh", titleFont, Brushes.White, startX + 30, startY + 70);
            g.DrawString("Thêm món dễ dàng", descFont, Brushes.LightGray, startX + 30, startY + 90);

            // ===== CỘT PHẢI =====
            int col2 = startX + 280;

            g.DrawImage(iconTable, new Rectangle(col2, startY, 30, 30));
            g.DrawString("Quản lý bàn", titleFont, Brushes.White, col2 + 30, startY);
            g.DrawString("Theo dõi trạng thái bàn", descFont, Brushes.LightGray, col2 + 30, startY + 20);

            g.DrawImage(iconPay, new Rectangle(col2, startY + 70, 30, 30));
            g.DrawString("Thanh toán", titleFont, Brushes.White, col2 + 30, startY + 70);
            g.DrawString("Xuất hóa đơn nhanh", descFont, Brushes.LightGray, col2 + 30, startY + 90);
        }

        private void guna2Panel_top_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            
                panel1.Visible = false;
            
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel1.BringToFront();

            if (fDoUong == null)
            {
                fDoUong = new DO_UONG();

                fDoUong.OnThanhToanThanhCong += () =>
                {
                    // ===== DASHBOARD =====
                    if (fTrangChu != null && !fTrangChu.IsDisposed)
                    {
                        fTrangChu.LoadDashboard();
                    }

                    // ===== QUẢN LÝ ĐƠN =====
                    if (fDon != null && !fDon.IsDisposed)
                    {
                        fDon.LoadDonTheoNgayVaBan();
                    }

                    // ===== ĐẶT BÀN =====
                    if (fBan != null && !fBan.IsDisposed)
                    {
                        fBan.ReloadCurrent();
                    }
                };

                fDoUong.TopLevel = false;
                fDoUong.FormBorderStyle = FormBorderStyle.None;
                fDoUong.Dock = DockStyle.Fill;
                panel1.Controls.Add(fDoUong);
            }

            fDoUong.BringToFront();
            fDoUong.Show();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel1.BringToFront();

            if (fBan == null)
            {
                fBan = new DAC_BAN();
                fBan.formMain = this;
                fBan.TopLevel = false;
                fBan.FormBorderStyle = FormBorderStyle.None;
                fBan.Dock = DockStyle.Fill;
                panel1.Controls.Add(fBan);
            }

            fBan.BringToFront();
            fBan.Show();
        }
        public void OpenOrder(int maBan)
        {
            panel1.Visible = true;
            panel1.BringToFront();

            if (fDoUong == null || fDoUong.IsDisposed)
            {
                fDoUong = new DO_UONG();

                fDoUong.TopLevel = false;
                fDoUong.FormBorderStyle = FormBorderStyle.None;
                fDoUong.Dock = DockStyle.Fill;
                panel1.Controls.Add(fDoUong);
            }

            fDoUong.formBan = fBan;   
            fDoUong.SetBan(maBan);

            fDoUong.BringToFront();
            fDoUong.Show();
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            TAI_KHOAN f = new TAI_KHOAN(currentUser);
            f.ShowDialog();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel1.BringToFront();

            if (fGioiThieu == null || fGioiThieu.IsDisposed)
            {
                fGioiThieu = new GIOI_THIEU();

                fGioiThieu.TopLevel = false;
                fGioiThieu.FormBorderStyle = FormBorderStyle.None;
                fGioiThieu.Dock = DockStyle.Fill;

                panel1.Controls.Add(fGioiThieu);
            }

            fGioiThieu.BringToFront();
            fGioiThieu.Show();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel1.BringToFront();

            if (fQLMon == null || fQLMon.IsDisposed)
            {
                fQLMon = new QL_MON();
                fQLMon.MenuChanged += () =>
                {
                    if (fDoUong != null && !fDoUong.IsDisposed)
                        fDoUong.ReloadMenuCards();
                };

                fQLMon.TopLevel = false;
                fQLMon.FormBorderStyle = FormBorderStyle.None;
                fQLMon.Dock = DockStyle.Fill;

                panel1.Controls.Add(fQLMon);
            }

            fQLMon.BringToFront();
            fQLMon.Show();
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel1.BringToFront();

            if (fTrangChu == null || fTrangChu.IsDisposed)
            {
                fTrangChu = new TRANG_CHU();

                fTrangChu.TopLevel = false;
                fTrangChu.FormBorderStyle = FormBorderStyle.None;
                fTrangChu.Dock = DockStyle.Fill;

                panel1.Controls.Add(fTrangChu);
            }

            fTrangChu.BringToFront();
            fTrangChu.Show();

           
            fTrangChu.LoadDashboard();
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel1.BringToFront();

            if (fDon == null || fDon.IsDisposed)
            {
                fDon = new QL_DON();
                fDon.formMain = this;

                fDon.TopLevel = false;
                fDon.FormBorderStyle = FormBorderStyle.None;
                fDon.Dock = DockStyle.Fill;

                panel1.Controls.Add(fDon);
            }

            fDon.BringToFront();
            fDon.Show();
        }
    }
    }
    public static class GraphicsExtension
    {
        public static void FillRoundedRectangle(this Graphics g, Brush brush, Rectangle rect, int radius)
        {
            using (var path = RoundedRect(rect, radius))
            {
                g.FillPath(brush, path);
            }
        }

        public static void DrawRoundedRectangle(this Graphics g, Pen pen, Rectangle rect, int radius)
        {
            using (var path = RoundedRect(rect, radius))
            {
                g.DrawPath(pen, path);
            }
        }

        private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle rect, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            int d = radius * 2;

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }
    }

 

