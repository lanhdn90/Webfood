using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOnline.Models
{
    public class DonHangViewModel
    {
        public List<HoaDonClass> DonHangChuaDuyet { get; set; }
        public List<HoaDonClass> DonHangDaDuyet { get; set; }
    }
}