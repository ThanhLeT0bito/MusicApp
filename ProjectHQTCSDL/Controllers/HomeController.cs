using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            UserMain newUser = new UserMain()
            {
                idUser = 3,
                tenDangNhap = "user01",
                matKhau = "123"
            };

            Session["User"] = newUser;
            return View();
        }
        public ActionResult Search(String search)
        {

            if (string.IsNullOrEmpty(search))
            {
                ViewBag.MessageSeach = "Vui lòng nhập từ khóa tìm kiếm.";
                ViewBag.inputSearch = search;
                var songs1 = db.Musics.ToList();

                return View(songs1);
            }
            ViewBag.inputSearch = search;
            //var searchResults = db.Database.SqlQuery<Music>("SELECT * FROM dbo.TimKiemNhac(@TuKhoa)", new SqlParameter("@TuKhoa", keyword)).ToList();
            var songs = db.Musics
                .Where(p => p.tenBaiHat.Contains(search))
                .ToList();

            if (songs.Count == 0)
            {
                ViewBag.MessageSearch = "Shop hiện nay không có sản phẩm này.";
            }
            return View(songs);
        }

        //temponary
        public ActionResult JustChangeMusic(int idMusic)
        {
            var getMusic = db.Musics.FirstOrDefault(x => x.idMusic == idMusic);
            if (getMusic != null)
            {

                Session["Music"] = getMusic.urlBaiHat;
                Session["BaiHat"] = getMusic;
            }
            return new EmptyResult();
        }
        //
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

        public ActionResult OpenMusicFromSinger(int idCaSi)
        {
            var getSinger = db.CaSis.FirstOrDefault(x => x.idCaSi == idCaSi);
            var getMusic = db.Musics.Where(x => x.idCaSi == getSinger.idCaSi).ToList();
            ViewBag.Singer = getSinger;
            @ViewBag.sl = getMusic.Count;

            return View(getMusic);
        }

        [HttpPost]
        public ActionResult AddPlaylist(string inputValue)
        {
            if (inputValue != null && inputValue != "")
            {
                var getUser = Session["User"] as UserMain;
                var check = db.Playlists.FirstOrDefault(x => x.tenPlaylist == inputValue);
                if (getUser != null && check == null)
                {
                    Playlist newPlaylist = new Playlist()
                    {
                        idUser = getUser.idUser,
                        tenPlaylist = inputValue,
                        ngayTao = DateTime.Now
                    };
                    db.Playlists.Add(newPlaylist);
                    db.SaveChanges();
                    if (ViewBag.Message != null)
                        ViewBag.Message = null;
                    Session["ShowInputPlaylist"] = null;
                }
                else
                {
                    ViewBag.Message = "Bạn chưa Đăng nhập Hoặc Trùng tên Playlist";
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ShowInputPlaylist()
        {
            if (Session["ShowInputPlaylist"] == null)
            {
                Session["ShowInputPlaylist"] = true;
            }
            else
            {
                Session["ShowInputPlaylist"] = null;

            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ShowPlaylist(int idPlaylist)
        {
            var getPlaylistItem = db.Playlist_Item.Where(x => x.idPlaylist == idPlaylist).ToList();
            var getPlaylist = db.Playlists.FirstOrDefault(x => x.idPlaylist == idPlaylist);
            if (getPlaylist != null)
                ViewBag.namePlaylist = getPlaylist.tenPlaylist;
            ViewBag.sl = getPlaylistItem.Count;
            return View(getPlaylistItem);
        }
        //selected Playlist
        public ActionResult SelectedPlaylist(int idPlaylist)
        {
            var getPlaylist = db.Playlists.FirstOrDefault(x => x.idPlaylist == idPlaylist);
            Session["Playlist"] = getPlaylist;
            return RedirectToAction("Search", "Home");
        }

        //add music to playlist
        public ActionResult AddMusicToPlaylist(int idMusic) 
        {
            var getPlaylist = Session["PLaylist"] as Playlist;
            if (getPlaylist != null)
            {
                var checkPLItem = db.Playlist_Item.Where(x => x.idMusic == idMusic && x.idPlaylist == getPlaylist.idPlaylist).FirstOrDefault();
                if (checkPLItem == null)
                {
                    Playlist_Item newPlaylistItem = new Playlist_Item()
                    {
                        idPlaylist = getPlaylist.idPlaylist,
                        idMusic = idMusic,
                    };
                    db.Playlist_Item.Add(newPlaylistItem);
                    db.SaveChanges();
                }

            }
            return RedirectToAction("ShowPlaylist", "Home", new {idPLaylist = getPlaylist.idPlaylist });
        }

    }
}