using GSF;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using VietQR_QRCodeGeneration.Models;
using VietQR_QRCodeGeneration.Ultil;
using static System.Net.Mime.MediaTypeNames;

namespace VietQR_QRCodeGeneration.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var listBanks = new List<BankModel>();
            listBanks = BankModel.GetListBanks();

            ViewBag.ListBanks = new SelectList(listBanks.Select(x => new SelectListItem { Value = x.BankID.ToString(), Text = x.BankName}), "BankID", "BankName");

            return View();
        }

        public ActionResult Create(string bankcode = "", string accnumber = "", string amount = "", string content = "")
        {
            string base64QRCode = "";
            decimal d_amount = 0;
            if (bankcode.IsNullOrWhiteSpace() || bankcode.IsEmpty())
            {
                ViewBag.ErrorMessage = "Không được để trống mã ngân hàng!";
                return View("Index");
            }
            if(accnumber.IsNullOrWhiteSpace() || accnumber.IsEmpty())
            {
                ViewBag.ErrorMessage = "Không được để trống số tài khoản!";
                return View("Index");
            }
            if (!(amount.IsNullOrWhiteSpace() || amount.IsEmpty()))
            {
                try
                {
                    d_amount = decimal.Parse(amount);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Số tiền nhập không đúng định dạng!";
                    return View("Index");
                }
            }
            
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                string qrcontent = Function.createQRCodeContent(bankcode, accnumber, d_amount, content);

                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrcontent, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    using (Bitmap qrCodeImage = qrCode.GetGraphic(3))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            qrCodeImage.Save(memoryStream, ImageFormat.Png);
                            byte[] qrCodeBytes = memoryStream.ToArray();
                            base64QRCode = Convert.ToBase64String(qrCodeBytes);
                            ViewBag.QrCodeImage = "data:image/png;base64," + base64QRCode;
                        }
                    }
                }
            }
            ViewBag.BankCode = bankcode;
            ViewBag.AccNumber = accnumber;
            ViewBag.Amount = amount;
            ViewBag.Content = content;

            return View("Index");
        }
    }
}
