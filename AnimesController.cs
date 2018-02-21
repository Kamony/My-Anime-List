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
    public class AnimesController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Animes
        public ActionResult Index()
        {
           return View();
        }

        public ActionResult Search(string searchString)
        {
            var animes = from m in _db.Animes
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                animes = animes.Where(s => s.AnimeName.Contains(searchString));
            }

            return View("ShowAllAnimes",animes.ToList());
        }

        public ActionResult ShowAnimeByGenre(int id)
        {
            var usableAnimes = from a in _db.Animes
                               where a.GenreId == id
                               select a;
            return View(usableAnimes.ToList());
        }

        public ActionResult ShowNewestAnime()
        {
            var animes = 
                from a in _db.Animes
                select a;
          
            return PartialView("_ShowNewestAnime",animes.ToList().OrderByDescending(x => x.Year).Take(5));
        }
        public ActionResult ShowOldestAnime()
        {
            var animes =
                from a in _db.Animes
                select a;
            
            return PartialView("_ShowNewestAnime", animes.ToList().OrderBy(x => x.Year).Take(5));
        }

        public ActionResult ShowOngoingAnime()
        {
            var animes =
              from a in _db.Animes
              where a.Finished == false
              select a;
            return PartialView("_ShowNewestAnime", animes.ToList().Take(5));
        }

        public ActionResult ShowShortestAnime()
        {
            var animes=_db.Animes.Include(a => a.Genre);
            return PartialView("_ShowNewestAnime", animes.ToList().OrderBy(x => x.Episodes).Take(5));
        }

        public ActionResult ShowAllAnimes()
        {
            var animes = _db.Animes.Include(a => a.Genre);
            return View(animes.ToList().OrderBy(x=>x.AnimeName));
        }
        [Authorize]
        public ActionResult addToList(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anime anime = _db.Animes.Find(id);
            if (anime == null)
            {
                return HttpNotFound();
            }
            AnimeList animeList = new AnimeList();

            string currentUserID = User.Identity.GetUserId();
            ApplicationUser currentUser = _db.Users.FirstOrDefault(
                x => x.Id == currentUserID);
            

            var availibleAnimes =
                from c in _db.Animes
                where !(from o in _db.AnimeLists
                        where o.UserId == currentUserID
                        select o.AnimeId)
                        .Contains(c.ID)
                select c;

            if (availibleAnimes.Any())
            {
                if (availibleAnimes.ToList().Contains(anime))
                {
                    animeList.User = currentUser;
                    animeList.Anime = anime;
                    _db.AnimeLists.Add(animeList);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("Index","AnimeLists");
        }


        // GET: Animes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anime anime = _db.Animes.Find(id);
            if (anime == null)
            {
                return HttpNotFound();
            }
            return View(anime);
        }
        [Authorize]
        public ActionResult Manage()
        {
            var animes = _db.Animes.Include(a => a.Genre);
            return View(animes.ToList());
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.GenreId = new SelectList(_db.Genres, "Id", "GenreName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "Id,AnimeName,GenreId,Description,Year,Episodes,Finished,Image")] Anime anime)
        {
            if (ModelState.IsValid)
            {
                _db.Animes.Add(anime);
                _db.SaveChanges();
                return RedirectToAction("Manage");
            }
            ViewBag.GenreId = new SelectList(_db.Genres, "Id", "GenreName", anime.GenreId);
            return View(anime);
        }

        // GET: Genres/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anime anime = _db.Animes.Find(id);
            if (anime == null)
            {
                return HttpNotFound();
            }
            ViewBag.GenreId = new SelectList(_db.Genres, "Id", "GenreName", anime.GenreId);
            return View(anime);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "Id,AnimeName,GenreId,Description,Year,Episodes,Finished,Image")] Anime anime)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(anime).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Manage");
            }
            ViewBag.GenreId = new SelectList(_db.Genres, "Id", "GenreName", anime.GenreId);
            return View(anime);
        }

        // GET: Genres/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           Anime anime = _db.Animes.Find(id);
            if (anime == null)
            {
                return HttpNotFound();
            }
            return View(anime);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Anime genre = _db.Animes.Find(id);
            _db.Animes.Remove(genre);
            _db.SaveChanges();
            return RedirectToAction("Manage");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
