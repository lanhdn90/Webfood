using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOnline.Models
{
    public class GioHang
    {
        public GioHang()
        {
            ListItem = new List<GioHangItem>();
        }
        public List<GioHangItem> ListItem { get; set; }
        public void AddToCart(GioHangItem item)
        {
            if (ListItem.Where(s => s.TenSanPham.Equals(item.TenSanPham)).Any())
            {
                var myItem = ListItem.Single(s => s.TenSanPham.Equals(item.TenSanPham));
                myItem.SoLuong += item.SoLuong;
                myItem.Tong += item.SoLuong * Convert.ToDouble(item.Gia.Trim().Replace(",", string.Empty).Replace(".", string.Empty));
            }
            else
            {
                ListItem.Add(item);
            }
        }
        public bool XoaSanPham(int lngProductSellID)
        {
            GioHangItem existsItem = ListItem.Where(x => x.MaSp == lngProductSellID).SingleOrDefault();
            if (existsItem != null)
            {
                ListItem.Remove(existsItem);
            }
            return true;
        }
        public bool CapNhatSoLuong(int lngProductSellID, int intQuantity)
        {
            GioHangItem existsItem = ListItem.Where(x => x.MaSp == lngProductSellID).SingleOrDefault();
            if (existsItem != null)
            {
                existsItem.SoLuong = intQuantity;
                existsItem.Tong = existsItem.SoLuong * Convert.ToDouble(existsItem.Gia.Replace(",", string.Empty).Replace(".", string.Empty));
            }
            return true;
        }
        public bool GioHangRong()
        {
            ListItem.Clear();
            return true;
        }

        public class GioHangItem
        {
            public string Anh { get; set; }
            public int MaSp { get; set; }
            public string TenSanPham { get; set; }
            public string Gia { get; set; }
            public int SoLuong { get; set; }
            public double Tong { get; set; }
        }
    }
}