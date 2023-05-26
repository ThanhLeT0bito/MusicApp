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

        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(UserMain user)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(user.tenDangNhap))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(user.matKhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                //admin
                if (ModelState.IsValid)
                {
                    var getUser = db.UserMains.FirstOrDefault(x => x.tenDangNhap == user.tenDangNhap && x.matKhau == user.matKhau && x.isAdmin == true);
                    if (getUser != null)
                    {
                        return RedirectToAction("Index", "HomeAdmin");
                    }
                }
                if (ModelState.IsValid)
                {
                    var getUser = db.UserMains.FirstOrDefault(x => x.tenDangNhap == user.tenDangNhap && x.matKhau == user.matKhau && x.isAdmin != true);
                    if(getUser!= null)
                    {
                        Session["User"] = getUser;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                    }

                }

            }
            return View();
        }
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(UserMain user)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(user.nameUser))
                    ModelState.AddModelError(String.Empty, "không được để trống");
                if(String.IsNullOrEmpty(user.tenDangNhap))
                    ModelState.AddModelError(String.Empty, "không được để trống");
                if (String.IsNullOrEmpty(user.matKhau))
                    ModelState.AddModelError(String.Empty, "không được để trống");
                if (ModelState.IsValid)
                {
                    var getUser = db.UserMains.FirstOrDefault(x => x.tenDangNhap == user.tenDangNhap && x.matKhau == user.matKhau && x.isAdmin != true);
                    if (getUser != null)
                    {
                    ModelState.AddModelError(String.Empty, "Đã có người dùng");
                    }
                    else
                    {
                        db.UserMains.Add(user);
                        db.SaveChanges();
                        return RedirectToAction("SignIn", "Home");
                    }
                }
            }
            return View();
        }

        public ActionResult SignOut()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Index()
        {
            var getLstSong = db.Musics.ToList();
            ViewBag.getLstSong = getLstSong;
            //UserMain newUser = new UserMain()
            //{
            //    idUser = 3,
            //    tenDangNhap = "user01",
            //    matKhau = "123"
            //};

            //Session["User"] = newUser;
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
            var tempSearch = db.TimKiemNhac(tuKhoa: search).ToList() ;
            var query = $"SELECT * FROM dbo.TimKiemNhac(@TuKhoa)";
            var parameter = new SqlParameter("@TuKhoa", search);

            var result = db.Database.SqlQuery<Music>(query, parameter).ToList();

            var songs = db.Musics
                .Where(p => p.tenBaiHat.Contains(search))
                .ToList();

            if (songs.Count == 0)
            {
                ViewBag.MessageSearch = "hiện nay không có sản phẩm này.";
            }
            return View(result);
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
            var getCategory_Items= db.Database.SqlQuery<Category_Item>("SELECT * FROM dbo.FUNC_LayDSTheoCategory(@TuKhoa)", new SqlParameter("@TuKhoa", idCategory)).ToList();
            if (getcategory != null)
                ViewBag.nameCategory = getcategory.nameCategory;
            ViewBag.sl = getCategoryItem.Count;
            return View(getCategory_Items);
        }

        public ActionResult OpenMusicFromSinger(int idCaSi)
        {
            var getSinger = db.CaSis.FirstOrDefault(x => x.idCaSi == idCaSi);
            var getMusic = db.Musics.Where(x => x.idCaSi == getSinger.idCaSi).ToList();
            ViewBag.Singer = getSinger;
            @ViewBag.sl = getMusic.Count;

            var getDSMusic = db.Database.SqlQuery<Music>("SELECT * FROM dbo.FUNC_LayDSBHTheoCaSi(@TuKhoa)", new SqlParameter("@TuKhoa", idCaSi)).ToList();

            return View(getDSMusic);
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
            Session["PLaylist"] = getPlaylist;
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
        public ActionResult DeleteMusicFromPlaylist(int idPI)
        {
            var getPlaylist = Session["PLaylist"] as Playlist;
            var getPlayListItem = db.Playlist_Item.FirstOrDefault(x => x.idPI == idPI);
            if (getPlayListItem != null)
            {
                db.Playlist_Item.Remove(getPlayListItem);
                db.SaveChanges();
            }
            return RedirectToAction("ShowPlaylist", "Home", new { idPLaylist = getPlaylist.idPlaylist });
        }

        //comment
        public ActionResult ShowComment(int idMusic)
        {
            var getComment = db.Comments.Where(x => x.idMusic == idMusic).ToList();
            ViewBag.sl = getComment.Count;
            return View(getComment);
        }
        public ActionResult AddComment(string inputValue)
        {
            var getMusic = Session["BaiHat"] as Music;
            if (inputValue != null && inputValue != "")
            {
                var getUser = Session["User"] as UserMain;
                if (getUser != null && getMusic != null)
                {
                    Comment newComment = new Comment()
                    {
                        idUser = getUser.idUser,
                        idMusic = getMusic.idMusic,
                        commentContent = inputValue,
                    };
                    db.Comments.Add(newComment);
                    db.SaveChanges();
                }
            }
            
            return RedirectToAction("ShowComment", "Home", new { idMusic = getMusic.idMusic });
        }
        public ActionResult ShowLyric(int idLyric)
        {
            var getComment = db.Lyrics.FirstOrDefault(x => x.idLyric == idLyric);
            return View(getComment);
        }
    }
}