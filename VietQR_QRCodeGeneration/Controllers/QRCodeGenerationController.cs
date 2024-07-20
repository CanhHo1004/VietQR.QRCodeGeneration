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
                var qrContent = createQRCodeContent(bankCode, accountNumber, amount, message);
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

        private string createQRCodeContent(string bankCode, string accountNumber, decimal amount = 0, string message = "")
        {
            var contentQRCode = "";
            var amount_str = "";
            var message_str = "";

            var payload_format_indicator = "000201";    //(1) Payload mặc định 
            var point_of_initiation_method = "010211";  //(2) Định dạng mặc định là QR tĩnh 
            var country_code = "5802VN"; //Mã quốc gia
            var money_country_code = "5303704"; //(8) Đơn vị tiền tệ VNĐ
            var payload_type = "0208QRIBFTTA"; //(7) Phương thức giao dịch: Chuyển tiền đến số tài khoản
            var merchant_account_info_template = "0010A000000727"; //(4) khai báo template của thông tin tài khoản
            var payment_info = String.Format("00{0}{1}01{2}{3}", bankCode.Length.ToString("00"), bankCode, accountNumber.Length.ToString("00"), accountNumber); //(6) Thông tin ngân hàng và tài khoản thụ hưởng
            var len_payment_info = String.Format("01{0}", payment_info.Length.ToString("00")); //(5) Khai báo độ dài của thông tin ngân hàng + tài khoản
            var len_account_payment_info = String.Format("38{0}", (merchant_account_info_template + len_payment_info + payment_info + payload_type).Length.ToString("00")); //(3)Khai báo thông tin và tổng độ dài của thông tin thụ hưởng
            var crc_place_holder = "6304";

            if (amount != 0)
            {
                amount_str += String.Format("54{0}{1}", amount.ToString().Length.ToString("00"), amount);
            }
            if (!message.Equals(""))
            {
                message_str += String.Format("62{0}08{1}{2}", (message.Length + 4).ToString("00"), (message.Length).ToString("00"), message);
            }

            contentQRCode += payload_format_indicator + 
                point_of_initiation_method + 
                len_account_payment_info + 
                merchant_account_info_template + 
                len_payment_info + payment_info + 
                payload_type + 
                money_country_code +
                amount_str +
                country_code +
                message_str +
                crc_place_holder;

            var crc = Function.CRC16Trans(contentQRCode);

            contentQRCode += crc;

            return contentQRCode;
        }
    }
}