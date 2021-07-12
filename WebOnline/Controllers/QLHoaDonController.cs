using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnline.Models;

namespace WebOnline.Controllers
{
    public class QLHoaDonController : Controller
    {
        // GET: QLHoaDon
        private static int mahd;

        public ActionResult Index()
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var model = new DonHangViewModel()
                {
                    DonHangDaDuyet = (from a in shop.HoaDon
                                      join b in shop.TaiKhoan on a.MaTaiKhoan equals b.MaTaiKhoan
                                      join c in shop.Nguoi on b.MaNguoi equals c.MaNguoi
                                      where a.TrangThai == true
                                      select new HoaDonClass()
                                      {
                                          MaHoaDon = a.MaHoaDon,
                                          TenKhachHang = c.Ho + " " + c.Ten,
                                          DiaChi = c.DiaChi,
                                          Email = c.Email,
                                          SDT = c.Phone,
                                          DiaChiGiaoHang = a.DiaChiGiaoHang,
                                          ThoiGianGiaoHang = a.ThoiGianGiaoHang,
                                          TongTien = a.TongTien
                                      }).ToList(),
                    DonHangChuaDuyet = (from a in shop.HoaDon
                                        join b in shop.TaiKhoan on a.MaTaiKhoan equals b.MaTaiKhoan
                                        join c in shop.Nguoi on b.MaNguoi equals c.MaNguoi
                                        where a.TrangThai == false
                                        select new HoaDonClass()
                                        {
                                            MaHoaDon = a.MaHoaDon,
                                            TenKhachHang = c.Ho + " " + c.Ten,
                                            DiaChi = c.DiaChi,
                                            Email = c.Email,
                                            SDT = c.Phone,
                                            DiaChiGiaoHang = a.DiaChiGiaoHang,
                                            ThoiGianGiaoHang = a.ThoiGianGiaoHang,
                                            TongTien = a.TongTien
                                        }).ToList()
                };

                return View(model);
            }

        }

        //[HttpGet]
        //public ActionResult ThemHd()
        //{
        //    if (Session["MaTk"] == null)
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //    else
        //    {
        //        return View();
        //    }

        //}

        //[HttpPost]
        //public ActionResult ThemHd(HoaDon model)
        //{
        //    if (Session["MaTk"] == null)
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //    else
        //    {
        //        DatabaseEntities shop = new DatabaseEntities();
        //        HoaDon hd = new HoaDon();
        //        hd.MaTaiKhoan = model.MaTaiKhoan;

        //        hd.DiaChiGiaoHang = model.DiaChiGiaoHang;
        //        hd.TongTien = 0;
        //        shop.HoaDon.Add(hd);
        //        shop.SaveChanges();
        //        Response.Redirect("Index");
        //        return View("Index");
        //    }

        //}

        [HttpGet]
        public ActionResult SuaHd(int id)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                mahd = id;
                DatabaseEntities shop = new DatabaseEntities();
                var model = shop.HoaDon.SingleOrDefault(s => s.MaHoaDon == id);

                return View(model);
            }

        }
        [HttpPost]
        public ActionResult SuaHd(HoaDon hoaDon)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var hd = shop.HoaDon.SingleOrDefault(s => s.MaHoaDon == mahd);
                hd.MaTaiKhoan = hoaDon.MaTaiKhoan;
                hd.DiaChiGiaoHang = hoaDon.DiaChiGiaoHang;
                shop.SaveChanges();
                var model = shop.HoaDon.OrderByDescending(s => s.MaHoaDon).ToList();
                return View("Index", model);
            }

        }


        public ActionResult XoaHd(int id)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var model = new DonHangViewModel()
                {
                    DonHangDaDuyet = (from a in shop.HoaDon
                                      join b in shop.TaiKhoan on a.MaTaiKhoan equals b.MaTaiKhoan
                                      join c in shop.Nguoi on b.MaNguoi equals c.MaNguoi
                                      where a.TrangThai == true
                                      select new HoaDonClass()
                                      {
                                          MaHoaDon = a.MaHoaDon,
                                          TenKhachHang = c.Ho + " " + c.Ten,
                                          DiaChi = c.DiaChi,
                                          Email = c.Email,
                                          SDT = c.Phone,
                                          DiaChiGiaoHang = a.DiaChiGiaoHang,
                                          ThoiGianGiaoHang = a.ThoiGianGiaoHang,
                                          TongTien = a.TongTien
                                      }).ToList(),
                    DonHangChuaDuyet = (from a in shop.HoaDon
                                        join b in shop.TaiKhoan on a.MaTaiKhoan equals b.MaTaiKhoan
                                        join c in shop.Nguoi on b.MaNguoi equals c.MaNguoi
                                        where a.TrangThai == false
                                        select new HoaDonClass()
                                        {
                                            MaHoaDon = a.MaHoaDon,
                                            TenKhachHang = c.Ho + " " + c.Ten,
                                            DiaChi = c.DiaChi,
                                            Email = c.Email,
                                            SDT = c.Phone,
                                            DiaChiGiaoHang = a.DiaChiGiaoHang,
                                            ThoiGianGiaoHang = a.ThoiGianGiaoHang,
                                            TongTien = a.TongTien
                                        }).ToList()
                };


                var hd = shop.HoaDon.SingleOrDefault(s => s.MaHoaDon == id);
                if (hd != null)
                {
                    foreach (var item in shop.ChiTietHoaDon.Where(c => c.MaHoaDon == id).ToList())
                    {
                        var sp = shop.SanPham.SingleOrDefault(s => s.MaSP == item.MaSP);
                        sp.SoLuong += item.SoLuong;
                        shop.ChiTietHoaDon.Remove(item);
                    }
                    shop.HoaDon.Remove(hd);
                    shop.SaveChanges();
                }

                return RedirectToAction("Index", model);
            }


        }

        public ActionResult ChiTietHd(int id)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                mahd = id;
                ViewBag.mahd = id;
                DatabaseEntities shop = new DatabaseEntities();
                var model = shop.ChiTietHoaDon.Where(c => c.MaHoaDon == mahd).OrderByDescending(c => c.ChiTietHoaDon1).ToList();
                List<SelectListItem> sanPham = new List<SelectListItem>();
                for (int i = 0; i < shop.SanPham.ToList().Count; i++)
                {
                    SelectListItem sl = new SelectListItem() { Text = shop.SanPham.ToList()[i].TenSP, Value = shop.SanPham.ToList()[i].MaSP.ToString() };
                    sanPham.Add(sl);
                }
                ViewBag.Sp = sanPham;
                return View(model);
            }

        }
        [HttpPost]
        public ActionResult ThemChiTietHd(int SanPham, int SoLuong)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var sp = shop.SanPham.SingleOrDefault(s => s.MaSP == SanPham);
                var hd = shop.HoaDon.SingleOrDefault(h => h.MaHoaDon == mahd);
                ChiTietHoaDon ct = new ChiTietHoaDon();
                ct.MaHoaDon = mahd;
                ct.MaSP = SanPham;
                ct.SoLuong = SoLuong;
                ct.TongTien = SoLuong * sp.GiaBan;
                shop.ChiTietHoaDon.Add(ct);
                hd.TongTien += ct.TongTien;
                shop.SaveChanges();
                return RedirectToAction("ChiTietHd", new { id = mahd });
            }

        }
        public ActionResult XoaChiTietHd(int id)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();

                var ct = shop.ChiTietHoaDon.SingleOrDefault(s => s.ChiTietHoaDon1 == id);
                var hd = shop.HoaDon.SingleOrDefault(h => h.MaHoaDon == mahd);
                if (ct != null)
                {
                    shop.ChiTietHoaDon.Remove(ct);
                    hd.TongTien -= ct.TongTien;
                    shop.SaveChanges();
                }

                return RedirectToAction("ChiTietHd", new { id = mahd });

            }

        }
        //[HttpPost]
        //public ActionResult ThemChiTietHd(ChiTietHoaDon model)
        //{
        //    DatabaseEntities shop=new DatabaseEntities();

        //    return View();
        //}
        [HttpPost]
        public ActionResult SuaChiTietHd(int id, int soLuong)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var ct = shop.ChiTietHoaDon.SingleOrDefault(c => c.ChiTietHoaDon1 == id);

                ct.HoaDon.TongTien += (soLuong * ct.SanPham.GiaBan) - ct.TongTien;
                ct.SoLuong = soLuong;
                ct.TongTien = soLuong * ct.SanPham.GiaBan;
                shop.SaveChanges();
                return this.Json(new { data = true }, JsonRequestBehavior.AllowGet); ;
            }

        }
        public ActionResult XacNhanDonHang()
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var model = shop.HoaDon.Where(h => h.TrangThai == false).ToList();
                return View(model);
            }

        }

        public ActionResult Duyet(int id)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var model = new DonHangViewModel()
                {
                    DonHangDaDuyet = (from a in shop.HoaDon
                                      join b in shop.TaiKhoan on a.MaTaiKhoan equals b.MaTaiKhoan
                                      join c in shop.Nguoi on b.MaNguoi equals c.MaNguoi
                                      where a.TrangThai == true
                                      select new HoaDonClass()
                                      {
                                          MaHoaDon = a.MaHoaDon,
                                          TenKhachHang = c.Ho + " " + c.Ten,
                                          DiaChi = c.DiaChi,
                                          Email = c.Email,
                                          SDT = c.Phone,
                                          DiaChiGiaoHang = a.DiaChiGiaoHang,
                                          ThoiGianGiaoHang = a.ThoiGianGiaoHang,
                                          TongTien = a.TongTien
                                      }).ToList(),
                    DonHangChuaDuyet = (from a in shop.HoaDon
                                        join b in shop.TaiKhoan on a.MaTaiKhoan equals b.MaTaiKhoan
                                        join c in shop.Nguoi on b.MaNguoi equals c.MaNguoi
                                        where a.TrangThai == false
                                        select new HoaDonClass()
                                        {
                                            MaHoaDon = a.MaHoaDon,
                                            TenKhachHang = c.Ho + " " + c.Ten,
                                            DiaChi = c.DiaChi,
                                            Email = c.Email,
                                            SDT = c.Phone,
                                            DiaChiGiaoHang = a.DiaChiGiaoHang,
                                            ThoiGianGiaoHang = a.ThoiGianGiaoHang,
                                            TongTien = a.TongTien
                                        }).ToList()
                };


                var hd = shop.HoaDon.SingleOrDefault(h => h.MaHoaDon == id);
                hd.TrangThai = true;

                var listChiTiet = shop.ChiTietHoaDon.Where(c => c.MaHoaDon == id).ToList();
                foreach (var item in listChiTiet)
                {
                    var sp = shop.SanPham.SingleOrDefault(s => s.MaSP == item.MaSP);
                    if (sp.SoLuong > item.SoLuong)
                    {
                        sp.SoLuong -= item.SoLuong;
                    }

                }
                shop.SaveChanges();
                return RedirectToAction("Index", model);
            }

        }
    }
}