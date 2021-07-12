using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOnline.Models
{
    public class HoaDonClass
    {
        public int MaHoaDon { get; set; }
        public Nullable<int> MaTaiKhoan { get; set; }
        public string TenKhachHang { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public Nullable<System.DateTime> ThoiGianGiaoHang { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public Nullable<bool> TrangThai { get; set; }
        public Nullable<decimal> TongTien { get; set; }
    }
}