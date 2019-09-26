using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebBanGiay.Models;

namespace WebBanGiay.Controllers
{
    public class SearchController : Controller
    {
        dbQLBanGiayDataContext db = new dbQLBanGiayDataContext();
        [HttpPost]
        public ActionResult KetQua(FormCollection f, int? page)
        {
            string sKey = f["txtSearch"].ToString();
            List<Giay> lstKQ = db.Giays.Where(n => n.TenGiay.Contains(sKey)).ToList();
            ViewBag.Key = sKey;
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            if (lstKQ.Count == 0)
            {
                ViewBag.Thongbao = "Không tìm thấy sản phẩm nào!";
                return View(db.Giays.OrderBy(n => n.TenGiay).ToPagedList(pageNumber, pageSize));
            }
            ViewBag.Thongbao = "Đã tìm thấy " + " " + lstKQ.Count + " " + " kết quả";

            return View(lstKQ.OrderBy(n => n.TenGiay).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult KetQua(int? page, string sKey)
        {
            ViewBag.Key = sKey;
            List<Giay> lstKQ = db.Giays.Where(n => n.TenGiay.Contains(sKey)).ToList();

            int pageNumber = (page ?? 1);
            int pageSize = 9;
            if (lstKQ.Count == 0)
            {
                ViewBag.Thongbao = "Không tìm thấy sản phẩm nào!";
                return View(db.Giays.OrderBy(n => n.TenGiay).ToPagedList(pageNumber, pageSize));
            }
            ViewBag.Thongbao = "Đã tìm thấy " + lstKQ.Count + " kết quả";

            return View(lstKQ.OrderBy(n => n.TenGiay).ToPagedList(pageNumber, pageSize));
        }
    }
}