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
    public partial class TAO_TK : Form
    {
       
        public TAO_TK()
        {
            InitializeComponent();
           
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2CustomCheckBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string name = guna2TextBox1.Text.Trim();   
            string user = guna2TextBox2.Text.Trim();   
            string email = guna2TextBox3.Text.Trim();  
            string pass = guna2TextBox4.Text;         
            string confirm = guna2TextBox5.Text;       
            bool agree = guna2CustomCheckBox1.Checked;

            

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Chưa nhập họ tên!");
                return;
            }

            if (string.IsNullOrEmpty(user))
            {
                MessageBox.Show("Chưa nhập tên đăng nhập!");
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Chưa nhập email!");
                return;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
            }
            catch
            {
                MessageBox.Show("Email không hợp lệ!");
                return;
            }

            if (string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Chưa nhập mật khẩu!");
                return;
            }

            if (pass.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải >= 6 ký tự!");
                return;
            }

            if (pass != confirm)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!");
                return;
            }

            if (!agree)
            {
                MessageBox.Show("Bạn phải đồng ý điều khoản!");
                return;
            }

            
            using (var db = new QL_CA_FEEntities11())
            {

                var check = db.accounts.FirstOrDefault(x => x.username == user || x.email == email);
                if (check != null)
                {
                    MessageBox.Show(check.username == user ? "Tên đăng nhập đã tồn tại!" : "Email đã được sử dụng!");
                    return;
                }

                account acc = new account()
                {
                    username = user,
                    password = HashHelper.HashPassword(pass),
                    email = email, 
                    role = "staff"
                };

                db.accounts.Add(acc);
                db.SaveChanges();

                MessageBox.Show("Đăng ký thành công!");


                this.Close();
            }
        }

        private void TAO_TK_Load(object sender, EventArgs e)
        {

            guna2TextBox4.UseSystemPasswordChar = true;
            guna2TextBox5.UseSystemPasswordChar = true;
        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void eye_Click(object sender, EventArgs e)
        {

        }

        private void guna2CustomCheckBox2_Click(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox2.Checked)
            {
                
                guna2TextBox4.UseSystemPasswordChar = false;
                guna2TextBox5.UseSystemPasswordChar = false;
            }
            else
            {
                
                guna2TextBox4.UseSystemPasswordChar = true;
                guna2TextBox5.UseSystemPasswordChar = true;
            }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

            this.Close();
        }
    }
}
