using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnline.Models;

namespace WebOnline.Controllers
{
    public class QLNhaCungCapController : Controller
    {
        // GET: QLNhaCungCap
        private static int maNCC;
        public ActionResult Index()
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var model = shop.NhaCungCap.ToList();
                return View(model);
            }


        }
        [HttpGet]
        public ActionResult ThemNhaCC()
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
        public ActionResult ThemNhaCC(NhaCungCap model)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                NhaCungCap NCC = new NhaCungCap();
                NCC.TenNCC = model.TenNCC;
                NCC.Diachi = model.Diachi;
                NCC.Phone = model.Phone;
                NCC.GhiChu = model.GhiChu;
                shop.NhaCungCap.Add(NCC);
                shop.SaveChanges();
                Response.Redirect("Index");
                return RedirectToAction("Index", model);
            }

        }
        [HttpGet]
        public ActionResult SuaNhaCC(int id)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                maNCC = id;
                DatabaseEntities shop = new DatabaseEntities();
                var model = shop.NhaCungCap.SingleOrDefault(s => s.MaNCC == id);

                return View(model);
            }


        }
        [HttpPost]
        public ActionResult SuaNhaCC(NhaCungCap nCC)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var nha = shop.NhaCungCap.SingleOrDefault(s => s.MaNCC == maNCC);
                nha.TenNCC = nCC.TenNCC;
                nha.Diachi = nCC.Diachi;
                nha.Phone = nCC.Phone;
                nha.GhiChu = nCC.GhiChu;
                shop.SaveChanges();
                var model = shop.NhaCungCap.OrderByDescending(s => s.MaNCC).ToList();
                return View("Index", model);
            }

        }


        public ActionResult XoaNhaCC(int id)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var hang = shop.NhaCungCap.SingleOrDefault(s => s.MaNCC == id);
                if (hang != null)
                {
                    shop.NhaCungCap.Remove(hang);
                    shop.SaveChanges();
                }
                var model = shop.NhaCungCap.OrderByDescending(s => s.MaNCC).ToList();
                return RedirectToAction("Index", model);
            }


        }
    }
}