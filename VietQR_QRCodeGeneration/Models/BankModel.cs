using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VietQR_QRCodeGeneration.Models
{
    public class BankModel
    {
        public int BankID { get; set; }
        public string BankName { get; set; }

        public static List<BankModel> GetListBanks()
        {
            var banks = new List<BankModel>();
            banks.Add(new BankModel() { BankID = 970405, BankName = "Ngân hàng Nông nghiệp và Phát triển Nông thôn Việt Nam (Agribank)" });
            banks.Add(new BankModel() { BankID = 970436, BankName = "Ngân hàng TMCP Ngoại thương Việt Nam (Vietcombank)" });
            banks.Add(new BankModel() { BankID = 970415, BankName = "Ngân hàng TMCP Công Thương Việt Nam (VietinBank)" });
            banks.Add(new BankModel() { BankID = 970418, BankName = "Ngân hàng TMCP Đầu tư và Phát triển Việt Nam (BIDV)" });
            banks.Add(new BankModel() { BankID = 970407, BankName = "Ngân hàng TMCP Kỹ Thương Việt Nam (Techcombank)" });
            banks.Add(new BankModel() { BankID = 970416, BankName = "Ngân hàng TMCP Á Châu (ACB)" });
            banks.Add(new BankModel() { BankID = 970403, BankName = "Ngân hàng TMCP Sài Gòn Thương Tín (Sacombank)" });
            banks.Add(new BankModel() { BankID = 970423, BankName = "Ngân hàng TMCP Tiên Phong (TPBank)" });
            banks.Add(new BankModel() { BankID = 970422, BankName = "Ngân hàng TMCP Quân đội (MB Bank)" });
            banks.Add(new BankModel() { BankID = 970432, BankName = "Ngân hàng TMCP Việt Nam Thịnh Vượng (VPBank)" });
            banks.Add(new BankModel() { BankID = 970426, BankName = "Ngân hàng TMCP Hàng Hải Việt Nam (MSB)" });
            banks.Add(new BankModel() { BankID = 970429, BankName = "Ngân hàng TMCP Sài Gòn (SCB)" });
            banks.Add(new BankModel() { BankID = 970406, BankName = "Ngân hàng TMCP Đông Á (DongA Bank)" });
            banks.Add(new BankModel() { BankID = 970441, BankName = "Ngân hàng TMCP Quốc tế Việt Nam (VIB)" });
            banks.Add(new BankModel() { BankID = 970437, BankName = "Ngân hàng TMCP Phát triển Thành phố Hồ Chí Minh (HDBank)" });
            banks.Add(new BankModel() { BankID = 970431, BankName = "Ngân hàng TMCP Xuất Nhập Khẩu Việt Nam (Eximbank)" });
            banks.Add(new BankModel() { BankID = 970428, BankName = "Ngân hàng TMCP Nam Á (Nam A Bank)" });
            banks.Add(new BankModel() { BankID = 970454, BankName = "Ngân hàng TMCP Bản Việt (Viet Capital Bank)" });
            banks.Add(new BankModel() { BankID = 970427, BankName = "Ngân hàng TMCP Việt Á (Viet A Bank)" });
            banks.Add(new BankModel() { BankID = 970448, BankName = "Ngân hàng TMCP Phương Đông (OCB)" });

            return banks.ToList();
        }
    }
}