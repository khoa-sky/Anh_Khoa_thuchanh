using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_CAFE
{
    public partial class UserControl2 : UserControl
    {
        public event Action OnSoLuongChanged;
        public int GiaGoc => giaGoc;
        public int MaMon { get; set; }

        public int SoLuong
        {
            get { return (int)guna2NumericUpDown1.Value; }
        }

        public event Action<UserControl2> OnDelete;
        int giaGoc = 0;
        public UserControl2()
        {
            InitializeComponent();

            guna2NumericUpDown1.Minimum = 1;
            guna2NumericUpDown1.Maximum = 100;
            guna2NumericUpDown1.Value = 1;

            CapNhatTien();
        }
        public void SetData(int maMon, string ten, int gia)
        {
            MaMon = maMon;

            lab_ten.Text = ten;
            giaGoc = gia;

            guna2NumericUpDown1.Value = 1;

            CapNhatTien();
        }
        
        void CapNhatTien()
        {
            int soLuong = (int)guna2NumericUpDown1.Value;
            int tong = giaGoc * soLuong;

            lab_gia.Text = tong.ToString("N0") + " VND";
        }


        private void guna2NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            CapNhatTien();
            OnSoLuongChanged?.Invoke();
        }

        private void lab_gia_Click(object sender, EventArgs e)
        {

        }

        private void gunaLinePanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public string TenMon
        {
            get { return lab_ten.Text; } 
            set { lab_ten.Text = value; }
        }

        public string Gia
        {
            get { return lab_gia.Text; } 
            set { lab_gia.Text = value; }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            OnDelete?.Invoke(this);
        }
        public void UpdateGia(string ten, int giaMoi)
        {
            lab_ten.Text = ten;
            giaGoc = giaMoi;

            CapNhatTien(); // 🔥 cập nhật lại tiền
        }
    }
}
