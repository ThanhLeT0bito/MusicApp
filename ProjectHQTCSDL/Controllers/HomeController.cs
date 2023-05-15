using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectHQTCSDL.Models;

namespace ProjectHQTCSDL.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        ProjectMusicEntities db = new ProjectMusicEntities();
        
        public ActionResult Index()
        {
            var getLstSong = db.Musics.ToList();
            ViewBag.getLstSong = getLstSong;
            return View();
        }

        public ActionResult ChangeMusic(int idMusic)
        {
            var getMusic = db.Musics.FirstOrDefault(x => x.idMusic == idMusic);
            if (getMusic != null)
            {

                Session["Music"] = getMusic.urlBaiHat;
                Session["BaiHat"] = getMusic;
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangeMusicCate(int idMusic, int idCategory)
        {
            var getMusic = db.Musics.FirstOrDefault(x => x.idMusic == idMusic);
            if (getMusic != null)
            {

                Session["Music"] = getMusic.urlBaiHat;
                Session["BaiHat"] = getMusic;
            }

            return RedirectToAction("OpenCategory", "Home", new {idCategory = idCategory });
        }
        public ActionResult OpenCategory(int idCategory)
        {
            //int idCategory
            var getCategoryItem = db.Category_Item.Where(x => x.idCategory == idCategory).ToList();
            var getcategory = db.Categories.FirstOrDefault(x => x.idCategory == idCategory);
            if (getcategory != null)
                ViewBag.nameCategory = getcategory.nameCategory;
            ViewBag.sl = getCategoryItem.Count;
            return View(getCategoryItem);
        }
    }
}