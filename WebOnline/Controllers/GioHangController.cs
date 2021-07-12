using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnline.Models;

namespace WebOnline.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        public ActionResult XoaSanPham(int id)
        {
            GioHang objCart = (GioHang)Session["Cart"];
            if (objCart != null)
            {
                objCart.XoaSanPham(id);
                Session["Cart"] = objCart;
            }
            return RedirectToAction("index");
        }
        // thêm vào giỏ hàng 1 sản phẩm có id = id của sản phẩm
        [HttpPost]
        public ActionResult ThemVaoGioHang(int id, int soLuong)
        {

            DatabaseEntities db = new DatabaseEntities();
            var p = db.SanPham.SingleOrDefault(s => s.MaSP.Equals(id));

            if (p != null)
            {
                GioHang objCart = (GioHang)Session["Cart"];
                if (objCart == null)
                {
                    objCart = new GioHang();
                }
                GioHang.GioHangItem item = new GioHang.GioHangItem()
                {
                    Anh = p.Anh,
                    TenSanPham = p.TenSP,
                    MaSp = p.MaSP,

                    Gia = p.GiaBan.ToString(),
                    SoLuong = soLuong,
                    Tong = Convert.ToDouble(p.GiaBan.ToString().Trim().Replace(",", string.Empty).Replace(".", string.Empty)) * soLuong
                };
                objCart.AddToCart(item);
                Session["Cart"] = objCart;

            }
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject("Thêm thành công", Formatting.Indented, jss);
            return this.Json(result, JsonRequestBehavior.AllowGet); ;

        }
        // cập nhật giỏ hàng theo loại sản phẩm và số lượng

        public ActionResult CapNhatSoLuong(string maSp, int soLuong)
        {
            int id = Convert.ToInt32(maSp.Substring(7, maSp.Length - 7));
            GioHang objCart = (GioHang)Session["Cart"];
            if (objCart != null)
            {
                objCart.CapNhatSoLuong(id, soLuong);
                Session["Cart"] = objCart;
            }
            return RedirectToAction("index");
        }
        // giỏ hàng mặc định, nếu session = null thì hiện không có hàng trong giỏ, nếu có thì trả lại list các sản phẩm
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.Title = "Giỏ hàng";
                DatabaseEntities shop = new DatabaseEntities();
                GioHangViewModel model = new GioHangViewModel();
                model.GioHang = (GioHang)Session["Cart"];
                var maNguoi = Convert.ToInt32(Session["MaTk"]);
                var person = shop.Nguoi.Find(shop.TaiKhoan.Find(maNguoi).MaNguoi);
                ViewBag.Person = new Nguoi()
                {
                    MaNguoi = person.MaNguoi,
                    Ho = person.Ho,
                    Ten = person.Ten,
                    DiaChi = person.DiaChi,
                    Phone = person.Phone,
                    Email = person.Email
                };
                return View(model);
            }
            
        }

        [HttpPost]
        public ActionResult ThanhToan(string diachi)
        {
            DatabaseEntities shop = new DatabaseEntities();
            GioHangViewModel model = new GioHangViewModel();
            model.GioHang = (GioHang)Session["Cart"];
            decimal tong = model.GioHang.ListItem.Sum(item => decimal.Parse(item.Gia) * item.SoLuong);
            HoaDon hd = new HoaDon();
            var maNguoi = Convert.ToInt32(Session["MaTk"].ToString());
            hd.MaTaiKhoan = maNguoi;

            hd.DiaChiGiaoHang = diachi;
            hd.ThoiGianGiaoHang = DateTime.Now;
            hd.NgayTao = DateTime.Now;
            hd.TongTien = tong;
            hd.TrangThai = false;
            shop.HoaDon.Add(hd);
            shop.SaveChanges();

            var hoaDon = (from h in shop.HoaDon orderby h.MaHoaDon descending select h).FirstOrDefault();
            foreach (var item in model.GioHang.ListItem)
            {
                ChiTietHoaDon ct = new ChiTietHoaDon();
                ct.MaHoaDon = hoaDon.MaHoaDon;
                ct.MaSP = item.MaSp;
                ct.SoLuong = item.SoLuong;
                ct.TongTien = decimal.Parse(item.Tong.ToString(CultureInfo.InvariantCulture));
                shop.ChiTietHoaDon.Add(ct);
                shop.SaveChanges();
            }
            model.GioHang.ListItem.Clear();
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject("Thanh toán thành công", Formatting.Indented, jss);
            return this.Json(result, JsonRequestBehavior.AllowGet); ;
        }

        public ActionResult NhapThongTin()
        {
            return View();
        }
    }
}