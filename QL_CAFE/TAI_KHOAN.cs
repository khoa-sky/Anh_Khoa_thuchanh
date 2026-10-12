using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QL_CAFE
{
    
    public partial class TAI_KHOAN : Form
    {
        string username;
        public TAI_KHOAN(string user)
        {
            InitializeComponent();
            username = user;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void TAI_KHOAN_Load(object sender, EventArgs e)
        {
            using (var db = new QL_CA_FEEntities11())
            {
                var acc = db.accounts.FirstOrDefault(x => x.username == username);

                if (acc != null)
                {
                    guna2TextBox1.Text = acc.username;
                    guna2TextBox2.Text = acc.email;
                    if (acc.role == "staff")
                        guna2TextBox3.Text = "Nhân viên";
                    else if (acc.role == "admin")
                        guna2TextBox3.Text = "Quản lý";
                    else
                        guna2TextBox3.Text = acc.role;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy tài khoản!");
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show(
        "Bạn có chắc muốn đăng xuất?",
        "Xác nhận",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question
    );

            if (rs == DialogResult.Yes)
            {
                
                DANG_NHAP f = new DANG_NHAP();
                f.Show();

                
                Application.OpenForms["Form1"]?.Close();
                this.Close();
            }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
