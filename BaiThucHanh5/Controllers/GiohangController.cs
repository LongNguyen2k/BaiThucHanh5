using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaiThucHanh5.Models;
using System.Net;
using System.Net.Mail;
namespace BaiThucHanh5.Controllers
{
  
    public class GiohangController : Controller
    {
        private qlbanhangEntities db = new qlbanhangEntities();
        // GET: Giohang
        public ActionResult Index()
        {
            // trong giỏ hàng sẽ có nhiều item nên sẽ có 1 list cái CartItem
            List<CartItem> giohang = Session["giohang"] as List<CartItem>; 

            return View(giohang);
        }
        // tạo thêm phương thức thêm sản phẩm vào giỏ hàng
        public RedirectToRouteResult AddToCart(string MaSP)
        {
            if (Session["giohang"] == null)
            {
                Session["giohang"] = new List<CartItem>();

            }
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            // kiểm tra sản phẩm khách đang chọn có trong giỏ hang hay chưa 
            if (giohang.FirstOrDefault(m => m.MaSP == MaSP) == null) // chưa có trong giỏ
            {
                SanPham sp = db.SanPhams.Find(MaSP);
                CartItem newItem = new CartItem();
                newItem.MaSP = MaSP;
                newItem.TenSP = sp.TenSP;
                newItem.SoLuong = 1;
                newItem.DonGia = Convert.ToDouble(sp.Dongia);
                giohang.Add(newItem);
            }
            else // sản phẩm đã có trong giỏ hàng =>> tăng 1 
            {
                CartItem cartItem = giohang.FirstOrDefault(m => m.MaSP == MaSP);
                cartItem.SoLuong++;

            }
            Session["giohang"] = giohang;

            // trả qua action Index
            return RedirectToAction("Index");

        }
        public RedirectToRouteResult Update(string MaSP, int txtSoluong)
        {
            // tim CartItem muốn Xóa
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            CartItem item = giohang.FirstOrDefault(m => m.MaSP == MaSP);
            if (item != null)
            {
                item.SoLuong = txtSoluong;
                Session["giohang"] = giohang;
            }
            return RedirectToAction("Index");
        }
        public RedirectToRouteResult DelCartItem(string maSP)
        {
            // tim CartItem muốn Xóa
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            CartItem item = giohang.FirstOrDefault(m => m.MaSP == maSP);
            if (item != null)
            {
                giohang.Remove(item);
                Session["giohang"] = giohang;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Order(string Email, string Phone)
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            string sMsg = "<html><body><table border ='1'<caption>Thông Tin Đặt Hàng</caption> <tr><th>STT</th><th>Tên hàng </th> <th> Số Lượng </th><th>Đơn Giá </th> <th>Thành Tiền </th>  </tr> ";
            int i = 0;
            double tongTien = 0;
            foreach (CartItem item in giohang)
            {
                i++;
                sMsg += "<tr>";
                sMsg += "<td>" + i.ToString() + "</td>";
                sMsg += "<td>" + item.TenSP + "</td>";
                sMsg += "<td>" + item.SoLuong.ToString() + "</td>";
                sMsg += "<td>" + item.DonGia.ToString() + "</td>";
                sMsg += "<td>" + String.Format("{0:#,###}",item.SoLuong * item.DonGia) + "</td>";
                sMsg += "</tr>";
                tongTien += item.SoLuong * item.DonGia;
            }
            sMsg += "<tr> <th colspan='5'> Tổng Cộng: "
            + String.Format("{0:#,### vnd}", tongTien) + "</th></tr></table>";
            /// Đây là địa chỉ Email của người gửi thư qua cho bạn   ,, Email này là email nhận thư //
            MailMessage mail = new MailMessage("mahoanghainguyenpro@gmail.com", Email, "Thông Tin Đơn Hàng", sMsg);
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("mahoanghainguyenpro","Nguyen16102000");
            mail.IsBodyHtml = true;
            client.Send(mail);
            return RedirectToAction("Index", "Home");
            // gửi email cho khách hàng 

        }
    }
}