using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectHQTCSDL.Models;

namespace ProjectHQTCSDL.Controllers.ADMIN
{
    public class MusicController : Controller
    {
        private ProjectMusicEntities db = new ProjectMusicEntities();

        // GET: Music
        public ActionResult Index()
        {
            var musics = db.Musics.Include(m => m.CaSi).Include(m => m.Lyric).Include(m => m.TacGia);
            return View(musics.ToList());
        }

        // GET: Music/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Music music = db.Musics.Find(id);
            if (music == null)
            {
                return HttpNotFound();
            }
            return View(music);
        }

        // GET: Music/Create
        public ActionResult Create()
        {
            ViewBag.idCaSi = new SelectList(db.CaSis, "idCaSi", "tenCaSi");
            ViewBag.idLyric = new SelectList(db.Lyrics, "idLyric", "loiBaihat");
            ViewBag.idtacGia = new SelectList(db.TacGias, "idTacGia", "tenTacGia");
            return View();
        }

        // POST: Music/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idMusic,idCaSi,idtacGia,idLyric,tenBaiHat,urlBaiHat,thongTinBaiHat,imgMusic")] Music music)
        {
            if (ModelState.IsValid)
            {
                db.Musics.Add(music);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCaSi = new SelectList(db.CaSis, "idCaSi", "tenCaSi", music.idCaSi);
            ViewBag.idLyric = new SelectList(db.Lyrics, "idLyric", "loiBaihat", music.idLyric);
            ViewBag.idtacGia = new SelectList(db.TacGias, "idTacGia", "tenTacGia", music.idtacGia);
            return View(music);
        }

        // GET: Music/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Music music = db.Musics.Find(id);
            if (music == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCaSi = new SelectList(db.CaSis, "idCaSi", "tenCaSi", music.idCaSi);
            ViewBag.idLyric = new SelectList(db.Lyrics, "idLyric", "loiBaihat", music.idLyric);
            ViewBag.idtacGia = new SelectList(db.TacGias, "idTacGia", "tenTacGia", music.idtacGia);
            return View(music);
        }

        // POST: Music/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idMusic,idCaSi,idtacGia,idLyric,tenBaiHat,urlBaiHat,thongTinBaiHat,imgMusic")] Music music)
        {
            if (ModelState.IsValid)
            {
                db.Entry(music).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCaSi = new SelectList(db.CaSis, "idCaSi", "tenCaSi", music.idCaSi);
            ViewBag.idLyric = new SelectList(db.Lyrics, "idLyric", "loiBaihat", music.idLyric);
            ViewBag.idtacGia = new SelectList(db.TacGias, "idTacGia", "tenTacGia", music.idtacGia);
            return View(music);
        }

        // GET: Music/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Music music = db.Musics.Find(id);
            if (music == null)
            {
                return HttpNotFound();
            }
            return View(music);
        }

        // POST: Music/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Music music = db.Musics.Find(id);
            //db.Musics.Remove(music);  
            db.DeleteMusicById(id);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
