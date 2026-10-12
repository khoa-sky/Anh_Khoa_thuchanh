using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QL_CAFE
{
    internal sealed class MonEditorForm : Form
    {
        private readonly int? maMon;
        private readonly TextBox txtTen = new TextBox();
        private readonly NumericUpDown numGia = new NumericUpDown();
        private readonly ComboBox cboLoai = new ComboBox();
        private readonly TextBox txtBarcode = new TextBox();
        private readonly PictureBox picPreview = new PictureBox();
        private readonly Button btnChonAnh = new Button();
        private readonly Button btnBoAnh = new Button();
        private readonly Button btnLuu = new Button();
        private readonly Button btnHuy = new Button();
        private string selectedSourceImage;
        private string currentImagePath;
        private bool removeCurrentImage;

        public MonEditorForm(int? id = null)
        {
            maMon = id;
            Text = id.HasValue ? "Cập nhật món" : "Thêm món mới";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(600, 450);
            Font = new Font("Segoe UI", 10F);
            BuildUi();
            LoadData();
        }

        private void BuildUi()
        {
            AddLabel("Tên món", 28, 28);
            txtTen.SetBounds(145, 25, 410, 30);

            AddLabel("Loại món", 28, 78);
            cboLoai.SetBounds(145, 75, 250, 32);
            cboLoai.DropDownStyle = ComboBoxStyle.DropDownList;

            AddLabel("Giá (VNĐ)", 28, 128);
            numGia.SetBounds(145, 125, 250, 32);
            numGia.Minimum = 1;
            numGia.Maximum = 100000000;
            numGia.ThousandsSeparator = true;

            AddLabel("Barcode", 28, 178);
            txtBarcode.SetBounds(145, 175, 250, 30);
            txtBarcode.MaxLength = 100;

            AddLabel("Hình món", 28, 228);
            picPreview.SetBounds(145, 225, 210, 140);
            picPreview.BorderStyle = BorderStyle.FixedSingle;
            picPreview.SizeMode = PictureBoxSizeMode.Zoom;

            btnChonAnh.Text = "Chọn ảnh";
            btnChonAnh.SetBounds(375, 225, 120, 38);
            btnChonAnh.Click += BtnChonAnh_Click;

            btnBoAnh.Text = "Bỏ ảnh";
            btnBoAnh.SetBounds(375, 275, 120, 38);
            btnBoAnh.Click += (s, e) =>
            {
                selectedSourceImage = null;
                removeCurrentImage = true;
                SetPreview(null);
            };

            btnLuu.Text = "Lưu";
            btnLuu.SetBounds(335, 392, 105, 40);
            btnLuu.BackColor = Color.SeaGreen;
            btnLuu.ForeColor = Color.White;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Click += BtnLuu_Click;

            btnHuy.Text = "Hủy";
            btnHuy.SetBounds(450, 392, 105, 40);
            btnHuy.DialogResult = DialogResult.Cancel;

            Controls.AddRange(new Control[]
            {
                txtTen, cboLoai, numGia, txtBarcode, picPreview,
                btnChonAnh, btnBoAnh, btnLuu, btnHuy
            });
            AcceptButton = btnLuu;
            CancelButton = btnHuy;
        }

        private void AddLabel(string text, int x, int y)
        {
            Controls.Add(new Label { Text = text, AutoSize = true, Location = new Point(x, y + 4) });
        }

        private void LoadData()
        {
            using (var db = new QL_CA_FEEntities11())
            {
                var loai = db.loai_mon.OrderBy(x => x.ten_loai).ToList();
                cboLoai.DataSource = loai;
                cboLoai.DisplayMember = "ten_loai";
                cboLoai.ValueMember = "ma_loai";

                if (!maMon.HasValue) return;
                var item = db.mons.FirstOrDefault(x => x.ma_mon == maMon.Value);
                if (item == null) throw new InvalidOperationException("Món không còn tồn tại.");

                txtTen.Text = item.ten_mon;
                numGia.Value = Math.Max(numGia.Minimum, Math.Min(numGia.Maximum, item.gia));
                txtBarcode.Text = item.barcode ?? string.Empty;
                currentImagePath = item.hinh_anh;
                if (item.ma_loai.HasValue) cboLoai.SelectedValue = item.ma_loai.Value;
                SetPreview(ResolveImagePath(currentImagePath));
            }
        }

        private void BtnChonAnh_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Tệp hình ảnh|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                dialog.Title = "Chọn hình món";
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                selectedSourceImage = dialog.FileName;
                removeCurrentImage = false;
                SetPreview(selectedSourceImage);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            string ten = txtTen.Text.Trim();
            string barcode = txtBarcode.Text.Trim();
            if (string.IsNullOrWhiteSpace(ten))
            {
                MessageBox.Show("Vui lòng nhập tên món!");
                txtTen.Focus();
                return;
            }
            if (cboLoai.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn loại món!");
                return;
            }

            string importedImage = null;
            try
            {
                using (var db = new QL_CA_FEEntities11())
                {
                    if (!string.IsNullOrEmpty(barcode) && db.mons.Any(x => x.barcode == barcode && (!maMon.HasValue || x.ma_mon != maMon.Value)))
                    {
                        MessageBox.Show("Barcode đã được sử dụng cho món khác!");
                        txtBarcode.Focus();
                        return;
                    }

                    mon item;
                    if (maMon.HasValue)
                    {
                        item = db.mons.FirstOrDefault(x => x.ma_mon == maMon.Value);
                        if (item == null) throw new InvalidOperationException("Món không còn tồn tại.");
                    }
                    else
                    {
                        item = new mon();
                        db.mons.Add(item);
                    }

                    if (!string.IsNullOrEmpty(selectedSourceImage))
                    {
                        importedImage = ImportImage(selectedSourceImage);
                        item.hinh_anh = importedImage;
                    }
                    else if (removeCurrentImage)
                    {
                        item.hinh_anh = null;
                    }

                    item.ten_mon = ten;
                    item.gia = Decimal.ToInt32(numGia.Value);
                    item.ma_loai = Convert.ToInt32(cboLoai.SelectedValue);
                    item.barcode = string.IsNullOrEmpty(barcode) ? null : barcode;
                    db.SaveChanges();
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(importedImage) && File.Exists(importedImage))
                {
                    try { File.Delete(importedImage); } catch { }
                }
                MessageBox.Show("Không thể lưu món!\n" + ex.Message);
            }
        }

        private static string ImportImage(string sourcePath)
        {
            string directory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "QL_CAFE", "Images");
            Directory.CreateDirectory(directory);
            string extension = Path.GetExtension(sourcePath).ToLowerInvariant();
            string destination = Path.Combine(directory, Guid.NewGuid().ToString("N") + extension);
            File.Copy(sourcePath, destination, false);
            return destination;
        }

        private static string ResolveImagePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;
            return Path.IsPathRooted(path) ? path : Path.Combine(Application.StartupPath, path);
        }

        private void SetPreview(string path)
        {
            var old = picPreview.Image;
            picPreview.Image = null;
            old?.Dispose();
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return;
            using (var image = Image.FromFile(path)) picPreview.Image = new Bitmap(image);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) picPreview.Image?.Dispose();
            base.Dispose(disposing);
        }
    }
}