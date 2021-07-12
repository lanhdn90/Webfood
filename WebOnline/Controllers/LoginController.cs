using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnline.Dao;
using WebOnline.Models;

namespace WebOnline.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var res = dao.Login(model.UserName, model.PassWord);
                if (res == 1)
                {
                    var user = dao.GetByID(model.UserName);
                    Session["MaTk"] = user.MaTaiKhoan;
                    Session["TenTk"] = user.TaiKhoan1;
                    Session["Role"] = user.RoleID;
                    if (user.RoleID == 4)
                    {
                        return RedirectToAction("Index1", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "QLHoaDon");
                    }
                }
                else if (res == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (res == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa.");
                }
                else if (res == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không đúng.");
                }
            }
            return View("Index");

        }

        public ActionResult DangXuat()
        {
            Session["MaTk"] = null;
            Session["TenTk"] = null;
            return Redirect("/Home/Index");
        }
        public ActionResult dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult dangky(TaiKhoan user)
        {
            DatabaseEntities shop = new DatabaseEntities();
             Nguoi nguoi = new Nguoi();
            shop.Nguoi.Add(nguoi);
            var per = (from h in shop.Nguoi orderby h.MaNguoi descending select h).FirstOrDefault();

            if (shop.TaiKhoan.SingleOrDefault(x=>x.TaiKhoan1==user.TaiKhoan1)==null)
            {
                TaiKhoan tk = new TaiKhoan();
                tk.TaiKhoan1 = user.TaiKhoan1;
                tk.MatKhau = user.MatKhau;
                tk.MaNguoi = per.MaNguoi;
                tk.RoleID = 4;
                tk.Status = true;
                shop.TaiKhoan.Add(tk);
                shop.SaveChanges();
                ModelState.AddModelError("", "Đăng ký thành công");
            }
            else
            {
                ModelState.AddModelError("", "Tài khoản đã tồn tại");
            }


            return View();
            

        }
    }
}