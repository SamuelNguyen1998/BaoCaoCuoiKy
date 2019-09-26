using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BotDetect.Web.Mvc;
using WebBanGiay.Models;

namespace WebBanGiay.Controllers
{
    public class NguoiDungController : Controller
    {
        dbQLBanGiayDataContext db = new dbQLBanGiayDataContext();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }

        private bool KiemTraUser(string Id)
        {
            return db.KhachHangs.Count(n => n.ID == Id) > 0;
        }

        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KhachHang kh)
        {
            var HoTenKH = collection["HoTenKH"];
            var Taikhoan = collection["Taikhoan"];
            var Matkhau = collection["Matkhau"];
            var MatkhauRepeat = collection["MatkhauRepeat"];
            var Email = collection["Email"];
            var DiachiKH = collection["DiachiKH"];
            var DienthoaiKH = collection["DienthoaiKH"];
            var Ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);

            var khoa1 = db.KhachHangs.Where(x => x.ID.Equals(Taikhoan)).ToList();
            var khoa2 = db.KhachHangs.Where(x => x.Email.Equals(Email)).ToList();

            if (String.IsNullOrEmpty(HoTenKH))
            {
                ViewData["ErrorName"] = "Họ tên không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(Taikhoan))
            {
                ViewData["ErrorUsername"] = "Nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(Matkhau))
            {
                ViewData["ErrorPassword"] = "Nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(MatkhauRepeat))
            {
                ViewData["ErrorPasswordRP"] = "Mật khẩu không trùng khớp";
            }
            else if (String.IsNullOrEmpty(Email))
            {
                ViewData["ErrorEmail"] = "Email không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(DienthoaiKH))
            {
                ViewData["ErrorPhone"] = "Điện thoại không được luôn";
            }
            else if (khoa1.Count == 0)
            {
                if (khoa2.Count == 0)
                {
                    kh.HoTen = HoTenKH;
                    kh.ID = Taikhoan;
                    kh.Pass = Matkhau;
                    kh.Email = Email;
                    kh.DiaChi = DiachiKH;
                    kh.DienThoai = DienthoaiKH;
                    kh.NgaySinh = DateTime.Parse(Ngaysinh);
                    db.KhachHangs.InsertOnSubmit(kh);
                    db.SubmitChanges();
                    ViewBag.Thongbao = "Đăng Ký thành công";
                    return RedirectToAction("DangNhap");
                }
                else
                {
                    ViewData["loi"] = "Email này đã được sử dụng";
                }
            }
            else
            {
                ViewData["loi"] = "Tên đăng nhập của bạn đã được sử dụng";
            }
            return this.Dangky();
        }


        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }

        [HttpPost]
        //[CaptchaValidation("CaptchaCode", "registerCapcha", "Mã xác nhận không đúng!")]
        public ActionResult Dangnhap(FormCollection getInfo)
        {
            var tendn = getInfo["Taikhoan"];
            var matkhau = getInfo["Matkhau"];

            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["ErrorUsername"] = "Bạn chưa nhập tài khoản";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["ErrorPassword"] = "Bạn chưa nhập mật khẩu";
            }
            else
            {
                KhachHang kh = db.KhachHangs.SingleOrDefault(x => x.ID == tendn && x.Pass == matkhau);
                if (kh != null)
                {
                    ViewBag.DynamicScripts = "validateUSER()";
                    ViewBag.Thongbao = "Đăng nhập thành công";
                    Session["Taikhoan"] = kh;


                }
                else
                    ViewBag.Thongbao = "Đăng nhập không thành công, Thông tin tài khoản mật khẩu không chính xác";

            }
            return View();
        }

        public ActionResult DangNhap2(FormCollection collection)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            return RedirectToAction("ThongTin", "NguoiDung");
        }

        public ActionResult DangXuat()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                ViewBag.DX = "Đăng ký";
            }
            else
            {
                KhachHang kh = (KhachHang)Session["TaiKhoan"];
                ViewBag.DX = "Đăng xuất";
            }
            return PartialView();
        }
        public ActionResult DangXuat2(FormCollection collection)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangKy", "NguoiDung");
            }
            Session["Taikhoan"] = null;
            return RedirectToAction("DangKy", "NguoiDung");
        }
      //  public static void RegisterRoutes(RouteCollection routes)
      //  {
      //      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      //      routes.IgnoreRoute("{*botdetect}",
      //new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });
      //  }
    }
}
