using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanGiay.Models;
using Common;


namespace WebBanGiay.Controllers
{
    public class GioHangController : Controller
    {
        dbQLBanGiayDataContext data = new dbQLBanGiayDataContext();
       


        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(string sMaGiay, string sURL)
        {
            //Lấy ra Session giỏ hàng
            List<GioHang> lstGioHang = LayGioHang();
            //Kiểm tra đt này tồn tại trong Session["GioHang"] chưa?
            GioHang SanPham = lstGioHang.Find(n => n.sMaGiay == sMaGiay);
            if (SanPham == null)
            {
                SanPham = new GioHang(sMaGiay);
                lstGioHang.Add(SanPham);
                return Redirect(sURL);
            }
            else
            {
                SanPham.iSoLuong++;
                return Redirect(sURL);
            }
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }
        private float TongTien()
        {
            float fTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                fTongTien = lstGioHang.Sum(n => n.fThanhTien);
            }
            return fTongTien;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        public ActionResult XoaGioHang(string sMaSP)
        {
            //Lấy giỏ hàng từ session
            List<GioHang> lstGioHang = LayGioHang();
            //Kiểm tra đt đã có trong session ["GioHang"]
            GioHang SanPham = lstGioHang.SingleOrDefault(n => n.sMaGiay == sMaSP);
            if (SanPham != null)
            {
                lstGioHang.RemoveAll(n => n.sMaGiay == sMaSP);
                return RedirectToAction("GioHang");
            }
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang(string sMaSP, FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang SanPham = lstGioHang.SingleOrDefault(n => n.sMaGiay == sMaSP);
            if (SanPham != null)
            {
                SanPham.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaAll()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            // ChiTietDonHang ct = new ChiTietDonHang();
            List<ChiTietDonHang> ct1 = new List<ChiTietDonHang>();
            Giay g = new Giay();
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<GioHang> gh = LayGioHang();
            dh.IdKhachHang = kh.ID;
            dh.NgayBan = DateTime.Now;
            var TrangThai = "Chưa giao hàng";
            //var NgayGiao = string.Format("{0:MM/dd/yyyy}", collection["NgayGiaoHang"]);
            dh.TongTien = TongTien();
            //dh.NgayGiao = DateTime.Parse(NgayGiao);
            dh.TrangThai = TrangThai;
            dh.DaThanhToan = false;
            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ct = new ChiTietDonHang();
                ct1.Add(new ChiTietDonHang()
                {
                    IDGiay = item.sMaGiay,
                    IDDonHang = dh.ID,
                    SoLuong = item.iSoLuong,
                    DonGia = item.fGiaBan

                });
                ct.IDDonHang = dh.ID;
                ct.IDGiay = item.sMaGiay;
                ct.SoLuong = item.iSoLuong;
                ct.DonGia = item.fGiaBan;

                data.ChiTietDonHangs.InsertOnSubmit(ct);
            }
            data.SubmitChanges();
            Session["GioHang"] = null;

            string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/neworder.html"));
          //  string content = System.IO.File.ReadAllText(Server.MapPath("~/Views/GioHang/GuiMail.html"));

            content = content.Replace("{{HoTen}}", kh.HoTen);
            content = content.Replace("{{DienThoai}}", kh.DienThoai);
            content = content.Replace("{{Email}}", kh.Email);
            content = content.Replace("{{DiaChi}}", kh.DiaChi);
            string IDGiay = null;
            string SoLuong = null;
            string DonGia = null;
            foreach (var item in ct1)
            {
                IDGiay = string.Join(",", item.IDGiay);
                SoLuong = string.Join(",", item.SoLuong);
                DonGia = string.Join(",", item.DonGia);
            }          
           // content = content.Replace("{{IDDonHang}}", );
            content = content.Replace("{{TenGiay}}", IDGiay);
            content = content.Replace("{{SoLuong}}", SoLuong);
            content = content.Replace("{{DonGia}}", DonGia);
            content = content.Replace("{{TongTien}}", dh.TongTien.ToString());
            
            var toEmail = kh.Email;

            new MailHelper().SendMail(toEmail, "Đơn hàng mới từ PSneakerP", content);

            return RedirectToAction("XacNhanDonHang", "GioHang");
        }
        public ActionResult XacNhanDonHang()
        {
           

            return View();
        }
        public ActionResult TenKH()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                ViewBag.TKH = "Đăng nhập";
            }
            else
            {
                KhachHang kh = (KhachHang)Session["TaiKhoan"];
                ViewBag.TKH = kh.HoTen;
            }
            return PartialView();
        }
  
          
    }
}