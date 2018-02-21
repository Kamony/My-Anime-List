using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyAnime_updatedDB.Models;

namespace MyAnime_updatedDB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           // Vytvoreni administratora
            
            //RoleManager<IdentityRole> spravceRoli = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext()));
            //spravceRoli.Create(new IdentityRole("admin"));
            //UserManager<ApplicationUser> spravceUzivatelu = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            //ApplicationUser uzivatel = spravceUzivatelu.FindByName("admin@email.com");
            //spravceUzivatelu.AddToRole(uzivatel.Id, "admin");

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Manage()
        {
            return View("AdminSection");
        }
    }
}