using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_CAFE
{
    public partial class DO_UONG : Form
    {
        public event Action OnThanhToanThanhCong;
        List<UserControl1> danhSachCard = new List<UserControl1>();
        List<mon> danhSachMon;

        public int banDangChon = 0;
        public Form formBan;
        public DO_UONG()
        {
            InitializeComponent();
            flowLayoutPanel2.Visible = false;
           
        }

        private void DO_UONG_Load(object sender, EventArgs e)
        {
            flowLayoutPanel2.Controls.Clear();
            flowLayoutPanel2.Visible = false;
            ReloadMenuCards();
        }

        public void ReloadMenuCards()
        {
            var selectedIds = new HashSet<int>(
                flowLayoutPanel2.Controls.OfType<UserControl2>().Select(x => x.MaMon));

            foreach (var card in danhSachCard) card.Dispose();
            danhSachCard.Clear();
            flowLayoutPanel1.Controls.Clear();

            using (var db = new QL_CA_FEEntities11())
            {
                danhSachMon = db.mons.OrderBy(x => x.ten_mon).ToList();
            }

            foreach (var item in danhSachMon)
            {
                var current = item;
                var card = new UserControl1
                {
                    MaMon = current.ma_mon,
                    Tag = current.ma_loai
                };
                card.SetData(current.ma_mon, current.ten_mon, current.gia, current.hinh_anh, current.barcode);
                card.OnClickCard += () => ThemMon(current.ma_mon, current.ten_mon, current.gia);
                if (selectedIds.Contains(current.ma_mon)) card.Chon();
                danhSachCard.Add(card);
                flowLayoutPanel1.Controls.Add(card);
            }
        }

        void ThemMon(int maMon, string ten, int gia)
        {
            using (var db = new QL_CA_FEEntities11())
            {
                var monDB = db.mons.First(x => x.ma_mon == maMon);
                ten = monDB.ten_mon;
                gia = monDB.gia;
            }

            var daCo = flowLayoutPanel2.Controls
                .OfType<UserControl2>()
                .FirstOrDefault(x => x.MaMon == maMon);

            
            if (daCo != null)
            {
                flowLayoutPanel2.Controls.Remove(daCo);
                daCo.Dispose();

                var card = danhSachCard.FirstOrDefault(x => x.MaMon == maMon);
                if (card != null)
                    card.BoChon();

                if (flowLayoutPanel2.Controls.Count == 0)
                    flowLayoutPanel2.Visible = false;

                return;
            }

            

            UserControl2 uc = new UserControl2();
            uc.OnSoLuongChanged += () =>
            {
                TinhTien();
            };
            uc.SetData(maMon, ten, gia);

            uc.OnDelete += (item) =>
            {
                flowLayoutPanel2.Controls.Remove(item);
                item.Dispose();
                TinhTien();

                var card = danhSachCard.FirstOrDefault(x => x.MaMon == item.MaMon);
                if (card != null)
                    card.BoChon();

                if (flowLayoutPanel2.Controls.Count == 0)
                    flowLayoutPanel2.Visible = false;
            };

            flowLayoutPanel2.Controls.Add(uc);
            flowLayoutPanel2.Visible = true;
            TinhTien();

            var cardMoi = danhSachCard.FirstOrDefault(x => x.MaMon == maMon);
            if (cardMoi != null)
                cardMoi.Chon();
        }

        
        void LoadMon(int? maLoai = null)
        {
            foreach (var card in danhSachCard)
            {
                if (maLoai == null)
                    card.Visible = true;
                else
                    card.Visible = (int)card.Tag == maLoai;
            }
        }
        public void XoaCard(int maMon)
        {
            
            var card = danhSachCard.FirstOrDefault(x => x.MaMon == maMon);

            if (card != null)
            {
                flowLayoutPanel1.Controls.Remove(card);
                danhSachCard.Remove(card);
                card.Dispose();
            }

            
            var item = flowLayoutPanel2.Controls
                .OfType<UserControl2>()
                .FirstOrDefault(x => x.MaMon == maMon);

            if (item != null)
            {
                flowLayoutPanel2.Controls.Remove(item);
                item.Dispose();

                TinhTien();
            }

           
            if (flowLayoutPanel2.Controls.Count == 0)
            {
                flowLayoutPanel2.Visible = false;
            }
        }





        private void guna2VScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
          
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            LoadMon(2);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
       
        public void SetBan(int maBan)
        {
            banDangChon = maBan;

            
            flowLayoutPanel2.Controls.Clear();
            flowLayoutPanel2.Visible = false;

            foreach (var card in danhSachCard)
            {
                card.BoChon();
            }
        }


        private void guna2Button2_Click(object sender, EventArgs e)
        {
            
            LoadMon();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            
            LoadMon(1);
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            LoadMon(3);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
           
        }
        void TinhTien()
        {
            int tong = 0;

            foreach (UserControl2 item in flowLayoutPanel2.Controls)
            {
                tong += item.SoLuong * item.GiaGoc;
            }

            
            label5.Text = tong.ToString("N0");

            int giam = 0;

            if (checkBox1.Checked)
            {
                giam = (int)(tong * 0.2); 
                label6.Text = "20%";
            }
            else
            {
                label6.Text = "0%";
            }

            int thanhTien = tong - giam;

            
            label7.Text = thanhTien.ToString("N0");
        }
        string TaoNoiDungQR()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("===============CAFE KT==============");
            sb.AppendLine("");
            sb.AppendLine($"BAN: {banDangChon}");
            sb.AppendLine("------------------------");

            foreach (UserControl2 item in flowLayoutPanel2.Controls)
            {
                string ten = RemoveUnicode(item.TenMon).PadRight(15);
                string sl = $"x{item.SoLuong}".PadLeft(5);
                sb.AppendLine($"{ten}{sl}");
            }

            sb.AppendLine("------------------------");

            int tong = int.Parse(label5.Text.Replace(",", ""));
            int thanhTien = int.Parse(label7.Text.Replace(",", ""));

            sb.AppendLine($"TONG:       {tong:N0} VND");
            sb.AppendLine($"THANH TOAN: {thanhTien:N0} VND");

            sb.AppendLine("");
            sb.AppendLine($"Thoi gian: {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine("Cam on quy khach!");

            return sb.ToString();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanel2.Controls.Count == 0)
            {
                MessageBox.Show("Chưa chọn món!");
                return;
            }
            if (banDangChon <= 0)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi thanh toán!");
                return;
            }

            int tongTien = flowLayoutPanel2.Controls.OfType<UserControl2>().Sum(x => x.SoLuong * x.GiaGoc);
            int giamGia = checkBox1.Checked ? (int)(tongTien * 0.2m) : 0;
            int thanhTien = tongTien - giamGia;

            using (var qr = new Form_QR(TaoNoiDungQR()))
            {
                if (qr.ShowDialog() != DialogResult.OK) return;
            }

            try
            {
                using (var db = new QL_CA_FEEntities11())
                using (var transaction = db.Database.BeginTransaction())
                {
                    if (!db.bans.Any(x => x.ma_ban == banDangChon))
                        throw new InvalidOperationException("Bàn đã chọn không còn tồn tại.");

                    var hd = new hoa_don
                    {
                        ma_ban = banDangChon,
                        thoi_gian = DateTime.Now,
                        trang_thai = "Đã thanh toán"
                    };
                    db.hoa_don.Add(hd);
                    db.SaveChanges();

                    foreach (UserControl2 item in flowLayoutPanel2.Controls)
                    {
                        if (!db.mons.Any(x => x.ma_mon == item.MaMon))
                            throw new InvalidOperationException("Một món trong đơn không còn tồn tại.");
                        db.chi_tiet_hoa_don.Add(new chi_tiet_hoa_don
                        {
                            ma_hd = hd.ma_hd,
                            ma_mon = item.MaMon,
                            so_luong = item.SoLuong,
                            gia = item.GiaGoc
                        });
                    }
                    db.thanh_toan.Add(new thanh_toan
                    {
                        ma_hd = hd.ma_hd,
                        tong_tien = tongTien,
                        giam_gia = giamGia,
                        thanh_tien = thanhTien,
                        phuong_thuc = "QR",
                        ngay_tt = DateTime.Now
                    });
                    db.SaveChanges();
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lưu thanh toán!\n" + ex.Message);
                return;
            }

            OnThanhToanThanhCong?.Invoke();
            flowLayoutPanel2.Controls.Clear();
            flowLayoutPanel2.Visible = false;
            checkBox1.Checked = false;
            label5.Text = "0";
            label6.Text = "0%";
            label7.Text = "0";

            if (formBan is DAC_BAN fBan)
            {
                fBan.ResetBanUI();
                fBan.LoadHoaDon(banDangChon);
                fBan.HighlightBanDangXem();
                fBan.Show();
            }
            Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            TinhTien();
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
        
        

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            string code = txtScan.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Chưa nhập mã!");
                return;
            }

           
            var mon = danhSachMon.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(x.barcode) && x.barcode.ToUpperInvariant() == code);

            if (mon != null)
            {
                ThemMon(mon.ma_mon, mon.ten_mon, mon.gia);
            }
            else
            {
                MessageBox.Show("Không tìm thấy món!");
            }

            txtScan.Clear();
        }
        public void UpdateCard(int maMon, string ten, int gia, string hinh)
        {
            var card = danhSachCard.FirstOrDefault(x => x.MaMon == maMon);
            if (card != null)
            {
                card.SetData(maMon, ten, gia, hinh, "");
            }

            
            var list = flowLayoutPanel2.Controls.OfType<UserControl2>();

            foreach (var item in list)
            {
                if (item.MaMon == maMon)
                {
                    item.UpdateGia(ten, gia);
                }
            }

            
            TinhTien();
        }

    }

        
  }
        
  
        
 
 
