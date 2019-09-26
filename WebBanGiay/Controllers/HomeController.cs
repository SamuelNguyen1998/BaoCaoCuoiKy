using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using WebBanGiay.Models;

namespace WebBanGiay.Controllers
{
    public class HomeController : Controller
    {
        dbQLBanGiayDataContext data = new dbQLBanGiayDataContext();

        private List<Giay>Laygiaymoi(int count)
        {
            
            return data.Giays.ToList();
        }
        public ActionResult Index(int? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            var giaymoi = Laygiaymoi(5);
            return View(giaymoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult HangSanXuat()
        {
            var hangsanxuat = from hang in data.HangSXes select hang;
            return PartialView(hangsanxuat);
        }
        public ActionResult SPtheoHang(string id, int? page)
        {
            int pagesize = 6;
            int pagenum = (page ?? 1);

            var hangsanxuat = from hang in data.Giays where hang.MaHang == id select hang;
            return View(hangsanxuat.ToPagedList(pagenum, pagesize));
        }
        public ActionResult LoaiSP()
        {
            var hangsanxuat = from hang in data.LoaiGiays select hang;
            return PartialView(hangsanxuat);
        }
        public ActionResult SPtheoLoai(string id, int? page)
        {
            int pagesize = 6;
            int pagenum = (page ?? 1);

            var loaidt = from hang in data.Giays where hang.MaLoai == id select hang;
            return View(loaidt.ToPagedList(pagenum, pagesize));
        }
        public ActionResult ChiTietSP(string id)
        {
            var sp = from hang in data.Giays where hang.MaGiay == id select hang;
            return View(sp.Single());

        }
        public ActionResult SPBanChay(string id, int? page)
        {
            int pagesize = 6;
            int pagenum = (page ?? 1);
            var spbanchay = from sp in data.ChiTietDonHangs join giay in data.Giays on sp.IDGiay equals giay.MaGiay select giay;           
            return View(spbanchay.ToList().ToPagedList(pagenum, pagesize));
        }

        public ActionResult HoTro()
        {
            return View();
        }

    }
}
