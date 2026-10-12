using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QL_CAFE
{
    public partial class OTP : Form
    {
        private readonly string otpCode;
        private readonly string email;
        private readonly DateTime expiresAtUtc;
        private int attemptsRemaining = 5;
        private List<Guna.UI2.WinForms.Guna2TextBox> otpBoxes;

        public OTP(string otp, string mail, DateTime expiresAt)
        {
            InitializeComponent();
            otpCode = otp;
            email = mail;
            expiresAtUtc = expiresAt;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string userOTP = string.Join("", otpBoxes.Select(x => x.Text));
            if (userOTP.Length != 6 || !userOTP.All(char.IsDigit))
            {
                MessageBox.Show("OTP phải gồm đủ 6 chữ số!");
                return;
            }
            if (DateTime.UtcNow > expiresAtUtc)
            {
                MessageBox.Show("OTP đã hết hạn. Vui lòng yêu cầu mã mới!");
                Close();
                return;
            }
            if (userOTP == otpCode)
            {
                MessageBox.Show("Xác nhận thành công!");
                Hide();
                using (var form = new TAO_MK(email)) form.ShowDialog();
                Close();
                return;
            }

            attemptsRemaining--;
            if (attemptsRemaining <= 0)
            {
                MessageBox.Show("Bạn đã nhập sai quá số lần cho phép!");
                Close();
            }
            else MessageBox.Show($"Sai OTP! Còn {attemptsRemaining} lần thử.");
        }

        private void OTP_Load(object sender, EventArgs e)
        {
            otpBoxes = new List<Guna.UI2.WinForms.Guna2TextBox>
            {
                guna2TextBox1, guna2TextBox2, guna2TextBox3,
                guna2TextBox4, guna2TextBox5, guna2TextBox6
            };
            foreach (var box in otpBoxes)
            {
                box.MaxLength = 1;
                box.TextAlign = HorizontalAlignment.Center;
                box.TextChanged += Otp_TextChanged;
                box.KeyDown += Otp_KeyDown;
            }
            otpBoxes[0].Focus();
        }

        private void Otp_TextChanged(object sender, EventArgs e)
        {
            var current = sender as Guna.UI2.WinForms.Guna2TextBox;
            if (current != null && current.Text.Length == 1)
            {
                int index = otpBoxes.IndexOf(current);
                if (index < otpBoxes.Count - 1) otpBoxes[index + 1].Focus();
            }
        }

        private void Otp_KeyDown(object sender, KeyEventArgs e)
        {
            var current = sender as Guna.UI2.WinForms.Guna2TextBox;
            int index = otpBoxes.IndexOf(current);
            if (e.KeyCode == Keys.Back && current.Text == "" && index > 0)
            {
                otpBoxes[index - 1].Focus();
                otpBoxes[index - 1].SelectionStart = 1;
            }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e) => Close();
    }
}