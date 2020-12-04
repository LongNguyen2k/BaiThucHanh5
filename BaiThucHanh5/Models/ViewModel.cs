﻿using System.ComponentModel.DataAnnotations;
namespace BaiThucHanh5.Models
{
    public class ViewModel
    {
        public KhachHang khachhang { get; set; }
        public CTHD cthd { get; set; }
        public HoaDon hoadon { get; set; }
        public LoaiSP loaisp { get; set; }
        public Nhanvien nhanvien { get; set; }
        public SanPham sanpham { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.##0}")]
        public double Thanhtien { get; set; }
    }
}