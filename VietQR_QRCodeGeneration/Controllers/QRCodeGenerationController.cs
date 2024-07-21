using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Web.Http;
using VietQR_QRCodeGeneration.Models;
using VietQR_QRCodeGeneration.Ultil;

namespace VietQR_QRCodeGeneration.Controllers
{
    public class QRCodeGenerationController : ApiController
    {
        [BearAuth]
        [Route("api/generateQRCode")]
        [HttpPost]
        public Response generateQRCode(inputInfo data) 
        {
            string bankCode = data.bankCode;
            string accountNumber = data.accountNumber;
            string amount_str = data.amount;
            string message = data.contentBilling;
            decimal amount = 0;
            try
            {
                amount = decimal.Parse(amount_str);
            }
            catch(Exception ex)
            {
                return new Response { resultCode = (int)HttpStatusCode.BadRequest, resultString = "Số tiền không đúng định dạng!", resultData = ex.Message };
            }
            string base64QRCode = "";

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                var qrContent = Function.createQRCodeContent(bankCode, accountNumber, amount, message);
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    using (Bitmap qrCodeImage = qrCode.GetGraphic(3))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            qrCodeImage.Save(ms, ImageFormat.Png);
                            byte[] byteImage = ms.ToArray();
                            base64QRCode = Convert.ToBase64String(byteImage);
                        }
                    }
                }
            }
            return new Response { resultCode = (int)HttpStatusCode.OK, resultString = "Success", resultData = base64QRCode };
        }
    }
}