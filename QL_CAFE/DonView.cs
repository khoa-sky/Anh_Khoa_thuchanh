using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace QL_CAFE
{
    internal class DonView
    {
        public string MaHD { get; set; }

        public string TenBan { get; set; }

        public DateTime? ThoiGian { get; set; }

        public string TenMon { get; set; }

        public Image HinhAnh { get; set; }

        public int SoLuong { get; set; }

        public int Gia { get; set; }

        public int ThanhTien { get; set; }
    }
}
