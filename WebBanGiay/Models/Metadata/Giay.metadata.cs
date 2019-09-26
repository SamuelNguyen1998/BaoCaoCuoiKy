using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebBanGiay.Models
{
    [MetadataTypeAttribute(typeof(GiayMetadata))]
    public partial class Giay
    {
        internal sealed class GiayMetadata
        {
            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Mã giày")]

            public string MaGiay { get; set; }

            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Tên giày")]

            public string TenGiay { get; set; }

            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Mã hãng")]

            public string MaHang { get; set; }

            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Mã loại")]

            public string MaLoai { get; set; }

           
            [Display(Name = "Màu sắc")]

            public string MauSac { get; set; }

            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Hình ảnh")]

            public string HinhAnh { get; set; }

            [Display(Name = "Giá bán")]
            [Required(ErrorMessage = "Không được để trống")]

            public Nullable<decimal> GiaBan { get; set; }

            [Display(Name = "Trạng thái")]
            [Required(ErrorMessage = "Không được để trống")]
            public string TrangThai { get; set; }

            [Display(Name = "Thanh toán online")]
            public string ThanhToanOnline { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(HangSXMetadata))]
    public partial class HangSX
    {
        internal sealed class HangSXMetadata
        {
            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Mã hãng")]

            public string MaHang { get; set; }

            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Tên hãng")]

            public string TenHang { get; set; }

            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Logo")]
            public string Logo { get; set; }
        }
    }
    [MetadataTypeAttribute(typeof(DonHangMetadata))]
    public partial class DonHang
    {
        internal sealed class DonHangMetadata
        {
            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Mã đơn hàng")]

            public string ID { get; set; }

            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Khách hàng")]

            public string IdKhachHang { get; set; }

            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Ngày đặt")]

            public string NgayBan { get; set; }

            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Tổng tiền")]
            public string TongTien { get; set; }

            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Trạng thái")]
            public string TrangThai { get; set; }

            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Đã thanh toán ?")]
            public string DaThanhToan { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(KhachHangMetadata))]
    public partial class KhachHang
    {
        internal sealed class KhachHangMetadata
        {
            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Tên khách hàng")]

            public string HoTen { get; set; }

            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Địa chỉ ")]

            public string DiaChi { get; set; }

            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = " Số điện thoại")]
            public string DienThoai { get; set; }

            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Email ")]
            public string Email { get; set; }

           
        }
    }

    [MetadataTypeAttribute(typeof(ChiTietDonHangMetadata))]
    public partial class ChiTietDonHang
    {
        internal sealed class ChiTietDonHangMetadata
        {
            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "ID")]

            public string ID { get; set; }

            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Mã Đơn Hàng ")]

            public string IDDonHang { get; set; }

            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Tên Giày")]
            public string IDGiay { get; set; }

            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Số Lượng ")]
            public string SoLuong { get; set; }

            [Required(ErrorMessage = "Không được để trống")]
            [Display(Name = "Đơn giá ")]
            public string DonGia { get; set; }

        }
    }
}