using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnline.Models;

namespace WebOnline.Controllers
{
    public class QLTinTucController : Controller
    {
        private static int matin;
        // GET: QLTinTuc
        public ActionResult Index()
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var model = shop.TinTuc.OrderByDescending(t => t.MaTin).ToList();
                return View(model);
            }

        }
        [HttpGet]
        public ActionResult ThemTin()
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
        [ValidateInput(false)]
        public ActionResult ThemTin(TinTuc model, HttpPostedFileBase file)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                TinTuc tin = new TinTuc();
                tin.TieuDe = model.TieuDe;
                file = file ?? Request.Files["file"];
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        var path = Path.Combine(Server.MapPath("~/images/news/"), fileName);
                        file.SaveAs(path);
                        tin.Anh = "/images/news/" + fileName;


                    }
                }
                else
                {
                    tin.Anh = "/images/p-1.png";


                }
                tin.NoiDung = model.NoiDung;
                tin.MaTaiKhoan = Convert.ToInt32(Session["MaTk"].ToString().Trim());
                tin.NgayDang = DateTime.Now;
                shop.TinTuc.Add(tin);
                shop.SaveChanges();
                Response.Redirect("Index");
                return RedirectToAction("Index", model);
            }

        }
        [HttpGet]
        public ActionResult SuaTin(int id)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                matin = id;
                DatabaseEntities shop = new DatabaseEntities();
                var model = shop.TinTuc.SingleOrDefault(s => s.MaTin == id);

                return View(model);
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaTin(TinTuc tin, string img, HttpPostedFileBase file)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                file = file ?? Request.Files["file"];

                DatabaseEntities shop = new DatabaseEntities();
                var tintuc = shop.TinTuc.SingleOrDefault(s => s.MaTin == matin);


                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        var path = Path.Combine(Server.MapPath("~/images/news/"), fileName);
                        file.SaveAs(path);
                        tintuc.TieuDe = tin.TieuDe;
                        tintuc.NoiDung = tin.NoiDung;
                        tintuc.Anh = "/images/news/" + fileName;
                        shop.SaveChanges();
                    }
                }
                else
                {
                    tintuc.TieuDe = tin.TieuDe;
                    tintuc.NoiDung = tin.NoiDung;
                    tintuc.Anh = img;
                    shop.SaveChanges();
                }


                var model = shop.TinTuc.OrderByDescending(s => s.MaTin).ToList();
                return View("Index", model);
            }

        }


        public ActionResult XoaTin(int id)
        {
            if (Session["MaTk"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                DatabaseEntities shop = new DatabaseEntities();
                var model = shop.TinTuc.OrderByDescending(s => s.MaTin).ToList();

                var tintuc = shop.TinTuc.SingleOrDefault(s => s.MaTin == id);
                if (tintuc != null)
                {
                    shop.TinTuc.Remove(tintuc);
                    shop.SaveChanges();
                }

                return RedirectToAction("Index", model);
            }


        }
    }
}