using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace QL_CAFE
{
    public partial class QUEN_MK : Form
    {
        private DateTime lastSentAt = DateTime.MinValue;

        public QUEN_MK()
        {
            InitializeComponent();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e) => Close();

        private string GenerateOTP()
        {
            byte[] bytes = new byte[4];
            using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(bytes);
            int value = BitConverter.ToInt32(bytes, 0) & int.MaxValue;
            return (100000 + value % 900000).ToString();
        }

        private void SendOTP(string toEmail, string otp)
        {
            string smtpUser = Environment.GetEnvironmentVariable("CAFE_SMTP_USER");
            string smtpPassword = Environment.GetEnvironmentVariable("CAFE_SMTP_PASSWORD");
            if (string.IsNullOrWhiteSpace(smtpUser) || string.IsNullOrWhiteSpace(smtpPassword))
                throw new InvalidOperationException("Chưa cấu hình CAFE_SMTP_USER và CAFE_SMTP_PASSWORD.");

            using (var mail = new MailMessage())
            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                mail.From = new MailAddress(smtpUser);
                mail.To.Add(toEmail);
                mail.Subject = "Mã OTP khôi phục mật khẩu";
                mail.Body = $"Mã OTP của bạn là: {otp}. Mã có hiệu lực trong 5 phút.";
                smtp.Credentials = new NetworkCredential(smtpUser, smtpPassword);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string email = guna2TextBox1.Text.Trim();
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập email!");
                guna2TextBox1.Focus();
                return;
            }

            try { _ = new MailAddress(email); }
            catch
            {
                MessageBox.Show("Email không hợp lệ!");
                return;
            }

            if (DateTime.UtcNow - lastSentAt < TimeSpan.FromSeconds(60))
            {
                MessageBox.Show("Vui lòng chờ 60 giây trước khi gửi lại OTP!");
                return;
            }

            using (var db = new QL_CA_FEEntities11())
            {
                if (!db.accounts.Any(x => x.email == email))
                {
                    MessageBox.Show("Email chưa đăng ký!");
                    return;
                }
            }

            try
            {
                guna2Button1.Enabled = false;
                string otp = GenerateOTP();
                SendOTP(email, otp);
                lastSentAt = DateTime.UtcNow;
                MessageBox.Show("Đã gửi OTP về email!");
                Hide();
                using (var form = new OTP(otp, email, DateTime.UtcNow.AddMinutes(5))) form.ShowDialog();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gửi mail thất bại!\n" + ex.Message);
                guna2Button1.Enabled = true;
            }
        }
    }
}