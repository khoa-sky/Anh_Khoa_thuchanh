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
using System.Diagnostics;
using PDF = iTextSharp.text;
using PDFP = iTextSharp.text.pdf;
using Excel = Microsoft.Office.Interop.Excel;


namespace QL_CAFE
{
    public partial class DAC_BAN : Form
    {
        List<dynamic> allHoaDon = new List<dynamic>();
        Dictionary<string, System.Drawing.Image> imageCache = new Dictionary<string, System.Drawing.Image>();
        List<ban> danhSachBan = new List<ban>();
        int banDangXem = -1;
        public Form1 formMain;

        public DAC_BAN()
        {
            InitializeComponent();


        }


        private void userControl33_Load(object sender, EventArgs e)
        {

        }

        private void DAC_BAN_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            using (var db = new QL_CA_FEEntities11())
            {
                danhSachBan = db.bans
                    .OrderBy(x => x.ten_ban == "Mang đi" ? 0 : 1)
                    .ThenBy(x => x.ma_ban)
                    .ToList();

                foreach (var b in danhSachBan)
                {
                    var ban = b;

                    UserControl3 card = new UserControl3();
                    card.SetData(ban.ten_ban, ban.trang_thai, ban.hinh_anh);

                    card.BoChon();

                    card.OnClickBan += () =>
                    {

                        if (banDangXem != ban.ma_ban)
                        {
                            banDangXem = ban.ma_ban;

                            LoadHoaDon(ban.ma_ban);

                            guna2ComboBox1.SelectedItem = ban.ten_ban;
                        }

                        ResetBanUI();
                        card.Highlight();
                    };


                    card.OnDoubleClickBan += () =>
                    {
                        formMain.OpenOrder(ban.ma_ban);
                    };

                    flowLayoutPanel1.Controls.Add(card);
                }
            }
            guna2ComboBox1.Items.Clear();

            guna2ComboBox1.Items.Add("Tất cả");

            foreach (var b in danhSachBan)
            {
                guna2ComboBox1.Items.Add(b.ten_ban);
            }

            guna2ComboBox1.SelectedIndex = 0;
            LoadHoaDonTatCa();
        }
        public void LoadHoaDon(int maBan)
        {
            using (var db = new QL_CA_FEEntities11())
            {
                var raw = (from hd in db.hoa_don
                           join ct in db.chi_tiet_hoa_don on hd.ma_hd equals ct.ma_hd
                           join m in db.mons on ct.ma_mon equals m.ma_mon
                           where hd.trang_thai == "Đã thanh toán" && hd.ma_ban == maBan
                           orderby hd.ma_hd descending
                           select new
                           {
                               MaHD = "HD" + hd.ma_hd,
                               TenBan = hd.ban.ten_ban,
                               ThoiGian = hd.thoi_gian,
                               TenMon = m.ten_mon,
                               HinhAnh = m.hinh_anh,
                               SoLuong = ct.so_luong,
                               Gia = ct.gia,
                               ThanhTien = ct.so_luong * ct.gia
                           }).ToList(); 

                // 👉 convert ảnh
                var ds = raw.Select(x => new
                {
                    x.MaHD,
                    x.TenBan,
                    x.ThoiGian,
                    x.TenMon,
                    HinhAnh = LoadImage(x.HinhAnh), 
                    x.SoLuong,
                    x.Gia,
                    x.ThanhTien
                }).ToList();
                allHoaDon = ds.Cast<dynamic>().ToList();
                dataGridView1.DataSource = ds;

                SetupGrid(); 
            }
        }
        public void LoadHoaDonTatCa()
        {
            using (var db = new QL_CA_FEEntities11())
            {
                var raw = (from hd in db.hoa_don
                           join ct in db.chi_tiet_hoa_don on hd.ma_hd equals ct.ma_hd
                           join m in db.mons on ct.ma_mon equals m.ma_mon
                           where hd.trang_thai == "Đã thanh toán"
                           orderby hd.ma_hd descending
                           select new
                           {
                               MaHD = "HD" + hd.ma_hd,
                               TenBan = hd.ban.ten_ban,
                               ThoiGian = hd.thoi_gian,
                               TenMon = m.ten_mon,
                               HinhAnh = m.hinh_anh,
                               SoLuong = ct.so_luong,
                               Gia = ct.gia,
                               ThanhTien = ct.so_luong * ct.gia
                           }).ToList();

                var ds = raw.Select(x => new
                {
                    x.MaHD,
                    x.TenBan,
                    x.ThoiGian,
                    x.TenMon,
                    HinhAnh = LoadImage(x.HinhAnh),
                    x.SoLuong,
                    x.Gia,
                    x.ThanhTien
                }).ToList();
                allHoaDon = ds.Cast<dynamic>().ToList();
                dataGridView1.DataSource = ds;

                SetupGrid();
            }
        }
        System.Drawing.Image LoadImage(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            if (imageCache.ContainsKey(path))
                return imageCache[path];

            string fullPath = Path.Combine(Application.StartupPath, path);

            if (!File.Exists(fullPath))
                return null;

            using (var img = System.Drawing.Image.FromFile(fullPath))
            {
                System.Drawing.Image clone = new Bitmap(img, new Size(80, 80));
                imageCache[path] = clone;
                return clone;
            }
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            foreach (var img in imageCache.Values)
            {
                img.Dispose();
            }

            imageCache.Clear();

            base.OnFormClosed(e);
        }
        void SetupGrid()
        {
            dataGridView1.RowTemplate.Height = 70;

            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 167, 69);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Orange;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            dataGridView1.Columns["Gia"].DefaultCellStyle.Format = "N0";
            dataGridView1.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";


            if (dataGridView1.Columns["HinhAnh"] is DataGridViewImageColumn imgCol)
            {
                imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
                imgCol.Width = 80;
            }
        }
        public void HighlightBanDangXem()
        {
            foreach (UserControl3 card in flowLayoutPanel1.Controls)
            {
                // so sánh theo tên bàn
                if (card.TenBan == GetTenBan(banDangXem))
                {
                    card.Highlight();
                }
            }
        }
        string GetTenBan(int maBan)
        {
            using (var db = new QL_CA_FEEntities11())
            {
                var ban = db.bans.FirstOrDefault(x => x.ma_ban == maBan);
                return ban?.ten_ban;
            }
        }
        public void ResetBanUI()
        {
            foreach (UserControl3 card in flowLayoutPanel1.Controls)
            {
                card.BoChon();
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string keyword = guna2TextBox1.Text.ToLower().Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                
                dataGridView1.DataSource = allHoaDon;
                return;
            }

            var filtered = allHoaDon
                .Where(x =>
                    x.MaHD.ToString().ToLower().Contains(keyword) ||
                    x.TenBan.ToString().ToLower().Contains(keyword) ||
                    x.TenMon.ToString().ToLower().Contains(keyword)
                )
                .ToList();

            dataGridView1.DataSource = filtered;
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = guna2ComboBox1.SelectedItem.ToString();

            if (selected == "Tất cả")
            {
                LoadHoaDonTatCa(); // ⭐ load tất cả hóa đơn
            }
            else
            {
                var ban = danhSachBan.FirstOrDefault(x => x.ten_ban == selected);

                if (ban != null)
                {
                    banDangXem = ban.ma_ban;        

                    LoadHoaDon(ban.ma_ban);        

                    ResetBanUI();                  

                    HighlightBanDangXem();          
                }
                if (selected == "Tất cả")
                {
                    banDangXem = -1;        

                    LoadHoaDonTatCa();

                    ResetBanUI();           
                }
            }

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string filePath = @"C:\Users\Public\HoaDonCafe.pdf";

            XuatHoaDonPDF(filePath);

            MessageBox.Show("Xuất PDF thành công!");

            Process.Start("explorer.exe", filePath);
        }
        void XuatHoaDonPDF(string path)
        {
            var doc = new PDF.Document(
       PDF.PageSize.A4,
       20,
       20,
       20,
       20);

            PDFP.PdfWriter.GetInstance(
                doc,
                new FileStream(path, FileMode.Create));

            doc.Open();

            // ================= FONT =================

            PDF.Font font =
                PDF.FontFactory.GetFont(
                    @"C:\Windows\Fonts\arial.ttf",
                    PDFP.BaseFont.IDENTITY_H,
                    true,
                    11,
                    PDF.Font.NORMAL);

            PDF.Font fontBold =
                PDF.FontFactory.GetFont(
                    @"C:\Windows\Fonts\arial.ttf",
                    PDFP.BaseFont.IDENTITY_H,
                    true,
                    14,
                    PDF.Font.BOLD);

            PDF.Font fontTitle =
                PDF.FontFactory.GetFont(
                    @"C:\Windows\Fonts\arial.ttf",
                    PDFP.BaseFont.IDENTITY_H,
                    true,
                    22,
                    PDF.Font.BOLD);

            PDF.Font fontHeader =
                PDF.FontFactory.GetFont(
                    @"C:\Windows\Fonts\arial.ttf",
                    PDFP.BaseFont.IDENTITY_H,
                    true,
                    11,
                    PDF.Font.BOLD);

            PDF.Font fontTotal =
                PDF.FontFactory.GetFont(
                    @"C:\Windows\Fonts\arial.ttf",
                    PDFP.BaseFont.IDENTITY_H,
                    true,
                    14,
                    PDF.Font.BOLD);

            // ===== FONT COLOR =====

            fontTitle.Color = PDF.Color.WHITE;

            fontHeader.Color = PDF.Color.WHITE;

            fontTotal.Color = PDF.Color.RED;

            // ================= TITLE =================

            PDFP.PdfPTable titleTable =
                new PDFP.PdfPTable(1);

            titleTable.WidthPercentage = 100;

            PDFP.PdfPCell titleCell =
                new PDFP.PdfPCell(
                    new PDF.Phrase(
                        "☕ HÓA ĐƠN THANH TOÁN - CAFE KT",
                        fontTitle));

            titleCell.HorizontalAlignment =
                PDF.Element.ALIGN_CENTER;

            titleCell.VerticalAlignment =
                PDF.Element.ALIGN_MIDDLE;

            titleCell.BackgroundColor =
                new PDF.Color(40, 167, 69);

            titleCell.Padding = 15;

            titleCell.Border = 0;

            titleTable.AddCell(titleCell);

            doc.Add(titleTable);

            doc.Add(new PDF.Paragraph(" "));

            // ================= INFO =================

            PDFP.PdfPTable infoTable =
                new PDFP.PdfPTable(2);

            infoTable.WidthPercentage = 100;

            infoTable.SetWidths(new float[] { 1, 2 });

            PDFP.PdfPCell info1 =
                new PDFP.PdfPCell(
                    new PDF.Phrase("CAFE KT", fontBold));

            info1.Border = 0;

            PDFP.PdfPCell info2 =
                new PDFP.PdfPCell(
                    new PDF.Phrase(
                        "Ngày xuất: " +
                        DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                        font));

            info2.HorizontalAlignment =
                PDF.Element.ALIGN_RIGHT;

            info2.Border = 0;

            infoTable.AddCell(info1);

            infoTable.AddCell(info2);

            doc.Add(infoTable);

            doc.Add(new PDF.Paragraph(" "));

            // ================= TABLE =================

            List<string> cols = new List<string>();

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Name == "HinhAnh")
                    continue;

                cols.Add(col.HeaderText);
            }

            PDFP.PdfPTable table =
                new PDFP.PdfPTable(cols.Count);

            table.WidthPercentage = 100;

            table.SpacingBefore = 10f;

            // ===== HEADER =====

            foreach (string colName in cols)
            {
                PDFP.PdfPCell cell =
                    new PDFP.PdfPCell(
                        new PDF.Phrase(
                            colName,
                            fontHeader));

                cell.BackgroundColor =
                    new PDF.Color(45, 52, 54);

                cell.HorizontalAlignment =
                    PDF.Element.ALIGN_CENTER;

                cell.VerticalAlignment =
                    PDF.Element.ALIGN_MIDDLE;

                cell.Padding = 8;

                table.AddCell(cell);
            }

            int tongTien = 0;

            // ================= DATA =================

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow)
                    continue;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.OwningColumn.Name == "HinhAnh")
                        continue;

                    string value = "";

                    if (cell.Value != null)
                    {
                        if (cell.OwningColumn.Name == "Gia"
                            || cell.OwningColumn.Name == "ThanhTien")
                        {
                            int v =
                                Convert.ToInt32(cell.Value);

                            value =
                                v.ToString("N0") + " VNĐ";

                            if (cell.OwningColumn.Name == "ThanhTien")
                                tongTien += v;
                        }
                        else if (cell.OwningColumn.Name == "ThoiGian")
                        {
                            DateTime dt =
                                Convert.ToDateTime(cell.Value);

                            value =
                                dt.ToString("dd/MM/yyyy HH:mm");
                        }
                        else
                        {
                            value =
                                cell.Value.ToString();
                        }
                    }

                    PDFP.PdfPCell dataCell =
                        new PDFP.PdfPCell(
                            new PDF.Phrase(
                                value,
                                font));

                    dataCell.HorizontalAlignment =
                        PDF.Element.ALIGN_CENTER;

                    dataCell.VerticalAlignment =
                        PDF.Element.ALIGN_MIDDLE;

                    dataCell.Padding = 6;

                    table.AddCell(dataCell);
                }
            }

            doc.Add(table);

            doc.Add(new PDF.Paragraph(" "));

            // ================= TOTAL =================

            PDFP.PdfPTable totalTable =
                new PDFP.PdfPTable(2);

            totalTable.WidthPercentage = 40;

            totalTable.HorizontalAlignment =
                PDF.Element.ALIGN_RIGHT;

            PDFP.PdfPCell total1 =
                new PDFP.PdfPCell(
                    new PDF.Phrase(
                        "TỔNG TIỀN",
                        fontBold));

            total1.BackgroundColor =
                new PDF.Color(255, 215, 0);

            total1.HorizontalAlignment =
                PDF.Element.ALIGN_CENTER;

            total1.Padding = 8;

            PDFP.PdfPCell total2 =
                new PDFP.PdfPCell(
                    new PDF.Phrase(
                        tongTien.ToString("N0") + " VNĐ",
                        fontTotal));

            total2.BackgroundColor =
                new PDF.Color(255, 215, 0);

            total2.HorizontalAlignment =
                PDF.Element.ALIGN_CENTER;

            total2.Padding = 8;

            totalTable.AddCell(total1);

            totalTable.AddCell(total2);

            doc.Add(totalTable);

            doc.Add(new PDF.Paragraph(" "));

            // ================= FOOTER =================

            PDF.Paragraph footer =
                new PDF.Paragraph(
                    "Cảm ơn quý khách!",
                    fontBold);

            footer.Alignment =
                PDF.Element.ALIGN_CENTER;

            doc.Add(footer);

            doc.Close();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn!");
                return;
            }

            string maHDText = dataGridView1.CurrentRow.Cells["MaHD"].Value.ToString();

            // HD12 -> 12
            int maHD = int.Parse(maHDText.Replace("HD", ""));

            DialogResult rs = MessageBox.Show(
                "Bạn có chắc muốn hủy hóa đơn này?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (rs != DialogResult.Yes) return;

            using (var db = new QL_CA_FEEntities11())
            {
                var hd = db.hoa_don.FirstOrDefault(x => x.ma_hd == maHD);
                if (hd != null)
                {
                    hd.trang_thai = "Đã hủy";
                    db.SaveChanges();
                }
                Form1 main = this.formMain;

                if (main != null && main.fDon != null)
                {
                    main.fDon.LoadDonTheoNgayVaBan();
                }
            }

            MessageBox.Show("Hủy hóa đơn thành công!");

            // reload lại
            if (banDangXem == -1)
                LoadHoaDonTatCa();
            else
                LoadHoaDon(banDangXem);

        }
        void XuatExcel(string path)
        {
            Excel.Application app = new Excel.Application();

            Excel.Workbook wb = app.Workbooks.Add(Type.Missing);

            Excel.Worksheet ws = wb.ActiveSheet;

            ws.Name = "HoaDon";

            int colIndex = 1;
            int rowIndex = 1;

            // ================= TITLE =================
            ws.Cells[rowIndex, 1] = "☕ HÓA ĐƠN THANH TOÁN - CAFE KT";

            Excel.Range title =
                ws.Range["A1", "G1"];

            title.Merge();

            title.Font.Size = 20;
            title.Font.Bold = true;

            title.Font.Color =
                ColorTranslator.ToOle(Color.White);

            title.Interior.Color =
                ColorTranslator.ToOle(
                    Color.FromArgb(40, 167, 69));

            title.RowHeight = 35;

            title.HorizontalAlignment =
                Excel.XlHAlign.xlHAlignCenter;

            title.VerticalAlignment =
                Excel.XlVAlign.xlVAlignCenter;

            rowIndex += 2;

            // ================= INFO =================
            ws.Cells[rowIndex, 1] = "CAFE KT";

            ws.Cells[rowIndex, 2] =
                "Ngày xuất: " +
                DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            Excel.Range info =
                ws.Range["A3", "B3"];

            info.Font.Bold = true;

            rowIndex += 2;

            // ================= HEADER =================
            colIndex = 1;

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Name == "HinhAnh")
                    continue;

                ws.Cells[rowIndex, colIndex] = col.HeaderText;

                Excel.Range headerCell =
                    ws.Cells[rowIndex, colIndex];

                headerCell.Font.Bold = true;

                headerCell.Font.Color =
                    ColorTranslator.ToOle(Color.White);

                headerCell.Interior.Color =
                    ColorTranslator.ToOle(
                        Color.FromArgb(45, 52, 54));

                headerCell.HorizontalAlignment =
                    Excel.XlHAlign.xlHAlignCenter;

                headerCell.VerticalAlignment =
                    Excel.XlVAlign.xlVAlignCenter;

                headerCell.Borders.LineStyle =
                    Excel.XlLineStyle.xlContinuous;

                headerCell.RowHeight = 28;

                colIndex++;
            }

            rowIndex++;

            int tongTien = 0;

            // ================= DATA =================
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow)
                    continue;

                colIndex = 1;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.OwningColumn.Name == "HinhAnh")
                        continue;

                    string value = "";

                    if (cell.Value != null)
                    {
                        if (cell.OwningColumn.Name == "Gia"
                            || cell.OwningColumn.Name == "ThanhTien")
                        {
                            int v = Convert.ToInt32(cell.Value);

                            value = v.ToString("N0") + " VNĐ";

                            if (cell.OwningColumn.Name == "ThanhTien")
                                tongTien += v;
                        }
                        else if (cell.OwningColumn.Name == "ThoiGian")
                        {
                            DateTime dt =
                                Convert.ToDateTime(cell.Value);

                            value =
                                dt.ToString("dd/MM/yyyy HH:mm");
                        }
                        else
                        {
                            value = cell.Value.ToString();
                        }
                    }

                    ws.Cells[rowIndex, colIndex] = value;

                    Excel.Range dataCell =
                        ws.Cells[rowIndex, colIndex];

                    // border
                    dataCell.Borders.LineStyle =
                        Excel.XlLineStyle.xlContinuous;

                    // zebra row
                    if (rowIndex % 2 == 0)
                    {
                        dataCell.Interior.Color =
                            ColorTranslator.ToOle(
                                Color.FromArgb(245, 245, 245));
                    }

                    // căn giữa
                    dataCell.HorizontalAlignment =
                        Excel.XlHAlign.xlHAlignCenter;

                    dataCell.VerticalAlignment =
                        Excel.XlVAlign.xlVAlignCenter;

                    colIndex++;
                }

                rowIndex++;
            }

            // ================= TOTAL =================
            rowIndex += 2;

            ws.Cells[rowIndex, 5] = "TỔNG TIỀN:";

            ws.Cells[rowIndex, 6] =
                tongTien.ToString("N0") + " VNĐ";

            Excel.Range total =
                ws.Range["E" + rowIndex, "F" + rowIndex];

            total.Font.Bold = true;

            total.Font.Size = 14;

            total.Font.Color =
                ColorTranslator.ToOle(Color.Red);

            total.Interior.Color =
                ColorTranslator.ToOle(
                    Color.FromArgb(255, 235, 59));

            total.Borders.LineStyle =
                Excel.XlLineStyle.xlContinuous;

            // ================= AUTO FIT =================
            ws.Columns.AutoFit();

            // width đẹp hơn
            ws.Columns["A"].ColumnWidth = 15;
            ws.Columns["B"].ColumnWidth = 15;
            ws.Columns["C"].ColumnWidth = 22;
            ws.Columns["D"].ColumnWidth = 25;
            ws.Columns["E"].ColumnWidth = 10;
            ws.Columns["F"].ColumnWidth = 15;
            ws.Columns["G"].ColumnWidth = 18;

            // ================= SAVE =================
            wb.SaveAs(path);

            wb.Close();

            app.Quit();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Files (*.xlsx)|*.xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                XuatExcel(sfd.FileName);
                MessageBox.Show("Xuất Excel thành công!");
            }

        }
        public void ReloadCurrent()
        {
            if (banDangXem == -1)
                LoadHoaDonTatCa();
            else
                LoadHoaDon(banDangXem);
        }
    }
}

