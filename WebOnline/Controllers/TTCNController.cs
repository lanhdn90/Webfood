using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnline.Models;

namespace WebOnline.Controllers
{
    public class TTCNController : Controller
    {
        // GET: TTCN

        DatabaseEntities database = new DatabaseEntities();



        public ActionResult Index()
        {
            var userName = Convert.ToInt32(Session["MaTk"]);
            var findU = database.TaiKhoan.Find(userName);
            var model = database.Nguoi.Find(findU.MaNguoi);
            SetViewBagG();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string Password, string Newpassword)
        {
            var userName = Convert.ToInt32(Session["MaTk"]);
            var model = database.TaiKhoan.Find(userName);
            if (model.MatKhau != Password)
            {
                ModelState.AddModelError("", "Mật khẩu không đúng.");

            }
            else
            {
                HomeController home = new HomeController();
                home.UpdateUser(model.MaNguoi, Newpassword);
            }
            SetViewBagG();
            var findP = database.Nguoi.Find(model.MaNguoi);
            return View("Index", findP);
        }

        [HttpPost]
        public ActionResult Edit(Nguoi n)
        {
            var userName = Convert.ToInt32(Session["MaTk"]);
            var model = database.TaiKhoan.Find(userName);
            //new Share().UpdatePerson(person);
            SetViewBagG();
            var findP = database.Nguoi.Find(model.MaNguoi);
            findP.GioiTInh = n.GioiTInh;
            findP.Ho = n.Ho;
            findP.Ten = n.Ten;
            findP.DiaChi = n.DiaChi;
            findP.NgaySinh = n.NgaySinh;
            findP.Email = n.Email;
            findP.Phone = n.Phone;
            database.SaveChanges();
            return View("Index", findP);
        }




        public void SetViewBagG(string selectedID = null)
        {
            SelectList GenGender = new SelectList(new[] {
                new {Text = "Nam", Value = true},
                new {Text = "Nữ", Value = false},
            }, "Value", "Text");
            ViewBag.GenGender = GenGender;
        }


        public List<HoaDon> listHD()
        {
            var userName = Convert.ToInt32(Session["MaTk"]);
            var model = database.HoaDon.Where(x => x.MaTaiKhoan == userName).OrderByDescending(x => x.MaHoaDon).ToList();
            return model;
        }

        public List<ChiTietHoaDon> listCTHD(int id)
        {
            var model = database.ChiTietHoaDon.Where(x => x.MaHoaDon == id).ToList();
            return model;
        }

        public ActionResult Index1(int id)
        {
            var model = listHD();
            if(id == 0)
            {
                id = model[0].MaHoaDon;
            }
            ViewBag.listHD = model;
            ViewBag.listCTHD = listCTHD(id);
            return View();

        }
    }
}
