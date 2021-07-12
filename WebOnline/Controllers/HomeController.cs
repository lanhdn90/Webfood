using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnline.Models;

namespace WebOnline.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DatabaseEntities shop = new DatabaseEntities();
            var model = shop.LoaiSP.OrderByDescending(h => h.MaLoai).Skip(0).Take(5).ToList();
            return View(model);
        }

        public ActionResult Index1()
        {
            DatabaseEntities shop = new DatabaseEntities();
            var model = shop.LoaiSP.OrderByDescending(h => h.MaLoai).Skip(0).Take(5).ToList();
            return View(model);
        }
        public ActionResult ChiTietSp(int id)
        {
            DatabaseEntities shop = new DatabaseEntities();
            var model = shop.SanPham.SingleOrDefault(s => s.MaSP == id);
            var list = shop.SanPham.Where(s => s.MaLoai == model.MaLoai).ToList();
            var hang = shop.LoaiSP.SingleOrDefault(h => h.MaLoai == model.MaLoai);
            var listhang = shop.LoaiSP.ToList();
            listhang.Remove(hang);
            list.Remove(model);
            ViewBag.LienQuan = list.ToList().Skip(0).Take(6).ToList();
            ViewBag.Hang = listhang.ToList();
            return View(model);
        }

        public ActionResult TinTuc(int? page)
        {
            IPagedList<TinTuc> model = DsTin(page);
            return View(model);
        }

        public IPagedList<TinTuc> DsTin(int? page)
        {
            DatabaseEntities shop = new DatabaseEntities();
            var tin = shop.TinTuc.OrderByDescending(t => t.NgayDang);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return tin.ToPagedList(pageNumber, pageSize);
        }
        public ActionResult ChiTietTinTuc(int id)
        {
            DatabaseEntities shop = new DatabaseEntities();
            var model = shop.TinTuc.Where(t => t.MaTin == id).SingleOrDefault();

            return View(model);
        }
        public ActionResult ChuyenMuc()
        {
            DatabaseEntities shop = new DatabaseEntities();
            var model = shop.LoaiSP.ToList();
            return View(model);
        }

        public ActionResult ChiTietChuyenMuc(int id)
        {
            DatabaseEntities shop = new DatabaseEntities();
            var model = shop.SanPham.Where(s => s.MaLoai == id).ToList();
            ViewBag.TenChuyenMuc = model[0].LoaiSP.TenLoaiSP;
            return View(model);
        }

        public ActionResult TimKiem(string search, int? page)
        {
            ViewBag.KQ = search;
            var model = DanhSachTimKiem(search, page);
            return View(model);
        }

        public IPagedList<SanPham> DanhSachTimKiem(string search, int? page)

        {
            DatabaseEntities shop = new DatabaseEntities();
            var model = shop.SanPham.Where(s => s.TenSP.ToLower().Contains(search.ToLower())).OrderByDescending(c => c.MaSP);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            model.ToPagedList(pageNumber, pageSize);
            return model.ToPagedList(pageNumber, pageSize);
        }

        public ActionResult TTcanhan()
        {
            DatabaseEntities shop = new DatabaseEntities();
            var userName = Convert.ToInt32(Session["MaTk"]);
            var findU = shop.TaiKhoan.Find(userName);
            var model = shop.Nguoi.Find(findU.MaNguoi);
            SetViewBagG();
            return View(model);
        }

        [HttpPost]
        public ActionResult TTcanhan(string Password, string Newpassword)
        {
            DatabaseEntities shop = new DatabaseEntities();
            var userName = Convert.ToInt32(Session["MaTk"]);
            var model = shop.TaiKhoan.Find(userName);
            if (model.MatKhau != Password)
            {
                ModelState.AddModelError("", "Mật khẩu không đúng.");

            }
            else
            {
                UpdateUser(model.MaNguoi, Newpassword);
            }
            SetViewBagG();
            var findP = shop.Nguoi.Find(model.MaNguoi);
            return View("TTcanhan", findP);
        }

        [HttpPost]
        public ActionResult UpdatePerson(Nguoi person, HttpPostedFileBase file)
        {
            DatabaseEntities shop = new DatabaseEntities();
            var userName = Convert.ToInt32(Session["MaTk"]);
            var model1 = shop.TaiKhoan.Find(userName);
            file = file ?? Request.Files["file"];
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                if (fileName != null)
                {
                    var path = Path.Combine(Server.MapPath("~/images/products/"), fileName);
                    file.SaveAs(path);

                    var model = shop.Nguoi.Find(person.MaNguoi);
                    model.Ho = person.Ho;
                    model.Ten = person.Ten;
                    model.DiaChi = person.DiaChi;
                    model.NgaySinh = person.NgaySinh;
                    model.GioiTInh = person.GioiTInh;
                    model.Phone = person.Phone;
                    model.Anh = "/images/products/" + fileName; ;
                    model.Email = person.Email;
                    shop.SaveChanges();
                }
            }
            else
            {
                var model = shop.Nguoi.Find(person.MaNguoi);
                model.Ho = person.Ho;
                model.Ten = person.Ten;
                model.DiaChi = person.DiaChi;
                model.NgaySinh = person.NgaySinh;
                model.GioiTInh = person.GioiTInh;
                model.Phone = person.Phone;
                model.Anh = "/images/p-8.png";
                model.Email = person.Email;
                shop.SaveChanges();
            }
            SetViewBagG();
            var findP = shop.Nguoi.Find(model1.MaNguoi);
            return View("TTcanhan", findP);
        }

        public void UpdateUser(long? personID, string newPass)
        {
            DatabaseEntities shop = new DatabaseEntities();
            var model = shop.TaiKhoan.SingleOrDefault(x => x.MaNguoi == personID);
            model.MatKhau = newPass;
            shop.SaveChanges();

        }

        public void SetViewBagG(string selectedID = null)
        {
            SelectList GenGender = new SelectList(new[] {
                new {Text = "Nam", Value = true},
                new {Text = "Nữ", Value = false},
            }, "Value", "Text");
            ViewBag.GenGender = GenGender;
        }
    }
}