using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanGiay.Models
{
    public class GioHang
    {
        dbQLBanGiayDataContext data = new dbQLBanGiayDataContext();
        public string sMaGiay { set; get; }
        public string sTenGiay { set; get; }
        public string sHinhAnh { set; get; }
        public float fGiaBan { set; get; }
        public int iSoLuong { set; get; }
        public float fThanhTien
        {
            get { return iSoLuong * fGiaBan; }
        }
        public GioHang(string MaGiay)
        {
            sMaGiay = MaGiay;
            Giay dt = data.Giays.Single(n => n.MaGiay == sMaGiay);
            sTenGiay = dt.TenGiay;
            sHinhAnh = dt.HinhAnh;
            fGiaBan = float.Parse(dt.GiaBan.ToString());
            iSoLuong = 1;
        }
    }
}