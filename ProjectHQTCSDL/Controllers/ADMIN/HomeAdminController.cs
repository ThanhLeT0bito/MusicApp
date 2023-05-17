using ProjectHQTCSDL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectHQTCSDL.Controllers.ADMIN
{
    public class HomeAdminController : Controller
    {
        // GET: HomeAdmin
        ProjectMusicEntities db = new ProjectMusicEntities();
        public ActionResult Index()
        {
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

        public ActionResult AddCategory(string inputValue)
        {
            if (inputValue != null && inputValue != "")
            {
                var check = db.Categories.FirstOrDefault(x => x.nameCategory == inputValue);
                if ( check == null)
                {
                    Category newCate = new Category()
                    {
                        nameCategory = inputValue,
                        ngayTao = DateTime.Now,
                    };
                    db.Categories.Add(newCate);
                    db.SaveChanges();
                   
                    if (ViewBag.Message != null)
                        ViewBag.Message = null;
                    Session["ShowInputPlaylist"] = null;
                }
                else
                {
                    ViewBag.Message = "Bị trùng tên Category";
                }
            }
            return RedirectToAction("Index", "HomeAdmin");
        }
        public ActionResult ShowInputCategory()
        {
            if (Session["ShowInputCategory"] == null)
            {
                Session["ShowInputCategory"] = true;
            }
            else
            {
                Session["ShowInputCategory"] = null;

            }

            return RedirectToAction("Index", "HomeAdmin");
        }

        public ActionResult OpenCategory(int idCategory)
        {
            //int idCategory
            var getCategoryItem = db.Category_Item.Where(x => x.idCategory == idCategory).ToList();
            var getcategory = db.Categories.FirstOrDefault(x => x.idCategory == idCategory);
            Session["Category"] = getcategory;
            if (getcategory != null)
                ViewBag.nameCategory = getcategory.nameCategory;
            ViewBag.sl = getCategoryItem.Count;
            return View(getCategoryItem);
        }

        public ActionResult SelectedCategory(int idCategory)
        {
            var getCategory = db.Categories.FirstOrDefault(x => x.idCategory == idCategory);
            Session["Category"] = getCategory;
            return RedirectToAction("Search", "HomeAdmin");
        }

        public ActionResult AddMusictoCategory(int idMusic)
        {
            var getCategory = Session["Category"] as Category;
            var checkMusic = db.Category_Item.FirstOrDefault(x => x.idMusic == idMusic && x.idCategory == getCategory.idCategory);
            if (checkMusic == null && getCategory != null)
            {
                Category_Item newCategory_Item = new Category_Item()
                {
                    idCategory = getCategory.idCategory,
                    idMusic = idMusic,
                };
                db.Category_Item.Add(newCategory_Item);
                db.SaveChanges();
            }
            return RedirectToAction("OpenCategory", "HomeAdmin", new { idCategory = getCategory.idCategory });
        }

        public ActionResult DeleteMusicToCategory(int idCT)
        {
            var getCategory = Session["Category"] as Category;
            var getCategoryItem = db.Category_Item.FirstOrDefault(x => x.idCT == idCT);
            if (getCategoryItem != null && getCategory != null)
            {
                db.Category_Item.Remove(getCategoryItem);
                db.SaveChanges();
            }
            return RedirectToAction("OpenCategory", "HomeAdmin", new { idCategory = getCategory.idCategory });
        }
    }
}