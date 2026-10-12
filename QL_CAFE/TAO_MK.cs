using Guna.UI2.WinForms;
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
    public partial class TAO_MK : Form
    {
        string email;

        public TAO_MK(string mail)
        {
            InitializeComponent();
            email = mail;
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string pass = guna2TextBox1.Text;
            string confirm = guna2TextBox2.Text;

            if (string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Nhập mật khẩu!");
                return;
            }

            if (pass.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải >= 6 ký tự!");
                return;
            }

            if (pass != confirm)
            {
                MessageBox.Show("Mật khẩu không khớp!");
                return;
            }

            try
            {
                using (var db = new QL_CA_FEEntities11())
                {
                    var acc = db.accounts.FirstOrDefault(x => x.email == email);

                    if (acc == null)
                    {
                        MessageBox.Show("Không tìm thấy tài khoản!");
                        return;
                    }

                    acc.password =
                    HashHelper.HashPassword(pass);
                    db.SaveChanges();

                    MessageBox.Show("Đổi mật khẩu thành công!");

                    
                    guna2TextBox1.Clear();
                    guna2TextBox2.Clear();

                    

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi!\n" + ex.Message);
            }
        }

        private void TAO_MK_Load(object sender, EventArgs e)
        {
            guna2TextBox1.UseSystemPasswordChar = true;
            guna2TextBox2.UseSystemPasswordChar = true;
        }

        private void guna2CustomCheckBox2_Click(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox1.Checked)
            {
                guna2TextBox1.UseSystemPasswordChar = false;
                guna2TextBox2.UseSystemPasswordChar = false;
            }
            else
            {
                guna2TextBox1.UseSystemPasswordChar = true;
                guna2TextBox2.UseSystemPasswordChar = true;
            }
        }
    }
}
