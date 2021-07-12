using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnline.Models;

namespace WebOnline.Controllers
{
    public class QLLoaiSPController : Controller
    {
        // GET: QLLoaiSP
        private static int mahang;
        public ActionResult Index()
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var model = shop.LoaiSP.ToList();
                return View(model);
            }


        }
        [HttpGet]
        public ActionResult ThemHang()
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult ThemHang(LoaiSP model)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                LoaiSP hang = new LoaiSP();
                hang.TenLoaiSP = model.TenLoaiSP;

                shop.LoaiSP.Add(hang);
                shop.SaveChanges();
                Response.Redirect("Index");
                return RedirectToAction("Index", model);
            }

        }

        [HttpGet]
        public ActionResult SuaLoaiSP(int id)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                mahang = id;
                DatabaseEntities shop = new DatabaseEntities();
                var model = shop.LoaiSP.SingleOrDefault(s => s.MaLoai == id);

                return View(model);
            }


        }

        [HttpPost]
        public ActionResult SuaLoaiSP(LoaiSP hangsx)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var hang = shop.LoaiSP.SingleOrDefault(s => s.MaLoai == mahang);
                hang.TenLoaiSP = hangsx.TenLoaiSP;

                shop.SaveChanges();
                var model = shop.LoaiSP.OrderByDescending(s => s.MaLoai).ToList();
                return View("Index", model);
            }

        }

        public ActionResult XoaHang(int id)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();

                var hang = shop.LoaiSP.SingleOrDefault(s => s.MaLoai == id);
                if (hang != null)
                {
                    if (UpdateSP(id))
                    {
                        shop.LoaiSP.Remove(hang);
                        shop.SaveChanges();
                    }

                }
                JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
                var result = JsonConvert.SerializeObject("Xóa thành công", Formatting.Indented, jss);
                return this.Json(result, JsonRequestBehavior.AllowGet); ;
            }

        }

        public bool UpdateSP(int id)
        {
            try
            {
                DatabaseEntities shop = new DatabaseEntities();
                var list = shop.SanPham.Where(x => x.MaLoai == id).ToList();
                foreach (var item in list)
                {
                    shop.SanPham.Remove(item);

                }
                shop.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}