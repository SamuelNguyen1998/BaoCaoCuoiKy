using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebBanGiay.Models;

namespace WebBanGiay.Controllers
{
    public class AdminController : Controller
    {

        dbQLBanGiayDataContext db = new dbQLBanGiayDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["TaiKhoanAdmin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                return View();
            }


        }
        public ActionResult Giay(int? page)
        {
            if (Session["TaiKhoanAdmin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;
                //return View(db.DienThoais.ToList());
                return View(db.Giays.ToList().OrderBy(n => n.MaGiay).ToPagedList(pageNumber, pageSize));
            }

        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["userError"] = "không được bỏ trống!";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["passError"] = "phải được nhập!";
            }
            else
            {
                AD ad = db.ADs.SingleOrDefault(n => n.Username == tendn && n.Pass == matkhau);
                if (ad != null)
                {
                    ViewBag.Thongbao = "Đăng nhập thành công!";
                    Session["TaiKhoanAdmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ThemGiay()
        {
            ViewBag.MaHang = new SelectList(db.HangSXes.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
            ViewBag.MaLoai = new SelectList(db.LoaiGiays.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemGiay(Giay dt, HttpPostedFileBase fileUpLoad, FormCollection fc)
        {
            var MaDT = fc["MaGiay"];
            var khoa = db.Giays.Where(x => x.MaGiay.Equals(MaDT)).ToList();
            ViewBag.MaHang = new SelectList(db.HangSXes.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
            ViewBag.MaLoai = new SelectList(db.LoaiGiays.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            if (khoa.Count == 0)
            {
                if (fileUpLoad == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn hình ảnh";
                    return View();
                }
                else
                {
                    var filename = Path.GetFileName(fileUpLoad.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), filename);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpLoad.SaveAs(path);
                    }
                    dt.HinhAnh = filename;
                    dt.MaGiay = MaDT;
                    db.Giays.InsertOnSubmit(dt);
                    db.SubmitChanges();

                }
                return RedirectToAction("Giay");
            }
            else
            {
                ViewData["loi"] = "Mã giày này bị trùng";
            }
            return this.ThemGiay();
        }
        public ActionResult ChiTietGiay(string id)
        {
            Giay dt = db.Giays.SingleOrDefault(n => n.MaGiay == id);
            //ViewBag.MaDienThoai = dt.MaDienThoai;
            if (dt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dt);
        }
        [HttpGet]
        public ActionResult XoaGiay(string id)
        {
            Giay dt = db.Giays.SingleOrDefault(n => n.MaGiay == id);
            //ViewBag.MaDienThoai = dt.MaDienThoai;
            if (dt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dt);
        }
        [HttpPost, ActionName("XoaGiay")]
        public ActionResult XacNhanXoa(string id)
        {
            Giay dt = db.Giays.SingleOrDefault(n => n.MaGiay == id);

            if (dt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Giays.DeleteOnSubmit(dt);
            db.SubmitChanges();
            return RedirectToAction("Giay");
        }
        [HttpGet]
        public ActionResult SuaGiay(string id)
        {
            if (Session["TaiKhoanAdmin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            Giay dt = db.Giays.SingleOrDefault(n => n.MaGiay == id);
            if (dt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaHang = new SelectList(db.HangSXes.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang", dt.MaHang);
            ViewBag.MaLoai = new SelectList(db.LoaiGiays.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", dt.MaLoai);
            return View(dt);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaGiay(String id, HttpPostedFileBase fileUpload)
        {
            Giay dt = db.Giays.SingleOrDefault(n => n.MaGiay == id);
            ViewBag.MaHang = new SelectList(db.HangSXes.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
            ViewBag.MaLoai = new SelectList(db.LoaiGiays.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh";
                UpdateModel(dt);
                db.SubmitChanges();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    fileUpload.SaveAs(path);
                    dt.HinhAnh = fileName;
                    UpdateModel(dt);
                    db.SubmitChanges();
                }
            }

            return RedirectToAction("Giay", "Admin");
        }
        public ActionResult NhaSanXuat(int? page)
        {
            if (Session["TaiKhoanAdmin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                int pageNumber = (page ?? 1);
                int pageSize = 5;
                return View(db.HangSXes.ToList().OrderBy(n => n.MaHang).ToPagedList(pageNumber, pageSize));
            }

        }
        [HttpGet]
        public ActionResult ThemNSX()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemNSX(HangSX hsx, HttpPostedFileBase logoNSX, FormCollection fc)
        {
            var MaHang = fc["MaHang"];
            var khoa = db.HangSXes.Where(x => x.MaHang.Equals(MaHang)).ToList();
            if (khoa.Count == 0)
            {
                if (logoNSX == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn hình ảnh";
                    return View();
                }
                else
                {
                    var filename = Path.GetFileName(logoNSX.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), filename);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        logoNSX.SaveAs(path);
                    }
                    hsx.MaHang = MaHang;
                    hsx.Logo = filename;
                    db.HangSXes.InsertOnSubmit(hsx);
                    db.SubmitChanges();
                }
                return RedirectToAction("NhaSanXuat");
            }
            else
            {
                ViewData["loi"] = "Mã hãng này bị trùng";
            }
            return this.ThemNSX();
        }
        [HttpGet]
        public ActionResult XoaNSX(string id)
        {
            HangSX hsx = db.HangSXes.SingleOrDefault(n => n.MaHang == id);
            //ViewBag.MaDienThoai = dt.MaDienThoai;
            if (hsx == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hsx);
        }
        [HttpPost, ActionName("XoaNSX")]
        public ActionResult XacNhanXoaNSX(string id)
        {
            try {
                HangSX hsx = db.HangSXes.SingleOrDefault(n => n.MaHang == id);
                var kt = db.Giays.Where(x => x.MaHang == id).ToList();
                //if (hsx == null)
                //{
                //    response.statuscode = 404;
                //    return null;
                //}
                if(kt != null)
                {
                    ViewData["Error"] = "Vui lòng xóa sản phẩm thuộc hãng giày này trước";
                    db.SubmitChanges();
                    return RedirectToAction("NhaSanXuat");
                }

                db.HangSXes.DeleteOnSubmit(hsx);
            }
            catch ( Exception ex)
            {
               
                ViewBag.onError = "Vui lòng xóa sản phẩm thuộc hãng giày này trước";
                //db.SubmitChanges();
                return RedirectToAction("NhaSanXuat");
            }
            //HangSX hsx = db.HangSXes.SingleOrDefault(n => n.MaHang == id);

            //if (hsx == null)
            //{
            //    Response.StatusCode = 404;
            //    return null;
            //}

            //db.HangSXes.DeleteOnSubmit(hsx);
            //db.SubmitChanges();
            
            return RedirectToAction("NhaSanXuat");
        }
        [HttpGet]
        public ActionResult SuaNSX(string id)
        {
            HangSX hsx = db.HangSXes.SingleOrDefault(n => n.MaHang == id);

            if (hsx == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hsx);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaNSX(String id, HttpPostedFileBase logoHSX)
        {
            ViewBag.MaHang = new SelectList(db.HangSXes.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
            HangSX hang = db.HangSXes.SingleOrDefault(n => n.MaHang == id);
            if (logoHSX == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh mới";
                UpdateModel(hang);
                db.SubmitChanges();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(logoHSX.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                        logoHSX.SaveAs(path);
                    hang.Logo = fileName;
                    UpdateModel(hang);
                    db.SubmitChanges();
                }
            }
            return RedirectToAction("NhaSanXuat", "Admin");
        }
        public ActionResult ChiTietNSX(string id)
        {
            HangSX hsx = db.HangSXes.SingleOrDefault(n => n.MaHang == id);
            //ViewBag.MaDienThoai = dt.MaDienThoai;
            if (hsx == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hsx);
        }
        public ActionResult DonHang(int? page)
        {
            if (Session["TaiKhoanAdmin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                int pageNumber = (page ?? 1);
                int pageSize = 5;
                //return View(db.DienThoais.ToList());
                return View(db.DonHangs.ToList().OrderBy(n => n.ID).ToPagedList(pageNumber, pageSize));

            }

        }
        public ActionResult ChiTietDonHang(int id, int? page)
        {
            
           
            if (Session["TaiKhoanAdmin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            
            else  
            {
                int pageNumber = (page ?? 1);
                int pageSize = 5;
                var chitietdonhang = from iddonhang in db.ChiTietDonHangs where iddonhang.IDDonHang == id select iddonhang;
                //return View(chitietdonhang.ToPagedList(pageNumber, pageSize));
                return View(db.ChiTietDonHangs.Where(n => n.IDDonHang == id).ToList().ToPagedList(pageNumber, pageSize));
                //return View(chitietdonhang);
              
            }

        }
        public ActionResult CapNhatHD(int id)
        {

            if (ModelState.IsValid)
            {
                DonHang dh = db.DonHangs.SingleOrDefault(n => n.ID == id);
                dh.TrangThai = "Đã giao hàng và khách hàng đã thanh toán đầy đủ";
                UpdateModel(dh);
                db.SubmitChanges();
            }
            return RedirectToAction("DonHang", "Admin");
        }


    }
}
