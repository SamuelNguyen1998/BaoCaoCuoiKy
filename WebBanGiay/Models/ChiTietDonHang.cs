using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanGiay.Models
{
    public class ChiTietDonHangs
    {
        dbQLBanGiayDataContext data = new dbQLBanGiayDataContext();
        public string sID { set; get; }
        public string sIDDonHang { set; get; }
        public string sIDGiay { set; get; }
        public float fDonGia { set; get; }
        public int iSoLuong { set; get; }
        public float fThanhTien
        {
            get { return iSoLuong * fDonGia; }
        }

        public ChiTietDonHangs(string IDDonHang)
        {
            
            sIDDonHang = IDDonHang;
            Giay dt = data.Giays.Single(n => n.MaGiay == sIDGiay);
            fDonGia = float.Parse(dt.GiaBan.ToString());
            iSoLuong = 1;
        }

    }
}