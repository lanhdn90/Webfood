using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnline.Models;

namespace WebOnline.Controllers
{
    public class QLSanPhamController : Controller
    {
        // GET: QLSanPham
        private static int masp;

        public ActionResult Index()
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var model = shop.SanPham.OrderByDescending(s => s.MaSP).ToList();
                return View(model);
            }
        }



        [HttpGet]
        public ActionResult ThemSp()
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var listLoai = shop.LoaiSP.OrderByDescending(p => p.MaLoai).ToList();
                List<SelectListItem> slSanPham = listLoai.Select(t => new SelectListItem() { Text = t.TenLoaiSP, Value = t.MaLoai.ToString() }).ToList();
                ViewBag.Loai = slSanPham;
                var listNhaCC = shop.NhaCungCap.OrderByDescending(p => p.MaNCC).ToList();
                List<SelectListItem> lSanPham = listNhaCC.Select(t => new SelectListItem() { Text = t.TenNCC, Value = t.MaNCC.ToString() }).ToList();
                ViewBag.NCC = lSanPham;
                return View();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemSp(SanPham model, HttpPostedFileBase file)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                file = file ?? Request.Files["file"];
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        var path = Path.Combine(Server.MapPath("~/images/products/"), fileName);
                        file.SaveAs(path);
                        SanPham sp = new SanPham();
                        sp.MaLoai = model.MaLoai;
                        sp.MaNCC = model.MaNCC;
                        sp.TenSP = model.TenSP;
                        sp.Anh = "/images/products/" + fileName;
                        sp.GiaBan = model.GiaBan;
                        sp.GiaNhap = model.GiaNhap;
                        sp.MoTa = model.MoTa;
                        sp.ChiTiet = model.ChiTiet;
                        sp.SoLuong = model.SoLuong;
                        sp.NgayDang = DateTime.Now;
                        shop.SanPham.Add(sp);
                        shop.SaveChanges();
                        Response.Redirect("Index");
                    }
                }
                else
                {
                    SanPham sp = new SanPham();
                    sp.MaLoai = model.MaLoai;
                    sp.MaNCC = model.MaNCC;
                    sp.TenSP = model.TenSP;
                    sp.Anh = "/images/p-8.png";
                    sp.GiaBan = model.GiaBan;
                    sp.GiaNhap = model.GiaNhap;
                    sp.MoTa = model.MoTa;
                    sp.SoLuong = model.SoLuong;
                    sp.ChiTiet = model.ChiTiet;
                    sp.NgayDang = DateTime.Now;
                    shop.SanPham.Add(sp);
                    shop.SaveChanges();
                    Response.Redirect("Index");
                }


                return RedirectToAction("Index", model);
            }

        }

        [HttpGet]
        public ActionResult SuaSp(int id)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                masp = id;
                DatabaseEntities shop = new DatabaseEntities();
                var model = shop.SanPham.SingleOrDefault(s => s.MaSP == id);
                var listLoai = shop.LoaiSP.OrderByDescending(p => p.MaLoai).ToList();
                List<SelectListItem> slSanPham = listLoai.Select(t => new SelectListItem() { Text = t.TenLoaiSP, Value = t.MaLoai.ToString() }).ToList();
                ViewBag.Loai = slSanPham;
                var listNhaCC = shop.NhaCungCap.OrderByDescending(p => p.MaNCC).ToList();
                List<SelectListItem> lSanPham = listNhaCC.Select(t => new SelectListItem() { Text = t.TenNCC, Value = t.MaNCC.ToString() }).ToList();
                ViewBag.NCC = lSanPham;
                return View(model);
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSp(SanPham sp, string img, HttpPostedFileBase file)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                file = file ?? Request.Files["file"];

                DatabaseEntities shop = new DatabaseEntities();
                var sanpham = shop.SanPham.SingleOrDefault(s => s.MaSP == masp);

                sanpham.MaLoai = sp.MaLoai;
                sanpham.MaNCC = sp.MaNCC;
                sanpham.TenSP = sp.TenSP;
                sanpham.GiaBan = sp.GiaBan;
                sanpham.GiaNhap = sp.GiaNhap;
                sanpham.MoTa = sp.MoTa;
                sanpham.ChiTiet = sp.ChiTiet;
                sanpham.SoLuong = sp.SoLuong;
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        var path = Path.Combine(Server.MapPath("~/images/products/"), fileName);
                        file.SaveAs(path);
                        sanpham.Anh = "/images/products/" + fileName;
                    }
                }
                else
                {
                    sanpham.Anh = sanpham.Anh;
                }
                shop.SaveChanges();

                var model = shop.SanPham.OrderByDescending(s => s.MaSP).ToList();
                return View("Index", model);
            }

        }

        [HttpPost]
        public ActionResult XoaSp(int id)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var sanpham = shop.SanPham.Find(id);
                if (sanpham != null)
                {
                    shop.SanPham.Remove(sanpham);
                    shop.SaveChanges();
                }
                JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
                var result = JsonConvert.SerializeObject("Xóa thành công", Formatting.Indented, jss);
                return this.Json(result, JsonRequestBehavior.AllowGet); ;
            }
        }
    }
}