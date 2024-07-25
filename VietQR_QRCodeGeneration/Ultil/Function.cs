using CRC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CRC;
using System.Windows;

namespace VietQR_QRCodeGeneration.Ultil
{
    public class Function
    {
        public enum ResultCode
        {
            Success = 200,
            Error = 400,
            Exception = 21,
            Timeout = 440,
            NotFound = 404,
            Unauthorized = 401
        }

        public static string MD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string prior to .NET 5
                StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string createQRCodeContent(string bankCode = "", string accountNumber = "", decimal amount = 0, string message = "")
        {
            var contentQRCode = "";
            var amount_str = "";
            var message_str = "";

            if (bankCode != null && accountNumber != null)
            {
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
            }
            return contentQRCode;
        }

        public static string CRC16Trans(string input)
        {
            byte[] inputByte = Encoding.UTF8.GetBytes(input);
            var crc = ComputeCRC16(inputByte, 0x11021, 0xFFFF, 0x0000, false);
            return crc.ToString("X4");
        }

        static ushort ComputeCRC16(byte[] data, int polynomial, int initialCrc, int xorOut, bool reverse)
        {
            int crc = initialCrc;

            foreach (byte b in data)
            {
                crc ^= (ushort)(b << 8);

                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x8000) != 0)
                    {
                        crc = (ushort)((crc << 1) ^ polynomial);
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
            }

            if (reverse)
            {
                crc = ReverseBits(crc);
            }

            return (ushort)(crc ^ xorOut);
        }

        static int ReverseBits(int value)
        {
            ushort result = 0;

            for (int i = 0; i < 16; i++)
            {
                result = (ushort)((result << 1) | (value & 1));
                value >>= 1;
            }

            return result;
        }

        public static object readDataFromGoogleSheet(string spreadsheetId, string credentialsPath)
        {

            //string credentialsPath = Server.MapPath("~/path-to-your-credentials-file.json");
            //spreadsheetId = "your-spreadsheet-id";

            GoogleSheetsHelper helper = new GoogleSheetsHelper(credentialsPath);
            string range = "Sheet1!A1:D10";
            var values = helper.ReadEntries(spreadsheetId, range);

            return values;
        }
    }
}