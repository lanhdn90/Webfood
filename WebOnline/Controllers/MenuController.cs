using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnline.Models;

namespace WebOnline.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            var role = Convert.ToInt32(Session["Role"].ToString());
            DatabaseEntities shop = new DatabaseEntities();
            var model = shop.Menu.Where(x => x.RoleID == role).ToList();
            return PartialView(model);
        }
    }
}