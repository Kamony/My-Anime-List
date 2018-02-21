using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyAnime_updatedDB.Models;

namespace MyAnime_updatedDB.Controllers
{
    [Authorize]
    public class AnimeListsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AnimeLists
        public ActionResult Index()
        {
            string currentUserID = User.Identity.GetUserId();

            var availibleAnimes =
                (from o in db.AnimeLists
                 where o.UserId == currentUserID
                 select o.Anime).ToList();

            return View(availibleAnimes);
        }



        private IEnumerable<AnimeList> GetMyAnimeList()
        {
            string currentUserID = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(
                x => x.Id == currentUserID);
            var animeLists = db.AnimeLists.Include(a => a.Anime);

            return animeLists.ToList().Where(x => x.User == currentUser);
        }

        public ActionResult BuildAnimeListTable()
        {

            return PartialView("_AnimeListTable", GetMyAnimeList());
        }

        public ActionResult BuildDropdownList()
        {
            string currentUserID = User.Identity.GetUserId();

            var availibleAnimes =
                from c in db.Animes
                where !(from o in db.AnimeLists
                        where o.UserId == currentUserID
                        select o.AnimeId)
                        .Contains(c.ID)
                select c;
            if (availibleAnimes.Any())
            {
                ViewBag.AnimeId = new SelectList(availibleAnimes, "ID", "AnimeName");
                return PartialView("_BuildDropDownList");
            }
            else
            {
                return PartialView("_EmptyDropDown");
            }
            //ViewBag.AnimeId = new SelectList(availibleAnimes, "ID", "AnimeName","Image");
            //return PartialView("_BuildDropDownList");
        }



        // GET: AnimeLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnimeList animeList = db.AnimeLists.Find(id);
            if (animeList == null)
            {
                return HttpNotFound();
            }
            return View(animeList);
        }

        // GET: AnimeLists/Create
        public ActionResult Create()
        {
            ViewBag.AnimeId = new SelectList(db.Animes, "ID", "AnimeName");
            
            return View();
        }

        // POST: AnimeLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AnimeId")] AnimeList animeList)
        {
            if (ModelState.IsValid)
            {
                string currentUserID = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(
                    x => x.Id == currentUserID);
                animeList.User = currentUser;
               
                db.AnimeLists.Add(animeList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AnimeId = new SelectList(db.Animes, "ID", "AnimeName", animeList.AnimeId);
            
            return PartialView("_AnimeListTable");
        }

      
        //// GET: AnimeLists/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AnimeList animeList = db.AnimeLists.Find(id);
        //    if (animeList == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.AnimeId = new SelectList(db.Animes, "ID", "AnimeName", animeList.AnimeId);

        //    return View(animeList);
        //}

        //// POST: AnimeLists/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,AnimeId,UserId")] AnimeList animeList)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(animeList).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.AnimeId = new SelectList(db.Animes, "ID", "AnimeName", animeList.AnimeId);

        //    return View(animeList);
        //}

        // GET: AnimeLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnimeList animeList = db.AnimeLists.Find(id);
            if (animeList == null)
            {
                return HttpNotFound();
            }
            db.AnimeLists.Remove(animeList);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //// POST: AnimeLists/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    AnimeList animeList = db.AnimeLists.Find(id);
        //    db.AnimeLists.Remove(animeList);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
