using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace QL_CAFE
{
    public partial class QL_MON : Form
    {
        List<dynamic> allMon = new List<dynamic>();
        Dictionary<string, Image> imageCache = new Dictionary<string, Image>();
        public event Action MenuChanged;
        private Guna.UI2.WinForms.Guna2Button btnThemMon;
        public QL_MON()
        {
            InitializeComponent();
            InitializeMenuButtons();
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedItem == null) return;

            string selected = guna2ComboBox1.SelectedItem.ToString();

            if (selected == "Tất cả")
            {
                dataGridView1.DataSource = allMon;
            }
            else
            {
                var filtered = allMon.Where(x => x.Loai == selected).ToList();
                dataGridView1.DataSource = filtered;
            }
        }


        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Chọn món cần xóa!");
                return;
            }

            int maMon = (int)dataGridView1.CurrentRow.Cells["MaMonGoc"].Value;

            if (MessageBox.Show("Xóa món này?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            using (var db = new QL_CA_FEEntities11())
            {
                var mon = db.mons.FirstOrDefault(x => x.ma_mon == maMon);

                if (mon != null)
                {
                    bool daBan = db.chi_tiet_hoa_don.Any(x => x.ma_mon == maMon);
                    if (daBan)
                    {
                        MessageBox.Show("Không thể xóa món đã phát sinh hóa đơn vì sẽ làm sai lịch sử doanh thu. Hãy sửa thông tin hoặc ngừng sử dụng món.");
                        return;
                    }
                    db.mons.Remove(mon);
                    db.SaveChanges();
                }
            }

            MessageBox.Show("Đã xóa!");

           

            RefreshMenu();
        }
        void LoadMon()
        {
            using (var db = new QL_CA_FEEntities11())
            {
                var raw = db.mons.Select(x => new
                {
                    x.ma_mon,
                    x.ten_mon,
                    Loai = x.loai_mon.ten_loai,
                    x.gia,
                    x.hinh_anh
                }).ToList();

                var ds = raw.Select(x => new MonView
                {
                    MaMonGoc = x.ma_mon,
                    MaMon = "MON" + x.ma_mon.ToString("D3"),
                    TenMon = x.ten_mon,
                    Loai = x.Loai,
                    Gia = x.gia,
                    HinhAnh = LoadImage(x.hinh_anh)
                }).ToList();

                allMon = ds.Cast<dynamic>().ToList();
                dataGridView1.DataSource = ds;
                SetupGrid();
            }
        }

        Image LoadImage(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            if (imageCache.ContainsKey(path))
                return imageCache[path];

            string fullPath = Path.Combine(Application.StartupPath, path);

            if (!File.Exists(fullPath)) return null;

            using (var img = Image.FromFile(fullPath))
            {
                Image clone = new Bitmap(img, new Size(80, 80));
                imageCache[path] = clone;
                return clone;
            }
        }


        void SetupGrid()
        {
            dataGridView1.RowTemplate.Height = 90;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ===== Bỏ viền xấu =====
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView1.BackgroundColor = Color.White;

            // ===== HEADER =====
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            dataGridView1.ColumnHeadersHeight = 40;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 52, 54);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // ===== BODY =====
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // ===== DÒNG XEN KẼ =====
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);

            // ===== SELECT =====
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;




            // ===== ẢNH =====
            if (dataGridView1.Columns["HinhAnh"] is DataGridViewImageColumn imgCol)
            {
                imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
                imgCol.Width = 100;
            }

            // ===== FORMAT GIÁ =====
            dataGridView1.Columns["Gia"].DefaultCellStyle.Format = "N0";
            dataGridView1.Columns["Gia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // ===== CĂN RIÊNG =====
            dataGridView1.Columns["TenMon"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // ===== HEADER TEXT =====
            dataGridView1.Columns["MaMon"].HeaderText = "Mã món";
            dataGridView1.Columns["TenMon"].HeaderText = "Tên món";
            dataGridView1.Columns["Loai"].HeaderText = "Loại";
            dataGridView1.Columns["Gia"].HeaderText = "Giá (VNĐ)";
            dataGridView1.Columns["HinhAnh"].HeaderText = "Hình";

            // ===== ẨN ID =====
            if (dataGridView1.Columns.Contains("MaMonGoc"))
                dataGridView1.Columns["MaMonGoc"].Visible = false;

            dataGridView1.Columns["TenMon"].ReadOnly = false;
            dataGridView1.Columns["Gia"].ReadOnly = false;

            // ❌ không cho sửa cái này
            dataGridView1.Columns["MaMon"].ReadOnly = true;
            dataGridView1.Columns["Loai"].ReadOnly = true;
            dataGridView1.Columns["HinhAnh"].ReadOnly = true;
        }


        private void guna2Button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Files (*.xlsx)|*.xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                XuatExcelMon(sfd.FileName);
                MessageBox.Show("Xuất Excel thành công!");
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Chọn món cần cập nhật!");
                return;
            }

            int maMon = (int)dataGridView1.CurrentRow.Cells["MaMonGoc"].Value;
            using (var editor = new MonEditorForm(maMon))
            {
                if (editor.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshMenu();
                    MessageBox.Show("Đã cập nhật món và hình trên card!");
                }
            }
        }

        private void QL_MON_Load(object sender, EventArgs e)
        {
            LoadMon();
            SetupGrid();

            using (var db = new QL_CA_FEEntities11())
            {
                var listLoai = db.loai_mon.Select(x => x.ten_loai).ToList();

                guna2ComboBox1.Items.Clear();
                guna2ComboBox1.Items.Add("Tất cả");

                foreach (var item in listLoai)
                    guna2ComboBox1.Items.Add(item);

                guna2ComboBox1.SelectedIndex = 0;
            }

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string keyword = guna2TextBox1.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                dataGridView1.DataSource = allMon;
                return;
            }

            var result = allMon.Where(x =>
                x.TenMon.ToLower().Contains(keyword) ||
                x.Loai.ToLower().Contains(keyword) ||
                x.MaMon.ToLower().Contains(keyword)
            ).ToList();

            dataGridView1.DataSource = result;
        }
        void XuatExcelMon(string path)
        {
            Excel.Application app = new Excel.Application();
            Excel.Workbook wb = app.Workbooks.Add(Type.Missing);
            Excel.Worksheet ws = wb.ActiveSheet;

            ws.Name = "DanhSachMon";

            int colIndex = 1;
            int rowIndex = 1;

            // ===== TIÊU ĐỀ =====
            ws.Cells[rowIndex, 1] = "DANH SÁCH MÓN";
            Excel.Range title = ws.Range["A1", "E1"];
            title.Merge();
            title.Font.Size = 16;
            title.Font.Bold = true;
            title.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            rowIndex++;

            ws.Cells[rowIndex, 1] = "CAFE KT";
            rowIndex++;

            ws.Cells[rowIndex, 1] = "Ngày: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            rowIndex += 2;

            // ===== HEADER =====
            colIndex = 1;

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Name == "HinhAnh" || col.Name == "MaMonGoc") continue;

                ws.Cells[rowIndex, colIndex] = col.HeaderText;
                ws.Cells[rowIndex, colIndex].Font.Bold = true;

                colIndex++;
            }

            rowIndex++;

            // ===== DATA =====
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                colIndex = 1;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.OwningColumn.Name == "HinhAnh" || cell.OwningColumn.Name == "MaMonGoc")
                        continue;

                    string value = "";

                    if (cell.Value != null)
                    {
                        if (cell.OwningColumn.Name == "Gia")
                        {
                            int v = Convert.ToInt32(cell.Value);
                            value = v.ToString("N0");
                        }
                        else
                        {
                            value = cell.Value.ToString();
                        }
                    }

                    ws.Cells[rowIndex, colIndex] = value;
                    colIndex++;
                }

                rowIndex++;
            }

            // ===== AUTO SIZE =====
            ws.Columns.AutoFit();

            // ===== SAVE =====
            wb.SaveAs(path);
            wb.Close();
            app.Quit();
        }
        void ImportExcel(string path)
        {
            Excel.Application app = new Excel.Application();
            Excel.Workbook wb = app.Workbooks.Open(path);
            Excel.Worksheet ws = wb.ActiveSheet;

            int row = 6;

            int demThem = 0;
            int demSua = 0;
            int demLoi = 0;

            List<string> loi = new List<string>();

            using (var db = new QL_CA_FEEntities11())
            {
                while (ws.Cells[row, 1].Value != null)
                {
                    try
                    {
                        string maMonText = ws.Cells[row, 1].Value?.ToString();
                        string ten = ws.Cells[row, 2].Value?.ToString();
                        string loai = ws.Cells[row, 3].Value?.ToString();
                        string giaText = ws.Cells[row, 4].Value?.ToString();

                        // ===== VALIDATE =====
                        if (string.IsNullOrEmpty(ten) || string.IsNullOrEmpty(giaText))
                        {
                            loi.Add($"Dòng {row}: thiếu dữ liệu");
                            demLoi++;
                            row++;
                            continue;
                        }

                        int gia;
                        if (!int.TryParse(giaText.Replace(",", ""), out gia))
                        {
                            loi.Add($"Dòng {row}: giá không hợp lệ");
                            demLoi++;
                            row++;
                            continue;
                        }

                        int maMon = 0;
                        if (!string.IsNullOrEmpty(maMonText))
                        {
                            int.TryParse(maMonText.Replace("MON", ""), out maMon);
                        }

                        var mon = db.mons.FirstOrDefault(x => x.ma_mon == maMon);

                        if (mon != null)
                        {
                            // ===== UPDATE =====
                            mon.ten_mon = ten;
                            mon.gia = gia;
                            demSua++;
                        }
                        else
                        {
                            // ===== ADD =====
                            var loaiMon = db.loai_mon.FirstOrDefault(x => x.ten_loai == loai);

                            db.mons.Add(new mon
                            {
                                ten_mon = ten,
                                gia = gia,
                                ma_loai = loaiMon?.ma_loai
                            });

                            demThem++;
                        }
                    }
                    catch
                    {
                        loi.Add($"Dòng {row}: lỗi không xác định");
                        demLoi++;
                    }

                    row++;
                }

                db.SaveChanges();
            }

            wb.Close();
            app.Quit();

            // ===== HIỂN THỊ KẾT QUẢ =====
            string msg =
                $"✔ Thêm mới: {demThem}\n" +
                $"✔ Cập nhật: {demSua}\n" +
                $"❌ Lỗi: {demLoi}";

            if (loi.Count > 0)
            {
                msg += "\n\nChi tiết lỗi:\n" + string.Join("\n", loi.Take(5));
            }

            MessageBox.Show(msg, "Kết quả import");
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files (*.xlsx)|*.xlsx";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ImportExcel(ofd.FileName);
                RefreshMenu();
            }
        }
        private void InitializeMenuButtons()
        {
            btnThemMon = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Thêm món",
                BorderRadius = 15,
                FillColor = Color.RoyalBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(230, 72),
                Location = new Point(60, 680)
            };
            btnThemMon.Click += BtnThemMon_Click;
            guna2Button1.Location = new Point(320, 680);
            guna2Button3.Location = new Point(580, 680);
            guna2Button4.Location = new Point(840, 680);
            guna2Button2.Location = new Point(1100, 680);
            guna2Button1.Size = guna2Button2.Size = guna2Button3.Size = guna2Button4.Size = new Size(230, 72);
            guna2Panel1.AutoScroll = true;
            guna2Panel1.AutoScrollMinSize = new Size(0, 800);
            guna2Panel1.AutoScrollMargin = new Size(0, 30);
            guna2Panel1.VerticalScroll.SmallChange = 30;
            guna2Panel1.VerticalScroll.LargeChange = 120;
            guna2Panel1.Controls.Add(btnThemMon);
            btnThemMon.BringToFront();
        }

        private void BtnThemMon_Click(object sender, EventArgs e)
        {
            using (var editor = new MonEditorForm())
            {
                if (editor.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshMenu();
                    MessageBox.Show("Thêm món thành công và card đã được cập nhật!");
                }
            }
        }

        private void RefreshMenu()
        {
            LoadMon();
            MenuChanged?.Invoke();
        }

    }
}
