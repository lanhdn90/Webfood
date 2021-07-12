using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnline.Dao;
using WebOnline.Models;

namespace WebOnline.Controllers
{
    public class QLTaiKhoanController : Controller
    {
        // GET: QLTaiKhoan
        private static int matk;
        public ActionResult Index()
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var model = shop.TaiKhoan.ToList();
                return View(model);
            }

        }

        public void SetViewBag(string selectedID = null)
        {
            DatabaseEntities shop = new DatabaseEntities();
            List<Quyen> role = shop.Quyen.ToList();
            SelectList List = new SelectList(role, "RoleID", "Quyen1");
            ViewBag.roleList = List;
        }

        [HttpGet]
        public ActionResult ThemTk()
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                SetViewBag();
                return View();
            }

        }

        [HttpPost]
        public ActionResult ThemTk(TaiKhoan model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var id = dao.Insert(model);
                var db = new DatabaseEntities();
                if (id == null)
                {
                    return RedirectToAction("Index", "SanPham");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm Sản phẩm thành công");
                }
            }
            return RedirectToAction("Index", model);

        }
        [HttpGet]
        public ActionResult SuaTk(int id)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                matk = id;
                DatabaseEntities shop = new DatabaseEntities();
                var model = shop.TaiKhoan.SingleOrDefault(s => s.MaTaiKhoan == id);
                SetViewBag();
                return View(model);
            }

        }
        [HttpPost]
        public ActionResult SuaTk(TaiKhoan taiKhoan)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var tk = shop.TaiKhoan.SingleOrDefault(s => s.MaTaiKhoan == matk);
                tk.TaiKhoan1 = taiKhoan.TaiKhoan1;
                tk.MatKhau = taiKhoan.MatKhau;
                tk.RoleID = taiKhoan.RoleID;
                shop.SaveChanges();
                var model = shop.TaiKhoan.OrderByDescending(s => s.MaTaiKhoan).ToList();
                return RedirectToAction("Index", model);
            }

        }


        public ActionResult XoaTk(int id)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var model = shop.TaiKhoan.OrderByDescending(s => s.MaTaiKhoan).ToList();

                var tk = shop.TaiKhoan.SingleOrDefault(s => s.MaTaiKhoan == id);
                if (tk != null)
                {
                    shop.TaiKhoan.Remove(tk);
                    shop.SaveChanges();
                }

                return RedirectToAction("Index", model);
            }


        }

        public bool ChangeStatusTK(int id)
        {
            DatabaseEntities shop = new DatabaseEntities();
            var model = shop.TaiKhoan.Find(id);
            if (model.Status == true)
            {
                model.Status = false;
                shop.SaveChanges();
                return false;
            }
            else
            {
                model.Status = true;
                shop.SaveChanges();
                return true;
            }
        }

        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var res = ChangeStatusTK(id);
            return Json(new
            {
                status = res
            });
        }
    }
}