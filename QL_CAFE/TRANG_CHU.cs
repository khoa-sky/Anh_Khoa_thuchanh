using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace QL_CAFE
{
    public partial class TRANG_CHU : Form
    {
        public TRANG_CHU()
        {
            InitializeComponent();

        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void TRANG_CHU_Load(object sender, EventArgs e)
        {
            LoadDashboard();
            StyleUI();

            LoadComboChart();

        }

        private void lab_tong_Click(object sender, EventArgs e)
        {

        }

        private void TRANG_CHU_Activated(object sender, EventArgs e)
        {
            LoadDashboard();
        }
        public void LoadDashboard()
        {
            using (var db = new QL_CA_FEEntities11())
            {
                // ===== 1. DOANH THU =====
                var hoaDonDaThanhToan = db.hoa_don
                    .Where(x => x.trang_thai == "Đã thanh toán")
                    .ToList();
                var doanhThu = hoaDonDaThanhToan.Sum(GetDoanhThuHoaDon);

                lab_tong.Text = doanhThu.ToString("N0") + " VND";


                // ===== 2. SỐ ĐƠN =====
                var soDon = db.hoa_don
                    .Count(x => x.trang_thai == "Đã thanh toán");

                lab_don.Text = soDon.ToString();


                // ===== 3. MÓN BÁN CHẠY =====
                var top = db.hoa_don
                    .Where(x => x.trang_thai == "Đã thanh toán")
                    .SelectMany(x => x.chi_tiet_hoa_don)
                    .GroupBy(x => x.ma_mon)
                    .Select(g => new
                    {
                        MaMon = g.Key,
                        Tong = g.Sum(x => x.so_luong)
                    })
                    .OrderByDescending(x => x.Tong)
                    .FirstOrDefault();

                if (top != null)
                {
                    var mon = db.mons.FirstOrDefault(x => x.ma_mon == top.MaMon);
                    lab_món.Text = mon != null
                        ? mon.ten_mon + " (" + top.Tong + ")"
                        : "Không có dữ liệu";
                }
                else
                {
                    lab_món.Text = "Không có dữ liệu";
                }
            }

            // ===== 4. BIỂU ĐỒ CỘT =====
            LoadChartCot();

            // ===== 5. BIỂU ĐỒ TRÒN =====
            LoadChartTron();
        }
        void LoadChartCot()
        {
            chartCot.Series.Clear();
            chartCot.ChartAreas.Clear();
            chartCot.Titles.Clear();

            // ===== AREA =====
            ChartArea area = new ChartArea("area");

            area.BackColor = Color.White;

            area.AxisX.MajorGrid.Enabled = false;

            area.AxisY.MajorGrid.LineColor = Color.Gainsboro;

            area.AxisX.LabelStyle.ForeColor = Color.Black;
            area.AxisY.LabelStyle.ForeColor = Color.Black;

            area.AxisX.LabelStyle.Font =
                new Font("Segoe UI", 9, FontStyle.Bold);

            area.AxisY.LabelStyle.Font =
                new Font("Segoe UI", 9);

            chartCot.ChartAreas.Add(area);

            // ===== TITLE =====
            Title title = new Title();

            title.Text = "THỐNG KÊ DOANH THU";

            title.Font =
                new Font("Segoe UI", 14, FontStyle.Bold);

            title.ForeColor = Color.FromArgb(120, 60, 0);

            chartCot.Titles.Add(title);

            // ===== SERIES =====
            Series series = new Series("Doanh thu");

            series.ChartType = SeriesChartType.Column;

            series.IsValueShownAsLabel = true;

            series.Font =
                new Font("Segoe UI", 9, FontStyle.Bold);

            series.LabelForeColor = Color.Black;

            series.Color =
                Color.FromArgb(52, 152, 219);

            series.BorderWidth = 2;

            series.BorderColor = Color.White;

            series.ToolTip =
                "#VALX\nDoanh thu: #VALY{N0} VND";

            series["PointWidth"] = "0.5";

            using (var db = new QL_CA_FEEntities11())
            {
                var hoaDon = db.hoa_don
                    .Where(x => x.trang_thai == "Đã thanh toán" && x.thoi_gian.HasValue)
                    .ToList();

                string mode = combo_cot.Text;

                // ================= THEO NGÀY =================
                if (mode == "Theo ngày")
                {
                    var data = hoaDon
                        .GroupBy(x => x.thoi_gian.Value.Date)
                        .Select(g => new
                        {
                            SortKey = g.Key,
                            Label = g.Key.ToString("dd/MM"),

                            Tong = g.Sum(GetDoanhThuHoaDon)
                        })
                        .OrderBy(x => x.SortKey)
                        .ToList();

                    foreach (var item in data)
                    {
                        series.Points.AddXY(item.Label, item.Tong);
                    }
                }

                // ================= THEO THÁNG =================
                else if (mode == "Theo tháng")
                {
                    var data = hoaDon
                        .GroupBy(x => new { x.thoi_gian.Value.Year, x.thoi_gian.Value.Month })
                        .Select(g => new
                        {
                            SortKey = new DateTime(g.Key.Year, g.Key.Month, 1),
                            Label = "T" + g.Key.Month + "/" + g.Key.Year,

                            Tong = g.Sum(GetDoanhThuHoaDon)
                        })
                        .OrderBy(x => x.SortKey)
                        .ToList();

                    foreach (var item in data)
                    {
                        series.Points.AddXY(item.Label, item.Tong);
                    }
                }

                // ================= THEO NĂM =================
                else if (mode == "Theo năm")
                {
                    var data = hoaDon
                        .GroupBy(x => x.thoi_gian.Value.Year)
                        .Select(g => new
                        {
                            SortKey = g.Key,
                            Label = g.Key.ToString(),

                            Tong = g.Sum(GetDoanhThuHoaDon)
                        })
                        .OrderBy(x => x.SortKey)
                        .ToList();

                    foreach (var item in data)
                    {
                        series.Points.AddXY(item.Label, item.Tong);
                    }
                }
            }

            chartCot.Series.Add(series);

            // ===== LEGEND =====
            chartCot.Legends[0].Font =
                new Font("Segoe UI", 9, FontStyle.Bold);

            chartCot.Legends[0].BackColor = Color.Transparent;
        }

        // ================= BIỂU ĐỒ TRÒN =================
        void LoadChartTron()
        {
            chartTron.Series.Clear();
            chartTron.ChartAreas.Clear();
            chartTron.Titles.Clear();

            // ===== AREA =====
            ChartArea area = new ChartArea("area");

            area.BackColor = Color.White;

            chartTron.ChartAreas.Add(area);

            // ===== TITLE =====
            Title title = new Title();

            title.Text = "PHÂN TÍCH ĐỒ UỐNG";

            title.Font =
                new Font("Segoe UI", 14, FontStyle.Bold);

            title.ForeColor =
                Color.FromArgb(120, 60, 0);

            chartTron.Titles.Add(title);

            // ===== SERIES =====
            Series pie = new Series("Món");

            pie.ChartType =
                SeriesChartType.Doughnut;

            pie.IsValueShownAsLabel = true;

            pie.Font =
                new Font("Segoe UI", 9, FontStyle.Bold);

            pie.LabelForeColor = Color.Black;

            // ===== KHÔNG HIỆN % =====
            pie.Label = "#VALX\n#VAL";

            // ===== TOOLTIP =====
            pie.ToolTip =
                "#VALX\nGiá trị: #VAL";

            // ===== MÀU =====
            pie.Palette =
                ChartColorPalette.BrightPastel;

            // ===== ĐỘ RỘNG LỖ =====
            pie["DoughnutRadius"] = "45";

            using (var db = new QL_CA_FEEntities11())
            {
                string mode = combo_tron.Text;

                // ================= TOP BÁN CHẠY =================
                if (mode == "Top bán chạy")
                {
                    var data = db.hoa_don
                        .Where(x => x.trang_thai == "Đã thanh toán")
                        .SelectMany(x => x.chi_tiet_hoa_don)
                        .GroupBy(x => x.ma_mon)
                        .Select(g => new
                        {
                            MaMon = g.Key,

                            Tong = g.Sum(x => x.so_luong)
                        })
                        .OrderByDescending(x => x.Tong)
                        .Take(5)
                        .ToList();

                    foreach (var item in data)
                    {
                        var mon = db.mons
                            .FirstOrDefault(x =>
                                x.ma_mon == item.MaMon);

                        if (mon != null)
                        {
                            pie.Points.AddXY(
                                mon.ten_mon,
                                item.Tong);
                        }
                    }
                }

                // ================= THEO DOANH THU =================
                else if (mode == "Theo doanh thu")
                {
                    var data = db.hoa_don
                        .Where(x => x.trang_thai == "Đã thanh toán")
                        .SelectMany(x => x.chi_tiet_hoa_don)
                        .GroupBy(x => x.ma_mon)
                        .Select(g => new
                        {
                            MaMon = g.Key,

                            Tong = g.Sum(
                                x => x.so_luong * x.gia)
                        })
                        .OrderByDescending(x => x.Tong)
                        .Take(5)
                        .ToList();

                    foreach (var item in data)
                    {
                        var mon = db.mons
                            .FirstOrDefault(x =>
                                x.ma_mon == item.MaMon);

                        if (mon != null)
                        {
                            pie.Points.AddXY(
                                mon.ten_mon,
                                item.Tong);
                        }
                    }
                }

                // ================= THEO LOẠI NƯỚC =================
                else if (mode == "Theo loại nước")
                {
                    var data = db.mons
                        .GroupBy(x => x.ma_loai)
                        .Select(g => new
                        {
                            Loai = g.Key,

                            Tong = g.Count()
                        })
                        .ToList();

                    foreach (var item in data)
                    {
                        string tenLoai = "";

                        if (item.Loai == 1)
                            tenLoai = "Cafe";

                        else if (item.Loai == 2)
                            tenLoai = "Trà";

                        else
                            tenLoai = "Nước ép";

                        pie.Points.AddXY(
                            tenLoai,
                            item.Tong);
                    }
                }
            }

            chartTron.Series.Add(pie);

            // ===== LEGEND =====
            chartTron.Legends[0].Font =
                new Font("Segoe UI", 9, FontStyle.Bold);

            chartTron.Legends[0].BackColor =
                Color.Transparent;
        }
        void StyleUI()
        {
            // ===== CHART CỘT =====
            chartCot.BackColor = Color.Transparent;
            chartCot.ChartAreas.Clear();
            var area1 = new ChartArea("area");
            area1.BackColor = Color.White;
            area1.AxisX.MajorGrid.Enabled = false;
            area1.AxisY.MajorGrid.LineColor = Color.LightGray;
            area1.AxisX.LabelStyle.ForeColor = Color.Black;
            area1.AxisY.LabelStyle.ForeColor = Color.Black;
            chartCot.ChartAreas.Add(area1);

            // ===== CHART TRÒN =====
            chartTron.BackColor = Color.Transparent;
            chartTron.ChartAreas.Clear();
            var area2 = new ChartArea("area");
            area2.BackColor = Color.White;
            chartTron.ChartAreas.Add(area2);

            // ===== FONT =====
            lab_tong.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lab_don.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lab_món.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        }
        void LoadComboChart()
        {
            // ===== COMBO CỘT =====
            combo_cot.Items.Clear();

            combo_cot.Items.Add("Theo ngày");
            combo_cot.Items.Add("Theo tháng");
            combo_cot.Items.Add("Theo năm");

            combo_cot.SelectedIndex = 0;

            // ===== COMBO TRÒN =====
            combo_tron.Items.Clear();

            combo_tron.Items.Add("Top bán chạy");
            combo_tron.Items.Add("Theo doanh thu");
            combo_tron.Items.Add("Theo loại nước");

            combo_tron.SelectedIndex = 0;
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadChartCot();
        }

        private void combo_tron_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadChartTron();
        }
        private static int GetDoanhThuHoaDon(hoa_don hoaDon)
        {
            var payment = hoaDon.thanh_toan.OrderByDescending(x => x.ma_tt).FirstOrDefault();
            if (payment != null && payment.thanh_tien.HasValue) return payment.thanh_tien.Value;
            return hoaDon.chi_tiet_hoa_don.Sum(x => (x.so_luong ?? 0) * (x.gia ?? 0));
        }

    }
 }

