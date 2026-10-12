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
    public partial class DANG_NHAP : Form
    {

        public DANG_NHAP()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void DANG_NHAP_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
 
          
            Panel overlay = new Panel();
            overlay.Size = guna2Panel1.Size;
            overlay.Location = new Point(0, 0);
            

            guna2Panel1.Controls.Add(overlay);
            overlay.BringToFront();


            // ===== TIÊU ĐỀ =====
            Label lblTitle = new Label();
            lblTitle.Text = "KT Cafe";
            lblTitle.Font = new Font("Segoe UI", 26, FontStyle.Bold);
            lblTitle.ForeColor = Color.Black;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(40, 80);


            // ===== MÔ TẢ =====
            Label lblDesc = new Label();
            lblDesc.Text = "Thưởng thức cà phê mỗi ngày ☕\nĐậm đà • Thơm ngon • Chất lượng";
            lblDesc.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            lblDesc.ForeColor = Color.White;
            lblDesc.BackColor = Color.Transparent;
            lblDesc.AutoSize = true;
            lblDesc.Location = new Point(40, 140);


            // ===== THÊM VÀO PANEL =====
            overlay.Controls.Add(lblTitle);
            overlay.Controls.Add(lblDesc);

            guna2TextBox1.Clear();
            txtPassword.Clear();
        }
        bool isShow = false;
        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            if (isShow)
            {
                txtPassword.UseSystemPasswordChar = true;
                eye.Image = Properties.Resources.hide;
                isShow = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = false;
                eye.Image = Properties.Resources.view;
                isShow = true;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string user = guna2TextBox1.Text.Trim();
            string pass = txtPassword.Text.Trim();

            // ===== VALIDATE =====

            if (string.IsNullOrEmpty(user))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!");
                guna2TextBox1.Focus();
                return;
            }

            if (string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!");
                txtPassword.Focus();
                return;
            }

            // tránh nhập toàn dấu cách
            if (user.All(char.IsWhiteSpace))
            {
                MessageBox.Show("Tên đăng nhập không hợp lệ!");
                return;
            }

            if (pass.All(char.IsWhiteSpace))
            {
                MessageBox.Show("Mật khẩu không hợp lệ!");
                return;
            }

            try
            {
                using (var db = new QL_CA_FEEntities11())
                {
                    var acc = db.accounts.FirstOrDefault(x => x.username == user);
                    if (acc != null && HashHelper.VerifyPassword(pass, acc.password))
                    {
                        if (HashHelper.NeedsUpgrade(acc.password))
                        {
                            acc.password = HashHelper.HashPassword(pass);
                            db.SaveChanges();
                        }
                        MessageBox.Show("Đăng nhập thành công!");

                        Form1 f = new Form1(user, acc.role);
                        f.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối database!\n" + ex.Message);
            }
        }

        private void gunaLinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TAO_TK f = new TAO_TK();

            this.Hide(); 
            f.FormClosed += (s, args) =>
            {
                this.Show(); 
            };

            f.Show();

        }

        private void guna2CustomCheckBox1_Click(object sender, EventArgs e)
        {

        }

        private void gunaLinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                QUEN_MK f = new QUEN_MK();

                this.Hide();
                f.ShowDialog();

               
                guna2TextBox1.Clear();
                txtPassword.Clear();

                this.Show();            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không mở được form quên mật khẩu!\n" + ex.Message);
            }
        }
    }
}
